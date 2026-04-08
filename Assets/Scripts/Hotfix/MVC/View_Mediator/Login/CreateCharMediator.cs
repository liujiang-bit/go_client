// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    Tuesday, December 24, 2019
// Update Time         :    Tuesday, December 24, 2019
// Class Description   :    CreateCharMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections.Generic;
using Skyunion;
using Client;
using Data;
using PureMVC.Interfaces;
using Spine.Unity;
using SprotoType;
using ListView = Client.ListView;
using System.Text;
using UnityEngine.Diagnostics;

namespace Game {    
    public class CreateCharMediator : GameMediator {
        #region Member
        public static string NameMediator = "CreateCharMediator";

        private List<CivilizationDefine> m_civilizationTable;
        
        private List<string> m_preLoadRes = new List<string>();
        
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        
        private Dictionary<int,ArmsDefine> m_arms = new Dictionary<int,ArmsDefine>();
        private Dictionary<int,HeroDefine> m_hero = new Dictionary<int,HeroDefine>();
        
        
        
        private Vector3 m_initSelectImage = Vector3.zero;

        private CreateCharFlagItemView m_preSelectBox = null;
        private CivilizationDefine m_civilizationSelected = null;

        private PlayerProxy _playerProxy;
        private BagProxy _bagProxy;
        private CurrencyProxy _currencyProxy;

        private ConfigDefine configDefine;

        private GameObject m_finder;
        private GameObject m_finderUI;

        private Animator m_rootAni;

        private OpenType m_OpenType = OpenType.CreateRole;
        private bool m_bCivilizationItemEnough = false;
        private int m_ItemShopCost = 0;
        
        enum OpenType
        {
            CreateRole = 0,
            ChangeCivilization = 1
        }
        #endregion

