using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Framework.core;
using Framework.visual;
using Microsoft.Xna.Framework.Graphics;
using DuckstazyLive.app;
using Microsoft.Xna.Framework;
using DuckstazyLive.game.levels;
using Framework.utils;

namespace DuckstazyLive.game
{
    public class GameWorld : BaseElement
    {
        public GameWorld()            
        {
            CustomGeomerty sky = GeometryFactory.createGradient(Constants.WORLD_VIEW_X, Constants.WORLD_VIEW_Y, Constants.WORLD_VIEW_WIDTH, Constants.WORLD_VIEW_HEIGHT, new Color(63, 181, 242), new Color(217, 240, 254));
            CustomGeomerty ground = GeometryFactory.createGradient(Constants.GROUND_X, Constants.GROUND_Y, Constants.GROUND_WIDTH, Constants.GROUND_HEIGHT, new Color(55, 29, 0), new Color(92, 48, 11));

            Rect levelBounds = new Rect(Constants.WORLD_VIEW_X, Constants.WORLD_VIEW_Y, Constants.WORLD_VIEW_WIDTH, Constants.WORLD_VIEW_HEIGHT);
            Level.levelBounds = levelBounds;

            GlobalPillsPool pool = new GlobalPillsPool(Constants.PILLS_POOL_SIZE);            
            Level.Pool = pool;            

            Hero hero = new Hero();
            Level level = new Level1(hero);            
            level.init();

            Texture2D grassTex = Application.sharedResourceMgr.getTexture(Res.IMG_GRASS);
            TiledImage grass = new TiledImage(grassTex, Constants.GROUND_WIDTH, grassTex.Height);
            grass.x = Constants.GROUND_X;
            grass.y = Constants.GROUND_Y;
            grass.setAlign(BaseElement.ALIGN_MIN, BaseElement.ALIGN_MAX);

            sky.addChild(level);                        
            addChild(sky);
            addChild(ground);
            addChild(grass);            
        }

        public override void draw()
        {
            base.draw();
        }        
    }
}
