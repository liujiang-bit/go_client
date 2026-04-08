// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Thursday, April 04, 2019
// Update Time         :    Thursday, April 04, 2019
// Class Description   :    GMGlobalMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameFramework;
using PureMVC.Patterns;
using PureMVC.Interfaces;
using Skyunion;
using SprotoType;
using Client;
using System;
using Hotfix;
using UnityEngine.EventSystems;
using System.Linq;

namespace Game
{
    public class TroopLineMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "TroopLineMediator";

        private bool m_bRecordInBack = false;
        private GameObject m_root = null;
        private Dictionary<string, List<List<Vector2>>> m_dicMapTroop = new Dictionary<string, List<List<Vector2>>>();
        private Dictionary<string, List<List<Vector2>>> m_dicOwnerTroop = new Dictionary<string, List<List<Vector2>>>();

        #endregion

        public TroopLineMediator() : base(NameMediator, null)
        {
            MediatorName = NameMediator;
        }
        //IMediatorPlug needs
        public TroopLineMediator(object viewComponent) : base(NameMediator, null) { }

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Map_ObjectInfo.TagName,
                Army_ArmyList.TagName,

            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Map_ObjectInfo.TagName:
                    {
                        OnMapTroopLine(notification);
                    }
                    break;
                case Army_ArmyList.TagName:
                    {
                        OnOwnerTroopLine(notification);
                    }
                    break;


