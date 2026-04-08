#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace IG.MapEditor
{
    public class TileMap : MonoBehaviour
    {
        [SerializeField]
        public int tile_row /*{ get; private set; }*/ = 40;
        [SerializeField]
        public int tile_col /*{ get; private set; }*/ = 40;
        [SerializeField]
        public int tile_size /*{ get; private set; }*/ = 180;
        public Color gridColor { get; set; } = Color.red;
        public int tileId { get; set; } = 1;
        public GameObject currentTile;
        public bool showProvince = true;
        private Mesh m_provinceMesh = null;

        public void CreateMap(int w, int h, int size)
        {
            tile_row = h;
            tile_col = w;
            tile_size = size;
            var tiles = GetComponentsInChildren<Tile>();
            foreach (var tile in tiles)
            {
                DestroyImmediate(tile.gameObject);
            }
            for(int i = 0; i < tile_row; i++)
            {
                for (int j = 0; j < tile_col; j++)
                {
                    var tile = new GameObject($"{j}_{i}", typeof(Tile));
                    tile.transform.parent = transform;
                    tile.transform.position = new Vector3(j * tile_size + tile_size / 2, 0, i * tile_size + tile_size / 2);
#if UNITY_EDITOR
                    GameObject obj = PrefabUtility.InstantiatePrefab(currentTile, tile.transform) as GameObject;
                    obj.transform.SetParent(tile.transform);
                    obj.transform.localPosition = Vector3.zero;
#endif
                }
            }
        }

        public void ReplaceTile(int x, int y)
        {
            var tile = transform.Find($"{x}_{y}");
            if(tile.GetChild(0) != null && tile.GetChild(0).name.Equals(currentTile.name))
            {
                return;
            }
            while(tile.childCount > 0)
            {
                DestroyImmediate(tile.GetChild(0).gameObject);
            }
#if UNITY_EDITOR
            GameObject obj = PrefabUtility.InstantiatePrefab(currentTile, tile.transform) as GameObject;
            obj.transform.SetParent(tile.transform);
            obj.transform.localPosition = Vector3.zero;
#endif
        }
        public void RotateTile(int x, int y)
        {
            var tile = transform.Find($"{x}_{y}");
            if (tile)
            {
                tile.transform.Rotate(Vector3.up, 90);
            }
        }

        public void LoadMapData()
        {


        }

        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
        }
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

            if(showProvince)
            {

                //if(m_provinceMesh == null)
                //{
                //    m_provinceMesh = new Mesh();

                //    int nHeight = tile_row * 2 + 1;
                //    int nWidth = tile_col * 2 + 1;
                //    int cellSize = tile_size / 2;

                //    // 生成顶点
                //    var verts = new Vector3[nWidth * nHeight];
                //    for(int i = 0; i < nHeight; i++)
                //    {
                //        for (int j = 0; j < nWidth; j++)
                //        {
                //            int nIndex = i * nWidth + j;
                //            verts[nIndex] = new Vector3(j* cellSize, 0, i * cellSize);
                //        }
                //    }
                //    m_provinceMesh.vertices = verts;
                //    // 生成三角形
                //    var verts = new Vector3[nWidth * nHeight];
                //    for (int i = 0; i < nHeight-1; i++)
                //    {
                //        for (int j = 0; j < nWidth-1; j++)
                //        {
                //            int nIndex = i * nWidth + j;
                //            verts[nIndex] = new Vector3(j * cellSize, 0, i * cellSize);
                //        }
                //    }
                //    m_provinceMesh.vertices = verts;
                //}
            }
        }

        private void OnRenderObject()
        {
        }
    }
}