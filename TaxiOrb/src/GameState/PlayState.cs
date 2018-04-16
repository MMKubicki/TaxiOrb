namespace TaxiOrb.GameState
{
	using Microsoft.Xna.Framework;
	using Microsoft.Xna.Framework.Graphics;
 

	public class PlayState : GameState
    {
        private readonly GraphicsDeviceManager graphics;
        BasicEffect effect;
        VertexPositionTexture[] floorVerts;

        

        //Copied from Tutorial => NOT ALTERED YET
        private Matrix world = Matrix.CreateTranslation(new Vector3(0, 0, 0));
        private Matrix view = Matrix.CreateLookAt(new Vector3(0, 0, 10), new Vector3(0, 0, 0), Vector3.UnitY);
        private Matrix projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(45), 800f / 480f, 0.1f, 100f);
        
        public PlayState(Game game) : base(game)
		{
            effect = new BasicEffect(game.GraphicsDevice);
            
            floorVerts = new VertexPositionTexture[6];
            floorVerts[0].Position = new Vector3(-20, -20, 0);
            floorVerts[1].Position = new Vector3(-20, 20, 0);
            floorVerts[2].Position = new Vector3(20, -20, 0);
            floorVerts[3].Position = floorVerts[1].Position;
            floorVerts[4].Position = new Vector3(20, 20, 0);
            floorVerts[5].Position = floorVerts[2].Position;
           
    }

		public override void Update(GameTime gameTime)
		{

        }

        //Copied from Tutorial => NOT ALTERED YET
         private void DrawModel(Model taxiOrb, Matrix world, Matrix view, Matrix projection)
         {
             foreach (ModelMesh mesh in taxiOrb.Meshes)
             {
                 foreach (BasicEffect effect in mesh.Effects)
                 {
                    effect.EnableDefaultLighting();

                     effect.World = world;
                     effect.View = view;
                     effect.Projection = projection;
                 }

                 mesh.Draw();
             }
         }
     
 

        void DrawGround()
        {
            // The assignment of effect.View and effect.Projection
            // are nearly identical to the code in the Model drawing code.
            var cameraPosition = new Vector3(15, 10, 10);
            var cameraLookAtVector = Vector3.Zero;
            var cameraUpVector = Vector3.UnitZ;

            effect.View = Matrix.CreateLookAt(
                cameraPosition, cameraLookAtVector, cameraUpVector);

            float aspectRatio = game.GraphicsDevice.Viewport.AspectRatio;
          
            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            float nearClipPlane = 1;
            float farClipPlane = 200;

            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                fieldOfView, aspectRatio, nearClipPlane, farClipPlane);


            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();

                game.GraphicsDevice.DrawUserPrimitives(
                    // We’ll be rendering two trinalges
                    PrimitiveType.TriangleList,
                    // The array of verts that we want to render
                    floorVerts,
                    // The offset, which is 0 since we want to start 
                    // at the beginning of the floorVerts array
                    0,
                    // The number of triangles to draw
                    2);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
		{
			game.GraphicsDevice.Clear(Color.Blue);
           // DrawGround();
            DrawModel(Resources.taxiOrb, world, view, projection);
          DrawModel(Resources.collectorOrb, world, view, projection);
          

        }
	}
}
