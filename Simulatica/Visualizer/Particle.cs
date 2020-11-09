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
		public static Texture2D texture, currentTexture;

        public float x, y, z, size = 20;
		public float alpha = 1;
        public Color color;
        public int type;

        public static Particle FromString(string line)
        {
			Particle p = new Particle();
			line = line.Replace("[", "");
			line = line.Replace("]", "");
			line = line.Replace(",", ":");

			var str = line.Split(":");

			currentTexture = texture;

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
								if (o.Value.ToLower() == "1") currentTexture = texture;
								else currentTexture = texture;
								break;
						}
					}
					//color adjustment
					Color[] oryginalData = new Color[currentTexture.Width * currentTexture.Height];
					Color[] Data = new Color[currentTexture.Width * currentTexture.Height];
					Color c = p.color;
					Console.WriteLine("  "+c);
					//Color c = new Color(Vector4.Normalize(new Vector4(p.color.R, p.color.G, p.color.B, p.alpha)));
					texture.GetData<Color>(oryginalData);
					for(int i = 0; i < currentTexture.Width * currentTexture.Height; i++)
                    {
						Data[i] = new Color(oryginalData[i].ToVector4() * c.ToVector4());
						Console.WriteLine(Data[i]);
					}
					currentTexture.SetData<Color>(Data);

					break;
				}
			}
			return p;
		}

		public static void Load(ContentManager Content, AnimationConfig config)
        {
			Config = config;

			texture = Content.Load<Texture2D>("dot");
		}

		public void Draw2D(SpriteBatch batch)
		{
			batch.Draw(texture, new Rectangle((int)(x - size / 2), (int)(y - size / 2), (int)size, (int)size), color * alpha);
		}
		public void Draw3D(BasicEffect effect, GraphicsDeviceManager graphics, Vector3 NormalBox)
		{

			//Console.WriteLine(x + "  " + y + "  " + z);
			//Console.WriteLine(new Vector3(x, y, z) * NormalBox + new Vector3(-1, 0, 1) * (size / 100));
			effect.Texture = currentTexture;

			x *= -1;
			y *= -1;

			VertexPositionTexture[] vertex = new VertexPositionTexture[6];

			vertex[0].Position = new Vector3(x, y, z) * NormalBox + new Vector3(-1, 0, -1) * (size/100);
			vertex[2].Position = new Vector3(x, y, z) * NormalBox + new Vector3(-1, 0, 1) * (size / 100);
			vertex[1].Position = new Vector3(x, y, z) * NormalBox + new Vector3(1, 0, -1) * (size / 100);
			vertex[3].Position = vertex[1].Position;
			vertex[4].Position = new Vector3(x, y, z) * NormalBox + new Vector3(1, 0, 1) * (size / 100);
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
	}
}
