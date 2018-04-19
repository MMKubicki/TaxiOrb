
namespace TaxiOrb
{
	using System;
	using System.Linq;

	using Microsoft.Xna.Framework;

#if WINDOWS
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main(string[] args)
        {
			var resolution = new Point(1280,720);

	        if (args.Length > 0)
	        {
		        if (args.Contains("--res"))
		        {
			        var position = args.Select((t, i) => new { text = t, index = i }).First(t => t.text.Contains("--res"))
				        .index;

			        var res = args[position + 1].Split('x');
					resolution = new Point(int.Parse(res[0]), int.Parse(res[1]));
		        }
	        }

			using (var game = new GameMain(resolution.X, resolution.Y))
	        {
		        game.Run();
			}
        }
    }
#endif
}
