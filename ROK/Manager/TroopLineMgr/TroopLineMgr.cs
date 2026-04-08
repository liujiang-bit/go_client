using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ROK
{
    public class TroopLineMgr
    {
        private static readonly TroopLineMgr m_instance = new TroopLineMgr();

        private List<TroopLine> m_lst_troop_line = new List<TroopLine>();

        private float m_troop_line_off_set;

        public static TroopLineMgr GetInstance()
        {
            return TroopLineMgr.m_instance;
        }

        public void Reset()
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

        public void SetTroopLinePath(TroopLine troop_line, Vector2[] path, GameObject troop, Action<TroopLine> action, string res_path = "troop/troop_line_mine")
        {
            path = Common.SmoothTroopLine(path, 0.5f);
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = new Vector3(path[i].x, -this.m_troop_line_off_set, path[i].y + this.m_troop_line_off_set);
            }
            TroopLine troopLine = troop_line;
            if (!this.m_lst_troop_line.Contains(troop_line))
            {
                CoreUtils.assetService.Instantiate(res_path, (GameObject gameObject) =>
                {
                    troopLine = gameObject.GetComponent<TroopLine>();
                    this.m_lst_troop_line.Add(troopLine);
                    troopLine.GetComponent<LineRenderer>().SetVertexCount(array.Length);
                    troopLine.GetComponent<LineRenderer>().SetPositions(array);
                    troopLine.Fade(false);
                    action?.Invoke(troopLine);
                });
                return;
            }
            troopLine.GetComponent<LineRenderer>().positionCount = array.Length;
            troopLine.GetComponent<LineRenderer>().SetPositions(array);
            troopLine.Fade(false);
            action?.Invoke(troopLine);
        }

        public void SetTroopLinePathNoFade(TroopLine troop_line, Vector2[] path, GameObject troop, Action<TroopLine> action, string res_path = "troop/troop_line_mine")
        {
            path = Common.SmoothTroopLine(path, 0.5f);
            Vector3[] array = new Vector3[path.Length];
            for (int i = 0; i < path.Length; i++)
            {
                array[i] = new Vector3(path[i].x, -this.m_troop_line_off_set, path[i].y + this.m_troop_line_off_set);
            }
            TroopLine troopLine = troop_line;
            if (!this.m_lst_troop_line.Contains(troop_line))
            {
                CoreUtils.assetService.Instantiate(res_path, (GameObject gameObject) =>
                {
                    troopLine = gameObject.GetComponent<TroopLine>();
                    this.m_lst_troop_line.Add(troopLine);
                    action?.Invoke(troopLine);
                });
                return;
            }
            troopLine.GetComponent<LineRenderer>().positionCount = array.Length;
            troopLine.GetComponent<LineRenderer>().SetPositions(array);
            action?.Invoke(troopLine);
        }

        public void DestroyTroopLine(TroopLine troop_line)
        {
            troop_line.Fade(true);
            this.m_lst_troop_line.Remove(troop_line);
            CoreUtils.assetService.Destroy(troop_line.gameObject, TroopLine.s_fade_time);
        }

        public void SetTroopLineColor(TroopLine troop_line, Color color)
        {
            troop_line.SetColor(color);
        }
    }
}