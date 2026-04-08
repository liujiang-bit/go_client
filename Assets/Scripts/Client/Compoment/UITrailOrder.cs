using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace Client
{
    public class UITrailOrder : MonoBehaviour
    {
        private void Awake()
        {
        }
        private void Start()
        {
            Canvas canvas = GetComponentInParent<Canvas>();
            if(canvas != null)
            {
                var particles = GetComponentsInChildren<TrailRenderer>(true);
                foreach (var p in particles)
                {
                    var render = p.GetComponentInChildren<Renderer>();
                    if (render != null)
                    {
                        render.sortingOrder = canvas.sortingOrder + 1;
                    }
                }
            }
        }
    }
}