namespace TaxiOrb
{
	using Microsoft.Xna.Framework.Graphics;

	public static class Resources
	{
		//Load at startup
		public static Texture2D Pixel { get; set; }
		public static SpriteFont Font { get; set; }

		//Load at init
		public static Model TaxiOrb { get; set; }
        public static Texture2D Background { get; set; }
        public static Texture2D AcaLogo { get; set; }
		public static SpriteFont Font22 { get; set; }

	}
}
