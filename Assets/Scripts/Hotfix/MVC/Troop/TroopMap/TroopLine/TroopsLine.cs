using System.Collections.Generic;
using System.Text;
using Client;
using Game;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public class TroopLineInfo
    {
        private MarchLine m_line = null;

        public enum LinePart
        {
            LineStart,
            LineEnd,
        }

        public bool bIsOwner;
        public bool IsActive { get; private set; } = true;

        private enum LoadStatus
        {
            None = 0,
            Loading = 1,
            Loaded = 2,
            Destroy = 3,
        }

        private LoadStatus m_status = LoadStatus.None;
        private Vector2[] m_pathPos = null;
        private Dictionary<LinePart, GameObject> m_linePart = new Dictionary<LinePart, GameObject>();
        private Color m_lineColor;
        private bool m_isSetLineColor = false;

        public void LoadTroopLine(bool isFade, string res_path = "troop_line_mine")
        {
            m_status = LoadStatus.Loading;
            MarchLineMgr.Instance().CreateTroopLine((troopLine) =>
            {
                m_line = troopLine;
                if(m_status == LoadStatus.Destroy)
                {
                    RemoveLine();
                    return;
                }
                m_status = LoadStatus.Loaded;
                if(isFade)
                {
                    m_line.Fade(false);
                }
                if(m_isSetLineColor)
                {
                    ChangeLineColor(m_lineColor);
                }
                SetTroopLinePath(m_pathPos);
                LoadLinePart(LinePart.LineStart);
                LoadLinePart(LinePart.LineEnd);
                m_line.gameObject.SetActive(IsActive);
            }, res_path);
        }

        private void LoadLinePart(LinePart linePart)
        {
            string prefabName = GetLinePartPrefabName(linePart);
            if (string.IsNullOrEmpty(prefabName)) return;
            CoreUtils.assetService.Instantiate(prefabName, (go) =>
            {
                if (m_status == LoadStatus.Destroy)
                {
                    CoreUtils.assetService.Destroy(go);
                    return;
                }
                m_linePart[linePart] = go;
                go.transform.SetParent(m_line.transform);
                Vector3 pos;
                if (GetLinePartPosition(linePart, out pos))
                {
                    go.transform.position = pos;
                }
                if(m_isSetLineColor)
                {
                    ChangeLinePartColor(linePart, m_lineColor);
                }               
            });
        }

        private string GetLinePartPrefabName(LinePart linePart)
        {
            string prefabName = string.Empty;
            switch (linePart)
            {
                case LinePart.LineStart:
                    prefabName = "troop_line_mine_start";
                    break;
                case LinePart.LineEnd:
                    prefabName = "troop_line_mine_end";
                    break;
            }
            return prefabName;
        }

        private bool GetLinePartPosition(LinePart linePart, out Vector3 pos)
        {
            pos = Vector3.zero;
            if (m_pathPos == null || m_pathPos.Length < 2) return false;
            switch(linePart)
            {
                case LinePart.LineStart:
                    pos = new Vector3(m_pathPos[0].x, 0, m_pathPos[0].y);
                    break;
                case LinePart.LineEnd:
                    pos = new Vector3(m_pathPos[m_pathPos.Length -1].x, 0, m_pathPos[m_pathPos.Length - 1].y);
                    break;
            }
            return true;
        }

        public void SetTroopLinePath(Vector2[] path)
        {
            if (path == null || path.Length < 0) return;
            m_pathPos = path;
            if(m_status == LoadStatus.Loaded && m_line != null)
            {
                MarchLineMgr.Instance().SetTroopLinePath(m_line, m_pathPos);
            }
            foreach(var kv in m_linePart)
            {
                Vector3 pos;
                if(GetLinePartPosition(kv.Key, out pos))
                {
                    kv.Value.transform.position = pos;
                }
            }
        }

        private void ChangeLinePartColor(LinePart linePart, Color color)
        {
            GameObject go = null;
            if (!m_linePart.TryGetValue(linePart, out go)) return;
            if (go == null) return;
            var helper = go.GetComponent<ChangeSpriteColor>();
            if (helper)
            {
                ChangeSpriteColor.SetColor(helper, color);
            }
        }

        public void ChangeLineColor(Color color)
        {
            m_isSetLineColor = true;
            m_lineColor = color;
            if(m_line != null)
            {
                MarchLineMgr.Instance().SetTroopLineColor(m_line, color);
            }
            foreach (var kv in m_linePart)
            {
                ChangeLinePartColor(kv.Key, color);
            }
        }
        public void SetActive(bool active)
        {
            IsActive = active;
            if (m_line != null)
            {
                m_line.gameObject.SetActive(active);
            }
        }
        public void Destroy()
        {
            m_status = LoadStatus.Destroy;
            RemoveLine();
        }

        private void RemoveLine()
        {
            if (m_line != null)
            {
                MarchLineMgr.Instance().DestroyTroopLine(m_line);
                m_line = null;
            }
        }
    }

    public sealed class TroopsLine : ITroopLine
    {
        private const string res_path = "troop_line_send_troop";
        private const string line_mine_end = "troop_line_mine_end";
        private TroopProxy m_TroopProxy;
        private RallyTroopsProxy m_RallyTroopsProxy;
        private WorldMapObjectProxy m_WorldMapObjectProxy;
        private PlayerProxy m_playerProxy;
        private readonly Dictionary<int, TroopLineInfo> dicTroopLines = new Dictionary<int, TroopLineInfo>();
        private readonly Dictionary<int, MarchLine> dicTroopLinesAOI= new  Dictionary<int, MarchLine>();
        private readonly Dictionary<int, GameObject> dicTroopLinesAOIEnd= new  Dictionary<int, GameObject>();

        public TroopsLine()
        {
            m_TroopProxy = AppFacade.GetInstance().RetrieveProxy(TroopProxy.ProxyNAME) as TroopProxy;
            m_RallyTroopsProxy= AppFacade.GetInstance().RetrieveProxy(RallyTroopsProxy.ProxyNAME) as RallyTroopsProxy;
            m_WorldMapObjectProxy= AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
        }

        public Color GetLineColor(int id)
        {
            Color color = Color.white;  
            ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
            if (armyData == null)
            {
                return color;
            }

            if (armyData.isPlayerHave&& !armyData.isRally)
            {
                return RS.green;
            }
            
            if(m_playerProxy.CurrentRoleInfo.guildId != 0 && armyData.guild == m_playerProxy.CurrentRoleInfo.guildId&&!armyData.isRally)
            {
                color = RS.blue;
                return color;
            }
            
            
            if (armyData.isRally)
            {
                if (m_RallyTroopsProxy.HasRallyed(armyData.targetId))
                {
                    color = RS.red;
                }
                else if (m_RallyTroopsProxy.IsCaptainByarmIndex(armyData.troopId))
                {
                    color = RS.green;
                    
                }else if (m_RallyTroopsProxy.isRallyTroopHaveGuid(armyData.armyRid))
                {
                    color = RS.blue;

                }

                return color;
            }

            MapObjectInfoEntity infoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(armyData.targetId);
            if (infoEntity == null)
            {
                return color;
            }

            RoleInfoEntity role = m_playerProxy.CurrentRoleInfo;
            if ((role.guildId != 0 && infoEntity.guildId != 0 && role.guildId == infoEntity.guildId))
            {
                color = RS.red;
            }
            else if (infoEntity.collectRid == role.rid)
            {
                color = RS.red;
            }
            else if (infoEntity.cityRid == role.rid)
            {
                color = RS.red;
            }
            else if (infoEntity.armyRid == role.rid)
            {
                color = RS.red;
            }                   
            return color;
        }

        public void AddSummonerTroopLine(int lineId, List<Vector2> path)
        {
            AddTroopLine(lineId, path, RS.green);
        }

        public void RemoveSummonerTroopLine(int lineId)
        {
            this.RemoveLine(lineId);
        }

        public void AddTroopLine(int lineId, List<Vector2> paths, Color color)
        {
            if (lineId == 0)
            {
                Debug.LogError("行军线id等于0了");
                return;
            }

            if (Application.isEditor)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine();
                foreach (var pos in paths)
                {
                    sb.Append("\t");
                    sb.Append(pos.ToString());
                }
                Color color2;
                ColorUtility.TryParseHtmlString("#" + (Time.frameCount % 255 * 12354687).ToString("X"), out color2);
                CoreUtils.logService.Debug($"{lineId}\tBattleData: addLine:{sb.ToString()}", color2);
            }

            TroopLineInfo lineInfo;
            if (!dicTroopLines.TryGetValue(lineId, out lineInfo))
            {
                //创建线路
                lineInfo = new TroopLineInfo();
                lineInfo.LoadTroopLine(true, "troop_line_send_troop");
                lineInfo.bIsOwner = m_TroopProxy.IsPlayOwnTroop(lineId);
                dicTroopLines.Add(lineId, lineInfo);
            }
            //更新线路
            lineInfo.SetTroopLinePath(paths.ToArray());
            lineInfo.ChangeLineColor(color);
        }

        public void UpdateLineColor(int id)
        {
            TroopLineInfo troopLineInfo;
            if (dicTroopLines.TryGetValue(id, out troopLineInfo))
            {
                Color color = GetLineColor(id);
                troopLineInfo.ChangeLineColor(color);
            }
        }
        public void UpdateAllLineColor()
        {
            foreach (var item in this.dicTroopLines)
            {
                Color color = GetLineColor(item.Key);
                item.Value.ChangeLineColor(color);
            }
        }

        public void UpdateTroopLine(MapViewLevel level)
        {
            foreach (var item in this.dicTroopLines.Values)
            {
                item.SetActive(item.bIsOwner || level <= MapViewLevel.Strategic);
            }
        }

        public void RemoveTroopLine(int id)
        {
            if(GameModeManager.Instance.CurGameMode == GameModeType.World)
            {
                // 自己的部队等待ui同步状态数据来删除
                ArmyData armyData =  WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                if (armyData != null)
                {
                    if (armyData.isPlayerHave&& !armyData.isRally)
                    {
                        return;
                    }
                }              
            }

            RemoveLine(id);
        }

        public void RemoveTroopLines()
        {
            if (this.dicTroopLines != null)
            {
                foreach (var item in this.dicTroopLines)
                {
                    item.Value.Destroy();
                    
                }
                dicTroopLines.Clear();
            }

            if (dicTroopLinesAOI != null)
            {
                foreach (var item in this.dicTroopLinesAOI.Values)
                {
                    CoreUtils.assetService.Destroy(item.gameObject);
                }
                dicTroopLinesAOI.Clear();
            }
        }

        public void SetAoiTroopLines(int id,List<Vector2> path,Color color)
        {
            MarchLine troopLines;
            if (dicTroopLinesAOI.TryGetValue(id, out troopLines))
            {
                MarchLineMgr.Instance().SetTroopLinePath(troopLines, path.ToArray());
                MarchLineMgr.Instance().SetTroopLineColor(troopLines,color);
                UpdateAOIEndPos(id, path);
                return;
            }

            MarchLineMgr.Instance().CreateTroopLine((troopLine) =>
            {
                MapObjectInfoEntity mapObjectInfoEntity = m_WorldMapObjectProxy.GetWorldMapObjectByobjectId(id);
                if (mapObjectInfoEntity != null)
                {
                    CoreUtils.assetService.Destroy(troopLine.gameObject);
                    return;
                }
                
                troopLine.gameObject.name = string.Format("{0}_{1}", "AOITroopLine_",id);
                MarchLineMgr.Instance().SetTroopLinePath(troopLine, path.ToArray());
                MarchLineMgr.Instance().SetTroopLineColor(troopLine,color);
                LoadAoiLinePart(id,troopLine.gameObject.transform,path,color);
                if (!dicTroopLinesAOI.ContainsKey(id))
                {
                    dicTroopLinesAOI.Add(id, troopLine);
                }
            }, res_path);
        }

        public void RemoveAoiTroopLines(int id)
        {
            MarchLine go;
            if (dicTroopLinesAOI.TryGetValue(id, out go))
            {
                CoreUtils.assetService.Destroy(go.gameObject);
            }

            dicTroopLinesAOI.Remove(id);
            
            GameObject goEnd;
            if (dicTroopLinesAOIEnd.TryGetValue(id, out goEnd))
            {
                if (goEnd != null)
                {                  
                    CoreUtils.assetService.Destroy(goEnd);
                }
                dicTroopLinesAOIEnd.Remove(id);
            }
           // Debug.LogError("删除aoi线"+dicTroopLinesAOI.Count);
        }

        public void UpdateAoiTroopLinesColor(int id,Color color)
        {
            MarchLine go;
            if (dicTroopLinesAOI.TryGetValue(id, out go))
            {
                go.SetColor(color);
            }

            GameObject goEnd;
            if (dicTroopLinesAOIEnd.TryGetValue(id, out goEnd))
            {
                if (goEnd != null)
                {
                    var helper = goEnd.GetComponent<ChangeSpriteColor>();
                    if (helper)
                    {
                        ChangeSpriteColor.SetColor(helper, color);
                    }  
                }
            }
        }

        private void LoadAoiLinePart(int id, Transform parent,List<Vector2> path, Color color)
        {
            if (dicTroopLinesAOI.ContainsKey(id))
            {
               return;
            }
            CoreUtils.assetService.Instantiate(line_mine_end, (go) =>
            {
                go.transform.SetParent(parent.transform);
                go.transform.position =  new Vector3(path[path.Count -1].x, 0, path[path.Count - 1].y);
                var helper = go.GetComponent<ChangeSpriteColor>();
                if (helper)
                {
                    ChangeSpriteColor.SetColor(helper, color);
                }

                if (go != null)
                {
                    if (!dicTroopLinesAOIEnd.ContainsKey(id))
                    {
                        dicTroopLinesAOIEnd.Add(id,go);
                    }
                }
            });
        }

        public void RemoveLine(int id)
        {
            TroopLineInfo troopInfo;
            if (dicTroopLines.TryGetValue(id, out troopInfo))
            {
                troopInfo.Destroy();
                dicTroopLines.Remove(id);
            }
        }

        private void UpdateAOIEndPos(int id, List<Vector2> path)
        {
            if (path == null)
            {
                return;
            }

            if (path.Count <= 0)
            {
                return;
            }

            GameObject go;
            if (dicTroopLinesAOIEnd.TryGetValue(id, out go))
            {
                if (go != null)
                {
                    go.transform.position = new Vector3(path[path.Count -1].x, 0, path[path.Count - 1].y);  
                }
            }
        }
    }
}