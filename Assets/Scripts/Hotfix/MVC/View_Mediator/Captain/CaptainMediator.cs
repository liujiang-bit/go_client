// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月30日
// Update Time         :    2019年12月30日
// Class Description   :    CaptainMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;
using SprotoType;
using Data;
using Spine;
using Spine.Unity;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace Game {
    public class CaptainMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "CaptainMediator";

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private HeroProxy m_heroProxy;
        private HeroProxy.Hero m_hero;
        private List<HeroProxy.Hero> m_ownHeros;
        private List<HeroProxy.Hero> m_summonHeros;
        private List<HeroProxy.Hero> m_noSummomHeros;
        private List<HeroProxy.Hero> m_heroes = new List<HeroProxy.Hero>();
        private int m_nHeroIndex;
        private HeroProxy.SortType m_sortType = HeroProxy.SortType.None;

        private Camera m_heroCamera;
        private Spine.Unity.SkeletonAnimation m_heroSkeleton;
        private Transform m_skillbg;
        private Transform m_3dNode;

        private Timer m_timer;
        private bool m_isLastSoundComplete = true;

        private const float TOCENTERDISTANCE = 2.3f;
        private float m_curCameraToCenterPos = 0;
        public AnimationCurve m_CameraDamping;
        public AnimationCurve m_SwitchSpeed;

        long m_roleId;

        #endregion

        //IMediatorPlug needs
        public CaptainMediator(object viewComponent) : base(NameMediator, viewComponent) { }


        public CaptainView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.GetNewHero,
                Hero_HeroStarUp.TagName,
                CmdConstant.UpdateHero,
                Hero_HeroSkillLevelUp.TagName,
                CmdConstant.ItemInfoChange,
                CmdConstant.HeroListVisible,
                CmdConstant.RefreshEquipRedPoint,
                CmdConstant.HeroSceneVisible,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.GetNewHero:
                    {
                        var heroId = (long)notification.Body;
                        SortByType(m_sortType, true);
                    }
                    break;
                case Hero_HeroStarUp.TagName:
                    {
                        var response = notification.Body as Hero_HeroStarUp.response;
                        if (response != null && response.result && view.m_UI_Item_CaptainStarUp.gameObject.activeSelf)
                        {
                            view.m_UI_Item_CaptainStarUp.OnAddStarExpSuccess();
                        }
                    }
                    break;
                case CmdConstant.UpdateHero:
                    {
                        HeroInfoEntity entity = notification.Body as HeroInfoEntity;
                        if(entity != null && entity.heroId == m_hero.config.ID)
                        {
                            view.m_UI_Item_CaptainData.setHero(m_hero);
                            view.m_UI_Item_CaptainEquip.SetHero(m_hero);
                            //view.m_btn_changeEquip_GameButton.gameObject.SetActive(m_hero.data != null);
                            RefreshEquipRedPoint(m_hero);
                            view.m_lbl_power_LanguageText.text = string.Format("{0}:{1}", LanguageUtils.getText(200014), m_hero.power.ToString("N0"));
                        }                        
                        view.m_sv_captainHead_ListView.ForceRefresh();
                    }
                    break;
                case Hero_HeroSkillLevelUp.TagName:
                    {
                        Hero_HeroSkillLevelUp.response response = notification.Body as Hero_HeroSkillLevelUp.response;
                        if (response != null && view.m_UI_Item_CaptainSkillUp.gameObject.activeSelf)
                        {
                            view.m_UI_Item_CaptainSkillUp.HeroSkillLevelUpSuceess((int)response.skillId, (int)response.skillLevel);
                        }
                    }
                    break;
                case CmdConstant.ItemInfoChange:
                    {
                        if(view.m_UI_Item_CaptainSkillUp.gameObject.activeSelf)
                        {
                            view.m_UI_Item_CaptainSkillUp.Refresh();
                        }
                    }
                    break;
                case CmdConstant.HeroListVisible:
                    bool state = (bool) notification.Body;
                    if (state)
                        ShowCaptainList();
                    else
                        HideCaptainList();
                    break;
                case CmdConstant.RefreshEquipRedPoint:
                    RefreshEquipRedPoint();
                    break;
                case CmdConstant.HeroSceneVisible:
                    bool visible = (bool) notification.Body;
                    if (visible)
                        view.m_UI_3D_Scene_Image.gameObject.SetActive(true);
                    else
                        view.m_UI_3D_Scene_Image.gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }



        #region UI template method

        public override void OpenAniEnd()
        {

        }

        public override void WinFocus()
        {

        }

        public override void WinClose()
        {
            if (m_timer != null)
            {
                m_timer.Cancel();
                m_timer = null;
            }
            if(m_hero != null)
            {
                CoreUtils.audioService.StopByName(m_hero.config.voiceSelect);
            }
            GameEventGlobalMediator tMediator = AppFacade.GetInstance().RetrieveMediator(GameEventGlobalMediator.NameMediator) as GameEventGlobalMediator;
            if (tMediator != null)
            {
                CoreUtils.audioService.PlayBgm(!tMediator.IsDay() ? RS.SoundCityNight : RS.SoundCityDay);              
            }
        }

        public override void PrewarmComplete()
        {

        }
        
        private bool m_bCheckArr = false;
        private bool m_bBeginDrag = false;
        private bool m_bMoveToCenter = false;
        private bool m_bMoveToCorner = false;
        private Vector2 m_beginTouchPos;

        public override void Update()
        {
            if (view.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.activeSelf)
            {
                if (Input.GetMouseButtonUp(0) && m_bCheckArr)
                {
                    view.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(false);
                }
                m_bCheckArr = true;
            }

            if(view.m_UI_Item_CaptainStroy.gameObject.activeSelf ||
                view.m_UI_Item_CaptainStarUp.gameObject.activeSelf || view.m_UI_Item_CaptainTalent.gameObject.activeSelf)
            {
                if (m_bMoveToCorner)
                {
                    float nToPos = 4.5f;
                    if (LanguageUtils.IsArabic())
                    {
                        nToPos = -4.5f;
                    }
                    var cPos = m_heroCamera.transform.localPosition.x;
                    if (m_heroCamera.transform.localPosition.x != nToPos)
                    {
                        if (cPos > nToPos)
                        {
                            cPos -= Time.deltaTime * 20.0f;
                            cPos = Mathf.Max(nToPos, cPos);
                        }
                        else
                        {
                            cPos += Time.deltaTime * 20.0f;
                            cPos = Mathf.Min(nToPos, cPos);
                        }
                        m_heroCamera.transform.localPosition = new Vector3(cPos, 0, m_heroCamera.transform.localPosition.z);
                    }
                    if(m_heroCamera.transform.localPosition.x == nToPos)
                    {
                        m_bMoveToCorner = false;
                    }
                }
            }
            else if (Input.GetMouseButtonDown(0) && (Input.touchCount <= 1))
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);

                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> list = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, list);
                //foreach (var item in list)
                //{
                //    Debug.Log(item.gameObject.name);
                //}
                if (list.Count > 0 && (list[0].gameObject.name.Equals("img_mask") || list[0].gameObject.name.Equals("btn_char")))
                {
                    if(Input.touchCount == 1)
                    {
                        var touch = Input.GetTouch(0);
                        m_beginTouchPos = touch.position;
                    }
                    else
                    {
                        m_beginTouchPos = Input.mousePosition;
                    }
                    m_bBeginDrag = true;
                }
                if (m_bBeginDrag)
                {
                    m_bMoveToCenter = false;
                }
            }
            else if (Input.GetMouseButtonUp(0) && (Input.touchCount <= 1))
            {
                if (m_bBeginDrag == true)
                {
                    m_bBeginDrag = false;
                    m_bMoveToCenter = true;
                    m_curCameraToCenterPos = m_heroCamera.transform.localPosition.x;
                    if (m_curCameraToCenterPos  < -TOCENTERDISTANCE)
                    {
                        if (m_nHeroIndex != 0)
                        {
                            OnHeroSelected(m_heroes[m_nHeroIndex - 1]);
                        }
                    }
                    else if(m_curCameraToCenterPos  > TOCENTERDISTANCE)
                    {
                        if (m_nHeroIndex != m_heroes.Count - 1)
                        {
                            OnHeroSelected(m_heroes[m_nHeroIndex + 1]);
                        }
                    }
                }
            }
            else if (m_bBeginDrag && (Input.touchCount <= 1))
            {
                Vector2 toucPos = Vector2.zero;
#if UNITY_ANDROID || UNITY_IOS
                toucPos = m_beginTouchPos;
                if (Input.touchCount == 1)
                {
                    var touch = Input.GetTouch(0);
                    toucPos = touch.position;
                }
                else
                {
                    toucPos = Input.mousePosition;
                }
#else
                toucPos = Input.mousePosition;
#endif
                var offX = m_beginTouchPos.x - toucPos.x;
                var cPos = m_heroCamera.transform.localPosition;
                cPos.x = offX / (Screen.width / 2) * 5.5f;
                m_heroCamera.transform.localPosition = cPos;
            }
            else if(m_bMoveToCenter)
            {
                var cPos = m_heroCamera.transform.localPosition.x;
                if (m_heroCamera.transform.localPosition.x != 0)
                {
                    float time = Mathf.Clamp(1 - Mathf.Abs(cPos) / Mathf.Abs(m_curCameraToCenterPos),0,1);
                    float speed = m_SwitchSpeed != null ? m_SwitchSpeed.Evaluate(time) : 1;
                    float damping = m_CameraDamping != null ? Mathf.Clamp(m_CameraDamping.Evaluate(time),0,speed)  : 0;

                    if (cPos > 0)
                    {
                        cPos -= Time.deltaTime * (speed - 0.9f * time * damping);
                        cPos = Mathf.Max(0, cPos);
                    }
                    else
                    {
                        cPos += Time.deltaTime * (speed - 0.9f * time * damping);
                        cPos = Mathf.Min(0, cPos);
                    }
                    m_heroCamera.transform.localPosition = new Vector3(cPos, 0, m_heroCamera.transform.localPosition.z);
                }
                if(cPos == 0)
                {
                    m_bMoveToCenter = false;
                }
            }
        }

        protected override void InitData()
        {
            IsOpenUpdate = true;
            m_heroProxy = AppFacade.GetInstance().RetrieveProxy(HeroProxy.ProxyNAME) as HeroProxy;
            var playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_roleId = playerProxy.CurrentRoleInfo.rid;


            List<string> prefabNames = new List<string>();
            prefabNames.Add("UI_LC_Captain");
            prefabNames.Add("UI_Item_CaptainHead");
            prefabNames.Add("UI_Item_CaptainSummon");
            prefabNames.Add("UI_Item_CaptainPartline");
            prefabNames.Add("UI_Item_CaptainSkill");
            prefabNames.Add("UI_LC_CaptainUnSum");
            //prefabNames.Add("HeroShow_01");
            //prefabNames.Add("HeroShow_02");   
            //prefabNames.Add("HeroShow_03");   
            //prefabNames.Add("HeroShow_04");   
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);

            UIAnimationCurve uiAnimationCurve = view.gameObject.GetComponent<UIAnimationCurve>();
            if (uiAnimationCurve != null)
            {
                m_CameraDamping = uiAnimationCurve.GetAnimationCurveByName("CameraDamping");
                m_SwitchSpeed = uiAnimationCurve.GetAnimationCurveByName("SwitchSpeed");
            }

            //InitUI();
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Item_CaptainData.AddShowMoreEvent(ShowMoreInfo);
            view.m_UI_Item_CaptainData.AddStarUpButtonClickedEvent(ShowStarUp);
            view.m_UI_Item_CaptainData.AddSkillUpButtonClickedEvent(ShowSkillUp);
            view.m_UI_Item_CaptainData.AddTalentButtonClickedEvent(ShowTalent);
            view.m_UI_Item_CaptainSkillUp.m_btn_back_GameButton.onClick.AddListener(HideSkillUp);
            
            //view.m_UI_Item_CaptainEquip.AddCharListener(OnCharClickEvent);
            
            view.m_btn_changeEquip_GameButton.onClick.AddListener(() =>
            {
                if (view.m_UI_Item_CaptainData.gameObject.activeSelf)
                {
                    view.m_UI_Item_CaptainData.Close();
                    view.m_UI_Item_CaptainEquip.Open();
                }
                else
                {
                    view.m_UI_Item_CaptainData.Open();
                    view.m_UI_Item_CaptainEquip.Close();
                }
            });

            view.m_btn_arr_GameButton.onClick.AddListener(()=>
            {
                if (!view.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.activeSelf)
                {
                    view.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(true);

                    view.m_UI_Item_CaptionArrType_star.SetSelected(m_sortType == HeroProxy.SortType.Star);
                    view.m_UI_Item_CaptionArrType_power.SetSelected(m_sortType == HeroProxy.SortType.Power);
                    view.m_UI_Item_CaptionArrType_level.SetSelected(m_sortType == HeroProxy.SortType.Level);
                    view.m_UI_Item_CaptionArrType_Quality.SetSelected(m_sortType == HeroProxy.SortType.Rare);

                    m_bCheckArr = false;
                }
                else
                {
                    view.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(false);
                    m_bCheckArr = true;
                }
            });

            view.m_UI_Item_CaptionArrType_star.AddClickEvent(() =>
            {
                SortByType(HeroProxy.SortType.Star);
            });
            view.m_UI_Item_CaptionArrType_level.AddClickEvent(() =>
            {
                SortByType(HeroProxy.SortType.Level);
            });
            view.m_UI_Item_CaptionArrType_power.AddClickEvent(() =>
            {
                SortByType(HeroProxy.SortType.Power);
            });
            view.m_UI_Item_CaptionArrType_Quality.AddClickEvent(() =>
            {
                SortByType(HeroProxy.SortType.Rare);
            });
            view.m_UI_Model_Interface.AddClickEvent(()=>
            {
                if (view.m_UI_Item_CaptainStroy.gameObject.activeSelf)
                {
                    HideInfo();
                }
                else if(view.m_UI_Item_CaptainStarUp.gameObject.activeSelf)
                {
                    if(view.m_UI_Item_CaptainStarUp.IsShowStarUpDescription)
                    {
                        view.m_UI_Item_CaptainStarUp.HideStarUpDescription();
                    }
                    else
                    {
                        HideStarUp();
                    }
                   
                }
                else if (view.m_UI_Item_CaptainTalent.gameObject.activeSelf)
                {
                    HideTalent();
                }
                else
                {
                    CoreUtils.uiManager.CloseUI(UI.s_captain);
                }
            });
            view.m_btn_powerQ_GameButton.onClick.AddListener(() =>
            {
                if (m_hero != null)
                {
                    HelpTipsDefine define = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(3002);
                    var data1 = LanguageUtils.getTextFormat(define.l_data1, ClientUtils.FormatComma(m_hero.power),
                        ClientUtils.FormatComma(m_hero.baseScore), ClientUtils.FormatComma(m_hero.levelScore), 
                        ClientUtils.FormatComma(m_hero.skillScore), ClientUtils.FormatComma(m_hero.talentScore));
                    HelpTip.CreateTip(LanguageUtils.getTextFormat(define.l_typeID, data1), view.m_btn_powerQ_GameButton.transform).SetStyle(HelpTipData.Style.arrowDown).SetWidth(define.width).Show();
                }
            });
            view.m_btn_char_GameButton.onClick.AddListener(OnCharClickEvent);

            m_heroCamera = view.m_UI_3D_Scene_Image.transform.GetComponentInChildren<Camera>();
            m_heroSkeleton = view.m_UI_3D_Scene_Image.transform.GetComponentInChildren<Spine.Unity.SkeletonAnimation>();
            m_3dNode = view.m_UI_3D_Scene_Image.transform.Find("3D/Scene");
            m_skillbg = view.m_UI_3D_Scene_Image.transform.Find("3D/img_skillbg");
        }

        protected override void BindUIData()
        {
        }

