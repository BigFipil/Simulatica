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
		Texture2D grid, pixel;
        private SpriteFont basicFont1;

        //Model model;
        AnimationConfig Config;
		Color c = Color.White;
		protected RenderTarget2D renderTarget, XZ, XY, YZ;

		private RenderTarget2D[] LabelX = new RenderTarget2D[10];
		private RenderTarget2D[] LabelY = new RenderTarget2D[10];
		private RenderTarget2D[] LabelZ = new RenderTarget2D[10];

		Vector3 cameraPosition = new Vector3(-8.6f, 8.7f, 5);
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

		public void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			grid = Content.Load<Texture2D>("Grid");
			pixel = Content.Load<Texture2D>("pixel");
			basicFont1 = Content.Load<SpriteFont>("BasicFont");

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

			RenderAllGrid(graphics, NormalBox);

			base.Initialize();
			RenderLabels(graphics, 64, basicFont1);
		}

		public void DrawBox(Vector3 cameraPosition, Vector3 cameraLookAtVector, Vector3 cameraUpVector)
        {
			// The assignment of effect.View and effect.Projection
			// are nearly identical to the code in the Model drawing code.

			effect.TextureEnabled = true;
			//effect.Texture = grid;

			effect.View = Matrix.CreateLookAt(
				cameraPosition, cameraLookAtVector, cameraUpVector);

			float aspectRatio =
				graphics.PreferredBackBufferWidth / (float)graphics.PreferredBackBufferHeight;
			float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
			float nearClipPlane = 1;
			float farClipPlane = 200;

			effect.Projection = Matrix.CreatePerspectiveFieldOfView(
				fieldOfView, aspectRatio, nearClipPlane, farClipPlane);

			//Console.WriteLine(NormalBox);
			effect.Texture = XY;
			DrawFace(new Vector3(0, 0, 0), new Vector3(0, -10, 0) * NormalBox, new Vector3(-10, 0, 0) * NormalBox, new Vector3(-10, -10, 0) * NormalBox); //floor xy
			effect.Texture = XZ;
			DrawFace(new Vector3(0, -10, 10) * NormalBox, new Vector3(-10, -10, 10) * NormalBox, new Vector3(0, -10, 0) * NormalBox, new Vector3(-10, -10, 0) * NormalBox);   //wall xz
			effect.Texture = YZ;
			DrawFace(new Vector3(0, 0, 10) * NormalBox, new Vector3(0, -10, 10) * NormalBox, new Vector3(0, 0, 0), new Vector3(0, -10, 0) * NormalBox);   //wall yz

			DrawLabels();
		}
		private void DrawLabels()
        {
			for(int i = 0; i < 10; i++)
			{
				effect.Texture = LabelX[i];
				DrawFace(new Vector3((i + 1), -0.35f, -0.35f) * NegateNormalBox, null, 25, new Vector2(1, 1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0));
				effect.Texture = LabelY[i];
				DrawFace(new Vector3(10.55f, (i + 1)-0.15f, 0f) * NegateNormalBox, null, 25, new Vector2(1, 1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0));
				effect.Texture = LabelZ[i];
				DrawFace(new Vector3(-0.35f, -0.35f, (i+1)) * NegateNormalBox, null, 25, new Vector2(1, 1), new Vector2(0, 1), new Vector2(1, 0), new Vector2(0, 0));
				//DrawFace(new Vector3(0, 0, 0), new Vector3(0, -2, 0) * NormalBox, new Vector3(-2, 0, 0) * NormalBox, new Vector3(-2, -2, 0) * NormalBox);
				//Console.WriteLine(new Vector3((i + 1), 0, -0.5f) * NormalBox);
			}
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

		protected void DrawFace(Vector3 center, Vector3? normal, float size)
		{
			VertexPositionTexture[] vertex = new VertexPositionTexture[6];

			Vector3 c1 = Vector3.Zero, c2 = Vector3.Zero, c3 = Vector3.Zero, c4 = Vector3.Zero;
			if(normal == null)
			{
				c1 = center + new Vector3(-1, 0, -1) * (size / 100);
				c3 = center + new Vector3(-1, 0, 1) * (size / 100);
				c2 = center + new Vector3(1, 0, -1) * (size / 100);
				c4 = center + new Vector3(1, 0, 1) * (size / 100);
				//Console.WriteLine("yeas");
			}

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


		protected void DrawFace(Vector3 center, Vector3? normal, float size, Vector2 t1, Vector2 t2, Vector2 t3, Vector2 t4)
		{
			VertexPositionTexture[] vertex = new VertexPositionTexture[6];

			Vector3 c1 = Vector3.Zero, c2 = Vector3.Zero, c3 = Vector3.Zero, c4 = Vector3.Zero;
			if (normal == null)
			{
				c1 = center + new Vector3(-1, 0, -1) * (size / 100);
				c3 = center + new Vector3(-1, 0, 1) * (size / 100);
				c2 = center + new Vector3(1, 0, -1) * (size / 100);
				c4 = center + new Vector3(1, 0, 1) * (size / 100);
				//Console.WriteLine("yeas");
			}

			vertex[0].Position = c1;
			vertex[1].Position = c2;
			vertex[2].Position = c3;
			vertex[3].Position = vertex[1].Position;
			vertex[4].Position = c4;
			vertex[5].Position = vertex[2].Position;

			vertex[0].TextureCoordinate = t1;
			vertex[1].TextureCoordinate = t2;
			vertex[2].TextureCoordinate = t3;

			vertex[3].TextureCoordinate = vertex[1].TextureCoordinate;
			vertex[4].TextureCoordinate = t4;
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
			
			// Draw the scene
			GraphicsDevice.Clear(Color.LightGray);

			DrawBox(cameraPosition, cameraLookAtVector, cameraUpVector);
			DrawParticles(path);

			SpriteBatch batch = new SpriteBatch(graphics.GraphicsDevice);
			batch.Begin();
			if(Config.SimulationBoxSize.Z < Math.Max(Config.SimulationBoxSize.X, Config.SimulationBoxSize.Y))
				batch.DrawString(basicFont1, Path.GetFileName(path), new Vector2(960, 70) - basicFont1.MeasureString(Path.GetFileName(path)) / 2, Color.Black);
			else
				batch.DrawString(basicFont1, Path.GetFileName(path), new Vector2(160, 70) - basicFont1.MeasureString(Path.GetFileName(path)) / 2, Color.Black);
			batch.End();
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
			Draw(new GameTime());

			GraphicsDevice.SetRenderTarget(renderTarget);
			GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true, DepthBufferWriteEnable = false };

			for (int i = 0; i < Files.Count(); i++)
			{
				SaveFrame(renderTarget, Files.ElementAt(i), "frame" + i + ".jpg");
				Console.WriteLine(Files.ElementAt(i));
			}

			GraphicsDevice.SetRenderTarget(null);

			string filename = "ffmpeg.exe";
			var proc = System.Diagnostics.Process.Start(filename, @" -y -r " + Config.OutputAnimationFramerate + " -start_number 0 -i " + Config.OutputPath + "\\frame%d.jpg" + @" -pix_fmt rgba " + Config.OutputPath + "\\out.mp4");
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
			//GraphicsDevice.Clear(Color.White);

			DrawSceneToTexture(renderTarget, path);

			Stream stream = File.Create(Config.OutputPath + "\\" + name);

			//Save as PNG
			renderTarget.SaveAsJpeg(stream, 1920, 1080);
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
			spriteBatch.Draw(LabelX[1], new Rectangle(0, 0, 64, 64), Color.White);
			spriteBatch.Draw(LabelY[1], new Rectangle(64, 0, 64, 64), Color.White);
			//spriteBatch.DrawString(basicFont1, "chuj", new Vector2(5, 5), Color.Black);
			try
			{
				DrawParticles(Files.FirstOrDefault());
            }
            catch { }

			spriteBatch.End();
		}

		private void RenderAllGrid(GraphicsDeviceManager graphics, Vector3 NormalBoxSize)
		{
			//Wymiary xz i yz sa przestawione CELOWO
			XY = new RenderTarget2D(graphics.GraphicsDevice, (int)(NormalBoxSize.X * 1600), (int)(NormalBoxSize.Y * 1600));
			XZ = new RenderTarget2D(graphics.GraphicsDevice, (int)(NormalBoxSize.Z * 1600), (int)(NormalBoxSize.X * 1600));
			YZ = new RenderTarget2D(graphics.GraphicsDevice, (int)(NormalBoxSize.Z * 1600), (int)(NormalBoxSize.Y * 1600));

			RenderGrid(graphics, XY);
			RenderGrid(graphics, XZ);
			RenderGrid(graphics, YZ);
		}

		private void RenderGrid(GraphicsDeviceManager graphics, RenderTarget2D rt)
		{
			var batch = new SpriteBatch(graphics.GraphicsDevice);
			Color c = Color.Gray;

			batch.Begin();

			graphics.GraphicsDevice.SetRenderTarget(rt);
			graphics.GraphicsDevice.Clear(Color.White);
			for (int i = 1; i < 10; i++)
			{
				batch.Draw(pixel, new Rectangle((int)(rt.Width / 10f * i - 1), (int)(0), (int)(2), (int)(rt.Height)), c);
				batch.Draw(pixel, new Rectangle((int)(0), (int)(rt.Height / 10f * i - 1), (int)(rt.Width), (int)(2)), c);
			}
			//TEST
			batch.Draw(pixel, new Rectangle(0, rt.Height-4, rt.Width, 4), Color.LightGreen);

			batch.End();
			graphics.GraphicsDevice.SetRenderTarget(null);
		}

		private void RenderLabels(GraphicsDeviceManager graphics, int size, SpriteFont font)
        {
			var batch = new SpriteBatch(graphics.GraphicsDevice);
			
			for (int i = 0; i < 10; i++)
			{
				batch.Begin();
				LabelX[i] = new RenderTarget2D(graphics.GraphicsDevice, size, size);
				graphics.GraphicsDevice.SetRenderTarget(LabelX[i]);
				graphics.GraphicsDevice.Clear(Color.Transparent);
				string t = "" + Math.Round(Config.SimulationBoxSize.X / 10 * (i + 1), 4);
				batch.DrawString(font, t, new Vector2(size, size) / 2 - font.MeasureString(t) / 2, Color.Black);
				batch.End();
			}
			for (int i = 0; i < 10; i++)
			{
				batch.Begin();
				LabelY[i] = new RenderTarget2D(graphics.GraphicsDevice, size, size);
				graphics.GraphicsDevice.SetRenderTarget(LabelY[i]);
				graphics.GraphicsDevice.Clear(Color.Transparent);
				string t = "" + Math.Round(Config.SimulationBoxSize.Y / 10 * (i + 1), 4);
				batch.DrawString(font, t, new Vector2(size, size) / 2 - font.MeasureString(t) / 2, Color.Black);
				batch.End();
			}
			for (int i = 0; i < 10; i++)
			{
				batch.Begin();
				LabelZ[i] = new RenderTarget2D(graphics.GraphicsDevice, size, size);
				graphics.GraphicsDevice.SetRenderTarget(LabelZ[i]);
				graphics.GraphicsDevice.Clear(Color.Transparent);
				string t = "" + Math.Round(Config.SimulationBoxSize.Z / 10 * (i + 1), 4);
				batch.DrawString(font, t, new Vector2(size, size) / 2 - font.MeasureString(t) / 2, Color.Black);
				batch.End();
			}

			
			graphics.GraphicsDevice.SetRenderTarget(null);
		}
	}
}