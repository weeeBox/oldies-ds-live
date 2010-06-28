using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using DuckstazyLive.app;
using DuckstazyLive.graphics;

namespace DuckstazyLive
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DuckstazyGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Matrix worldMatrix;
        Matrix projectionMatrix;
        Matrix viewMatrix;
        BasicEffect effect;
        
        Background background;
        Hero hero;
        Wave wave;
        
        public DuckstazyGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;            
            graphics.ApplyChanges();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);           

            Application app = new Application(640, 480);
            app.GraphicsDevice = GraphicsDevice;
            app.SpriteBatch = spriteBatch;

            InitializeMatrices();
            InitializeEffect();
            
            background = new Background(Constants.GROUND_HEIGHT);
            hero = new Hero();
            
            float w = app.Width;
            float h = 22.5f;
            float x = 0;
            float y = app.Height - (Constants.GROUND_HEIGHT + h) / 2;
            wave = new Wave(x, y, w, h, 10);                      

            base.Initialize();
        }

        private void InitializeMatrices()
        {
            worldMatrix = Matrix.Identity;
            viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreateOrthographicOffCenter(0, Width, Height, 0, 1.0f, 1000.0f);
        }

        private void InitializeEffect()
        {
            effect = new BasicEffect(GraphicsDevice, null);
            effect.World = worldMatrix;
            effect.View = viewMatrix;
            effect.Projection = projectionMatrix;
            effect.VertexColorEnabled = true;            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Resources.Instance.Init(Content);
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            hero.Update(dt);                       

            base.Update(gameTime);
        }       

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);            
                        
            background.DrawSky(effect);

            spriteBatch.Begin();
            hero.Draw(spriteBatch);
            spriteBatch.End();
            
            background.DrawGround(effect);            

            Effect customEffect = Resources.GetEffect(Res.EFFECT_WAVE);
            customEffect.Parameters["World"].SetValue(worldMatrix);
            customEffect.Parameters["View"].SetValue(viewMatrix);
            customEffect.Parameters["Projection"].SetValue(projectionMatrix);
            customEffect.Parameters["Timer"].SetValue((float)gameTime.TotalGameTime.TotalSeconds);            
            customEffect.Parameters["Amplitude"].SetValue(10.0f);
            customEffect.Parameters["WaveLength"].SetValue(Application.Instance.Width);
            customEffect.Parameters["Omega"].SetValue(4 * MathHelper.Pi);
            customEffect.Parameters["Top"].SetValue(0);
            customEffect.Parameters["Phase"].SetValue(0.3f);
            customEffect.Parameters["Color"].SetValue(new Color(93, 49, 12).ToVector4());
            wave.Draw(customEffect);            
                
            base.Draw(gameTime);
        }

        #region Helpers

        private int Width
        {
            get { return Application.Instance.Width; }
        }

        private int Height
        {
            get { return Application.Instance.Height; }
        }

        #endregion
    }
}
