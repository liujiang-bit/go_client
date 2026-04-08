using UnityEngine;

namespace Client
{
    public class TileMapHelper : MonoBehaviour
    {
        [HideInInspector]
        public int tile_row = 40;
        [HideInInspector]
        public int tile_col  = 40;
        [HideInInspector]
        public int tile_size = 180;
        [HideInInspector]
        public Color gridColor  = Color.red;
        public GameObject currentTile;
        void OnDrawGizmos()
        {
            Gizmos.color = gridColor;
            for (int i = 0; i <= tile_row; i++)
            {
                Gizmos.DrawLine(new Vector3(0, 0, i * tile_size), new Vector3(tile_col * tile_size, 0, i * tile_size));
            }
            for (int i = 0; i <= tile_col; i++)
            {
                Gizmos.DrawLine(new Vector3(i * tile_size, 0, 0), new Vector3(i * tile_size, 0, tile_row * tile_size));
            }
        }

        private void OnRenderObject()
        {
        }
    }
}