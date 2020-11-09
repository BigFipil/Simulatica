using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Visualizer
{
	public class Visual3D : Game
	{
		SpriteBatch spriteBatch;
		Texture2D grid;
		//Model model;
		AnimationConfig Config;
		Color c = Color.White;
        private RenderTarget2D renderTarget, XZ, XY, YZ;

		Vector3 cameraPosition = new Vector3(-7.6f, 8.7f, 5);
		Vector3 cameraLookAtVector = Vector3.Zero;
		Vector3 cameraUpVector = Vector3.UnitZ;
		Vector3 NormalBox, NegateNormalBox;

		//VertexPositionTexture[] floorVerts;
		BasicEffect effect;
		GraphicsDeviceManager graphics;

		IEnumerable<string> Files;

		public Visual3D(AnimationConfig config)
		{
			Config = config;

			string path = Assembly.GetEntryAssembly().Location;
			path = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\"));
			path = Path.Combine(path, @"Content");

			Content.RootDirectory = path;

			graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1920,
				PreferredBackBufferHeight = 1080,
			};
			graphics.ApplyChanges();

			renderTarget = new RenderTarget2D(GraphicsDevice, 1920, 1080, false,
			GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);


			Files = Directory.GetFiles(Config.OutputPath)
				.Where((val) => val.EndsWith(".txt"))
				.Where((val) => val.Contains("T="));

			Files = Files.OrderBy(s => double.Parse(s.Substring(s.IndexOf("T=") + 2).Replace(".txt", "")));

			if (Files.Count() == 0)
			{
				Console.WriteLine("Could not find simulation results in specific directory: " + Config.OutputPath);
			}
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			grid = Content.Load<Texture2D>("Grid");

			Particle.Load(Content, Config, graphics);

			base.LoadContent();
		}

        protected override void Initialize()
        {
			NormalBox = Vector3.Normalize(new Vector3(Config.SimulationBoxSize.X, Config.SimulationBoxSize.Y, Config.SimulationBoxSize.Z));
			NegateNormalBox = Vector3.Negate(NormalBox);
			NegateNormalBox.Z *= -1;

			cameraLookAtVector = NegateNormalBox / 2 * 10;
			cameraLookAtVector.Z *= 0.9f;
			//Console.WriteLine(Config.SimulationBoxSize);
			//Console.WriteLine(NegateNormalBox / 2 * 10);

			effect = new BasicEffect(graphics.GraphicsDevice);

			base.Initialize();
        }

		public void DrawBox(Vector3 cameraPosition, Vector3 cameraLookAtVector, Vector3 cameraUpVector)
        {
			// The assignment of effect.View and effect.Projection
			// are nearly identical to the code in the Model drawing code.

			effect.TextureEnabled = true;
			effect.Texture = grid;

			effect.View = Matrix.CreateLookAt(
				cameraPosition, cameraLookAtVector, cameraUpVector);

			float aspectRatio =
				graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
			float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
			float nearClipPlane = 1;
			float farClipPlane = 200;

			effect.Projection = Matrix.CreatePerspectiveFieldOfView(
				fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

			Console.WriteLine(NormalBox);

			DrawFace(new Vector3(0, 0, 0), new Vector3(0, -10, 0) * NormalBox, new Vector3(-10, 0, 0) * NormalBox, new Vector3(-10, -10, 0) * NormalBox); //floor xy
			DrawFace(new Vector3(0, -10, 10) * NormalBox, new Vector3(-10, -10, 10) * NormalBox, new Vector3(0, -10, 0) * NormalBox, new Vector3(-10, -10, 0) * NormalBox);   //wall xz
			DrawFace(new Vector3(0, 0, 10) * NormalBox, new Vector3(0, -10, 10) * NormalBox, new Vector3(0, 0, 0), new Vector3(0, -10, 0) * NormalBox);   //wall yz
		}

		protected void DrawFace(Vector3 c1, Vector3 c2, Vector3 c3, Vector3 c4)
		{
			VertexPositionTexture[] vertex = new VertexPositionTexture[6];

			vertex[0].Position = c1;
			vertex[1].Position = c2;
			vertex[2].Position = c3;
			vertex[3].Position = vertex[1].Position;
			vertex[4].Position = c4;
			vertex[5].Position = vertex[2].Position;

			vertex[0].TextureCoordinate = new Vector2(0, 0);
			vertex[1].TextureCoordinate = new Vector2(0, 1);
			vertex[2].TextureCoordinate = new Vector2(1, 0);

			vertex[3].TextureCoordinate = vertex[1].TextureCoordinate;
			vertex[4].TextureCoordinate = new Vector2(1, 1);
			vertex[5].TextureCoordinate = vertex[2].TextureCoordinate;

			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();
				graphics.GraphicsDevice.DrawUserPrimitives(
					// We’ll be rendering two trinalges
					PrimitiveType.TriangleList,
					// The array of verts that we want to render
					vertex,
					// The offset, which is 0 since we want to start
					// at the beginning of the floorVerts array
					0,
					// The number of triangles to draw
					2);
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

			
			DrawParticles(path);


			spriteBatch.End();

			// Drop the render target
			GraphicsDevice.SetRenderTarget(null);
		}

        private void DrawParticles(string path)
        {
			StreamReader reader = new StreamReader(path);
			string line;
			Particle p = new Particle();
			//Color c = Color.Black;
			//double x = 5, y = 5, z = 5, size = 0.1;
			
			do
			{
				line = reader.ReadLine();
				p = Particle.FromString(line);
				
				if(p.x >= 0 && p.x <= Config.SimulationBoxSize.X)
                {
					if(p.y >= 0 && p.y <= Config.SimulationBoxSize.Y)
                    {
						if (p.z >= 0 && p.z <= Config.SimulationBoxSize.Z)
						{
							/*double scale = NormalBox.X / Config.SimulationBoxSize.X;
							x *= -scale;
							y *= -scale;
							z *= scale;*/

							p.Draw3D(effect, graphics, NormalBox);

							//Console.WriteLine(p.x+"  "+p.y+"   "+p.z);
						}
                    }
                }

			} while (!reader.EndOfStream);
		}

        public void BakeAnimation()
		{
			LoadContent();
			Initialize();
			//Creating PNG frame images

			for (int i = 0; i < Files.Count(); i++)
			{
				SaveFrame(renderTarget, Files.ElementAt(i), "frame" + i + ".png");
				Console.WriteLine(Files.ElementAt(i));
			}

			string filename = "ffmpeg.exe";
			var proc = System.Diagnostics.Process.Start(filename, @" -y -r " + Config.OutputAnimationFramerate + " -start_number 0 -i " + Config.OutputPath + "\\frame%d.png" + @" -pix_fmt rgba " + Config.OutputPath + "\\out.mp4");
			/*
			 * -y means overwrite if such video already exists.
			 * -r stands for framerate
			 * -start_number wiadomo
			 * -i path to png's
			 * -pix_fmt pixel format
			 * */
		}

		private void SaveFrame(RenderTarget2D r, string path, string name)
		{
			GraphicsDevice.Clear(Color.White);

			DrawSceneToTexture(renderTarget, path);

			Stream stream = File.Create(Config.OutputPath + "\\" + name);

			//Save as PNG
			renderTarget.SaveAsPng(stream, 1920, 1080);
			stream.Dispose();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.LightGray);
			KeyboardState state = Keyboard.GetState();

			if (state.IsKeyDown(Keys.Right))
				cameraPosition.X += 0.1f;
			if (state.IsKeyDown(Keys.Left))
				cameraPosition.X -= 0.1f;
			if (state.IsKeyDown(Keys.Up))
				cameraPosition.Y -= 0.1f;
			if (state.IsKeyDown(Keys.Down))
				cameraPosition.Y += 0.1f;
			if (state.IsKeyDown(Keys.W))
				cameraPosition.Z -= 0.1f;
			if (state.IsKeyDown(Keys.S))
				cameraPosition.Z += 0.1f;

			spriteBatch.Begin();

			spriteBatch.Draw(grid, Vector2.Zero, Color.White);

			DrawBox(cameraPosition, cameraLookAtVector, cameraUpVector);

			DrawParticles(@"C:\Development\Simulatica\Simulatica\CalcEngine\bin\Debug\netcoreapp3.1\Result\T=2.txt");

			spriteBatch.End();
		}
	}
}