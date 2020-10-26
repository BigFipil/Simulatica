using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualizer
{
	public class App : Game
	{
		SpriteBatch spriteBatch;
		Texture2D texture2D;

		public App()
		{
			string path = Assembly.GetEntryAssembly().Location;
			path = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\"));
			path = Path.Combine(path, @"Content\bin");

			Content.RootDirectory = path;

			var graphics = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = 1920,
				PreferredBackBufferHeight = 1080,
			};
			graphics.ApplyChanges();
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);

			texture2D = Content.Load<Texture2D>("Grid");

			base.LoadContent();
		}

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			spriteBatch.Draw(texture2D, Vector2.Zero, Color.White);

			spriteBatch.End();
		}
	}
}