using System;
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
using DuckstazyLive.core.input;
using DuckstazyLive.env;
using DuckstazyLive.env.particles;
using DuckstazyLive.pills;
using DuckstazyLive.pills.effects;
using DuckstazyLiveXbox.pills;
using DuckstazyLive.game;
using DuckstazyLive.core.graphics;
using DuckstazyLive.debug;
using DuckstazyLive.framework.core;

namespace DuckstazyLive
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class DuckstazyGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;       

        RenderContext renderContext;
        Engine engine;
        FPS fps;
        TimerManager timerManager;
        
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
            int bufferWidth;
            int bufferHeight;
#if XBOX
            bufferWidth = 1920;
            bufferHeight = 1080;            
#else
            bufferWidth = 1280;
            bufferHeight = 720;            
#endif
            graphics.PreferredBackBufferWidth = bufferWidth;
            graphics.PreferredBackBufferHeight = bufferHeight;
            graphics.ApplyChanges();                       

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);           

            Application app = new Application(bufferWidth, bufferHeight);
            app.Init();
            app.GraphicsDevice = GraphicsDevice;
            app.SpriteBatch = spriteBatch;

            Matrix worldMatrix;
            Matrix viewMatrix;
            Matrix projectionMatrix;

            InitializeMatrices(out worldMatrix, out viewMatrix, out projectionMatrix);
            BasicEffect basicEffect = InitializeEffect(ref worldMatrix, ref viewMatrix, ref projectionMatrix);

            renderContext = new RenderContext(spriteBatch, basicEffect);

            Camera camera = new Camera(worldMatrix, viewMatrix, projectionMatrix);
            app.Camera = camera;

            engine = new Engine(0, 0, app.Width, app.Height - Constants.GROUND_HEIGHT);

            Console.WriteLine(app.Width + " " + app.Height);
            GDebug.Init(GraphicsDevice, basicEffect);

            fps = new FPS(0.2f, 20, 20);
            timerManager = new TimerManager(20);
            timerManager.AddTimer(app);
            timerManager.AddTimer(fps);
            timerManager.AddTimer(engine);

            base.Initialize();
        }

        private void InitializeMatrices(out Matrix world, out Matrix view, out Matrix projection)
        {
            world = Matrix.Identity;
            view = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
            projection = Matrix.CreateOrthographicOffCenter(0, Width, Height, 0, 1.0f, 1000.0f);            
        }

        private BasicEffect InitializeEffect(ref Matrix world, ref Matrix view, ref Matrix projection)
        {
            BasicEffect effect = new BasicEffect(GraphicsDevice, null);
            effect.World = world;
            effect.View = view;
            effect.Projection = projection;
            effect.VertexColorEnabled = true;

            return effect;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Resources.Instance.Init(Content);
            engine.LoadContent();
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
            float dt = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            timerManager.Update(dt);            
            
            base.Update(gameTime);
        }       

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.RenderState.MultiSampleAntiAlias = true;

            engine.Draw(renderContext);            

#if DEBUG
            GDebug.Flush();
#endif

            fps.Draw(renderContext);
                
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
