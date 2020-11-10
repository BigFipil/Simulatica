using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Visualizer
{
    public class Particle
    {
        public static AnimationConfig Config;
		public static Texture2D dot, ring, cross, currentTexture;
		public static Dictionary<Color, Texture2D> dotTex;
		public static Dictionary<Color, Texture2D> ringTex;
		public static Dictionary<Color, Texture2D> crossTex;
		private static int textureType = 1;

		public float x, y, z, size = 20;
		public float alpha = 1;
        public Color color;
        //public int type;

        public static Particle FromString(string line)
        {
			Particle p = new Particle();
			line = line.Replace("[", "");
			line = line.Replace("]", "");
			line = line.Replace(",", ":");

			var str = line.Split(":");

			currentTexture = dot;
			textureType = 1;

			foreach (var v in Config.particleBlueprints)
			{
				if (v.Name == str[0])
				{
					foreach (var o in v.outputInformations)
					{
						switch (o.Key.ToLower())
						{
							case "color":
								var prop = typeof(Color).GetProperty(o.Value);
								if (prop != null)
								{
									p.color = (Color)prop.GetValue(null, null);
								}
								else p.color = Color.Black;
								break;

							case "x":
								string tmp = o.Value.Replace("<", "").Replace(">", "");
								int index = Int32.Parse(tmp);

								p.x = (float)double.Parse(str[index]);
								break;

							case "y":
								string tmp2 = o.Value.Replace("<", "").Replace(">", "");
								int index2 = Int32.Parse(tmp2);

								p.y = (float)double.Parse(str[index2]);
								break;

							case "z":
								string tmp3 = o.Value.Replace("<", "").Replace(">", "");
								int index3 = Int32.Parse(tmp3);

								p.z = (float)double.Parse(str[index3]);
								break;

							case "alpha":
								p.alpha = (float)Double.Parse(o.Value);
								break;
							case "alfa":
								p.alpha = (float)Double.Parse(o.Value);
								break;
							case "type":
								if (!o.Value.Contains("<"))
								{
									if (o.Value == "1" || o.Value.ToLower() == "dot") { currentTexture = dot; textureType = 1; }
									else if (o.Value == "2" || o.Value.ToLower() == "ring"){ currentTexture = ring; textureType = 2; }
									else if (o.Value == "3" || o.Value.ToLower() == "cross"){ currentTexture = cross; textureType = 3; }
								}
								break;
							case "size":
								if (!o.Value.Contains("<")) p.size = Int32.Parse(o.Value);
								break;
						}
					}
					

					break;
				}
			}
			return p;
		}

		public static void Load(ContentManager Content, AnimationConfig config, GraphicsDeviceManager graphics = null)
        {
			Config = config;

			dot = Content.Load<Texture2D>("dot");
			ring = Content.Load<Texture2D>("ring");
			cross = Content.Load<Texture2D>("cross");

			if(graphics != null)
			{
				dotTex = new Dictionary<Color, Texture2D>();
				ringTex = new Dictionary<Color, Texture2D>();
				crossTex = new Dictionary<Color, Texture2D>();

				var colorList = new List<Color>() { Color.Black, Color.Red, Color.DarkRed, Color.Pink, Color.Purple,
				Color.Blue, Color.LightBlue, Color.DarkBlue, Color.Green, Color.LightGreen, Color.DarkGreen, Color.LightGray,
				Color.Gray, Color.DarkGray, Color.Yellow, Color.Orange, Color.Brown };

				foreach (var c in colorList)
				{
					dotTex.Add(c, SetTextureColorData(graphics, dot, c));
					ringTex.Add(c, SetTextureColorData(graphics, ring, c));
					crossTex.Add(c, SetTextureColorData(graphics, cross, c));
				}
			}
		}

		public void Draw2D(SpriteBatch batch)
		{
			batch.Draw(currentTexture, new Rectangle((int)(x - size / 2), (int)(y - size / 2), (int)size, (int)size), color * alpha);
		}
		public void Draw3D(BasicEffect effect, GraphicsDeviceManager graphics, Vector3 NormalBox)
		{

            //Console.WriteLine(x + "  " + y + "  " + z);
            //Console.WriteLine(new Vector3(x, y, z) * NormalBox + new Vector3(-1, 0, 1) * (size / 100));
            //effect.Texture = SetTextureColorData(graphics, currentTexture, color);
            if (dotTex.ContainsKey(color))
			{
				if (textureType == 3) effect.Texture = crossTex[color];
				else if (textureType == 2) effect.Texture = ringTex[color];
				else  effect.Texture = dotTex[color];
			}
            else
            {
				effect.Texture = SetTextureColorData(graphics, currentTexture, color);
			}


			x *= -1 * (10 / Config.SimulationBoxSize.X);
			y *= -1 * (10 / Config.SimulationBoxSize.Y);
			z *= 10 / Config.SimulationBoxSize.Z;

			VertexPositionTexture[] vertex = new VertexPositionTexture[6];
			
			vertex[0].Position = new Vector3(x, y, z) * NormalBox + new Vector3(-1, 0, -1) * (size/100);
			vertex[2].Position = new Vector3(x, y, z) * NormalBox + new Vector3(-1, 0, 1) * (size / 100);
			vertex[1].Position = new Vector3(x, y, z) * NormalBox + new Vector3(1, 0, -1) * (size / 100);
			vertex[3].Position = vertex[1].Position;
			vertex[4].Position = new Vector3(x, y, z) * NormalBox + new Vector3(1, 0, 1) * (size / 100);
			vertex[5].Position = vertex[2].Position;

			//Console.WriteLine(vertex[0]);

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

		private static Texture2D SetTextureColorData(GraphicsDeviceManager graphics, Texture2D texture, Color color)
        {
			Texture2D t = new Texture2D(graphics.GraphicsDevice, 32, 32);

			//color adjustment
			Color[] oryginalData = new Color[t.Width * t.Height];
			Color[] Data = new Color[t.Width * t.Height];
			//Color c = color;
			//Console.WriteLine("  " + c);
			//Color c = new Color(Vector4.Normalize(new Vector4(p.color.R, p.color.G, p.color.B, p.alpha)));
			texture.GetData<Color>(oryginalData);
			for (int i = 0; i < t.Width * t.Height; i++)
			{
				Data[i] = new Color(oryginalData[i].ToVector4() * color.ToVector4());
			}
			t.SetData<Color>(Data);

			return t;
        }
	}
}
