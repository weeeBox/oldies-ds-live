using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;
using DuckstazyLive.core.graphics;

namespace DuckstazyLive.pills
{
    public class PillsManager : IDisposable
    {
        private int maxPillsCount;
        private int pillsCount;        
        private Pill[] pills;
        private List<PillListener> pillListeners;

        public PillsManager(int maxPillsCount)
        {
            this.maxPillsCount = maxPillsCount;
            pills = new Pill[maxPillsCount];
            for (int pillIndex = 0; pillIndex < maxPillsCount; pillIndex++)
            {
                pills[pillIndex] = new Pill(this);
            }
            pillListeners = new List<PillListener>();
        }

        #region Update

        public void Update(float dt)
        {
            for (int pillIndex = 0; pillIndex < pillsCount; pillIndex++)
            {
                Pill pill = pills[pillIndex];
                pill.Update(dt);
            }
        }               

        #endregion

        #region Lifecycle

        public Pill AddPill(PillType type, float x, float y)
        {
            return AddPill(type, x, y, 0.0f, 0.0f);
        }

        public Pill AddPill(PillType type, float x, float y, float vx, float vy)
        {
            Debug.Assert(pillsCount < maxPillsCount, "Out of pills: " + pillsCount);
            if (pillsCount == maxPillsCount)
            {
                return null;
            }

            Pill pill = pills[pillsCount];
            pill.Init(type, x, y, vx, vy, 0.0f);
            pillsCount++;

            return pill;
        }

        public void RemovePill(int index)
        {
            Debug.Assert(index >= 0 && index < pillsCount, index + "<" + pillsCount);
            pillsCount--;

            Pill pill = pills[index];
            pills[index] = pills[pillsCount];
            pills[pillsCount] = pill;
        }

        public void ClearPills()
        {
            pillsCount = 0;
        }

        #endregion

        #region Listeners
        public void AddPillListener(PillListener listener)
        {
            pillListeners.Add(listener);
        }

        public void RemovePillListener(PillListener listener)
        {
            pillListeners.Remove(listener);
        }

        public void OnPillAdded(Pill pill)
        {
            foreach (PillListener listener in pillListeners)
            {
                listener.PillAdded(pill);
            }
        }

        public void OnPillRemoved(Pill pill)
        {
            foreach (PillListener listener in pillListeners)
            {
                listener.PillRemoved(pill);
            }
        }
        #endregion

        #region Drawing

        public void Draw(SpriteBatch batch)
        {
            for (int pillIndex = 0; pillIndex < pillsCount; pillIndex++)
            {
                pills[pillIndex].Draw(batch);
            }
        }

        #endregion

        #region Cleanup

        public void Dispose()
        {
            pillListeners.Clear();
            pillListeners = null;

            for (int pillIndex = 0; pillIndex < pillsCount; pillIndex++)
            {
                pills[pillIndex].Dispose();
                pills[pillIndex] = null;
            }
            pills = null;
        }

        #endregion
    }
}
