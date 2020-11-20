using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using FFMediaToolkit.Encoding;
using FFMediaToolkit.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
//using System.Drawing;

namespace Visualizer
{
	public class Visual2D : Game
	{
		SpriteBatch spriteBatch;
		Texture2D Grid, pixel;
		AnimationConfig Config;

		SpriteFont basicFont1;

		Vector2 MaximumWindowSize { get; set; } = new Vector2(1920, 1080);
		Vector2 GridSize, OutputWindowSize, MarginOffsetSize = new Vector2(100, 100); //MarginOffsetSize/2 = margin size
		int LegendSize = 50;

		IEnumerable<string> Files;


		RenderTarget2D renderTarget;

		public Visual2D(AnimationConfig config)
		{
			Config = config;

			string path = Assembly.GetEntryAssembly().Location;
			path = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\"));
			path = Path.Combine(path, @"Content");

			Content.RootDirectory = path;

			var graphics = new GraphicsDeviceManager(this)
			{
				IsFullScreen = false,
				PreferredBackBufferWidth = (int)MaximumWindowSize.X,
				PreferredBackBufferHeight = (int)MaximumWindowSize.Y,
			};
			graphics.ApplyChanges();


			OutputWindowSize = CalcWindowSize();
			GridSize = OutputWindowSize - MarginOffsetSize - new Vector2(LegendSize, 0);


			renderTarget = new RenderTarget2D(GraphicsDevice, (int)OutputWindowSize.X+LegendSize, (int)OutputWindowSize.Y, false,
			GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);


			Files = Directory.GetFiles(Config.OutputPath + "\\Data")
				.Where((val) => val.EndsWith(".txt"))
				.Where((val) => val.Contains("T="));

			Files = Files.OrderBy(s => double.Parse(s.Substring(s.IndexOf("T=") + 2).Replace(".txt", "")));

			if (Files.Count() == 0)
			{
				Files = Directory.GetFiles(Config.OutputPath)
				.Where((val) => val.EndsWith(".txt"))
				.Where((val) => val.Contains("T="));

				Files = Files.OrderBy(s => double.Parse(s.Substring(s.IndexOf("T=") + 2).Replace(".txt", "")));

				Console.WriteLine("***Could not find simulation results in specific directory: " + Config.OutputPath);
			}
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);


			Grid = Content.Load<Texture2D>("Grid");
			pixel = Content.Load<Texture2D>("pixel");
			basicFont1 = Content.Load<SpriteFont>("BasicFont");

			Particle.Load(Content, Config);

