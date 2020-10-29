using System;
using System.IO;
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

		//int MaximumX = 1920, MaximumY = 1080; 
		//int X, Y;

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

			IsMouseVisible = true;
			Window.AllowUserResizing = true;
		}

		protected override void LoadContent()
		{
			spriteBatch = new SpriteBatch(GraphicsDevice);


			Grid = Content.Load<Texture2D>("Grid");
			pixel = Content.Load<Texture2D>("pixel");

			base.LoadContent();
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

			GraphicsDevice.Clear(Color.CornflowerBlue);

			spriteBatch.Begin();

			DrawGrid(spriteBatch, new Rectangle(200, 200, 800, 533));
			//spriteBatch.Draw(pixel, Vector2.Zero, Color.Red);

			spriteBatch.End();
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
		/*
		private void CalcWindowSize()
        {
			float min, max;

			if(Config.SimulationBoxSize.X / Config.SimulationBoxSize.Y < 1)
			{
				max = Config.SimulationBoxSize.Y;
				min = Config.SimulationBoxSize.X;
            }
            else
			{
				min = Config.SimulationBoxSize.Y;
				max = Config.SimulationBoxSize.X;
			}

			if(MaximumX / (float)MaximumY >= 1)
            {
				
            }
        }*/
	}
}