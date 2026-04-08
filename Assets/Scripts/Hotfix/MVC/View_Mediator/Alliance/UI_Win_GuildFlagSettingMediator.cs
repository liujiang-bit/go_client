// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, April 7, 2020
// Update Time         :    Tuesday, April 7, 2020
// Class Description   :    UI_Win_GuildFlagSettingMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PureMVC.Patterns;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using SprotoType;
using UnityEngine.UI;

namespace Game {
    public class UI_Win_GuildFlagSettingMediator : GameMediator {
        #region Member
        public static string NameMediator = "UI_Win_GuildFlagSettingMediator";


        #endregion

        //IMediatorPlug needs
        public UI_Win_GuildFlagSettingMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public UI_Win_GuildFlagSettingView view;
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private List<AllianceSignDefine> m_signFlag;

        private List<AllianceSignDefine> m_signFlagColor;

        private List<AllianceSignDefine> m_signFlagBig;

        private List<AllianceSignDefine> m_singFlagBigColor;

        private AllianceProxy m_allianceProxy;
        
        private CurrencyProxy m_crrProxy;

        
        public const int FlagLineCount = 8;
  

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Guild_ModifyGuildInfo.TagName
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case  Guild_ModifyGuildInfo.TagName:

                    if (notification.Body==null)
                    {
                        return;
                    }
                    Guild_ModifyGuildInfo.response response = notification.Body as Guild_ModifyGuildInfo.response;

                    if (response!=null && response.HasType && response.type == 5)
                    {
                        onOK();
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

        private bool isCreate = false;

        protected override void InitData()
        {
            m_allianceProxy = AppFacade.GetInstance().RetrieveProxy(AllianceProxy.ProxyNAME) as AllianceProxy;
            m_crrProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;

            m_signFlag = m_allianceProxy.SiginFlagSimples();
            m_signFlagColor = m_allianceProxy.SiginFlagSimpleColors();

            m_signFlagBig = m_allianceProxy.SiginFlags();
            m_singFlagBigColor = m_allianceProxy.SiginFlagColors();


            isCreate = m_allianceProxy.HasJionAlliance() == false;

        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type3.m_btn_close_GameButton.onClick.AddListener(OnClose);
            
            
            view.m_btn_change.gameObject.SetActive(isCreate==false);
            view.m_btn_ok.gameObject.SetActive(isCreate);
            
            m_preLoadRes.AddRange(view.m_sv_bg_ListView.ItemPrefabDataList);
            m_preLoadRes.AddRange(view.m_sv_img_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;

                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = FlagViewItemByIndex;
                
            
                view.m_sv_bg_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_bg_ListView.FillContent(m_signFlag.Count);
                
                ListView.FuncTab funcTab2 = new ListView.FuncTab();
                funcTab2.ItemEnter = FlagBigViewItemByIndex;
                
            
                view.m_sv_img_ListView.SetInitData(m_assetDic, funcTab2);
                view.m_sv_img_ListView.FillContent(m_signFlagBig.Count/FlagLineCount);
            });
            
            view.m_sd_bgcolor_GameSlider.maxValue = m_signFlagColor.Count-1;
            view.m_sd_imgColor_GameSlider.maxValue = m_singFlagBigColor.Count-1;
 
            view.m_UI_Model_GuildFlag.Clone((UI_Model_GuildFlag_SubView)view.data);

            var gameobj = view.m_sd_bgcolor_GameSlider.transform.Find("Background");

            if (gameobj!=null)
            {
                var imgs = gameobj.GetComponentsInChildren<PolygonImage>();
                for (int i = 0; i < imgs.Length; i++)
                {
                    var img = imgs[i];
                    ClientUtils.ImageSetColor(img,m_signFlagColor[i].colour);

                    if (! isCreate && m_allianceProxy.GetAlliance().signs[1] == m_signFlagColor[i].ID)
                    {
                        view.m_UI_Model_GuildFlag.m_flagColorIndex = i;
                    }
                }
            }
            
            var gameobj2 = view.m_sd_imgColor_GameSlider.transform.Find("Background");

            if (gameobj2!=null)
            {
                var imgs = gameobj2.GetComponentsInChildren<PolygonImage>();
                for (int i = 0; i < imgs.Length; i++)
                {
                    var img = imgs[i];
                    ClientUtils.ImageSetColor(img,m_singFlagBigColor[i].colour);
                    if (! isCreate && m_allianceProxy.GetAlliance().signs[3] == m_singFlagBigColor[i].ID)
                    {
                        view.m_UI_Model_GuildFlag.m_flagLogoColorIndex = i;
                    }
                }
            }

            view.m_sd_bgcolor_GameSlider.onValueChanged.AddListener(onFlagColor);
            view.m_sd_imgColor_GameSlider.onValueChanged.AddListener(onFlagLogoColor);
           
            
            view.m_btn_random_GameButton.onClick.AddListener(onRmd);
            
            view.m_btn_ok.m_btn_languageButton_GameButton.onClick.AddListener(onOK);
            
            view.m_btn_change.m_btn_languageButton_GameButton.onClick.AddListener(onChange);

            view.m_btn_change.m_lbl_line2_LanguageText.text = m_allianceProxy.Config.alliancSignAmend.ToString();
            
            ClientUtils.UIReLayout(view.m_btn_change.m_btn_languageButton_GameButton);
            
            
            
            setFlag();

            if (!isCreate)
            {
                for (int i = 0; i < m_signFlagBig.Count; i++)
                {
                    var flag = m_signFlagBig[i];

                    if (flag.ID == m_allianceProxy.GetAlliance().signs[2])
                    {
                        view.m_UI_Model_GuildFlag.m_flagLogoIndex = i ;
                        break;
                    }
                }
            }
        }