			if (!Directory.Exists(Path.GetFullPath(Config.OutputPath + "\\Frames")))
			{
				Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath + "\\Frames"));
			}
			else
			{
				Directory.Delete(Path.GetFullPath(Config.OutputPath + "\\Frames"), true);
				Directory.CreateDirectory(Path.GetFullPath(Config.OutputPath + "\\Frames"));
			}

			base.LoadContent();

		}

		public void BakeAnimation()
        {
			LoadContent();
			Console.WriteLine(GridSize);
			Console.WriteLine(OutputWindowSize);
			//Creating PNG frame images
			
            for (int i = 0; i < Files.Count(); i++)
            {
                SaveFrame(renderTarget, Files.ElementAt(i), "frame" + i + ".jpg");
                Console.WriteLine(Files.ElementAt(i));
            }

            string filename = "ffmpeg.exe";
			var proc = System.Diagnostics.Process.Start(filename, @" -y -r "+Config.OutputAnimationFramerate+" -start_number 0 -i "+ Config.OutputPath+ "\\Frames\\frame%d.jpg" + @" -pix_fmt yuv420p " + Config.OutputPath+ "\\SimulationOutput.mp4");
			/*
			 * -y means overwrite if such video already exists.
			 * -r stands for framerate
			 * -start_number wiadomo
			 * -i path to png's
			 * -pix_fmt pixel format
			 * */
		}

		private void DrawGrid(SpriteBatch batch, Rectangle rec, SpriteFont f, float alpha = 0.2f)
        {
			//float scale = 1, gridScale = 2;
			int Width = rec.Width;
			int Height = rec.Height;
			int X = rec.X;
			int Y = rec.Y;

			float max = Math.Max(Config.SimulationBoxSize.X, Config.SimulationBoxSize.Y);

			batch.Draw(pixel, new Rectangle(X,Y,Width, Height), Color.White);

			float gridSize = Math.Max(Width, Height) / 50.0f;

			for(int i = 1; i <= 50; i++)
            {
				if(i%5 == 0)
				{
					if (X + (int)(i * gridSize) < X + Width) batch.Draw(pixel, new Rectangle(X + (int)(i * gridSize), Y-10, 2, Height+10), Color.Black * alpha);
					if (Y + (int)(i * gridSize) < Y + Height) batch.Draw(pixel, new Rectangle(X-10, Y + (int)(i*gridSize), Width+10, 2), Color.Black * alpha);

					string s = ("" + Math.Round(max / 50 * i, 3));

					Vector2 size = f.MeasureString(s);
					if (X + (int)(i * gridSize) < X + Width) batch.DrawString(f, s, new Vector2(X + (int)(i * gridSize), MarginOffsetSize.Y / 4) - size / 2, Color.Black);
					if (Y + (int)(i * gridSize) < Y + Height) batch.DrawString(f, s, new Vector2(MarginOffsetSize.Y / 4, Y + (int)(i*gridSize)) - size / 2, Color.Black);
				}
				else
				{
					if (X + (int)(i * gridSize) < X + Width) batch.Draw(pixel, new Rectangle(X + (int)(i*gridSize), Y-10, 1, Height+10), Color.DarkGray * alpha);
					if (Y + (int)(i * gridSize) < Y + Height) batch.Draw(pixel, new Rectangle(X-10, Y + (int)(i * gridSize), Width+10, 1), Color.DarkGray * alpha);
				}
            }

		}
		
		private void DrawParticles(SpriteBatch batch, string path, Rectangle rec)
        {
			StreamReader reader = new StreamReader(path);
			string line;
			Particle p;

			do {

				line = reader.ReadLine();
				p = Particle.FromString(line);

				float scale =  GridSize.X / Config.SimulationBoxSize.X;
				p.x *= scale;
				p.y *= scale;

				//offset
				p.x += rec.X;
				p.y += rec.Y;

				if (rec.Contains((int)p.x, (int)p.y))
					p.Draw2D(batch);

			} while(!reader.EndOfStream);


		}

		private void DrawTitle(SpriteBatch batch, SpriteFont f, string Title, Color c)
		{
			Vector2 size = f.MeasureString(Title);

			Vector2 pos = new Vector2(OutputWindowSize.X / 2, OutputWindowSize.Y - MarginOffsetSize.Y / 4) - size / 2;

			batch.DrawString(f, Title, pos, c);
		}
		private void DrawLegend(SpriteBatch batch, SpriteFont f)
		{
			if(LegendSize != 0)
			for(int i = 0; i < Config.particleBlueprints.Count; i++)
            {
				var size = f.MeasureString(Config.particleBlueprints[i].Name);
				int tmp = (int)(OutputWindowSize.Y / (Config.particleBlueprints.Count + 1));

				batch.DrawString(f, Config.particleBlueprints[i].Name, OutputWindowSize - new Vector2(0, tmp * (i+1) - size.Y/2), Color.Black, 0.52f, Vector2.Zero, 1, SpriteEffects.None, 0);

					PropertyInfo p = null;
					if (Config.particleBlueprints[i].outputInformations.ContainsKey("Color")) p = typeof(Color).GetProperty(Config.particleBlueprints[i].outputInformations["Color"]);
					else if (Config.particleBlueprints[i].outputInformations.ContainsKey("color")) p = typeof(Color).GetProperty(Config.particleBlueprints[i].outputInformations["color"]);

					if(p != null)
                    {
						Color c = (Color)p.GetValue(null, null);
						batch.Draw(pixel, new Rectangle((OutputWindowSize - new Vector2(20, tmp * (i + 1) - size.Y / 2 - 8)).ToPoint(), new Point(5, 5)), c);
                    }
			}
		}

		private void DrawSceneToTexture(RenderTarget2D renderTarget, string path)
		{
			// Set the render target
			GraphicsDevice.SetRenderTarget(renderTarget);

			GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

			// Draw the scene
			GraphicsDevice.Clear(Color.Gold);
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

			Rectangle GridRec = new Rectangle((MarginOffsetSize/2).ToPoint(), GridSize.ToPoint());

			DrawGrid(spriteBatch, GridRec, basicFont1);

			DrawParticles(spriteBatch, path, GridRec);

			string title = Path.GetFileName(path);
			DrawTitle(spriteBatch, basicFont1, title, Color.Black);
			DrawLegend(spriteBatch, basicFont1);

			spriteBatch.End();

			// Drop the render target
			GraphicsDevice.SetRenderTarget(null);
		}

		private void SaveFrame(RenderTarget2D r, string path, string name)
        {
			GraphicsDevice.Clear(Color.White);

			DrawSceneToTexture(renderTarget, path);

			Stream stream = File.Create(Config.OutputPath + "\\Frames\\" + name);

			//Save as PNG
			renderTarget.SaveAsJpeg(stream, (int)OutputWindowSize.X + LegendSize, (int)OutputWindowSize.Y);
			stream.Dispose();
		}

		private Vector2 CalcWindowSize()
		{
			Vector2 NormalGrid = Vector2.Normalize(new Vector2(Config.SimulationBoxSize.X, Config.SimulationBoxSize.Y));
			Vector2 OS = MaximumWindowSize - MarginOffsetSize - new Vector2(LegendSize, 0);

			NormalGrid *= Math.Max(OS.X, OS.Y);

			float scale = Math.Min(OS.X / NormalGrid.X, OS.Y / NormalGrid.Y);

			return (NormalGrid * scale) + MarginOffsetSize + new Vector2(LegendSize, 0);
		}

	}
}
 