                default:
                    break;
            }
        }

        #region UI template method          

        protected override void InitData()
        {
            this.IsOpenUpdate = true;
        }

        protected override void BindUIEvent()
        {

        }

        protected override void BindUIData()
        {
            m_bRecordInBack = PlayerPrefs.GetInt("TroopLineRecord", 0) == 1;
        }

        public override void OnRemove()
        {
            base.OnRemove();
            GameObject.DestroyImmediate(m_root);
        }

        public override void Update()
        {
        }

        public override void LateUpdate()
        {
        }

        public override void FixedUpdate()
        {
        }
        #endregion

        public void ShowUI(bool bShow)
        {
            if (bShow)
            {
                m_root = new GameObject("TroopLineMediator");
                var handler = m_root.AddComponent<Client.OnGuiHandler>();
                handler.BindEvent(OnGUI);
            }
            else
            {
                if(m_root != null)
                {
                    GameObject.DestroyImmediate(m_root);
                    m_root = null;
                }
            }
        }
        private string m_currentID;
        private int m_nSelectedID = -1;
        private void OnGUI()
        {
            if (m_nSelectedID == -1)
            {
                int nIndex = -1;
                foreach (var name in m_dicMapTroop.Keys)
                {
                    nIndex++;
                    if (name.Equals(m_currentID))
                    {
                        break;
                    }
                }
                if (nIndex != -1)
                {
                    m_nSelectedID = nIndex;
                    m_currentID = m_dicMapTroop.Keys.ToArray()[nIndex];
                    ShowTroopLine(m_currentID);
                }
            }

            int nSelectedIndex = GUILayout.Toolbar(m_nSelectedID, m_dicMapTroop.Keys.ToArray(), GUILayout.Height(100));
            if(nSelectedIndex != m_nSelectedID)
            {
                m_currentID = m_dicMapTroop.Keys.ToArray()[nSelectedIndex];
                m_nSelectedID = nSelectedIndex;
                ShowTroopLine(m_currentID);
            }
            if (m_troopLineRoot != null && m_troopLineRoot.transform.childCount > 0)
            {
                int nVisible = 0;
                for(int i = 0; i < m_troopLineRoot.transform.childCount; i++)
                {
                    if(m_troopLineRoot.transform.GetChild(i).gameObject.activeSelf)
                    {
                        nVisible++;
                    }
                }
                float fValue = GUILayout.HorizontalSlider(nVisible, 0, m_troopLineRoot.transform.childCount);
                int nValue = Mathf.CeilToInt(fValue);
                if(nValue != nVisible)
                {
                    for (int i = 0; i < m_troopLineRoot.transform.childCount; i++)
                    {
                        m_troopLineRoot.transform.GetChild(i).gameObject.SetActive(i < nValue);
                    }
                }

            }
            GUILayout.BeginVertical(GUILayout.Width(Screen.width * 0.1f));
            if(GUILayout.Button("刷新显示行军线", GUILayout.Height(100)))
            {
                ShowTroopLine(m_currentID);
            }
            if (GUILayout.Button("清除历史", GUILayout.Height(100)))
            {
                m_dicMapTroop.Clear();
                m_dicOwnerTroop.Clear();
                if (m_troopLineRoot != null)
                {
                    GameObject.DestroyImmediate(m_troopLineRoot);
                }
            }
            bool bEnable = (GUILayout.Toggle(m_bRecordInBack, "后台录制(启动自动开启)"));
            if(bEnable != m_bRecordInBack)
            {
                m_bRecordInBack = bEnable;
                PlayerPrefs.SetInt("TroopLineRecord", m_bRecordInBack ? 1 : 0);
                PlayerPrefs.Save();
            }
            if (GUILayout.Button("退出", GUILayout.Height(100)))
            {
                Timer.Register(0.01f, () =>
                {
                    CoreUtils.uiManager.ShowUI(UI.s_mainInterface);
                    if (!m_bRecordInBack)
                    {
                        GlobalBehaviourManger.Instance.RemoveGlobalMediator(TroopLineMediator.NameMediator);
                    }
                    else
                    {
                        ShowUI(false);
                    }
                });
            }
            GUILayout.EndHorizontal();
        }

        private GameObject m_troopLineRoot = null;
        private void ShowTroopLine(string strID)
        {
            if (m_troopLineRoot != null)
            {
                GameObject.DestroyImmediate(m_troopLineRoot);
            }
            m_troopLineRoot = new GameObject("TroopLine");
            m_troopLineRoot.transform.SetParent(m_root.transform);
            List<List<Vector2>> paths;
            if(m_dicMapTroop.TryGetValue(strID, out paths))
            {
                foreach(var path in paths)
                {
                    if (path.Count > 0)
                    {
                        var obj = m_troopLineRoot.transform;
                        MarchLineMgr.Instance().CreateTroopLine((troopLine) =>
                        {
                            if (obj == null)
                            {
                                CoreUtils.assetService.Destroy(troopLine.gameObject);
                                return;
                            }
                            troopLine.name = $"{obj.childCount}";
                            troopLine.transform.SetParent(obj.transform);
                            troopLine.SetColor(new Color(UnityEngine.Random.Range(0, 1.0f), UnityEngine.Random.Range(0, 1.0f), UnityEngine.Random.Range(0, 1.0f)));
                            MarchLineMgr.Instance().SetTroopLinePath(troopLine, path.ToArray());
                        });
                    }
                }
            }
        }

        private void OnOwnerTroopLine(INotification notification)
        {
            Army_ArmyList.request req = notification.Body as Army_ArmyList.request;
            if (req == null)
            {
                return;
            }

            foreach (var kv in req.armyInfo)
            {
                ArmyInfo info = kv.Value;
                if (info == null || !info.HasArmyIndex)
                {
                    return;
                }

                if (info.HasPath && info.path != null)
                {
                    List<Vector2> paths = new List<Vector2>();
                    for (int i = 0; i < info.path.Count; i++)
                    {
                        paths.Add(new Vector2(info.path[i].x / 100.0f, info.path[i].y / 100.0f));
                    }
                    List<List<Vector2>> vectors;
                    if (!m_dicOwnerTroop.TryGetValue(info.armyIndex.ToString(), out vectors))
                    {
                        vectors = new List<List<Vector2>>();
                        m_dicOwnerTroop.Add(info.armyIndex.ToString(), vectors);
                    }
                    vectors.Add(paths);
                }
            }
        }
        private void OnMapTroopLine(INotification notification)
        {
            Map_ObjectInfo.request mapItemInfo = notification.Body as Map_ObjectInfo.request;
            if (mapItemInfo != null)
            {
                int id = (int)mapItemInfo.mapObjectInfo.objectId;
                ArmyData armyData = WorldMapLogicMgr.Instance.MapTroopHandler.GetITroopMgr().GetArmyData(id);
                //if (armyData != null)
                {
                    if (mapItemInfo.mapObjectInfo.HasObjectPath)
                    {
                        List<Vector2> paths = new List<Vector2>();
                        foreach (var pos in mapItemInfo.mapObjectInfo.objectPath)
                        {
                            paths.Add(new Vector2(pos.x / 100.0f, pos.y / 100.0f));
                        }
                        List<List<Vector2>> vectors;
                        if(!m_dicMapTroop.TryGetValue(id.ToString(), out vectors))
                        {
                            vectors = new List<List<Vector2>>();
                            m_dicMapTroop.Add(id.ToString(), vectors);
                        }
                        vectors.Add(paths);
                    }
                }
            }
        }
    }
}