#endregion

        private void SetCameraPosition(float pos)
        {
            m_heroCamera.transform.localPosition = new Vector3(pos, 0, m_heroCamera.transform.localPosition.z);
            m_curCameraToCenterPos = pos;
            m_bMoveToCenter = true;
        }

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

             InitUI();
        }

        List<ItemData> m_itemsData = new List<ItemData>();

        class ItemData
        {
            public int type;
            public List<HeroProxy.Hero> heros = new List<HeroProxy.Hero>();
            public string prefabName;
            public float height;
        }

        private void InitUI()
        {
            ListView.FuncTab functab = new ListView.FuncTab();
            functab.ItemEnter = OnItemEnter;
            functab.GetItemPrefabName = OnGetItemPrefabName;
            functab.GetItemSize = OnGetItemSize;
            view.m_sv_captainHead_ListView.SetInitData(m_assetDic, functab);

            if (LanguageUtils.IsArabic())
            {
                ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "OpenArb");
            }
            else
            {
                ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "OpenNoArb");
            }
            view.m_UI_Item_CaptainData.Open();


            var type = PlayerPrefs.GetInt($"Hero_SortType_{m_roleId}", (int)HeroProxy.SortType.Rare);
            if(!HotfixUtil.IsEnumDefine(typeof(HeroProxy.SortType), type))
            {
                type = (int)HeroProxy.SortType.Rare;
            }

            SortByType((HeroProxy.SortType)type);

            if(view.data != null)
            {
                if(view.data is GOScrptGuide)
                {
                    GoScrptGuide(view.data as GOScrptGuide);
                }
            }
        }

        private void GoScrptGuide(GOScrptGuide guide)
        {
            switch (guide.taskType)
            {
                case EnumTaskType.UseItem:
                    {
                        ShowAddExpGuide();
                    }
                    break;
                case EnumTaskType.ImposingAura:
                    ShowAddExpGuide();
                    break;
                case EnumTaskType.MasterTactician:
                    ShowSkillUpGuide();
                    break;
                case EnumTaskType.ALegendaryPerson:
                    ShowStarUpGuide();
                    break;
                case EnumTaskType.TheTalentedOne:
                    ShowTalentGuide();
                    break;
            }            
        }

        private void SortByType(HeroProxy.SortType type, bool force = false)
        {
            if (type == m_sortType && force==false)
                return;

            PlayerPrefs.SetInt($"Hero_SortType_{m_roleId}", (int)type);
            PlayerPrefs.Save();
            m_sortType = type;
            m_itemsData.Clear();
            view.m_sv_captainHead_ListView.Clear();
            m_heroProxy.GetHerosBySort(out m_ownHeros, out m_summonHeros, out m_noSummomHeros, type);
            m_heroes.Clear();
            m_heroes.AddRange(m_summonHeros);
            m_heroes.AddRange(m_ownHeros);
            m_heroes.AddRange(m_noSummomHeros);
            switch (type)
            {
                case HeroProxy.SortType.Rare:
                    view.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200026);
                    break;
                case HeroProxy.SortType.Star:
                    view.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200027);
                    break;
                case HeroProxy.SortType.Level:
                    view.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(200028);
                    break;
                case HeroProxy.SortType.Power:
                    view.m_lbl_arrtext_LanguageText.text = LanguageUtils.getText(300005);
                    break;
            }

            view.m_lbl_count_LanguageText.text = $"{m_ownHeros.Count}";
            float height1 = m_assetDic["UI_Item_CaptainPartline"].GetComponent<RectTransform>().sizeDelta.y;
            float height2 = m_assetDic["UI_LC_Captain"].GetComponent<RectTransform>().sizeDelta.y;
            float height3 = m_assetDic["UI_LC_CaptainUnSum"].GetComponent<RectTransform>().sizeDelta.y;
            

            List<HeroProxy.Hero> list1 = new List<HeroProxy.Hero>();
            list1.AddRange(m_summonHeros);
            list1.AddRange(m_ownHeros);

            if (list1.Count > 0)
            {
                for (int i = 0; i < list1.Count; i += 2)
                {
                    var item2 = new ItemData();
                    item2.type = 1;
                    item2.height = height2;
                    item2.prefabName = "UI_LC_Captain";
                    item2.heros.Add(list1[i]);
                    if (i + 1 < list1.Count)
                    {
                        item2.heros.Add(list1[i + 1]);
                    }
                    m_itemsData.Add(item2);
                }
                m_hero = list1[0];
            }
            if (m_noSummomHeros.Count > 0)
            {
                if (m_hero == null)
                {
                    m_hero = m_noSummomHeros[0];
                }
                var item1 = new ItemData();
                item1.type = 0;
                item1.height = height1;
                item1.prefabName = "UI_Item_CaptainPartline";
                m_itemsData.Add(item1);

                for (int i = 0; i < m_noSummomHeros.Count; i += 2)
                {
                    var item2 = new ItemData();
                    item2.type = 2;
                    item2.height = height2;
                    item2.prefabName = "UI_LC_CaptainUnSum";
                    item2.heros.Add(m_noSummomHeros[i]);
                    if (i + 1 < m_noSummomHeros.Count)
                    {
                        item2.heros.Add(m_noSummomHeros[i + 1]);
                    }
                    m_itemsData.Add(item2);
                }
            }
            view.m_sv_captainHead_ListView.FillContent(m_itemsData.Count);
            UpdateHero(false);
        }
        #region 任务引导箭头
        /// <summary>
        /// 定位到”+“
        /// </summary>
        private void ShowAddExpGuide()
        {
            HeroProxy.Hero expFullHero = null;
            int expFullIndex = 0;
            HeroProxy.Hero expUnFullHero = null;
            int expUnFullIndex = 0;
            for (int i = 0; i < m_itemsData.Count; ++i)
            {
                var item = m_itemsData[i];
                if (item.type == 0 || item.type == 2) continue;
                foreach (var hero in item.heros)
                {
                    if (hero.data == null) continue;
                    if (hero.IsLevelLimitByStar())
                    {
                        if (expFullHero == null)
                        {
                            expFullHero = hero;
                            expFullIndex = i;
                        }
                        else
                        {
                            if (hero.level > expFullHero.level)
                            {
                                expFullHero = hero;
                                expFullIndex = i;
                            }
                        }
                    }
                    else
                    {
                        if (expUnFullHero == null)
                        {
                            expUnFullHero = hero;
                            expUnFullIndex = i;
                        }
                        else
                        {
                            if (hero.level > expUnFullHero.level)
                            {
                                expUnFullHero = hero;
                                expUnFullIndex = i;
                            }
                        }
                    }
                }
            }
            if (expUnFullHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(expUnFullIndex);
                OnHeroSelected(expUnFullHero);
            }
            else if (expFullHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(expFullIndex);
                OnHeroSelected(expFullHero);
            }
            FingerTargetParam param = new FingerTargetParam();
            param.AreaTarget = view.m_UI_Item_CaptainData.m_UI_Model_CaptainExpBar.m_btn_add_GameButton.gameObject;
            param.ArrowDirection = (int)EnumArrorDirection.Up;
            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
        }
        /// <summary>
        /// 定位到“技能升级”
        /// </summary>
        private void ShowSkillUpGuide()
        {
            HeroProxy.Hero maxlevelHero = null;
            int maxlevelIndex = 0;
            HeroProxy.Hero meetHero = null;
            int meetIndex = 0;
            for (int i = 0; i < m_itemsData.Count; ++i)
            {
                var item = m_itemsData[i];
                if (item.type == 0 || item.type == 2) continue;
                foreach (var hero in item.heros)
                {
                    if (hero.data == null) continue;
                    Debug.LogError(LanguageUtils.getText( hero.config.l_nameID));
                    if (hero.IsCanUpSkill())
                    {
                        if (meetHero == null)
                        {
                            meetHero = hero;
                            meetIndex = i;
                        }
                        else
                        {
                            if (hero.level > meetHero.level)
                            {
                                meetHero = hero;
                                meetIndex = i;
                            }
                        }
                     
                    }
                    else
                    {
                        if (maxlevelHero == null)
                        {
                            maxlevelHero = hero;
                            maxlevelIndex = i;
                        }
                        else
                        {
                            if (hero.level > maxlevelHero.level)
                            {
                                maxlevelHero = hero;
                                maxlevelIndex = i;
                            }
                        }
                    }
                }
            }
            if (meetHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(meetIndex);
                OnHeroSelected(meetHero);
            }
            else if (maxlevelHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(maxlevelIndex);
                OnHeroSelected(maxlevelHero);
            }
            FingerTargetParam param = new FingerTargetParam();
            param.AreaTarget = view.m_UI_Item_CaptainData.m_UI_Model_StandardButton_skillup.gameObject;
            param.ArrowDirection = (int)EnumArrorDirection.Up;
            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
        }
        /// <summary>
        /// 定位到“升星箭头”
        /// </summary>
        /// <param name="type"></param>
        /// <param name="force"></param>
        private void ShowStarUpGuide()
        {
            HeroProxy.Hero expFullHero = null;
            int expFullIndex = 0;
            HeroProxy.Hero expUnFullHero = null;
            int expUnFullIndex = 0;
            for (int i = 0; i < m_itemsData.Count; ++i)
            {
                var item = m_itemsData[i];
                if (item.type == 0 || item.type == 2) continue;
                foreach (var hero in item.heros)
                {
                    if (hero.data == null) continue;
                    var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(hero.star);
                    if (heroStarCfg == null)
                    {
                        continue;
                    }

                    if (hero.level < heroStarCfg.starLimit)
                    {
                        if (expFullHero == null)
                        {
                            expFullHero = hero;
                            expFullIndex = i;
                        }
                        else
                        {
                            if (hero.level > expFullHero.level)
                            {
                                expFullHero = hero;
                                expFullIndex = i;
                            }
                        }
                    }
                    else
                    {
                        if (expUnFullHero == null)
                        {
                            expUnFullHero = hero;
                            expUnFullIndex = i;
                        }
                        else
                        {
                            if (hero.level > expUnFullHero.level)
                            {
                                expUnFullHero = hero;
                                expUnFullIndex = i;
                            }
                        }
                    }
                }
            }
            if (expUnFullHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(expUnFullIndex);
                OnHeroSelected(expUnFullHero);
            }
            else if (expFullHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(expFullIndex);
                OnHeroSelected(expFullHero);
            }
            FingerTargetParam param = new FingerTargetParam();
            param.AreaTarget = view.m_UI_Item_CaptainData.m_btn_starUp_GameButton.gameObject;
            param.ArrowDirection = (int)EnumArrorDirection.Up;
            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
        }
        /// <summary>
        /// 定位到“天赋”
        /// </summary>
        private void ShowTalentGuide()
        {
            HeroProxy.Hero expFullHero = null;
            int expFullIndex = 0;
            HeroProxy.Hero expUnFullHero = null;
            int expUnFullIndex = 0;
            for (int i = 0; i < m_itemsData.Count; ++i)
            {
                var item = m_itemsData[i];
                if (item.type == 0 || item.type == 2) continue;
                foreach (var hero in item.heros)
                {
                    if (hero.data == null) continue;
                    if (hero.GetCurPageRemainPoint(hero.talentIndex)<=0)
                    {
                        if (expFullHero == null)
                        {
                            expFullHero = hero;
                            expFullIndex = i;
                        }
                        else
                        {
                            if (hero.level > expFullHero.level)
                            {
                                expFullHero = hero;
                                expFullIndex = i;
                            }
                        }
                    }
                    else
                    {
                        if (expUnFullHero == null)
                        {
                            expUnFullHero = hero;
                            expUnFullIndex = i;
                        }
                        else
                        {
                            if (hero.level > expUnFullHero.level)
                            {
                                expUnFullHero = hero;
                                expUnFullIndex = i;
                            }
                        }
                    }
                }
            }
            if (expUnFullHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(expUnFullIndex);
                OnHeroSelected(expUnFullHero);
            }
            else if (expFullHero != null)
            {
                view.m_sv_captainHead_ListView.ScrollList2IdxCenter(expFullIndex);
                OnHeroSelected(expFullHero);
            }
            FingerTargetParam param = new FingerTargetParam();
            param.AreaTarget = view.m_UI_Item_CaptainData.m_UI_Model_StandardButton_gift. gameObject;
            param.ArrowDirection = (int)EnumArrorDirection.Up;
            CoreUtils.uiManager.ShowUI(UI.s_fingerInfo, null, param);
        }
