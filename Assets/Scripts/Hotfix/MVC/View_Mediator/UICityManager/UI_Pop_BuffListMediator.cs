// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年4月21日
// Update Time         :    2020年4月21日
// Class Description   :    UI_Pop_BuffListMediator
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
using System.Linq;

namespace Game
{

    public class BuffListItemData
    {
        public string title;
        public CityBuff cityBuff;
        public Timer timer;
        public CityBuffDefine cityBuffDefine;
    }
    public class UI_Pop_BuffListMediator : GameMediator
    {
        #region Member
        public static string NameMediator = "UI_Pop_BuffListMediator";

        private PlayerProxy m_playerProxy;

        private CityBuffProxy m_cityBuffProxy;

        private List<string> m_preLoadRes = new List<string>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();
        private List<BuffListItemData> m_cityBuffList = new List<BuffListItemData>();
        private Dictionary<int, Timer> m_timers = new Dictionary<int, Timer>();
        private Vector3 pos;//tip位置
        #endregion

        //IMediatorPlug needs
        public UI_Pop_BuffListMediator(object viewComponent) : base(NameMediator, viewComponent) { }



        public UI_Pop_BuffListView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>()
            {
                CmdConstant.CityBuffChange,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                default:
                case CmdConstant.CityBuffChange:
                    {
                        m_cityBuffList.ForEach((cityBuff) => {
                            if (cityBuff.timer != null)
                            {
                                cityBuff.timer.Cancel();
                                cityBuff = null;
                            }
                        });
                        m_cityBuffList = m_cityBuffProxy.GetCityBuffList();
                        view.m_sv_list_ListView.FillContent(m_cityBuffList.Count);
                        view.m_sv_list_ListView.ForceRefresh();
                    }
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

        }

        public override void PrewarmComplete()
        {

        }

        public override void Update()
        {

        }

        protected override void InitData()
        {
            m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            m_cityBuffProxy = AppFacade.GetInstance().RetrieveProxy(CityBuffProxy.ProxyNAME) as CityBuffProxy;
            if (view.data is Vector3)
            {
                pos = (Vector3)view.data;
            }

            m_preLoadRes.AddRange(view.m_sv_list_ListView.ItemPrefabDataList);
            m_cityBuffList = m_cityBuffProxy.GetCityBuffList();
            ClientUtils.PreLoadRes(view.gameObject, m_preLoadRes, (assetDic) =>
            {
                m_assetDic = assetDic;
                InitListView();
            });
            view.m_btn_more.gameObject.SetActive(GameModeManager.Instance.CurGameMode == GameModeType.World);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_more.m_btn_languageButton_GameButton.onClick.AddListener(OnArrowBtnClick);
        }

        protected override void BindUIData()
        {
            if(GameModeManager.Instance.CurGameMode == GameModeType.Expedition && LanguageUtils.GetLanguage() == SystemLanguage.Arabic)
            {
                var arabComponent = view.m_img_bg_PolygonImage.GetComponent<ArabLayoutCompment>();
                if(arabComponent != null)
                {
                    arabComponent.CalculateArabLayoutStyle_PosX(view.m_img_bg_PolygonImage.rectTransform);
                }
                view.m_pl_rect_ArabLayoutCompment.CalculateArabLayoutStyle_PosX(view.m_pl_rect_ArabLayoutCompment.transform as RectTransform);
            }

            Vector2 arrpos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(CoreUtils.uiManager.GetCanvas().transform as RectTransform, CoreUtils.uiManager.GetUICamera().WorldToScreenPoint(pos), CoreUtils.uiManager.GetUICamera(), out arrpos);
            var rectTransform = view.m_pl_pos_Animator.transform as RectTransform;
            rectTransform.anchoredPosition  = arrpos-new Vector2(0,20);
        }

        #endregion

