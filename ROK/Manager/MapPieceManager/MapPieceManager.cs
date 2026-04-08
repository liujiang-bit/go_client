using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class MapPieceManager
    {
        private Vector2 m_current_piece_center = Vector2.zero;

        private Vector2 m_current_large_piece_center = Vector2.zero;

        public float m_piece_width = 45f;

        public float m_piece_plane_width = 90f;

        private string m_land_root_path = "/land_root";

        private Transform m_land_root;

        private MapPieceDynamic m_dynamic_piece;

        private Dictionary<int, MapPiece> m_map_piece = new Dictionary<int, MapPiece>();

        private Dictionary<int, MapPieceLarge> m_map_piece_large = new Dictionary<int, MapPieceLarge>();

        private static readonly MapPieceManager m_instance = new MapPieceManager();

        private bool m_force_update_once;

        private List<int> m_to_be_remove_key = new List<int>();

        public bool forceUpdateOnce
        {
            get
            {
                return this.m_force_update_once;
            }
            set
            {
                this.m_force_update_once = value;
            }
        }

        public void Reset()
        {
            this.m_map_piece.Clear();
            this.m_map_piece_large.Clear();
        }

        public static MapPieceManager GetInstance()
        {
            return MapPieceManager.m_instance;
        }

        public void UpdateGrove(float city_pos_x, float city_pos_y, float remove_grove_distance)
        {
            foreach (MapPiece current in this.m_map_piece.Values)
            {
                current.UpdateGrove(city_pos_x, city_pos_y, remove_grove_distance);
            }
        }

        private Transform GetLandRoot()
        {
            if (this.m_land_root == null)
            {
                this.m_land_root = GameObject.Find(this.m_land_root_path).transform;
            }
            return this.m_land_root;
        }

        public void UpdatePiece(Vector2 pos)
        {
            Vector2 centerByPos = Common.GetCenterByPos(pos, this.m_piece_plane_width);
            if (centerByPos != this.m_current_large_piece_center || this.forceUpdateOnce)
            {
                int x = (int)(centerByPos.x / this.m_piece_plane_width);
                int y = (int)(centerByPos.y / this.m_piece_plane_width);
                this.DoUpdateLargePiece(x, y, this.forceUpdateOnce);
                this.m_current_large_piece_center = centerByPos;
            }
            Vector2 centerByPos2 = Common.GetCenterByPos(pos, this.m_piece_width);
            if (centerByPos2 != this.m_current_piece_center || this.forceUpdateOnce)
            {
                int x2 = (int)(centerByPos2.x / this.m_piece_width);
                int y2 = (int)(centerByPos2.y / this.m_piece_width);
                this.DoUpdatePiece(x2, y2, this.forceUpdateOnce);
                this.m_current_piece_center = centerByPos2;
            }
            int currentLodLevel = LodCamera.instance.GetCurrentLodLevel();
            float tile_width = MapDataManager.GetInstance().m_tile_width;
            Vector2 centerByPos3 = Common.GetCenterByPos(pos, tile_width);
            if (centerByPos3 != this.m_current_piece_center || this.forceUpdateOnce)
            {
                int x3 = (int)(centerByPos3.x / tile_width);
                int y3 = (int)(centerByPos3.y / tile_width);
                this.DoUpdateDynamicPiece(x3, y3, this.forceUpdateOnce, currentLodLevel);
                this.m_current_piece_center = centerByPos3;
            }
            if (this.forceUpdateOnce)
            {
                this.forceUpdateOnce = false;
            }
        }

        private void DoUpdatePiece(int x, int y, bool reset_all_tile)
        {
            this.m_to_be_remove_key.Clear();
            foreach (int current in this.m_map_piece.Keys)
            {
                if (reset_all_tile || (current != this.GetPieceKey(x - 1, y - 1) && current != this.GetPieceKey(x, y - 1) && current != this.GetPieceKey(x + 1, y - 1) && current != this.GetPieceKey(x - 1, y) && current != this.GetPieceKey(x, y) && current != this.GetPieceKey(x + 1, y) && current != this.GetPieceKey(x - 1, y + 1) && current != this.GetPieceKey(x, y + 1) && current != this.GetPieceKey(x + 1, y + 1)))
                {
                    this.m_to_be_remove_key.Add(current);
                }
            }
            for (int i = 0; i < this.m_to_be_remove_key.Count; i++)
            {
                int key = this.m_to_be_remove_key[i];
                if (this.m_map_piece[key] != null)
                {
                    this.m_map_piece[key].DestroySelf();
                }
                this.m_map_piece.Remove(key);
            }
            this.CreateNewPiece(x, y);
            this.CreateNewPiece(x - 1, y);
            this.CreateNewPiece(x + 1, y);
            this.CreateNewPiece(x, y + 1);
            this.CreateNewPiece(x, y - 1);
            this.CreateNewPiece(x - 1, y - 1);
            this.CreateNewPiece(x + 1, y - 1);
            this.CreateNewPiece(x - 1, y + 1);
            this.CreateNewPiece(x + 1, y + 1);
        }

        private void DoUpdateDynamicPiece(int x, int y, bool reset_all_tile, int lod)
        {
            if (this.m_dynamic_piece == null)
            {
                this.m_dynamic_piece = new GameObject("dynamic_piece")
                {
                    transform =
                {
                    parent = this.GetLandRoot()
                }
                }.AddComponent<MapPieceDynamic>();
            }
            if (reset_all_tile)
            {
                this.m_dynamic_piece.DestroySelf();
            }
            this.m_dynamic_piece.UpdatePiece(lod);
        }

        private void DoUpdateLargePiece(int x, int y, bool reset_all_tile)
        {
            List<int> list = new List<int>();
            foreach (int current in this.m_map_piece_large.Keys)
            {
                if (reset_all_tile || (current != this.GetPieceKey(x - 1, y - 1) && current != this.GetPieceKey(x, y - 1) && current != this.GetPieceKey(x + 1, y - 1) && current != this.GetPieceKey(x - 1, y) && current != this.GetPieceKey(x, y) && current != this.GetPieceKey(x + 1, y) && current != this.GetPieceKey(x - 1, y + 1) && current != this.GetPieceKey(x, y + 1) && current != this.GetPieceKey(x + 1, y + 1)))
                {
                    list.Add(current);
                }
            }
            for (int i = 0; i < list.Count; i++)
            {
                int key = list[i];
                if (this.m_map_piece_large[key] != null)
                {
                    this.m_map_piece_large[key].DestroySelf();
                }
                this.m_map_piece_large.Remove(key);
            }
            this.CreateNewLargePiece(x, y);
            this.CreateNewLargePiece(x - 1, y);
            this.CreateNewLargePiece(x + 1, y);
            this.CreateNewLargePiece(x, y + 1);
            this.CreateNewLargePiece(x, y - 1);
            this.CreateNewLargePiece(x - 1, y - 1);
            this.CreateNewLargePiece(x + 1, y - 1);
            this.CreateNewLargePiece(x - 1, y + 1);
            this.CreateNewLargePiece(x + 1, y + 1);
        }

        private void CreateNewLargePiece(int x, int y)
        {
            int pieceKey = this.GetPieceKey(x, y);
            float num = this.m_piece_plane_width / 2f;
            if (!this.m_map_piece_large.ContainsKey(pieceKey))
            {
                GameObject gameObject = new GameObject();
                float num2 = this.m_piece_plane_width / 2f;
                gameObject.transform.position = new Vector3((float)x * this.m_piece_plane_width + num2, 0f, (float)y * this.m_piece_plane_width + num2);
                gameObject.transform.SetParent(this.GetLandRoot(), true);
                MapPieceLarge mapPieceLarge = gameObject.AddComponent<MapPieceLarge>();
                this.m_map_piece_large.Add(pieceKey, mapPieceLarge);
                mapPieceLarge.Refresh();
            }
        }

        private void CreateNewPiece(int x, int y)
        {
            int pieceKey = this.GetPieceKey(x, y);
            float num = this.m_piece_width / 2f;
            if (!this.m_map_piece.ContainsKey(pieceKey))
            {
                GameObject gameObject = new GameObject();
                gameObject.transform.position = new Vector3((float)x * this.m_piece_width + num, 0f, (float)y * this.m_piece_width + num);
                gameObject.transform.SetParent(this.GetLandRoot(), true);
                MapPiece mapPiece = gameObject.AddComponent<MapPiece>();
                this.m_map_piece.Add(pieceKey, mapPiece);
                mapPiece.Refresh();
            }
        }

        private int GetPieceKey(int x, int y)
        {
            return x + 1 + (y + 1 << 8);
        }
    }
}