#endregion

        private string OnGetItemPrefabName(ListView.ListItem listItem)
        {
            return m_itemsData[listItem.index].prefabName;
        }

        private float OnGetItemSize(ListView.ListItem listItem)
        {
            return m_itemsData[listItem.index].height;
        }
        
        
        private void OnHeroSelected(HeroProxy.Hero hero)
        {
            if (hero == m_hero)
                return;

            for (int i = 0; i < m_itemsData.Count; i++)
            {
                if (m_itemsData[i].type == 0)
                    continue;
                if (m_itemsData[i].heros[0] == m_hero || (m_itemsData[i].heros.Count > 1 && m_itemsData[i].heros[1] == m_hero))
                {
                    m_hero = hero;
                    view.m_sv_captainHead_ListView.RefreshItem(i);
                    break;
                }
            }
            m_hero = hero;
            for(int i = 0; i < m_itemsData.Count; i++)
            {
                if (m_itemsData[i].type == 0)
                    continue;
                if(m_itemsData[i].heros[0] == m_hero || (m_itemsData[i].heros.Count > 1 && m_itemsData[i].heros[1] == m_hero))
                {
                    view.m_sv_captainHead_ListView.RefreshItem(i);
                    break;
                }
            }
            
            int curHeroIndex = -1;
            foreach (var curhero in m_heroes)
            {
                curHeroIndex++;
                if (curhero == m_hero)
                {
                    break;
                }
            }

            if (m_nHeroIndex < curHeroIndex)
            {
                SetCameraPosition(-10);
            }
            else
            {
                SetCameraPosition(10);
            }
            
            m_nHeroIndex = curHeroIndex;
            
            UpdateHero(true);
        }

        private void OnItemEnter(ListView.ListItem listItem)
        {
            var itemData = m_itemsData[listItem.index];
            if (listItem.isInit == false)
            {
                if(itemData.type == 1)
                {
                    var subView = new UI_LC_Captain_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.SetHero(itemData.heros);
                    subView.SelecteHero(m_hero);
                    subView.AddClickEvent(OnHeroSelected);
                }
                if (itemData.type == 2)
                {
                    var subView = new UI_LC_CaptainUnSum_SubView(listItem.go.GetComponent<RectTransform>());
                    listItem.data = subView;
                    subView.SetHero(itemData.heros);
                    subView.SelecteHero(m_hero);
                    subView.AddClickEvent(OnHeroSelected);
                }
                listItem.isInit = true;
            }
            else
            {
                if (itemData.type == 1)
                {
                    var subView = (UI_LC_Captain_SubView)listItem.data;
                    subView.SetHero(itemData.heros);
                    subView.SelecteHero(m_hero);
                }
                else if (itemData.type == 2)
                {
                    var subView = (UI_LC_CaptainUnSum_SubView)listItem.data;
                    subView.SetHero(itemData.heros);
                    subView.SelecteHero(m_hero);
                }
            }
        }

        private void RefreshEquipRedPoint(HeroProxy.Hero heroInfo = null)
        {
            var bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            int count = bagProxy.GetRegionRedPointCount();
            if (count > 0)
                view.m_UI_Common_Redpoint.ShowRedPoint(count);
            else 
                view.m_UI_Common_Redpoint.HideRedPoint();
        }

        private void UpdateHero(bool playVoice)
        {
            //切换背景音乐
            int country = m_hero.config.civilization;
            CivilizationDefine deinfe = CoreUtils.dataService.QueryRecord<CivilizationDefine>(country);
            if (deinfe != null)
            {
                if (!string.IsNullOrEmpty(deinfe.bgm))
                {
                    CoreUtils.audioService.PlayBgm(deinfe.bgm);
                }
            }

            view.m_UI_Item_CaptainEquip.SetHero(m_hero);
            view.m_UI_Item_CaptainData.setHero(m_hero);
            SystemOpenDefine systemOpen = CoreUtils.dataService.QueryRecord<SystemOpenDefine>(11000);
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (m_hero.data == null || systemOpen.openLv > playerProxy.GetTownHall())
            {
                view.m_btn_changeEquip_GameButton.gameObject.SetActive(false);
                view.m_UI_Item_CaptainData.m_UI_Item_EquipUse.gameObject.SetActive(false);
                if (view.m_UI_Item_CaptainEquip.gameObject.activeSelf)
                {
                    view.m_UI_Item_CaptainData.Open();
                    view.m_UI_Item_CaptainEquip.Close();
                }
            }
            else
            {
                view.m_UI_Item_CaptainData.m_UI_Item_EquipUse.gameObject.SetActive(true);
                view.m_btn_changeEquip_GameButton.gameObject.SetActive(true);
            }

            //view.m_btn_changeEquip_GameButton.gameObject.SetActive(m_hero.data != null);
            RefreshEquipRedPoint(m_hero);
            view.m_lbl_power_LanguageText.text = string.Format("{0}:{1}", LanguageUtils.getText(200014), m_hero.power.ToString("N0"));
            m_heroSkeleton.gameObject.SetActive(false);
            ClientUtils.LoadSpine(m_heroSkeleton, m_hero.config.heroModel, () =>
            {
                if (view == null || view.gameObject == null) return;
                m_heroSkeleton.gameObject.SetActive(true);
                PlayHeroAnimation("idle",true);
                if(playVoice)
                {
                    Timer.Register(0.5f, () =>
                    {
                        PlayHeroVoice(false);
                    });
                }
            });

            if (m_assetDic.ContainsKey(m_hero.config.heroScene))
            {
                GameObject heroScene = GameObject.Instantiate(m_assetDic[m_hero.config.heroScene]);
                if (m_3dNode.transform.childCount > 0)
                {
                    var childNode = m_3dNode.transform.GetChild(0);
                    GameObject.Destroy(childNode.gameObject);
                }
                heroScene.transform.SetParent(m_3dNode.transform);
                heroScene.transform.localPosition = Vector3.zero;
                heroScene.transform.localScale = Vector3.one;
            }
            else
            {
                CoreUtils.assetService.Instantiate(m_hero.config.heroScene, (heroScene) =>
                {
                    if (view == null || view.gameObject == null)
                    {
                        CoreUtils.assetService.Destroy(heroScene);
                        return;
                    }
                    if (m_3dNode.transform.childCount > 0)
                    {
                        var childNode = m_3dNode.transform.GetChild(0);
                        CoreUtils.assetService.Destroy(childNode.gameObject);
                    }
                    heroScene.transform.SetParent(m_3dNode.transform);
                    heroScene.transform.localPosition = Vector3.zero;
                    heroScene.transform.localScale = Vector3.one;
                });
            }
        }

        private void ShowMoreInfo()
        {
            if (m_hero == null) return;
            DataToOther(true);
            view.m_UI_Item_CaptainStroy.Open();
            view.m_UI_Item_CaptainStroy.setHero(m_hero);
        }
        private void HideInfo()
        {
            view.m_UI_Item_CaptainStroy.Close();
            OtherToData(true);
        }

        private void ShowStarUp()
        {
            if (m_hero == null) return;
            var heroStarCfg = CoreUtils.dataService.QueryRecord<Data.HeroStarDefine>(m_hero.star);
            if (heroStarCfg == null) return;
            if (m_hero.level < heroStarCfg.starLimit)
            {
                Tip.CreateTip(LanguageUtils.getTextFormat(166056, heroStarCfg.starLimit, m_hero.star + 1), Tip.TipStyle.Middle).Show();
                return;
            }
            DataToOther(true);
            view.m_UI_Item_CaptainStarUp.Show(m_hero);
        }

        private void HideStarUp()
        {            
            view.m_UI_Item_CaptainStarUp.Hide();
            OtherToData(true);
            view.m_UI_Item_CaptainData.setHero(m_hero);
        }

        private void ShowSkillUp()
        {
            if (m_hero == null) return;
            DataToOther(false);
            view.m_UI_Model_Interface.gameObject.SetActive(false);
            view.m_UI_Item_CaptainSkillUp.Show(m_hero);
            m_skillbg.gameObject.SetActive(true);
            view.m_btn_char_GameButton.gameObject.SetActive(false);
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.CommanderSkill);
        }

        private void HideSkillUp()
        {
            view.m_UI_Item_CaptainSkillUp.Hide();
            OtherToData(false);
            m_skillbg.gameObject.SetActive(false);
            view.m_btn_char_GameButton.gameObject.SetActive(true);
            view.m_UI_Model_Interface.gameObject.SetActive(true);
            view.m_UI_Item_CaptainData.setHero(m_hero);
        }

        private void ShowTalent()
        {
            if (m_hero == null) return;
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideTrigger, (int)EnumFuncGuide.CommanderTalent);
            DataToOther(true);
            view.m_UI_Item_CaptainTalent.Open(m_hero);
        }

        private void HideTalent()
        {
            view.m_UI_Item_CaptainTalent.Close();
            OtherToData(true);
        }

        private void ShowChangeEquipBtn()
        {
            SystemOpenDefine systemOpen = CoreUtils.dataService.QueryRecord<SystemOpenDefine>(11000);
            PlayerProxy playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            if (m_hero.data == null || systemOpen.openLv > playerProxy.GetTownHall())
            {
                return;
            }
            view.m_btn_changeEquip_GameButton.gameObject.SetActive(true);
        }

        private void ShowCaptainList()
        {
            ShowChangeEquipBtn();
            view.m_UI_Item_CaptainList_ArabLayoutCompment.gameObject.SetActive(true);
            if (LanguageUtils.IsArabic())
            {
                ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "OpenArb");
            }
            else
            {
                ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "OpenNoArb");
            }
        }

        private void HideCaptainList()
        {
            view.m_btn_changeEquip_GameButton.gameObject.SetActive(false);
            view.m_UI_Item_CaptainList_ArabLayoutCompment.gameObject.SetActive(false);
            ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "Data2Stroy");
        }

        private void DataToOther(bool isMoveCamera)
        {
            if(isMoveCamera)
            {
                m_bMoveToCorner = true;
                m_bMoveToCenter = false;
            }            
            view.m_btn_changeEquip_GameButton.gameObject.SetActive(false);
            view.m_UI_Item_CaptainData.Close();
            view.m_UI_Item_CaptainList_ArabLayoutCompment.gameObject.SetActive(false);
            ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "Data2Stroy");
            view.m_pl_power_ArabLayoutCompment.gameObject.SetActive(false);
            ClientUtils.PlayUIAnimation(view.m_char_anim_Animator, "Data2Stroy");
        }

        private void OtherToData(bool isMoveCamera)
        {
            if(isMoveCamera)
            {
                m_bMoveToCorner = false;
                m_bMoveToCenter = true;
            }
            view.m_UI_Pop_arrType_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_UI_Item_CaptainData.Open();
            view.m_UI_Item_CaptainList_ArabLayoutCompment.gameObject.SetActive(true);
            if (LanguageUtils.IsArabic())
            {
                ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "OpenArb");
            }
            else
            {
                ClientUtils.PlayUIAnimation(view.m_UI_Item_CaptainList_Animator, "OpenNoArb");
            }
            view.m_pl_power_ArabLayoutCompment.gameObject.SetActive(true);
            ClientUtils.PlayUIAnimation(view.m_char_anim_Animator, "Stroy2Data");
            ShowChangeEquipBtn();
        }
        

        private void OnCharClickEvent()
        {
            if (view.m_UI_Item_CaptainEquip.gameObject.activeSelf || view.m_UI_Item_CaptainStroy.gameObject.activeSelf ||
                view.m_UI_Item_CaptainTalent.gameObject.activeSelf || view.m_UI_Item_CaptainStarUp.gameObject.activeSelf)
            {
                return;
            }
            if (m_hero != null && m_heroSkeleton.AnimationState != null)
            {
                //HelpTip.CreateTip(LanguageUtils.getText(id), view.m_pl_talkPos.transform).SetStyle(HelpTipData.Style.arrowLeft).Show();
                var animationStandBy = m_heroSkeleton.AnimationState.Data.SkeletonData.FindAnimation("standby");
                if (animationStandBy != null)
                {
                    if (isAnimationComplete("standby"))
                    {
                        PlayHeroAnimation("standby", false, () => { PlayHeroAnimation("idle", true); });
                        PlayHeroVoice(true);
                    }
                }
                else if (m_isLastSoundComplete)
                {
                    if (m_timer != null)
                    {
                        m_timer.Cancel();
                        m_timer = null;
                    }
                    
                    var animationIdle = m_heroSkeleton.AnimationState.Data.SkeletonData.FindAnimation("idle");
                    if (animationIdle != null)
                    {
                        m_isLastSoundComplete = false;
                        PlayHeroVoice(true);
                        m_timer = Timer.Register(animationIdle.Duration, () => { m_isLastSoundComplete = true; });
                    }
                }
            }
        }

        private void PlayHeroVoice(bool isShowTalkTip)
        {
            if(isShowTalkTip)
            {
                var languageIds = m_hero.config.l_heroLanguage;
                int idx = Random.Range(0, languageIds.Count - 1);
                var id = languageIds[idx];
                HelpTipsDefine helpTipsCfg = CoreUtils.dataService.QueryRecord<HelpTipsDefine>(3003);
                Debug.Log(LanguageUtils.getText(id));
                TalkTip.CreateTip(LanguageUtils.getText(id), view.m_pl_talkPos_Image.transform)
                .SetStyle(TalkTipData.Style.arrowRight).SetWidth(helpTipsCfg.width).Show();
            }            
            
            if (!string.IsNullOrEmpty(m_hero.config.voiceSelect))
                CoreUtils.audioService.PlayOneShot(m_hero.config.voiceSelect);
        }

        private void PlayHeroAnimation(string animationName, bool isLoop,Action call = null)
        {
            if (m_heroSkeleton != null)
            {
                var animation = m_heroSkeleton.AnimationState.Data.SkeletonData.FindAnimation(animationName);
                if (animation != null)
                {

                    TrackEntry entry = m_heroSkeleton.AnimationState.SetAnimation(0, animationName, isLoop);
                    entry.Complete += (TrackEntry e) =>
                    {
                        if (call != null)
                        {
                            call.Invoke();
                        }
                    };

                }
            }
        }

        private bool isAnimationComplete(string animationName)
        {
            var currentEntry = m_heroSkeleton.AnimationState.GetCurrent(0);
            if (currentEntry.Animation.Name.Equals(animationName) && !currentEntry.IsComplete)
            {
                return false;
            }

            return true;
        }
    }
}