        //IMediatorPlug needs
        public CreateCharMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public CreateCharView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                Role_CreateRole.TagName,
                Role_RoleLogin.TagName,
                CmdConstant.ChangeCivilizationCmd,
                CmdConstant.FirstEnterCityStartEndter,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case Role_CreateRole.TagName:
                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        switch ((ErrorCode) error.errorCode)
                        {
                            case ErrorCode.ROLE_CREATE_MAX:
                            case ErrorCode.ROLE_SWITCH_NOT_FOUND_ROLE:
                            case ErrorCode.ROLE_SWITCH_NOT_FOUND_NODE:
                                string name = LanguageUtils.getTextFormat(100125, error.errorCode);
                                Tip.CreateTip(name).Show();
                                RoleInfoHelp.DeleteNewCreateRole();
                                break;
                        }
                    }
                    else
                    {
                        Role_CreateRole.response response = notification.Body as Role_CreateRole.response;

                        if (response.HasRid)
                        {
                            _playerProxy.LoginRole(response.rid);
                        }
                        AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking,new EventTrackingData(EnumEventTracking.Character));
                    }
                    break;
                case Role_RoleLogin.TagName:

                    if (notification.Type == RpcTypeExtend.RESPONSE_ERROR)
                    {
                        ErrorMessage error = (ErrorMessage)notification.Body;
                        
//                        if (error.errorCode == (int) ErrorCode.SHOPBOOTH_DELETE_FAIL)
//                        {
//                            
//                        }
                    }
                    else
                    {
                        var loginResponse = notification.Body as Role_RoleLogin.response;
                        if (loginResponse != null)
                        {
                            ServerTimeModule.Instance.SetServerZone(loginResponse.timezone);
                            
                        }
                        var net = AppFacade.GetInstance().RetrieveProxy(NetProxy.ProxyNAME) as NetProxy;
                        net.SetHeartSend();
                        AppFacade.GetInstance().SendNotification(CmdConstant.SwitchMainCityCmd,UI.s_CreateCharacter);
                    }
                    break;
                case CmdConstant.ChangeCivilizationCmd:
                    // 显示不符合更换要求的tip
                    string sb = notification.Body as string;
                    if (sb != null)
                    {
                        string tip = string.Format(LanguageUtils.getText(130403), sb);
                        Tip.CreateTip(tip, Tip.TipStyle.Middle).Show();
                    }
                    break;
                case CmdConstant.FirstEnterCityStartEndter:
                    {
                        //var timer1 = Timer.Register(0.3f, () =>
                        //{
                        //    CoreUtils.uiManager.CloseUI(UI.s_Loading);
                        //});
                        CoreUtils.uiManager.ShowUI(UI.s_mainInterface, () => {
                            AppFacade.GetInstance().SendNotification(CmdConstant.MainViewFirstInitEnd);
                            if (notification.Body != null)
                            {
                                CoreUtils.uiManager.CloseUI(notification.Body as UIInfo);
                            }
                        });
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
            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking,new EventTrackingData(EnumEventTracking.CreatingCharacter));
            AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.tutorial_begin));
        }

        public override void WinClose(){
            m_civilizationTable.Clear();
            m_preLoadRes.Clear();
            m_assetDic.Clear();
            m_arms.Clear();
            m_hero.Clear();
            
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            List<CivilizationDefine> list = CoreUtils.dataService.QueryRecords<CivilizationDefine>();
            m_civilizationTable = new List<CivilizationDefine>();
            //过滤不显示
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ifShow == 0)
                {
               
                }
                else
                {
                    m_civilizationTable.Add(list[i]);
                }
            }
            
            m_civilizationTable.Sort(CompareByCity);

            _playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            _bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            _currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_rootAni = view.gameObject.GetComponent<Animator>();
            if (view.data is int)
            {
                m_OpenType = (OpenType)view.data;
            }
        }

        protected override void BindUIEvent()
        {
            view.m_UI_btn_ok.m_btn_languageButton_GameButton.onClick.AddListener(OnCreateCountry);
            view.m_UI_change.m_btn_languageButton_GameButton.onClick.AddListener(OnChangeCountry);
            view.m_UI_Model_Interface.m_btn_back_GameButton.onClick.AddListener(CloseWindow);
            CoreUtils.audioService.PlayBgm(RS.SoundCreateChar);
            
            
            view.m_btn_proto_GameButton.onClick.AddListener(OnOpenProto);


            if (m_OpenType == OpenType.CreateRole)
            {
                if (m_rootAni != null && m_rootAni.runtimeAnimatorController.animationClips.Length > 0)
                {
                    Timer.Register(m_rootAni.runtimeAnimatorController.animationClips[0].length, () =>
                    {
                        if (m_finder != null)
                        {
                            m_finder.gameObject.SetActive(true);
                            m_finderUI.gameObject.SetActive(true);
                        }

                    });
                }
            }
            //else
            //{
               
            //}
//            if (m_finder != null)
//            {
//                m_finder.gameObject.SetActive(true);
//                m_finderUI.gameObject.SetActive(true);
//            }
            m_rootAni.SetInteger("action", (int)m_OpenType);

            string dialogVoice = "Voice_Guide_dialog001";
            ConfigDefine config = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            SystemLanguage language = LanguageUtils.GetLanguage();
            for (int i = 0; i < config.initialDialogueCivilization.Count; i++)
            {
                if (config.initialDialogueCivilization[i] == (int) language)
                {
                    var languageSet = CoreUtils.dataService.QueryRecord<LanguageSetDefine>((int) language);
                    if (languageSet != null && languageSet.enumSwitch == 1)
                    {
                        dialogVoice = config.initialDialogueAudio[i].Trim();
                        break;
                    }
                }
            }

            CoreUtils.audioService.PlayOneShot(dialogVoice);
           
        }

        private void OnOpenProto()
        {
            HotfixUtil.OpenUrl("https://account.ipreto.com/privacy_policy");
        }

        private void OnCreateCountry()
        {
            _playerProxy.CreateCountry(m_civilizationSelected);

            view.m_UI_btn_ok.m_btn_languageButton_GameButton.interactable = false;
            //AppFacade.GetInstance().SendNotification(CmdConstant.SwitchMainCityCmd,UI.s_CreateCharacter);

            Timer.Register(0.2f, () =>
            {
                if (view.m_UI_Common_Spin.gameObject!=null)
                {
                    view.m_UI_Common_Spin.gameObject.SetActive(true);
                }
            });
        }

        private int preLoadIndex = -1;
        
        private Vector3 m_orignSpinePos = Vector3.zero;

        protected override void BindUIData()
        {
            if (m_OpenType == OpenType.CreateRole)
            {
                //进入创角界面成功日志
                Role_CreateClick.request req = new Role_CreateClick.request();
                req.operateId = 1;
                AppFacade.GetInstance().SendSproto(req);
                preLoadIndex = -1;
                PreLoadSpine();
            }
            
            m_preLoadRes.AddRange(view.m_sv_list_country_ListView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject,m_preLoadRes , (assetDic)=> {
                m_assetDic = assetDic;
                
                
                Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = CountryViewItemByIndex;
                
            
                view.m_sv_list_country_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_country_ListView.FillContent(m_civilizationTable.Count);
                if (m_OpenType == OpenType.ChangeCivilization)
                {
                    var curID = _playerProxy.GetCivilization();
                    for (int i = 0; i < m_civilizationTable.Count; i++)
                    {
                        if (curID == m_civilizationTable[i].ID)
                        {
                            selectCounty(i);
                            break;
                        }
                    }
                    SetChangeUIInfo();
                }
            });

            view.m_pl_welcome.gameObject.SetActive(m_OpenType == OpenType.CreateRole);
            view.m_pl_charChoose_Animator.gameObject.SetActive(m_OpenType != OpenType.CreateRole);
            view.m_UI_btn_ok.gameObject.SetActive(m_OpenType == OpenType.CreateRole);
            view.m_UI_change.gameObject.SetActive(m_OpenType != OpenType.CreateRole);
            view.m_img_changeTip_PolygonImage.gameObject.SetActive(m_OpenType != OpenType.CreateRole);
            view.m_UI_Model_Interface.gameObject.SetActive(m_OpenType != OpenType.CreateRole);


            m_orignSpinePos = view.m_pl_heroOffset_SkeletonGraphic.transform.localPosition;
        }


        private void PreLoadSpine()
        {
            preLoadIndex++;
            if (preLoadIndex< m_civilizationTable.Count)
            {
                var civilization = m_civilizationTable[preLoadIndex];
                if (civilization.ifLock == 0)
                {
                    ClientUtils.LoadSpine(view.m_pl_heroOffset_SkeletonGraphic, civilization.heroRes, () =>
                    {
                        ClientUtils.LoadSprite(view.m_img_charCountryBg_PolygonImage, civilization.background,false,PreLoadSpine);
                    });
                }
                else
                {
                    PreLoadSpine();
                }
            }
        }
        public int CompareByCity(CivilizationDefine x, CivilizationDefine y)
        {

            int result = x.ifLock.CompareTo(y.ifLock);
            if (result == 0)
            {
                if (LanguageUtils.GetLanguage() == SystemLanguage.Arabic)
                {
                    if (x.ID == 101&& x.ID == 201)
                    {
                        return -1;
                    }
                    if (y.ID == 201 && x.ID == 101)
                    {
                        return 1;
                    }
                }

                if (LanguageUtils.GetLanguage() == SystemLanguage.Turkish)
                {
                    if (x.ID == 201 && y.ID == 101)
                    {
                        return -1;
                    }
                    if (y.ID == 201 && x.ID == 101)
                    {
                        return 1;
                    }
                }
            }
            return result;
        }

        void CountryViewItemByIndex(ListView.ListItem scrollItem)
        {
            CreateCharFlagItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<CreateCharFlagItemView>(scrollItem.go);
            
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<CreateCharFlagItemView>(scrollItem.go);
                
            }


            if (m_finder == null && scrollItem.index == 0 && m_OpenType == OpenType.CreateRole)
            {
                ClientUtils.UIAddEffect("UE_Finger",itemView.m_pl_finderPos, (obj) =>
                {
                    m_finder = obj; 
                    m_finder.gameObject.SetActive(false);
                });
                
                ClientUtils.UIAddEffect("UI_10009",itemView.m_pl_finderUIPos, (obj) =>
                {
                    m_finderUI = obj;
                    m_finderUI.gameObject.SetActive(false);
                });
            }
            
            
            var ctiy = m_civilizationTable[scrollItem.index];
            itemView.m_img_selectBox_PolygonImage.gameObject.SetActive(false);
            
            itemView.m_img_black_PolygonImage.gameObject.SetActive(ctiy.ifLock==1);

            if (ctiy!=null)
            {
                //底下小旗帜
                ClientUtils.LoadSprite(itemView.m_btn_selectCountry_PolygonImage,ctiy.civilizationFlag);
                itemView.m_lbl_countryButtomName_LanguageText.text = LanguageUtils.getText(ctiy.l_civilizationID);
                
                
                itemView.m_btn_selectCountry_GameButton.onClick.RemoveAllListeners();
                itemView.m_btn_selectCountry_GameButton.onClick.AddListener(() =>
                {
                    AppFacade.GetInstance().SendNotification(CmdConstant.EventTracking, new EventTrackingData(EnumEventTracking.ChooseCivilization));
                    if (m_finder != null)
                    {
                        CoreUtils.assetService.Destroy(m_finder);
                        CoreUtils.assetService.Destroy(m_finderUI);
                    }
//TODO zj rev
                    if (view.m_ck_proto_GameToggle.isOn)
                    {
                        if (ctiy.ifLock!=1)
                        {
                            selectCounty(scrollItem.index);
                        }
                        else
                        {
                            Tip.CreateTip(300108,Tip.TipStyle.Middle).Show();
                        }
                    }
                    else
                    {
                        Tip.CreateTip(100037,Tip.TipStyle.Middle).Show();
                    }
                });
            }
        }

        private void selectCounty(int index)
        {
            if (m_OpenType == OpenType.CreateRole)
            {
                view.m_pl_welcome.gameObject.SetActive(false);
                view.m_pl_charChoose_Animator.gameObject.SetActive(true);
            }
            
            CreateCharFlagItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<CreateCharFlagItemView>(view.m_sv_list_country_ListView.GetItemByIndex(index).go);

            if (m_preSelectBox !=null)
            {
                m_preSelectBox.m_img_selectBox_PolygonImage.gameObject.SetActive(false);
                m_preSelectBox.m_btn_selectCountry_GameButton.transform.localPosition =
                    m_preSelectBox.m_pl_unSelectPos.localPosition;
                m_preSelectBox.m_lbl_countryButtomName_LanguageText.gameObject.SetActive(true);
                m_preSelectBox.m_lbl_selectName_LanguageText.gameObject.SetActive(false);
                
                ClientUtils.PlayUIAnimation(view.m_pl_charChoose_Animator, "FlagOut");
            }
            
            itemView.m_img_selectBox_PolygonImage.gameObject.SetActive(true);
            itemView.m_btn_selectCountry_GameButton.transform.localPosition =
                itemView.m_pl_SelectPos.localPosition;
            itemView.m_lbl_selectName_LanguageText.gameObject.SetActive(true);
            itemView.m_lbl_countryButtomName_LanguageText.gameObject.SetActive(false);
            
            
            ClientUtils.PlayUIAnimation(itemView.m_img_selectBox_Animator,"UI_10011");


            m_preSelectBox = itemView;
            
            m_civilizationSelected = m_civilizationTable[index];
            
            if (m_civilizationSelected!=null)
            {
                if (m_OpenType == OpenType.CreateRole)
                {
                    //创建角色操作日志
                    Role_CreateClick.request req = new Role_CreateClick.request();
                    req.operateId = m_civilizationSelected.ID;
                    AppFacade.GetInstance().SendSproto(req);
                }
                else
                {
                    var curID = _playerProxy.GetCivilization();
                    view.m_UI_change.gameObject.SetActive(curID != m_civilizationSelected.ID);
                    
                }
               
                HeroDefine hero = CoreUtils.dataService.QueryRecord<HeroDefine>(m_civilizationSelected.initialHero);
                if(hero!=null)
                {
                    CoreUtils.audioService.PlayOneShot(hero.voiceOpening);
                }
                ClientUtils.LoadSpine(view.m_pl_heroOffset_SkeletonGraphic,m_civilizationSelected.heroRes);
                
                view.m_pl_heroOffset_SkeletonGraphic.transform.localPosition = new Vector3(m_orignSpinePos.x+m_civilizationSelected.aboutOffset,m_orignSpinePos.y,m_orignSpinePos.z);
                
                
                ClientUtils.LoadSprite(view.m_img_charCountryBg_PolygonImage,m_civilizationSelected.background);
                ClientUtils.LoadSprite(view.m_img_charCountryIcon_PolygonImage,m_civilizationSelected.civilizationMark);
                
                ClientUtils.ImageSetColor(view.m_img_charCountryFlag_PolygonImage,m_civilizationSelected.floorColour);
                ClientUtils.ImageSetColor(view.m_img_charCountryIcon_PolygonImage,m_civilizationSelected.markColour);
                ClientUtils.LoadSprite(view.m_img_charBuildBg_PolygonImage,m_civilizationSelected.cityShow);
                ClientUtils.LoadSprite(view.m_img_spUnitIcon_PolygonImage,m_civilizationSelected.featureArmsShow);
                
                //国家
                view.m_lbl_countryName_LanguageText.text = LanguageUtils.getText(m_civilizationSelected.l_civilizationID);
                itemView.m_lbl_selectName_LanguageText.text = LanguageUtils.getText(m_civilizationSelected.l_civilizationID);
                
                ClientUtils.TextSetColor(view.m_lbl_countryName_LanguageText,m_civilizationSelected.markColour); 
                ClientUtils.TextSetColor(view.m_lbl_init_LanguageText,m_civilizationSelected.markColour); 
                ClientUtils.TextSetColor(view.m_lbl_horeName_LanguageText,m_civilizationSelected.markColour); 
                
                //初始英雄
                view.m_lbl_horeName_LanguageText.text = LanguageUtils.getText(GetHeroByID(m_civilizationSelected.initialHero).l_nameID);
                ClientUtils.TextSetColor(view.m_lbl_init_LanguageText,m_civilizationSelected.markColour); 
                
                //特色单位
                view.m_lbl_spName_LanguageText.text = LanguageUtils.getText(m_civilizationSelected.l_addNameID);
                view.m_lbl_spDes_LanguageText.text = ClientUtils.SafeFormat(LanguageUtils.getText(m_civilizationSelected.l_addDesID),m_civilizationSelected.addData);
                if (m_civilizationSelected.featureArms.Count > 0)
                {
                    int armyID = 0;
                    if (m_civilizationSelected.armsName>0)
                    {
                        armyID = m_civilizationSelected.armsName;
                    }
                    else
                    {
                        armyID = m_civilizationSelected.featureArms[0];
                    }

                    ArmsDefine armDefine = GetArmsByID(armyID);
                    if (armDefine != null)
                    {
                        view.m_lbl_spUnitName_LanguageText.text = LanguageUtils.getText(armDefine.l_armsID);
                    }
                }
                CoreUtils.audioService.PlayBgm(m_civilizationSelected.bgm);
            }
        }

        public ArmsDefine GetArmsByID(int id)
        {
            if (m_arms.ContainsKey(id))
            {
                return m_arms[id];
            }
            else
            {
                var arm = CoreUtils.dataService.QueryRecord<ArmsDefine>(id);
                if (arm != null)
                {
                    m_arms.Add(id,arm);
                    return arm;
                }
                else
                {
                    CoreUtils.logService.Error("not finder arm"+id);
                }
            }
            return null;
        }
        
        public HeroDefine GetHeroByID(int id)
        {
            if (m_hero.ContainsKey(id))
            {
                return m_hero[id];
            }
            else
            {
                var arm = CoreUtils.dataService.QueryRecord<HeroDefine>(id);
                if (arm != null)
                {
                    m_hero.Add(id,arm);
                    return arm;
                }
                else
                {
                    CoreUtils.logService.Error("not finder arm"+id);
                }
            }

            return null;
        }


        public bool IsTool()
        {
            return m_bCivilizationItemEnough;
        }

        /// <summary>
        /// 更换文明
        /// </summary>
        private void OnChangeCountry()
        {
            if (!m_bCivilizationItemEnough)
            {
                if (!_currencyProxy.ShortOfDenar(m_ItemShopCost))
                {
                    CoreUtils.uiManager.ShowUI(UI.s_ChangeCivilization, null, m_civilizationSelected);
                }
            }
            else
            {
                CoreUtils.uiManager.ShowUI(UI.s_ChangeCivilization, null, m_civilizationSelected);
            }
        }
        /// <summary>
        /// 设置更换文明按钮信息
        /// </summary>
        private void SetChangeUIInfo()
        {
            configDefine = CoreUtils.dataService.QueryRecord<ConfigDefine>(0);
            var itemID = configDefine.civilizationAlterItem;
            ItemDefine itemDefine = CoreUtils.dataService.QueryRecord<ItemDefine>(itemID);
            if (itemDefine != null)
            {
                StringBuilder sb = new StringBuilder();
                var itemCount = _bagProxy.GetItemNum(itemID);
                if (itemCount < 1)
                {
                    //宝石
                    sb.Append(ClientUtils.FormatComma(itemDefine.shopPrice) );
                    
                    CurrencyDefine define = CoreUtils.dataService.QueryRecord<CurrencyDefine>((int)EnumCurrencyType.denar);
                    ClientUtils.LoadSprite(view.m_UI_change.m_img_icon2_PolygonImage, define.iconID);

                    
                    m_ItemShopCost = itemDefine.shopPrice;
                }
                else
                {
                    //道具
                    sb.Append(LanguageUtils.getTextFormat(145048,1));
                    ClientUtils.LoadSprite(view.m_UI_change.m_img_icon2_PolygonImage, itemDefine.itemIcon);
                }
                view.m_UI_change.m_lbl_line2_LanguageText.text = sb.ToString();
                ClientUtils.UIReLayout(view.m_UI_change.m_btn_languageButton_GameButton);
                
                m_bCivilizationItemEnough = itemCount >= 1;
            }
            
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        private void CloseWindow()
        {
            CoreUtils.uiManager.CloseUI(UI.s_PlayerChangeCivilization,true,true);
        }
        #endregion
    }
}