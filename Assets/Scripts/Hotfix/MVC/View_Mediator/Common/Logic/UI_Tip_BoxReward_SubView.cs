// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年5月7日
// Update Time         :    2020年5月7日
// Class Description   :    UI_Tip_BoxReward_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    public partial class UI_Tip_BoxReward_SubView : UI_SubView
    {
        private bool m_isShow= false;
        private bool m_isInitList = false;
        private int m_listPrefabLoadStatus = 0;
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<RewardGroupData> m_rewardGroupDataList = new List<RewardGroupData>();

        public bool IsShow
        {
            get { return m_isShow; }
            set { m_isShow = value; }
        }

        protected override void BindEvent()
        {
            base.BindEvent();
            m_btn_closeButton_GameButton.onClick.AddListener(OnClickClose);
            m_UI_Item_BoxTipsItem.gameObject.SetActive(false);
        }

        public void SetInfo(int boxID,Vector3 boxPos,float radius,string title)
        {
            m_isShow = true;
            gameObject.SetActive(true);
            m_lbl_dec_LanguageText.text = title;
            int childCount = m_pl_boxTips_GridLayoutGroup.transform.childCount;
            for (int i = childCount - 1; i > 0; i--)
            {
                CoreUtils.assetService.Destroy(m_pl_boxTips_GridLayoutGroup.transform.GetChild(i).gameObject);
            }
            RewardGetData rewardGetData = new RewardGetData();
            RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            rewardGetData.rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(boxID);
            int rewardCount = rewardGetData.rewardGroupDataList.Count;
            rewardGetData.rewardGroupDataList.ForEach((RewardGroupData) =>
            {
                GameObject go = CoreUtils.assetService.Instantiate(m_UI_Item_BoxTipsItem.gameObject);
                go.transform.SetParent(m_pl_boxTips_GridLayoutGroup.transform);
                go.SetActive(true);
                go.transform.localScale = Vector3.one;
                UI_Item_BoxTipsItem_SubView subView = new UI_Item_BoxTipsItem_SubView(go.GetComponent<RectTransform>());
                subView.m_pl_item.Refresh(RewardGroupData);
                subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(RewardGroupData.name);
                subView.m_lbl_num_LanguageText.text = RewardGroupData.number.ToString();
            });
            int itemHeight = (int)m_pl_boxTips_GridLayoutGroup.cellSize.y;
            //强刷一下 才能获取到真实的文本高度
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_info_VerticalLayoutGroup.GetComponent<RectTransform>());
            FixPos(boxPos,radius, 40+m_lbl_dec_LanguageText.preferredHeight + itemHeight * rewardCount);
        }

        private void FixPos(Vector3 targetPos,float radius, float height)
        {
            RectTransform imgRect = m_img_bg_PolygonImage.GetComponent<RectTransform>();
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_info_VerticalLayoutGroup.GetComponent<RectTransform>());
            imgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x,  height);
            m_pl_pos_Animator.transform.position =targetPos;
            Vector2 localPos = m_pl_pos_Animator.GetComponent<RectTransform>().anchoredPosition;
            var maxYPos =CoreUtils.uiManager.GetCanvas().GetComponent<RectTransform>().sizeDelta.y/2 - imgRect.sizeDelta.y/2;
            maxYPos = maxYPos < 0 ? 0 : maxYPos;
            maxYPos = maxYPos - Mathf.Abs(localPos.y);
            maxYPos = maxYPos > 0 ? 0 : maxYPos * Mathf.Sign(localPos.y);

            m_img_arrowSideR_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideL_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideTop_PolygonImage.gameObject.SetActive(false);

            
            if (localPos.x <=0)
            {
                localPos.Set(imgRect.rect.width/2 + m_img_arrowSideL_PolygonImage.rectTransform.rect.width+radius, maxYPos);
                imgRect.anchoredPosition = localPos;
                m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                m_img_arrowSideL_PolygonImage.rectTransform.anchoredPosition = new Vector2(0,-maxYPos);
            }
            else
            {
                localPos.Set(-(imgRect.rect.width/2 + m_img_arrowSideR_PolygonImage.rectTransform.rect.width+ radius), maxYPos);
                imgRect.anchoredPosition = localPos;
                m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                m_img_arrowSideR_PolygonImage.rectTransform.anchoredPosition = new Vector2(0,-maxYPos);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_info_VerticalLayoutGroup.GetComponent<RectTransform>());

        }

        public void SetInfo2(int itemPackage, Vector3 position, float radius)
        {
            m_isShow = true;

            m_lbl_dec_LanguageText.gameObject.SetActive(false);
            int childCount = m_pl_boxTips_GridLayoutGroup.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                m_pl_boxTips_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(false);
            }

            RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int rewardCount = rewardGroupDataList.Count;

            float itemHeight = m_UI_Item_BoxTipsItem.gameObject.GetComponent<RectTransform>().rect.height;
            RectTransform imgRect = m_img_bg_PolygonImage.GetComponent<RectTransform>();

            if (rewardCount > 7)
            {
                m_rewardGroupDataList = rewardGroupDataList;
                m_sv_list_view_ListView.gameObject.SetActive(true);
                imgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x, itemHeight * 7.5f+40f);
            }
            else
            {
                m_sv_list_view_ListView.gameObject.SetActive(false);
                imgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x, 20f + itemHeight * rewardCount+20f);
            }

            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(CoreUtils.uiManager.GetUICamera(), position);
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_root_RectTransform, 
                                                                    screenPoint, 
                                                                    CoreUtils.uiManager.GetUICamera(), 
                                                                    out localPos);

            m_img_arrowSideR_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideL_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideTop_PolygonImage.gameObject.SetActive(false);

            if (localPos.x <= Screen.width)
            {
                localPos.Set(localPos.x + (imgRect.rect.width / 2 + radius), localPos.y);
                m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
            }
            else
            {
                localPos.Set(localPos.x - (imgRect.rect.width / 2 + radius), localPos.y);
                m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
            }
            imgRect.transform.localPosition = localPos;
            gameObject.SetActive(true);

            if (rewardCount > 7)
            {
                if (m_listPrefabLoadStatus == 0)
                {
                    m_listPrefabLoadStatus = 1;
                    List<string> prefabNames = new List<string>();
                    prefabNames.AddRange(m_sv_list_view_ListView.ItemPrefabDataList);
                    ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);
                }
                else if (m_listPrefabLoadStatus == 2)
                {
                    RefreshList();
                }
            }
            else
            {
                rewardGroupDataList.ForEach((RewardGroupData) => {
                    GameObject go = CoreUtils.assetService.Instantiate(m_UI_Item_BoxTipsItem.gameObject);
                    go.transform.SetParent(m_pl_boxTips_GridLayoutGroup.transform);
                    go.SetActive(true);
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    UI_Item_BoxTipsItem_SubView subView = new UI_Item_BoxTipsItem_SubView(go.GetComponent<RectTransform>());
                    subView.m_pl_item.Refresh(RewardGroupData);
                    subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(RewardGroupData.name);
                    subView.m_lbl_num_LanguageText.text = RewardGroupData.number.ToString();
                });
            }

            int showRewardCount = rewardCount > 7 ? 7 : rewardCount;
            FixPos(position,radius, 40 + itemHeight * showRewardCount);
        }
        
        public void RefreshInfo(int itemPackage)
        {
            int childCount = m_pl_boxTips_GridLayoutGroup.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                m_pl_boxTips_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(false);
            }

            RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int rewardCount = rewardGroupDataList.Count;
            int itemHeight = (int)m_pl_boxTips_GridLayoutGroup.cellSize.y;
            RectTransform imgRect = m_img_bg_PolygonImage.GetComponent<RectTransform>();

            if (rewardCount > 7)
            {
                m_rewardGroupDataList = rewardGroupDataList;
                m_sv_list_view_ListView.gameObject.SetActive(true);
                imgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x, itemHeight * 7.5f+20);
            }
            else
            {
                m_sv_list_view_ListView.gameObject.SetActive(false);
                imgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x, 20f + itemHeight * rewardCount);
            }

            if (rewardCount > 7)
            {
                if (m_listPrefabLoadStatus == 0)
                {
                    m_listPrefabLoadStatus = 1;
                    List<string> prefabNames = new List<string>();
                    prefabNames.AddRange(m_sv_list_view_ListView.ItemPrefabDataList);
                    ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);
                }
                else if (m_listPrefabLoadStatus == 2)
                {
                    RefreshList();
                }
            }
            else
            {
                rewardGroupDataList.ForEach((RewardGroupData) => {
                    GameObject go = CoreUtils.assetService.Instantiate(m_UI_Item_BoxTipsItem.gameObject);
                    go.transform.SetParent(m_pl_boxTips_GridLayoutGroup.transform);
                    go.SetActive(true);
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    UI_Item_BoxTipsItem_SubView subView = new UI_Item_BoxTipsItem_SubView(go.GetComponent<RectTransform>());
                    subView.m_pl_item.Refresh(RewardGroupData);
                    subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(RewardGroupData.name);
                    subView.m_lbl_num_LanguageText.text = RewardGroupData.number.ToString();
                });
            }
        }

        public void SetInfo3(int itemPackage, string title, Vector3 position, float radius, int direc = 2)
        {
            m_isShow = true;
            if (string.IsNullOrEmpty(title))
            {
                m_lbl_dec_LanguageText.gameObject.SetActive(false);
            }
            else
            {
                m_lbl_dec_LanguageText.gameObject.SetActive(true);
                m_lbl_dec_LanguageText.text = title;
            }

            int childCount = m_pl_boxTips_GridLayoutGroup.transform.childCount;
            for (int i = childCount - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    m_pl_boxTips_GridLayoutGroup.transform.GetChild(i).gameObject.SetActive(false);
                }
                else
                {
                    CoreUtils.assetService.Destroy(m_pl_boxTips_GridLayoutGroup.transform.GetChild(i).gameObject);
                }
            }

            RewardGroupProxy m_rewardGroupProxy = AppFacade.GetInstance().RetrieveProxy(RewardGroupProxy.ProxyNAME) as RewardGroupProxy;
            List<RewardGroupData> rewardGroupDataList = m_rewardGroupProxy.GetRewardDataByGroup(itemPackage);
            int rewardCount = rewardGroupDataList.Count;

            RectTransform itemRect = m_UI_Item_BoxTipsItem.gameObject.GetComponent<RectTransform>();
            float itemHeight = itemRect.rect.height;
            RectTransform imgRect = m_img_bg_PolygonImage.GetComponent<RectTransform>();

            float textHeight = 0;
            if (!string.IsNullOrEmpty(title))
            {
                textHeight = m_lbl_dec_LanguageText.preferredHeight;
                LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_info_VerticalLayoutGroup.GetComponent<RectTransform>());
            }

            if (rewardCount > 7)
            {
                m_rewardGroupDataList = rewardGroupDataList;
                m_sv_list_view_ListView.gameObject.SetActive(true);
                imgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x, itemHeight * 7.5f + 40f+ textHeight);
            }
            else
            {
                m_sv_list_view_ListView.gameObject.SetActive(false);
                imgRect.sizeDelta = new Vector2(imgRect.sizeDelta.x, itemHeight * rewardCount + 40f+ textHeight);
            }

            Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(CoreUtils.uiManager.GetUICamera(), position);
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(m_root_RectTransform,
                                                                    screenPoint,
                                                                    CoreUtils.uiManager.GetUICamera(),
                                                                    out localPos);

            m_img_arrowSideR_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideL_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(false);
            m_img_arrowSideTop_PolygonImage.gameObject.SetActive(false);


            RectTransform rect = m_pl_pos_Animator.gameObject.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(0,0);

            if (direc <= 2)
            {
                if (localPos.x <= Screen.width)
                {
                    localPos.Set(localPos.x + (imgRect.rect.width / 2 + radius), localPos.y);
                    m_img_arrowSideL_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    localPos.Set(localPos.x - (imgRect.rect.width / 2 + radius), localPos.y);
                    m_img_arrowSideR_PolygonImage.gameObject.SetActive(true);
                }
            }
            else
            {
                if (direc == 3)// 上
                {
                    localPos.Set(localPos.x, localPos.y+ (imgRect.rect.height / 2 + radius));
                    m_img_arrowSideButtom_PolygonImage.gameObject.SetActive(true);
                }
                else //下
                {
                    localPos.Set(localPos.x, localPos.y - (imgRect.rect.height / 2 + radius));
                    m_img_arrowSideTop_PolygonImage.gameObject.SetActive(true);
                }
            }

            imgRect.transform.localPosition = localPos;
            gameObject.SetActive(true);

            if (rewardCount > 7)
            {
                if (m_listPrefabLoadStatus == 0)
                {
                    m_listPrefabLoadStatus = 1;
                    List<string> prefabNames = new List<string>();
                    prefabNames.AddRange(m_sv_list_view_ListView.ItemPrefabDataList);
                    ClientUtils.PreLoadRes(gameObject, prefabNames, LoadFinish);
                }
                else if (m_listPrefabLoadStatus == 2)
                {
                    RefreshList();
                }
            }
            else
            {
                rewardGroupDataList.ForEach((RewardGroupData) => {
                    GameObject go = CoreUtils.assetService.Instantiate(m_UI_Item_BoxTipsItem.gameObject);
                    go.transform.SetParent(m_pl_boxTips_GridLayoutGroup.transform);
                    go.SetActive(true);
                    go.transform.localScale = Vector3.one;
                    go.SetActive(true);
                    UI_Item_BoxTipsItem_SubView subView = new UI_Item_BoxTipsItem_SubView(go.GetComponent<RectTransform>());
                    subView.m_pl_item.Refresh(RewardGroupData);
                    subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(RewardGroupData.name);
                    subView.m_lbl_num_LanguageText.text = RewardGroupData.number.ToString();
                });
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_pl_info_VerticalLayoutGroup.GetComponent<RectTransform>());
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
            m_listPrefabLoadStatus = 2;
            RefreshList();
        }

        private void RefreshList()
        {
            if (!m_isInitList)
            {
                ListView.FuncTab functab = new ListView.FuncTab();
                functab.ItemEnter = ListViewItemByIndex;
                m_sv_list_view_ListView.SetInitData(m_assetDic, functab);
                m_isInitList = true;
            }
            m_sv_list_view_ListView.FillContent(m_rewardGroupDataList.Count);
        }

        private void ListViewItemByIndex(ListView.ListItem listItem)
        {
            RewardGroupData rewarData = m_rewardGroupDataList[listItem.index];
            UI_Item_BoxTipsItem_SubView subView = null;
            if (listItem.data == null)
            {
                subView = new UI_Item_BoxTipsItem_SubView(listItem.go.GetComponent<RectTransform>());
                listItem.data = subView;
            }
            else
            {
                subView = listItem.data as UI_Item_BoxTipsItem_SubView;
            }
            subView.m_pl_item.Refresh(rewarData);
            subView.m_lbl_name_LanguageText.text = LanguageUtils.getText(rewarData.name);
            subView.m_lbl_num_LanguageText.text = rewarData.number.ToString();
        }

        private void OnClickClose()
        {
            if (!m_isShow)
            {
                return;
            }

            m_isShow = false;
            gameObject.SetActive(false);
        } 
    }
}