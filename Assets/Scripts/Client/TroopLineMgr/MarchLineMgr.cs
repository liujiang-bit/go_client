using Skyunion;
using System;
using System.Collections.Generic;
using Client;
using UnityEngine;

namespace Client
{
    public class MarchLineMgr : MonoSingleton<MarchLineMgr>
    {
        private List<MarchLine> m_lst_troop_line = new List<MarchLine>();

        private float m_troop_line_off_set = 0;

        public void Reset()
        {
            this.DestroyAllTroopLine();
        }

        public void Clear()
        {
            this.DestroyAllTroopLine();
        }

        private void DestroyAllTroopLine()
        {
            int count = this.m_lst_troop_line.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.m_lst_troop_line[i] != null && this.m_lst_troop_line[i].gameObject != null)
                {
                    CoreUtils.assetService.Destroy(this.m_lst_troop_line[i].gameObject);
                }
            }

            this.m_lst_troop_line.Clear();
        }
        
        public void CreateTroopLine(Action<MarchLine> action, string res_path = "troop_line_mine")
        {
            CoreUtils.assetService.Instantiate(res_path, (GameObject gameObject) =>
            {
                var troopLine = gameObject.GetComponent<MarchLine>();
                gameObject.transform.SetParent(GetRoot());
                this.m_lst_troop_line.Add(troopLine);
                action?.Invoke(troopLine);
            });
        }

        public void SetTroopLinePath(MarchLine troopLine, Vector2[] path)
        {
            if (troopLine == null || path == null || path.Length < 2) return;
            path = Common.SmoothTroopLine(path, 0.5f);
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = new Vector3(path[i].x, -this.m_troop_line_off_set, path[i].y + this.m_troop_line_off_set);
            }

            troopLine.GetComponent<LineRenderer>().positionCount = array.Length;
            troopLine.GetComponent<LineRenderer>().SetPositions(array);
        }

        public void DestroyTroopLine(MarchLine troop_line)
        {
            if (troop_line != null)
            {
                if (troop_line.gameObject != null)
                {                   
                    CoreUtils.assetService.Destroy(troop_line.gameObject);
                }

                this.m_lst_troop_line.Remove(troop_line);
            }
        }

        public void SetTroopLineColor(MarchLine troop_line, Color color)
        {
            if (troop_line == null) return;
            troop_line.SetColor(color);
        }
        
                
        private Transform m_root;
        private const string m_root_path = "SceneObject/TroopLine_root";
        private Transform GetRoot()
        {
            if (this.m_root == null)
            {
                this.m_root = GameObject.Find(m_root_path).transform;
            }

            return this.m_root;
        }

    }
}