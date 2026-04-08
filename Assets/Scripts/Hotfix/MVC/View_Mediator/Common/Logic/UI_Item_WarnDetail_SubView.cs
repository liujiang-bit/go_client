// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月14日
// Update Time         :    2020年5月14日
// Class Description   :    UI_Item_WarnDetail_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;
using System.Collections.Generic;
using Data;
using SprotoType;

namespace Game {
    public partial class UI_Item_WarnDetail_SubView : UI_SubView
    {
        public void Refresh(EarlyWarningInfoEntity info, GameObject soldierPrefab)
        {
            m_info = info;
            m_soldierPrefab = soldierPrefab;
            m_UI_CapHeadMain.gameObject.SetActive(info.mainHeroId != 0);
            if (info.mainHeroId != 0)
            {
                m_UI_CapHeadMain.SetHero(info.mainHeroId, info.mainHeroLevel);
            }

            switch ((WarWarningType)info.earlyWarningType)
            {
                case WarWarningType.Transport:
                    m_pl_captain_ArabLayoutCompment.gameObject.SetActive(false);
                    m_pl_soldier_ArabLayoutCompment.gameObject.SetActive(false);
                    m_pl_reslist_ArabLayoutCompment.gameObject.SetActive(true);
                    ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
                    ClientUtils.LoadSprite(m_img_char_PolygonImage,config.transportIcon);
                    var index = info.armyIndex > 0 && info.armyIndex <= RS.TransportNameIndex.Length
                        ? info.armyIndex
                        : 1;
                    var txt = RS.TransportNameIndex[index-1];
                    m_lbl_woker_name_LanguageText.text = LanguageUtils.getTextFormat(184002, txt);
                    
                    var allLoad = 0.0;
                    if (info.transportResourceInfo != null)
                    {
                        foreach (var v in info.transportResourceInfo)
                        {
                            allLoad += v.load;
                        }
                    }

                    RefreshRes(info);
                    m_lbl_res_num_LanguageText.text = LanguageUtils.getTextFormat(184004 , allLoad);
                    break;
                default:
                    m_pl_captain_ArabLayoutCompment.gameObject.SetActive(true);
                    m_pl_soldier_ArabLayoutCompment.gameObject.SetActive(true);
                    m_pl_res.gameObject.SetActive(false);
                    m_UI_CapHeadSub.gameObject.SetActive(info.deputyHeroId != 0);
                    if (info.deputyHeroId != 0)
                    {
                        m_UI_CapHeadSub.SetHero(info.deputyHeroId, info.deputyHeroLevel);
                    }
                    m_lbl_number_LanguageText.text = LanguageUtils.getTextFormat(200037, ClientUtils.FormatComma(WarWarningProxy.GetSoldierCount(info)));
                    
                    RefreshSolider();
                    Data.HeroDefine heroCfg = CoreUtils.dataService.QueryRecord<Data.HeroDefine>((int)info.mainHeroId);
                    if(heroCfg != null)
                    {
                        m_lbl_name_LanguageText.text = LanguageUtils.getText(heroCfg.l_nameID);
                    }
                    break;
            }
        }

        private void RefreshSolider()
        {
            foreach(Transform child in m_pl_soldier_GridLayoutGroup.transform)
            {
                CoreUtils.assetService.Destroy(child.gameObject);
            }
            List<SoldierInfo> soldierInfoList = new List<SoldierInfo>(m_info.attackSoldiers.Values);
            soldierInfoList.Sort(SortSoldierInfo);
            foreach(var soldierInfo in soldierInfoList)
            {
                var go =  CoreUtils.assetService.Instantiate(m_soldierPrefab);
                go.transform.SetParent(m_pl_soldier_GridLayoutGroup.transform);
                go.transform.position = Vector3.zero;
                go.transform.localScale = Vector3.one;

                UI_Item_SoldierHead_SubView subView = new UI_Item_SoldierHead_SubView(go.GetComponent<RectTransform>());
                subView.SetSoldierInfo(SoldierProxy.GetArmyHeadIcon((int)soldierInfo.id), (int)soldierInfo.num);
            }
        }

        private void RefreshRes(EarlyWarningInfoEntity info)
        {
            if (resGoList==null || resGoList.Count <= 0)
            {
                resGoList = new List<UI_Model_Resources_SubView>();
                resGoList.Add(m_UI_Model_Resources1);
                resGoList.Add(m_UI_Model_Resources2);
                resGoList.Add(m_UI_Model_Resources3);
                resGoList.Add(m_UI_Model_Resources4);
            }
            
            for (int i = 0; i < resGoList.Count; i++)
            {
                if (info.transportResourceInfo != null &&  i < info.transportResourceInfo.Count && info.transportResourceInfo[i].load > 0)
                {
                    resGoList[i].gameObject.SetActive(true);
                    resGoList[i].SetRes((int)info.transportResourceInfo[i].resourceTypeId , info.transportResourceInfo[i].load,0);
                }
                else
                {
                    resGoList[i].gameObject.SetActive(false);
                }
            }
        }

        private int SortSoldierInfo(SoldierInfo info1, SoldierInfo info2)
        {
            if(info1.level != info2.level)
            {
                return info1.level < info2.level ? 1 : -1;
            }
            return info1.type.CompareTo(info2.type);
        }

        private EarlyWarningInfoEntity m_info = null;
        private GameObject m_soldierPrefab = null;
        private List<UI_Model_Resources_SubView> resGoList;
    }
}