        private void onChange()
        {
            
            var old = m_allianceProxy.GetAlliance().signs;
            var newData = view.m_UI_Model_GuildFlag.GetSigns();

            bool hasChange = false;

            for (int i = 0; i < old.Count; i++)
            {
                if (old[i]!=newData[i])
                {
                    hasChange = true;
                    break;
                }
            }


            if (hasChange)
            {
                if (!m_crrProxy.ShortOfDenar(m_allianceProxy.Config.alliancSignAmend))
                {
                    m_allianceProxy.SendEditAllianceInfo(5,"",view.m_UI_Model_GuildFlag.GetSigns());
                }
                else
                {
                    CoreUtils.uiManager.ShowUI(UI.s_GemShort);
                }
            }
            else
            {
                onOK();
            }


        }

        public float m_lastTime;
        public void PlaySoundUiCommonSlider()
        {
            if (Time.realtimeSinceStartup - m_lastTime > 0.1f)
            {
                m_lastTime = Time.realtimeSinceStartup;
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            }
        }

        private void onFlagColor(float value)
        {
            PlaySoundUiCommonSlider();
            view.m_UI_Model_GuildFlag.m_flagColorIndex= (int)value;
            view.m_UI_Model_GuildFlag.m_flagColorID = m_signFlagColor[view.m_UI_Model_GuildFlag.m_flagColorIndex].ID;
             setFlag();
        }
        
        private void onFlagLogoColor(float value)
        {
            int newVal = (int)value;
            if (newVal != view.m_UI_Model_GuildFlag.m_flagLogoColorIndex)
            {
                PlaySoundUiCommonSlider();
            }
            view.m_UI_Model_GuildFlag.m_flagLogoColorIndex = newVal;
            view.m_UI_Model_GuildFlag.m_flagLogoColorID = m_singFlagBigColor[view.m_UI_Model_GuildFlag.m_flagLogoColorIndex].ID;
            setFlag();
        }

        private void onOK()
        {
            UI_Model_GuildFlag_SubView old = (UI_Model_GuildFlag_SubView) view.data;
            
            old.Clone(view.m_UI_Model_GuildFlag);
            old.setFlag();

            CoreUtils.uiManager.CloseUI(UI.s_AllianceFlag);
        }


        private Timer m_rmdTimer;
        private void onRmd()
        {

            if (m_rmdTimer!=null)
            {
                m_rmdTimer.Cancel();
                m_rmdTimer = null;
                StartRmd();
                return;
            }
            m_rmdTimer = Timer.Register(0.6f,StartRmd);
            
        }

        private void StartRmd()
        {
            view.m_UI_Model_GuildFlag.RandomFlag();
            setFlag();
            view.m_sv_img_ListView.ForceRefresh();

            m_rmdTimer = null;
        }


        private void setFlag()
        {
            view.m_sd_bgcolor_GameSlider.value = view.m_UI_Model_GuildFlag.m_flagColorIndex;
            view.m_sd_imgColor_GameSlider.value = view.m_UI_Model_GuildFlag.m_flagLogoColorIndex;
            
            view.m_UI_Model_GuildFlag.setFlag();
            
            AllianceProxy.SetUIFlag(view.m_UI_Model_GuildFlag.GetSigns(), view.m_UI_alliance_1002.gameObject);
           


            view.m_img_flagRange_PolygonImage.color = view.m_UI_Model_GuildFlag.m_img_flag_PolygonImage.color;
            
            if (isCreate == false)
            {

              
                    

            }
        }

