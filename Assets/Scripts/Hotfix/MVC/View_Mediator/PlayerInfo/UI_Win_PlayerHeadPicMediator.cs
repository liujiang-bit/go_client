// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月21日
// Update Time         :    2020年4月21日
// Class Description   :    UI_Win_PlayerHeadPicMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;

namespace Game {

    public class UI_Win_PlayerHeadPicMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_PlayerHeadPicMediator";

        private int m_currentToggle = 1;//1是头像,2是头像框

        private int m_headSelectIndex = 0;
        private int m_headFrameSelectIndex = 0;

        private string m_headIcon;
        private string m_headFrameIcon;

        private List<PlayerHeadDefine> m_headDefines = new List<PlayerHeadDefine>();

        private List<PlayerHeadDefine> m_headFrames = new List<PlayerHeadDefine>();

        private PlayerProxy m_playerProxy;

        private int colNum = 4;

        private bool assetLoadedFinished = false;

        private GameObject m_playerHeadGO;
        #endregion

        //IMediatorPlug needs
        public UI_Win_PlayerHeadPicMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_PlayerHeadPicView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_SetRoleHead.TagName,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_SetRoleHead.TagName:
                    Role_SetRoleHead.response res = notification.Body as Role_SetRoleHead.response;
                    if(res!=null&&res.result)
                    {
                        Tip.CreateTip(781001).Show();
                        OnRefreshListView();

                        //WorldMapObjectProxy mapObjProxy = AppFacade.GetInstance().RetrieveProxy(WorldMapObjectProxy.ProxyNAME) as WorldMapObjectProxy;
                        //var objEntity = mapObjProxy.GetWorldMapObjectByRid(m_playerProxy.Rid);
                        //if(objEntity!=null)
                        //{
                        //    AppFacade.GetInstance().SendNotification(CmdConstant.MapObjectChange, objEntity);
                        //}
                    }
                    break;
                default:
                    break;
            }
        }

       

        #region UI template method

        public override void OpenAniEnd(){

        }

        public override void WinFocus(){
            
        }

        public override void WinClose(){
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            List<PlayerHeadDefine> heads = CoreUtils.dataService.QueryRecords<PlayerHeadDefine>();

            for(int i = 0;i<heads.Count;i++)
            {
                if(heads[i].group==1)
                {
                    m_headDefines.Add(heads[i]);
                }
                else
                {
                    m_headFrames.Add(heads[i]);
                }
            }
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_1.m_btn_btn_GameButton.onClick.AddListener(OnPage1);
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_2.m_btn_btn_GameButton.onClick.AddListener(OnPage2);
        }

        private void OnPage1()
        {
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_1.m_img_highLight_PolygonImage.gameObject.SetActive(true);
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_2.m_img_highLight_PolygonImage.gameObject.SetActive(false);
            if(m_currentToggle!=1)
            {
                m_currentToggle = 1;
                m_headSelectIndex = m_headDefines.FindIndex(i => i.ID == m_playerProxy.CurrentRoleInfo.headId);
                m_headSelectIndex = m_headSelectIndex < 0 ? 0 : m_headSelectIndex;
                OnRefreshListView();
                //view.m_sv_head_ListView.FillContent(OnDivide(m_headDefines.Count));
            }
        }

        private void OnPage2()
        {
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_1.m_img_highLight_PolygonImage.gameObject.SetActive(false);
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_2.m_img_highLight_PolygonImage.gameObject.SetActive(true);
            if(m_currentToggle!=2)
            {
                m_currentToggle = 2;
                m_headFrameSelectIndex = m_headFrames.FindIndex(i => i.ID == m_playerProxy.CurrentRoleInfo.headFrameID);
                m_headFrameSelectIndex = m_headFrameSelectIndex < 0 ? 0 : m_headFrameSelectIndex;
                OnRefreshListView();
                //view.m_sv_headframe_ListView.FillContent(OnDivide(m_headFrames.Count));
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.onClick.AddListener(() =>
            {
                Role_SetRoleHead.request req = new Role_SetRoleHead.request();
                if(m_currentToggle==1)
                {
                    req.id = m_headDefines[m_headSelectIndex].ID;
                    if(m_headDefines[m_headSelectIndex].get!=1&&!m_playerProxy.CurrentRoleInfo.headList.Contains(req.id))
                    {
                        Tip.CreateTip(781002,Tip.TipStyle.Middle).Show();
                        return;
                    }
                }
                else
                {
                    req.id = m_headFrames[m_headFrameSelectIndex].ID;
                    if (m_headFrames[m_headFrameSelectIndex].get != 1 && !m_playerProxy.CurrentRoleInfo.headFrameList.Contains(req.id))
                    {
                        Tip.CreateTip(781003,Tip.TipStyle.Middle).Show();
                        return;
                    }
                }
                AppFacade.GetInstance().SendSproto(req);
            });
            view.m_UI_Model_Window_Type2.m_btn_close_GameButton.onClick.AddListener(OnClose);
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_playerHeadPic);
        }

        protected override void BindUIData()
        {
            Client.ClientUtils.PreLoadRes(view.gameObject,new List<string> 
            { 
                "UI_LC_PlayerHeadPic",
                "UI_Item_PlayerHeadSelect"
            },OnLoadFinished);
        }
       
        #endregion

        private void OnLoadFinished(Dictionary<string,GameObject> dic)
        {
            view.gameObject.SetActive(true);
            assetLoadedFinished = true;
            m_playerHeadGO = dic["UI_Item_PlayerHeadSelect"];
            ListView.FuncTab funcTab = new ListView.FuncTab();
            funcTab.ItemEnter = InitHeadView;
            view.m_sv_head_ListView.SetInitData(dic, funcTab);
            view.m_sv_headframe_ListView.SetInitData(dic, funcTab);
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_1.m_img_highLight_PolygonImage.gameObject.SetActive(true);
            view.m_UI_Common_SideBtn.m_UI_Model_PageButton_2.m_img_highLight_PolygonImage.gameObject.SetActive(false);
            m_headSelectIndex = m_headDefines.FindIndex(i=>i.ID==m_playerProxy.CurrentRoleInfo.headId);
            m_headFrameSelectIndex = m_headDefines.FindIndex(i=>i.ID==m_playerProxy.CurrentRoleInfo.headFrameID);
            m_headSelectIndex = m_headSelectIndex < 0 ? 0 : m_headSelectIndex;
            m_headFrameSelectIndex = m_headFrameSelectIndex < 0 ? 0 : m_headFrameSelectIndex;
            OnRefreshListView();
        }

        private bool m_isFillHeadList = false;
        private bool m_isFillHeadFrameList = false;
        private void OnRefreshListView()
        {
            if (!assetLoadedFinished)
            {
                return;
            }
            PlayerHeadDefine originHead = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)m_playerProxy.CurrentRoleInfo.headId);
            PlayerHeadDefine originHeadFrame = CoreUtils.dataService.QueryRecord<PlayerHeadDefine>((int)m_playerProxy.CurrentRoleInfo.headFrameID);
            if(originHead!=null)
            {
                m_headIcon = originHead.icon;
            }
            if(originHeadFrame != null)
            {
                m_headFrameIcon = originHeadFrame.icon;
            }
            PlayerHeadDefine define;
            if (m_currentToggle==1)
            {
                view.m_sv_head_ListView.gameObject.SetActive(true);
                view.m_sv_headframe_ListView.gameObject.SetActive(false);
                view.m_UI_Model_Window_Type2.m_lbl_title_LanguageText.text = LanguageUtils.getText(781004);
                view.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(781009, m_playerProxy.CurrentRoleInfo.headList.Count+ m_headDefines.FindAll(i=>i.get==1).Count, m_headDefines.Count);
                define = m_headDefines[m_headSelectIndex];
                view.m_UI_Model_PlayerHead.LoadHeadImg(define.icon,()=>
                {
                     view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(true);
                });
                view.m_UI_Model_PlayerHead.LoadFrameImg(m_headFrameIcon);
                view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.interactable = m_playerProxy.CurrentRoleInfo.headId != define.ID;
                if (!m_isFillHeadList)
                {
                    view.m_sv_head_ListView.FillContent(OnDivide(m_headDefines.Count));
                    m_isFillHeadList = true;
                }
                view.m_sv_head_ListView.ForceRefresh();
            }
            else
            {
                view.m_sv_head_ListView.gameObject.SetActive(false);
                view.m_sv_headframe_ListView.gameObject.SetActive(true);
                view.m_UI_Model_Window_Type2.m_lbl_title_LanguageText.text = LanguageUtils.getText(781005);
                view.m_lbl_count_LanguageText.text = LanguageUtils.getTextFormat(781009, m_playerProxy.CurrentRoleInfo.headFrameList.Count + m_headFrames.FindAll(i => i.get == 1).Count, m_headFrames.Count);
                define = m_headFrames[m_headFrameSelectIndex];
                view.m_UI_Model_PlayerHead.LoadFrameImg(define.icon);
                view.m_UI_Model_PlayerHead.LoadHeadImg(m_headIcon,()=>
                {
                    view.m_pl_right_ArabLayoutCompment.gameObject.SetActive(true);
                });
                view.m_UI_Model_StandardButton_Blue.m_btn_languageButton_GameButton.interactable = m_playerProxy.CurrentRoleInfo.headFrameID!=define.ID;
                if (!m_isFillHeadFrameList)
                {
                    view.m_sv_headframe_ListView.FillContent(OnDivide(m_headFrames.Count));
                    m_isFillHeadFrameList = true;
                }
                view.m_sv_headframe_ListView.ForceRefresh();

            }
            view.m_lbl_desc_LanguageText.text = define.l_decID == 0 ? string.Empty : LanguageUtils.getText(define.l_decID);
            view.m_lbl_name_LanguageText.text = define.l_nameID == 0 ? string.Empty : LanguageUtils.getText(define.l_nameID);
            view.m_img_rank_PolygonImage.gameObject.SetActive(define.l_tagID != 0);
            view.m_lbl_rank_LanguageText.text = define.l_tagID == 0 ? string.Empty: LanguageUtils.getText(define.l_tagID);
        }

        private void InitHeadView(ListView.ListItem item)
        {
            int colIndex = item.index;
            if(item.go.transform.childCount==0&&!item.isInit)
            {
                item.isInit = true;
                for (int i = 0; i < colNum; i++)
                {
                    int k = i;
                    GameObject go =  CoreUtils.assetService.Instantiate(m_playerHeadGO);
                    go.transform.SetParent(item.go.transform);
                    go.transform.localScale = Vector3.one;
                    OnInitTrulyHeadItemView(go, k + colIndex * colNum);
                }
            }
            else
            {
                for (int i = 0; i < item.go.transform.childCount; i++)
                {
                    OnInitTrulyHeadItemView(item.go.transform.GetChild(i).gameObject, i + colIndex * colNum);
                }
            }
        }


        private void OnInitTrulyHeadItemView(GameObject go, int realIndex)
        {
            UI_Item_PlayerHeadSelectView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_PlayerHeadSelectView>(go);
            GrayChildrens gray = itemView.m_UI_Model_PlayerHead.gameObject.GetComponent<GrayChildrens>();
            if(gray==null)
            {
                gray = itemView.m_UI_Model_PlayerHead.gameObject.AddComponent<GrayChildrens>();
            }
            if (m_currentToggle == 1)
            {
                if (realIndex >= m_headDefines.Count)
                {
                    go.SetActive(false);
                    return;
                }
                if (!go.activeSelf)
                    go.SetActive(true);

                if ((!m_playerProxy.CurrentRoleInfo.headList.Contains(m_headDefines[realIndex].ID)) && m_headDefines[realIndex].get != 1)
                {
                    gray.Gray();
                    itemView.m_img_lock_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    gray.Normal();
                    itemView.m_img_lock_PolygonImage.gameObject.SetActive(false);
                }
                itemView.m_img_selet_PolygonImage.gameObject.SetActive(m_headSelectIndex == realIndex);
                itemView.m_img_using_PolygonImage.gameObject.SetActive(m_headDefines[realIndex].ID == m_playerProxy.CurrentRoleInfo.headId);
                itemView.m_UI_Model_PlayerHead.LoadHeadImg(m_headDefines[realIndex].icon);
                itemView.m_UI_Model_PlayerHead.m_img_circle_PolygonImage.gameObject.SetActive(false);
                itemView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_click_GameButton.onClick.AddListener(() =>
                {
                    m_headSelectIndex = realIndex;
                    OnRefreshListView();
                });
            }
            else if (m_currentToggle == 2)
            {
                if (realIndex >= m_headFrames.Count)
                {
                    go.SetActive(false);
                    return;
                }
                if (!go.activeSelf)
                    go.SetActive(true);

                if((!m_playerProxy.CurrentRoleInfo.headFrameList.Contains(m_headFrames[realIndex].ID)) && m_headFrames[realIndex].get != 1)
                {
                    gray.Gray();
                    itemView.m_img_lock_PolygonImage.gameObject.SetActive(true);
                }
                else
                {
                    gray.Normal();
                    itemView.m_img_lock_PolygonImage.gameObject.SetActive(false);
                }
                itemView.m_img_selet_PolygonImage.gameObject.SetActive(m_headFrameSelectIndex == realIndex);
                itemView.m_img_using_PolygonImage.gameObject.SetActive(m_headFrames[realIndex].ID == m_playerProxy.CurrentRoleInfo.headFrameID);
                itemView.m_UI_Model_PlayerHead.m_img_circle_PolygonImage.gameObject.SetActive(true);
                itemView.m_UI_Model_PlayerHead.LoadHeadImg(m_headIcon);
                itemView.m_UI_Model_PlayerHead.LoadFrameImg(m_headFrames[realIndex].icon);
                itemView.m_btn_click_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_click_GameButton.onClick.AddListener(()=>
                {
                    m_headFrameSelectIndex = realIndex;
                    OnRefreshListView();
                });
            }
            else
            {
                go.SetActive(false);
            }
        }

        private int OnDivide(int total)
        {
            return Mathf.CeilToInt(total / (float)colNum);
        }
    }
}