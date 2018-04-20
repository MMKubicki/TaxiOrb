namespace TaxiOrb
{
	using Microsoft.Xna.Framework;

	public static class Util
	{
		public static Color Minus(this Color color1, Color color2)
		{
			return new Color(color1.ToVector4() - color2.ToVector4());
		}
	}
}
