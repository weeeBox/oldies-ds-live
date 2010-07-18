using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DuckstazyLive.pills;
using DuckstazyLive.env.particles;
using DuckstazyLive;
using Microsoft.Xna.Framework.Graphics;

namespace DuckstazyLiveXbox.pills
{
    public class PillParticles : IPillListener
    {
        #region IPillListener Members

        public void PillAdded(Pill pill)
        {
            int particlesCount = 20;
            for (int particleIndex = 0; particleIndex < particlesCount; particleIndex++)
            {
                Particles.StartDrop(pill.x, pill.y, Color.Black);
            }
        }

        public void PillRemoved(Pill pill)
        {
            
        }

        #endregion

        private ParticlesManager Particles
        {
            get { return Application.Instance.Particles; }
        }
    }
}