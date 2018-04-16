using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TaxiOrb.GameState
{
    class EndState : GameState
    {
        protected EndState(Game game) : base(game)
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
       
            spriteBatch.Begin();

            spriteBatch.Draw(Resources.backround, new Rectangle(0,0,720,1280), Color.White);

            spriteBatch.End();
           
                
                
         }

      /*  public override void Update(GameTime gameTime)
        {
         
        } */
    }
}
