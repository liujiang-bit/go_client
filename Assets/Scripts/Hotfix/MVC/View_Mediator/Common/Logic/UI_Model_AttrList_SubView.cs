// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Friday, January 3, 2020
// Update Time         :    Friday, January 3, 2020
// Class Description   :    UI_Model_AttrList_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    
    public class AttrInfo
    {
        public int LanguageID;

        public string CrrValue;

        public string NextValue;

        public bool IsPerctangle = false;
    }
    
    
    public partial class UI_Model_AttrList_SubView : UI_SubView
    {
        private List<AttrInfo> m_attrs = new List<AttrInfo>(); 
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();


        public void ClearAllData()
        {
            m_attrs.Clear();
            m_preLoadRes.Clear();
        }

        public void AddAttr(int lanID,string crrValue,string nextValue,bool p = false)
        {
            var a = new AttrInfo();
            a.LanguageID = lanID;
            a.CrrValue = crrValue;
            a.NextValue = nextValue;
            a.IsPerctangle = p;
            m_attrs.Add(a);
        }

        public void AddGuildAttr(int lanID,string crrValue)
        {
            var a = new AttrInfo();
            a.LanguageID = lanID;
            a.CrrValue = crrValue;
            a.NextValue = String.Empty;
            a.IsPerctangle = false;
            m_attrs.Add(a);
        }

        public void RefreshUI()
        {
            
            m_UI_Model_AttrList_PolygonImage.rectTransform.sizeDelta = new Vector2(m_UI_Model_AttrList_PolygonImage.rectTransform.sizeDelta.x,40 *m_attrs.Count+20);
            
            m_preLoadRes.AddRange(m_sv_list_view_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;
                
                
                Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ViewItemByIndex;
                
            
                m_sv_list_view_ListView.SetInitData(m_assetDic, funcTab);
                m_sv_list_view_ListView.FillContent(m_attrs.Count);
            });
        }
        
        void ViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Model_AttrItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Model_AttrItemView>(scrollItem.go);
            
            var a = m_attrs[scrollItem.index];

            if (m_attrs!=null)
            {

                itemView.m_lbl_name_LanguageText.text = LanguageUtils.getText(a.LanguageID);


                if (string.IsNullOrEmpty(a.NextValue))
                {
                    if (a.IsPerctangle)
                    {
                        itemView.m_lbl_crrVaule_LanguageText.text = LanguageUtils.getTextFormat(300102, a.CrrValue);
                    }
                    else
                    {
                        itemView.m_lbl_crrVaule_LanguageText.text = a.CrrValue;
                    }
                }
                else
                {
                    if (a.IsPerctangle)
                    {
                        itemView.m_lbl_crrVaule_LanguageText.text =
                            LanguageUtils.getTextFormat(300103, a.CrrValue, a.NextValue);
                        //{0}%+<color=green>{1}%</color>
                    }
                    else
                    {
                        itemView.m_lbl_crrVaule_LanguageText.text =
                            LanguageUtils.getTextFormat(300104, a.CrrValue, a.NextValue);
                        //{0}+<color=green>{1}</color>
                    }
                }

                itemView.m_pl_line_PolygonImage.gameObject.SetActive(!(scrollItem.index == m_attrs.Count - 1));

            }
        }

    }
}