        void FlagViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_Item_GuildFlagColorView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_Item_GuildFlagColorView>(scrollItem.go);

            var data = m_signFlag[scrollItem.index];

            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_Item_GuildFlagColorView>(scrollItem.go);

            }
            ClientUtils.LoadSprite(itemView.m_btn_bg_PolygonImage,data.showIcon);
            
            itemView.m_btn_bg_GameButton.onClick.RemoveAllListeners();
            itemView.m_btn_bg_GameButton.onClick.AddListener(() =>
            {
                view.m_UI_Model_GuildFlag.m_flagID = data.ID;
                view.m_UI_Model_GuildFlag.setFlag();
            });
            
        }
        
        
        
        void FlagBigViewItemByIndex(ListView.ListItem scrollItem)
        {
            UI_LC_GuildFlagImgView itemView =
                MonoHelper.GetOrAddHotFixViewComponent<UI_LC_GuildFlagImgView>(scrollItem.go);

            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<UI_LC_GuildFlagImgView>(scrollItem.go);

            }

            for (int i = 0; i < FlagLineCount; i++)
            {
                setFlagLogo(scrollItem.index, i, itemView);
            }
            
        }

        private UI_Item_GuildFlagImg_SubView preFlagBig;


        private void setFlagLogo(int index,int off,UI_LC_GuildFlagImgView itemView)
        {
            var itemIndex = (index * FlagLineCount) + off;
            var data = m_signFlagBig[itemIndex];

            UI_Item_GuildFlagImg_SubView subView = null;

            switch (off)
            {
                case 0:
                    subView = itemView.m_UI_GuildFlagImg0;
                    break;
                case 1:
                    subView = itemView.m_UI_GuildFlagImg1;
                    break;
                case 2:
                    subView = itemView.m_UI_GuildFlagImg2;
                    break;
                case 3:
                    subView = itemView.m_UI_GuildFlagImg3;
                    break;
                case 4:
                    subView = itemView.m_UI_GuildFlagImg4;
                    break;
                case 5:
                    subView = itemView.m_UI_GuildFlagImg5;
                    break;
                case 6:
                    subView = itemView.m_UI_GuildFlagImg6;
                    break;
                case 7:
                    subView = itemView.m_UI_GuildFlagImg7;
                    break;
            }
            
            if (subView!=null)
            {
                ClientUtils.LoadSprite(subView.m_btn_icon_PolygonImage,data.showIcon);
                
//                Debug.Log(view.m_UI_Model_GuildFlag.m_flagLogoIndex+" bind  "+itemIndex);
                if (view.m_UI_Model_GuildFlag.m_flagLogoIndex == itemIndex)
                {
                   
                    if (preFlagBig!=null)
                    {
                        preFlagBig.m_img_select_PolygonImage.gameObject.SetActive(false);
                    }
                    
                    subView.m_img_select_PolygonImage.gameObject.SetActive(true);
                    preFlagBig = subView;
                }
                
                subView.m_btn_icon_GameButton.onClick.RemoveAllListeners();
                subView.m_btn_icon_GameButton.onClick.AddListener(() =>
                {
                    view.m_UI_Model_GuildFlag.m_flagLogoIndex = itemIndex;
                    view.m_UI_Model_GuildFlag.m_flagLogoID = data.ID;
                    view.m_UI_Model_GuildFlag.setFlag();
                    
                    AllianceProxy.SetUIFlag(view.m_UI_Model_GuildFlag.GetSigns(), view.m_UI_alliance_1002.gameObject);
                    
                    if (preFlagBig!=null)
                    {
                        preFlagBig.m_img_select_PolygonImage.gameObject.SetActive(false);
                    }
                    
                    subView.m_img_select_PolygonImage.gameObject.SetActive(true);
                    preFlagBig = subView;
                });
            }
            
            
        }

        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_AllianceFlag);
        }

        protected override void BindUIData()
        {
            view.m_UI_Model_Window_Type3.setWindowTitle(LanguageUtils.getText(730057));
//            view.m_btn_ok.m_lbl_line1_LanguageText.text = LanguageUtils.getText(730038);
            
            
            
//            view.m_UI_ok.m_lbl_line2_LanguageText.text = m_allianceProxy.Config.all
        }
       
        #endregion
    }
}