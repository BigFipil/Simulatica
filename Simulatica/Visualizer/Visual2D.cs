using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualizer
{
	public class Visual2D : Game
	{
		SpriteBatch spriteBatch;
		Texture2D Grid, pixel;
		AnimationConfig Config;

		Vector2 OutputWindowSize = new Vector2(1920, 1080);
		Vector2 GridSize;

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
				PreferredBackBufferWidth = (int)OutputWindowSize.X,
				PreferredBackBufferHeight = (int)OutputWindowSize.Y,
			};
			graphics.ApplyChanges();


			GridSize = CalcWindowSize();


			renderTarget = new RenderTarget2D(GraphicsDevice, (int)GridSize.X, (int)GridSize.Y, false,
			GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);


			IsMouseVisible = true;
			Window.AllowUserResizing = true;


			Files = Directory.GetFiles(Config.OutputPath)
				.Where((val) => val.EndsWith(".txt"))
				.Where((val) => val.Contains("T="));

			Files = Files.OrderBy(s => double.Parse(s.Substring(s.IndexOf("T=")+2).Replace(".txt","")) );

			if (Files.Count() == 0)
            {
				Console.WriteLine("Could not find simulation results in specific directory: "+Config.OutputPath);
            }
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);


			Grid = Content.Load<Texture2D>("Grid");
			pixel = Content.Load<Texture2D>("pixel");

			base.LoadContent();

			SaveFrame(renderTarget, Files.First(), "pic.png");
		}

		protected override void Draw(GameTime gameTime)
		{
			//Color c;
			//var prop = typeof(Color).GetProperty("Red");
			//if (prop != null)
			//{
			//	c = (Color)prop.GetValue(null, null);
			//}
			//else c = Color.Black;
			//Console.WriteLine(c);

			

			//spriteBatch.Begin();

			//DrawGrid(spriteBatch, new Rectangle(Vector2.Zero.ToPoint(), GridSize.ToPoint()));
			//spriteBatch.Draw(pixel, Vector2.Zero, Color.Red);

			//Console.WriteLine(Config.OutputPath);

			//DrawParticles(spriteBatch, Files.First());

			//spriteBatch.End();
		}

		private void DrawGrid(SpriteBatch batch, Rectangle rec, float alpha = 0.2f)
        {
			//float scale = 1, gridScale = 2;
			int Width = rec.Width;
			int Height = rec.Height;
			int X = rec.X;
			int Y = rec.Y;

			batch.Draw(pixel, new Rectangle(X,Y,Width, Height), Color.White);

			for(int i = 0; i < 10; i++)
			{
				batch.Draw(pixel, new Rectangle(X+(int)(Width / 10) * i, Y, 2, Height), Color.Black * alpha);

				for(int j = 0; j < 5; j++)
                {
					batch.Draw(pixel, new Rectangle(X+(int)(Width / 10) * i  +  (int)((Width / 50)*j), Y, 1, Height), Color.DarkGray * alpha);
				}
			}
			int h = 0;
			while(h < Height - (Width / 50))
            {
				batch.Draw(pixel, new Rectangle(X, Y + h, Width, 2), Color.Black * alpha);
				h += (Width / 50);
				batch.Draw(pixel, new Rectangle(X, Y + h, Width, 1), Color.DarkGray * alpha);
				h += (Width / 50);
				batch.Draw(pixel, new Rectangle(X, Y + h, Width, 1), Color.DarkGray * alpha);
				h += (Width / 50);
				batch.Draw(pixel, new Rectangle(X, Y + h, Width, 1), Color.DarkGray * alpha);
				h += (Width / 50);
				if(h < Height) batch.Draw(pixel, new Rectangle(X, Y + h, Width, 1), Color.DarkGray * alpha);
				h += (Width / 50);
			}
		}
		
		private void DrawParticles(SpriteBatch batch, string path)
        {
			StreamReader reader = new StreamReader(path);
			string line;

			Color c = Color.Black;
			double x = 0, y = 0;

			do {

				line = reader.ReadLine();
				line = line.Replace("[", "");
				line = line.Replace("]", "");
				line = line.Replace(",", ":");

				var str = line.Split(":");

				foreach(var v in Config.particleBlueprints)
                {
					if(v.Name == str[0])
                    {
						foreach(var o in v.outputInformations)
                        {
                            switch (o.Key.ToLower())
                            {
								case "color":
									var prop = typeof(Color).GetProperty(o.Value);
									if (prop != null)
									{
										c = (Color)prop.GetValue(null, null);
									}
									else c = Color.Black;
									break;

								case "x":
									string tmp = o.Value.Replace("<", "").Replace(">", "");
									int index = Int32.Parse(tmp);

									x = double.Parse(str[index]);
									break;

								case "y":
									string tmp2 = o.Value.Replace("<", "").Replace(">", "");
									int index2 = Int32.Parse(tmp2);

									y = double.Parse(str[index2]);
									break;
							}
                        }

						break;
                    }
                }

				double scale =  GridSize.X / Config.SimulationBoxSize.X;
				x *= scale;
				y *= scale;

				batch.Draw(pixel, new Rectangle((int)x, (int)y, 4, 4), c);

			} while(!reader.EndOfStream);


		}
		private Vector2 CalcWindowSize()
		{
			Vector2 NormalGrid = Vector2.Normalize(new Vector2(Config.SimulationBoxSize.X, Config.SimulationBoxSize.Y));
			Vector2 OS = OutputWindowSize - new Vector2(50, 50);

			NormalGrid *= Math.Max(OS.X, OS.Y);

			float scale = Math.Min(OS.X / NormalGrid.X, OS.Y / NormalGrid.Y);

			return NormalGrid*scale;
        }

		private void DrawSceneToTexture(RenderTarget2D renderTarget, string path)
		{
			// Set the render target
			GraphicsDevice.SetRenderTarget(renderTarget);

			GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };

			// Draw the scene
			GraphicsDevice.Clear(Color.CornflowerBlue);
			spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);

			DrawGrid(spriteBatch, new Rectangle(Vector2.Zero.ToPoint(), GridSize.ToPoint()));

			DrawParticles(spriteBatch, path);

			spriteBatch.End();

			// Drop the render target
			GraphicsDevice.SetRenderTarget(null);
		}

		private void SaveFrame(RenderTarget2D r, string path, string name)
        {
			GraphicsDevice.Clear(Color.White);

			DrawSceneToTexture(renderTarget, path);

			Stream stream = File.Create(Config.OutputPath + "\\pic.png");

			//Save as PNG
			renderTarget.SaveAsPng(stream, (int)GridSize.X, (int)GridSize.Y);
			stream.Dispose();
		}
	}
}
 