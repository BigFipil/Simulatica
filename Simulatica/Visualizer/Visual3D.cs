using System;
using System.IO;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Visualizer
{
	public class Visual3D : Game
	{
		SpriteBatch spriteBatch;
		Texture2D texture2D;
		AnimationConfig Config;
		Color c = Color.White;

		public Visual3D(AnimationConfig config)
		{
			Config = config;

			string path = Assembly.GetEntryAssembly().Location;
			path = Path.GetFullPath(Path.Combine(path, @"..\..\..\..\"));
			path = Path.Combine(path, @"Content");

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
			Color c;
			var prop = typeof(Color).GetProperty("Red");
			if (prop != null)
			{
				c = (Color)prop.GetValue(null, null);
			}
			else c = Color.Black;
			//Console.WriteLine(c);

			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			spriteBatch.Draw(texture2D, Vector2.Zero, Color.White);

			spriteBatch.End();
		}
	}
}