        private void InitListView()
        {
            {
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = ItemListEnter;
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
                view.m_sv_list_ListView.FillContent(m_cityBuffList.Count);
            }
        }
        private void ItemListEnter(ListView.ListItem scrollItem)
        {
            int index = scrollItem.index;
            BuffListItemData cityBuff = m_cityBuffList[index];

            UI_Item_BuffListItemView itemView = MonoHelper.GetOrAddHotFixViewComponent<UI_Item_BuffListItemView>(scrollItem.go);
            if (cityBuff.cityBuffDefine.tagData.Count > 0)
            {
                itemView.m_lbl_buffName_LanguageText.text = LanguageUtils.getTextFormat (cityBuff.cityBuffDefine.tipName, cityBuff.cityBuffDefine.tagData[0]);
            }
            else
            {
                itemView.m_lbl_buffName_LanguageText.text = LanguageUtils.getText(cityBuff.cityBuffDefine.tag);
            }
            ClientUtils.LoadSprite(itemView.m_img_icon_PolygonImage, cityBuff.cityBuffDefine.icon);

            if (cityBuff.cityBuff != null && cityBuff.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
            {
                itemView.m_pb_rogressBar_ArabLayoutCompment.gameObject.SetActive(true);
                if (m_timers.ContainsKey(index))
                {
                    if (m_timers[index] != null)
                    {
                        m_timers[index].Cancel();
                        m_timers[index] = null;
                    }
                }
                if (itemView.data is int)
                {
                    int lastindex = (int)itemView.data;
                    if (m_timers.ContainsKey(lastindex))
                    {
                        if (m_timers[lastindex] != null)
                        {
                            m_timers[lastindex].Cancel();
                            m_timers[lastindex] = null;
                        }
                    }
                }
                itemView.data = index;
                itemView.m_pb_rogressBar_GameSlider.minValue = 0;
                itemView.m_pb_rogressBar_GameSlider.maxValue = cityBuff.cityBuffDefine.duration;
                if (cityBuff.cityBuff.expiredTime == -1)
                {
                    itemView.m_pb_rogressBar_GameSlider.minValue = 0;
                    itemView.m_pb_rogressBar_GameSlider.maxValue = cityBuff.cityBuffDefine.duration;
                    itemView.m_pb_rogressBar_GameSlider.value = 1;
                    itemView.m_lbl_buffTime_LanguageText.text = "(缺)永久";

                }
                else
                {
                    itemView.m_pb_rogressBar_GameSlider.minValue = 0;
                    itemView.m_pb_rogressBar_GameSlider.maxValue = cityBuff.cityBuffDefine.duration;
                    itemView.m_lbl_buffTime_LanguageText.text = ClientUtils.FormatCountDown((int)(cityBuff.cityBuff.expiredTime - ServerTimeModule.Instance.GetServerTime()));
                    itemView.m_pb_rogressBar_GameSlider.value = cityBuff.cityBuff.expiredTime - ServerTimeModule.Instance.GetServerTime();
                    m_timers[index] = Timer.Register(1.0f, () =>
                    {
                        if (cityBuff.cityBuff.expiredTime > ServerTimeModule.Instance.GetServerTime())
                        {
                            itemView.m_lbl_buffTime_LanguageText.text = ClientUtils.FormatCountDown((int)(cityBuff.cityBuff.expiredTime - ServerTimeModule.Instance.GetServerTime()));
                            itemView.m_pb_rogressBar_GameSlider.value = cityBuff.cityBuff.expiredTime - ServerTimeModule.Instance.GetServerTime();
                            itemView.m_pb_rogressBar_ArabLayoutCompment.gameObject.SetActive(true);
                        }
                        else
                        {
                            itemView.m_pb_rogressBar_ArabLayoutCompment.gameObject.SetActive(false);
                        }

                    }, null, true, false, view.vb);
                }
            }
            else
            {
                itemView.m_pb_rogressBar_ArabLayoutCompment.gameObject.SetActive(false);
            }
        }

        private void OnArrowBtnClick()
        {
            CoreUtils.uiManager.ShowUI(UI.s_cityManager);
            CoreUtils.uiManager.CloseUI(UI.s_buffList);
        }
    }
}