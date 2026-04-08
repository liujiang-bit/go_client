using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class LevelDetailManager : MonoSingleton<LevelDetailManager>
    {
        private List<Action> _handlers = new List<Action>();
        private float m_previous_lod_Distance = 0f;
        private float m_lod_Distance = 0f;
        private Camera m_lodCamera;

        public void AddHandler(Action handler)
        {
            if (!_handlers.Contains(handler))
            {
                _handlers.Add(handler);
            }
        }

        public void RemoveHandler(Action handler)
        {
            if (_handlers.Contains(handler))
            {
                _handlers.Remove(handler);
            }
        }

        public void Dispath()
        {
            for (int i = 0; i < this._handlers.Count; i++)
            {
                this._handlers[i]();
            }
        }

        public float GetPreviousLodDistance()
        {
            return m_previous_lod_Distance;
        }

        public float GetLodDistance()
        {
            return m_lod_Distance;
        }

        public void UpdateLodDistance(Camera camera, float dist)
        {
            m_lod_Distance = camera.fieldOfView * dist;
            Dispath();
        }

        public void LateUpdate()
        {
            m_previous_lod_Distance = GetLodDistance();
        }
    }
}