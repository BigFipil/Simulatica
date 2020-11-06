using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualizer
{
	public class Visual3D : Game
	{
		SpriteBatch spriteBatch;
		Texture2D grid;
		AnimationConfig Config;
		Color c = Color.White;
        private RenderTarget2D renderTarget;

		VertexPositionTexture[] floorVerts;
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

			base.LoadContent();
		}

        protected override void Initialize()
        {
			floorVerts = new VertexPositionTexture[6];
			floorVerts[0].Position = new Vector3(-20, -20, 0);
			floorVerts[1].Position = new Vector3(-20, 20, 0);
			floorVerts[2].Position = new Vector3(20, -20, 0);
			floorVerts[3].Position = floorVerts[1].Position;
			floorVerts[4].Position = new Vector3(20, 20, 0);
			floorVerts[5].Position = floorVerts[2].Position;

			//Textures Coordinates
			floorVerts[0].TextureCoordinate = new Vector2(0, 0);
			floorVerts[1].TextureCoordinate = new Vector2(0, 1);
			floorVerts[2].TextureCoordinate = new Vector2(1, 0);

			floorVerts[3].TextureCoordinate = floorVerts[1].TextureCoordinate;
			floorVerts[4].TextureCoordinate = new Vector2(1, 1);
			floorVerts[5].TextureCoordinate = floorVerts[2].TextureCoordinate;

			effect = new BasicEffect(graphics.GraphicsDevice);

			base.Initialize();
        }

		public void DrawBox()
        {
			// The assignment of effect.View and effect.Projection
			// are nearly identical to the code in the Model drawing code.
			var cameraPosition = new Vector3(0, 40, 20);
			var cameraLookAtVector = Vector3.Zero;
			var cameraUpVector = Vector3.UnitZ;

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

			foreach (var pass in effect.CurrentTechnique.Passes)
			{
				pass.Apply();

				graphics.GraphicsDevice.DrawUserPrimitives(
					// We’ll be rendering two trinalges
					PrimitiveType.TriangleList,
					// The array of verts that we want to render
					floorVerts,
					// The offset, which is 0 since we want to start
					// at the beginning of the floorVerts array
					0,
					// The number of triangles to draw
					2);
			}
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			spriteBatch.Draw(grid, Vector2.Zero, Color.White);

			DrawBox();

			spriteBatch.End();
		}
	}
}