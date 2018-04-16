namespace TaxiOrb
{
	using Microsoft.Xna.Framework.Graphics;

	public static class Resources
	{
		//Load at startup
		public static Texture2D Pixel { get; set; }
		public static SpriteFont Font { get; set; }

        public static Model taxiOrb;
        public static Model collectorOrb;
        public static Texture2D backround;

        //Load at init
        public static Texture2D AcaLogo { get; set; }
	}
}
