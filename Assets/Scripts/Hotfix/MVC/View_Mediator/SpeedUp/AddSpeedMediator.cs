// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2020年2月10日
// Update Time         :    2020年2月10日
// Class Description   :    AddSpeedMediator
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
using System;
using Data;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Game {

    public enum EnumSpeedUpType
    {
        //通用加速
        common = 20101,
        //建造加速
        buiding = 20102,
        //训练加速
        train = 20103,
        //研究加速
        research = 20104,
        //治疗加速
        heal = 20105,
    }
    public class SpeedUpData 
    {
        //加速类型
        public EnumSpeedUpType type;
        //完成的时间戳
        public long finishTime;
        //总共需要的时间
        public long totalTime;
        //加速的图片
        public Sprite icon;
        //或加速的图片资源
        public string iconRes;
        //建筑和训练发对应队列索引
        public QueueInfo queue;
        //点击取消队列的回调
        public Action cancelCallback;
    }

    public class AddSpeedMediator : GameMediator {
        #region Member
        public static string NameMediator = "AddSpeedMediator";

        private BagProxy m_bagProxy;
        private CurrencyProxy m_currencyProxy;

        private SpeedUpData m_speedData;
        private List<ItemDefine> m_itemList = new List<ItemDefine>();
        private List<KeyValuePair<int, int>> m_itemIndex = new List<KeyValuePair<int, int>>();
        private bool isInitedListView;
        private Dictionary<string, GameObject> m_assetDic;
        private int showItemid = -1;
        private int batchUseNum;
        private Timer m_timer;

        //        private bool canSendSproto = true;
        private bool TipLoading = false;

        private int m_speedItemIndex = -1;

        #endregion

        //IMediatorPlug needs
        public AddSpeedMediator(object viewComponent ):base(NameMediator, viewComponent ) {
            this.IsOpenUpdate = true;
        }


        public AddSpeedView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                CmdConstant.CloseAddSpeed,
                CmdConstant.ItemInfoChange,
                Role_SpeedUp.TagName,
                CmdConstant.FuncGuideFiveSpeed,
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
                case CmdConstant.ItemInfoChange:
                    if (!isInitedListView)
                    {
                        return;
                    }
                    InitView();
                    break;
                case Role_SpeedUp.TagName:
                    CoreUtils.audioService.PlayOneShot("Sound_Ui_Speedup");
//                    canSendSproto = true;
                    Role_SpeedUp.response res = notification.Body as Role_SpeedUp.response;
                    if(res!=null&&res.HasFinishTime)
                    {
                        m_speedData.finishTime = res.finishTime;
                        OnSliderChange();
                    }
                    break;
                case CmdConstant.FuncGuideFiveSpeed:
                    //5分钟加速功能引导
                    FuncGuideFiveSpeedProcess();
                    break;
                case CmdConstant.CloseAddSpeed:
                    OnClose();
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
            if(m_timer!=null)
            {
                if(!m_timer.isCancelled)
                {
                    m_timer.Cancel();
                }
                m_timer = null;
            }
            CloseTip();
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
        }        

        protected override void InitData()
        {
            m_bagProxy = AppFacade.GetInstance().RetrieveProxy(BagProxy.ProxyNAME) as BagProxy;
            m_currencyProxy = AppFacade.GetInstance().RetrieveProxy(CurrencyProxy.ProxyNAME) as CurrencyProxy;
            m_speedData = view.data as SpeedUpData;
            m_speedData.totalTime = m_speedData.queue.firstFinishTime - m_speedData.queue.beginTime;
            m_speedData.finishTime = m_speedData.queue.finishTime ;
        }

        protected override void BindUIEvent()
        {
            view.m_UI_Model_Window_Type1.m_btn_close_GameButton.onClick.AddListener(OnClose);
            view.m_btn_stopOverSize_GameButton.onClick.AddListener(OnCancel);
            view.m_sv_list_ListView.onDragBegin = CloseTip;
        }
        private Alert m_alert;
        private void OnClose()
        {
            CoreUtils.uiManager.CloseUI(UI.s_speedUp);
            m_alert?.DestroySelf();
            CloseTip();
        }

        private void OnCancel()
        {
            m_speedData.cancelCallback?.Invoke();
            //CoreUtils.uiManager.CloseUI(UI.s_speedUp);
        }

        protected override void BindUIData()
        {
            ClientUtils.PreLoadRes(view.gameObject,view.m_sv_list_ListView.ItemPrefabDataList,(tmpDic)=>
            {
                m_assetDic = tmpDic;
                if (m_assetDic.Count < 1)
                {
                    return;
                }
                InitView();
                AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideCheck, (int)EnumFuncGuide.UseSpeedItem);
            });

            PolygonImage img = GetIMG(m_speedData.type);
            if(img!=null)
            {
                if (m_speedData.icon != null)
                {
                    img.sprite = m_speedData.icon;
                }
                else
                {
                    ClientUtils.LoadSprite(img, m_speedData.iconRes);
                }
            }
            else
            {
                CoreUtils.assetService.Instantiate(m_speedData.iconRes,(go)=>
                {
                    if(view!=null&&view.gameObject)
                    {
                        go.transform.SetParent(view.m_pl_build_ArabLayoutCompment.transform);
                        go.transform.localScale = Vector3.one * 0.3f;
                        go.transform.localPosition = Vector3.zero;
                    }
                    else
                    {
                        GameObject.DestroyImmediate(go);
                    }
                });
            }
            if(m_speedData.type == EnumSpeedUpType.heal)
            {
                view.m_img_hospital_PolygonImage.gameObject.SetActive(true);
                view.m_img_icon_PolygonImage.gameObject.SetActive(false);
            }

            if(m_speedData.cancelCallback==null)
            {
                view.m_img_stop_PolygonImage.gameObject.SetActive(false);
            }

            long leftTime = m_speedData.finishTime - ServerTimeModule.Instance.GetServerTime();
            if (leftTime>0)
            {
                OnSliderChange();
                m_timer = Timer.Register(1f, OnTimerComplete);
            }
            else
            {
                Debug.Log("特殊情况处理");
                view.m_pb_rogressBar_GameSlider.value = 1;
                view.m_lbl_time_LanguageText.text = LanguageUtils.getText(700023);
                Timer.Register(0.02f, () => {
                    OnClose();
                });
            }
        }

        private PolygonImage GetIMG(EnumSpeedUpType type)
        {
            view.m_pl_build_ArabLayoutCompment.gameObject.SetActive(false);
            view.m_img_hospital_PolygonImage.gameObject.SetActive(false);
            view.m_img_icon_PolygonImage.gameObject.SetActive(false);
            view.m_img_research_PolygonImage.gameObject.SetActive(false);
            switch (type)
            {
                case EnumSpeedUpType.heal:
                    view.m_img_hospital_PolygonImage.gameObject.SetActive(true);
                    return view.m_img_hospital_PolygonImage;
                case EnumSpeedUpType.research:
                    view.m_img_research_PolygonImage.gameObject.SetActive(true);
                    return view.m_img_researchicon_PolygonImage;
                case EnumSpeedUpType.train:
                    view.m_img_icon_PolygonImage.gameObject.SetActive(true);
                    return view.m_img_icon_PolygonImage;
                default:
                    view.m_pl_build_ArabLayoutCompment.gameObject.SetActive(true);
                    return null;
            }
        }

        private void OnTimerComplete()
        {
            long leftTime = m_speedData.finishTime-ServerTimeModule.Instance.GetServerTime();
            if (leftTime<=0)
            {
                //完成
                OnClose();
                return;
            }
            else
            {
                OnSliderChange();
                m_timer = Timer.Register(1f, OnTimerComplete);
            }
        }

        private void OnSliderChange()
        {
            long leftTime = m_speedData.finishTime - ServerTimeModule.Instance.GetServerTime();
            view.m_pb_rogressBar_GameSlider.value = 1- (float)leftTime / (float)m_speedData.totalTime;
            if(leftTime>=0)
            {
                TimeSpan ts = new TimeSpan(0, 0, (int)leftTime);
                if (ts.Days > 0)
                {
                    view.m_lbl_time_LanguageText.text = string.Format("{0} {1:D2}:{2:D2}:{3:D2}", string.Format(LanguageUtils.getText(128004), ts.Days), ts.Hours, ts.Minutes, ts.Seconds);
                }
                else
                {
                    view.m_lbl_time_LanguageText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", ts.Hours, ts.Minutes, ts.Seconds);
                }
            }
            else
            {
                view.m_lbl_time_LanguageText.text = "00:00:00";
            }
        }

        #endregion

        private void InitView()
        {
            if(m_speedData==null)
            {
                Debug.LogError("SpeedUpData参数为空");
                return;
            }

            if (!isInitedListView)
            {
                isInitedListView = true;
                ListView.FuncTab funcTab = new ListView.FuncTab();
                funcTab.ItemEnter = OnItemEnter;
                view.m_sv_list_ListView.SetInitData(m_assetDic, funcTab);
            }

            m_itemList = CoreUtils.dataService.QueryRecords<ItemDefine>().FindAll((i)=>
            {
                return (i.subType == (int)m_speedData.type || i.subType == (int)EnumSpeedUpType.common);
            });
            m_itemIndex.Clear();
            for(int i = 0;i<m_itemList.Count;i++)
            {
                int num = (int)m_bagProxy.GetItemNum(m_itemList[i].ID);
                if (m_itemList[i].subType != (int)EnumSpeedUpType.common&&num<=0)//代币购买的只会显示通用加速
                {
                    continue;
                }
                else if(num <= 0&& m_itemList[i].shopPrice<=0)//单价小于0的也不显示
                {
                    continue;
                }
                else
                {
                    m_itemIndex.Add(new KeyValuePair<int, int>(num, i));
                    if(m_itemList[i].subType == (int)EnumSpeedUpType.common&&num>0&& m_itemList[i].shortcutPrice>0)
                    {
                        m_itemIndex.Add(new KeyValuePair<int, int>(0, i));
                    }
                }
            }
            m_itemIndex.Sort((r1, r2) =>
            {
                long num1 = (r1.Key == 0 ? 1000000000 : 0) - m_itemList[r1.Value].subType*100 + r1.Value;
                long num2 = (r2.Key == 0 ? 1000000000 : 0) - m_itemList[r2.Value].subType*100 + r2.Value;
                return num1.CompareTo(num2);
            });
            view.m_sv_list_ListView.FillContent(m_itemIndex.Count);
            long itemnum = m_bagProxy.GetItemNum(showItemid);
            if (itemnum <= 1)
            {
                CloseTip();
            }
        }
        public void CloseTip(UnityEngine.EventSystems.PointerEventData data)
        {
            CloseTip();
        }
        public void CloseTip()
        {
            if (powTipView != null && powTipView.gameObject != null)
            {
                CoreUtils.assetService.Destroy(powTipView.gameObject);
                powTipView = null;
            }
            if (m_tipView != null)
            {
                m_tipView.Close();
                m_tipView = null;
            }
            showItemid = -1;
            TipLoading = false;
        }

        private void OnItemEnter(ListView.ListItem item)
        {
            if (!item.HasGameObject())
            {
                return;
            }

            int index = m_itemIndex[item.index].Value;
            int itemNum = m_itemIndex[item.index].Key;
            long itemIndex = m_bagProxy.GetItemIndex(m_itemList[index].ID);
            UI_Item_StandardUseItemView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
            itemView.m_lbl_itemName_LanguageText.text = LanguageUtils.getText(m_itemList[index].l_nameID);
            itemView.m_lbl_itemDesc_LanguageText.text = string.Format(LanguageUtils.getText(m_itemList[index].l_desID), m_itemList[index].topData.ToString("N0"));
            itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_count_LanguageText.text = string.Empty;
            itemView.m_UI_Item_Bag.m_UI_Model_Item.m_lbl_desc_LanguageText.text = LanguageUtils.getTextFormat(m_itemList[index].l_topID, m_itemList[index].topData.ToString("N0"));
            //设置品质图片
            ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_quality_PolygonImage, RS.ItemQualityBg[m_itemList[index].quality-1]);
            ClientUtils.LoadSprite(itemView.m_UI_Item_Bag.m_UI_Model_Item.m_img_icon_PolygonImage, m_itemList[index].itemIcon);

            if(showItemid== m_itemList[index].ID && itemNum>1)//显示批量使用
            {
                 batchUseNum = GetBatchUseNumber(m_itemList[index].data1);
                batchUseNum = batchUseNum > itemNum ? itemNum : batchUseNum;
                ShowPopMemu(item.index, $"X{batchUseNum}", () =>
                {
                    //                    if (!canSendSproto)
                    //                    {
                    //                        return;
                    //                    }
                    int overTime = OverTime(m_itemList[index].data1 * batchUseNum);
                    bool autoClose = IsFinished(m_itemList[index].data1 * batchUseNum);
                    if (overTime > 10)
                    {
                        Alert.CreateAlert(LanguageUtils.getTextFormat(300049, overTime.ToString("N0"))).SetRightButton(() =>
                        {
                            if (m_speedData.queue.finishTime <= 0)
                            {
                                OnClose();
                                return;
                            }
                            SendSproto(m_itemList[index].ID, false, batchUseNum);
                            showItemid = -1;
                            OnClose();
                        }).SetLeftButton().Show();
                    }
                    else
                    {
                        SendSproto(m_itemList[index].ID, false, batchUseNum);
                        showItemid = -1;
                        if (autoClose)
                        {
                            OnClose();
                        }
                    }

                });
            }

            if (itemNum<=0)//未拥有
            {
                itemView.m_UI_Model_Yellow.gameObject.SetActive(true);
                itemView.m_UI_Model_Blue_big.gameObject.SetActive(false);
                itemView.m_lbl_itemCount_LanguageText.text = string.Empty;
                itemView.m_UI_Model_Yellow.m_lbl_line2_LanguageText.text = m_itemList[index].shortcutPrice.ToString("N0");
                itemView.m_UI_Model_Yellow.m_lbl_line1_LanguageText.text = LanguageUtils.getText(300097);
                itemView.m_UI_Model_Yellow.m_lbl_line2_ContentSizeFitter.SetLayoutHorizontal();
                itemView.m_UI_Model_Yellow.m_pl_line2_HorizontalLayoutGroup.SetLayoutHorizontal();
                itemView.m_UI_Model_Yellow.m_btn_languageButton_VerticalLayoutGroup.SetLayoutVertical();
                ClientUtils.LoadSprite(itemView.m_UI_Model_Yellow.m_img_icon2_PolygonImage, m_currencyProxy.GeticonIdByType(EnumCurrencyType.denar));
                itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_Yellow.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
//                    if (!canSendSproto)
//                    {
//                        return;
//                    }
                    if (!m_currencyProxy.ShortOfDenar(m_itemList[index].shortcutPrice))
                    {
                        m_alert = UIHelper.DenarCostRemain(m_itemList[index].shortcutPrice, () =>
                        {
                            int overTime = OverTime(m_itemList[index].data1);
                            bool autoClose = IsFinished(m_itemList[index].data1);
                            if (overTime > 10)
                            {
                            Alert.CreateAlert(LanguageUtils.getTextFormat(300049, overTime.ToString("N0"))).SetRightButton(() =>
                                {
                                    if (m_speedData.queue.finishTime <= 0)
                                    {
                                        OnClose();
                                        return;
                                    }
                                    SendSproto(m_itemList[index].ID, true, 1);
                                    showItemid = -1;
                                    CloseTip();
                                    OnClose();
                                }).SetLeftButton().Show();
                            }
                            else
                            {
                                SendSproto(m_itemList[index].ID, true, 1);
                                showItemid = -1;
                                CloseTip();
                                if (autoClose)
                                {
                                    OnClose();
                                }
                            }
                        });
                
                    }
                });
            }
            else//已拥有
            {
                itemView.m_UI_Model_Yellow.gameObject.SetActive(false);
                itemView.m_UI_Model_Blue_big.gameObject.SetActive(true);
                itemView.m_lbl_itemCount_LanguageText.text = string.Format(LanguageUtils.getText(110001), itemNum);
                itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.RemoveAllListeners();
                itemView.m_UI_Model_Blue_big.m_btn_languageButton_GameButton.onClick.AddListener(() =>
                {
//                    if(!canSendSproto)
//                    {
//                        return;
//                    }
                    int overTime = OverTime(m_itemList[index].data1);
                    bool autoClose = IsFinished(m_itemList[index].data1);
                    if (overTime > 10)
                    {
                        Alert.CreateAlert(LanguageUtils.getTextFormat(300049,overTime.ToString("N0"))).SetRightButton(()=>
                        {
                            if (m_speedData.queue.finishTime <= 0)
                            {
                                OnClose();
                                return;
                            }
                            SendSproto(m_itemList[index].ID, false, 1);
                            if (showItemid != m_itemList[index].ID)
                            {
                                CloseTip();
                            }
                            showItemid = m_itemList[index].ID;
                            OnClose();
                        }).SetLeftButton().Show();
                    }
                    else
                    {
                        SendSproto(m_itemList[index].ID,false,1);
                        if (showItemid != m_itemList[index].ID)
                        {
                            CloseTip();
                        }
                        showItemid = m_itemList[index].ID;
                        if(autoClose)
                        {
                            OnClose();
                        }
                    }
                });
            }


        }

        private void SendSproto(int itemId,bool costDenar,int itemNum)
        {
//            if(!canSendSproto)
//            {
//                Timer.Register(1f,()=>
//                {
//                    canSendSproto = true;
//                });
//                return;
//            }
            Role_SpeedUp.request req = new Role_SpeedUp.request();
            req.costDenar = costDenar;
            req.queueIndex = m_speedData.queue.queueIndex;
            req.itemId = itemId;
            req.itemNum = itemNum;
            req.type = (long)m_speedData.type;
            //            canSendSproto = false;
            //Debug.LogError(itemId + " " + itemNum);
            AppFacade.GetInstance().SendSproto(req);
        }
        private UI_Pop_UseAddItemView powTipView;
        private HelpTip m_tipView;
        private void ShowPopMemu(int index, string btnName, UnityAction btnAction)
        {
            if (powTipView != null)
            {
                powTipView.m_UI_left.SetText(btnName);
                return;
            }

            if (TipLoading)
            {
                return;
            }
            TipLoading = true;

            if (m_tipView != null)
            {
                m_tipView.Close();
                m_tipView = null;
            }

            CoreUtils.assetService.Instantiate("UI_Pop_UseAddItem", (obj) =>
            {
                TipLoading = false;
                if (showItemid == -1 || batchUseNum < 1)
                {
                    CoreUtils.assetService.Destroy(obj);
                    return;
                }
                powTipView = MonoHelper.GetOrAddHotFixViewComponent<UI_Pop_UseAddItemView>(obj);

                powTipView.m_UI_left.gameObject.SetActive(true);
                powTipView.m_UI_right.gameObject.SetActive(false);

                powTipView.m_UI_left.SetText(btnName);
                ClientUtils.UIReLayout(powTipView.m_pl_bg_ContentSizeFitter.gameObject);

                powTipView.m_UI_left.m_btn_languageButton_GameButton.onClick.AddListener(btnAction);

                var item = view.m_sv_list_ListView.GetItemByIndex(index);
                //Debug.LogError(index);
                if (item != null)
                {
                    UI_Item_StandardUseItemView itemView = MonoHelper.AddHotFixViewComponent<UI_Item_StandardUseItemView>(item.go);
                    if (LanguageUtils.IsArabic())
                    {
                        m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.gameObject.GetComponent<RectTransform>().sizeDelta, itemView.m_UI_Model_Blue_big.m_root_RectTransform.position)
                     .SetStyle(HelpTipData.Style.arrowLeft).SetOffset(100).UnEnableTouchClose().Show();
                    }
                    else
                    {
                        m_tipView = HelpTip.CreateTip(powTipView.gameObject, powTipView.m_pl_bg_ContentSizeFitter.gameObject.GetComponent<RectTransform>().sizeDelta, itemView.m_UI_Model_Blue_big.m_root_RectTransform.position)
                   .SetStyle(HelpTipData.Style.arrowRight).SetOffset(100).UnEnableTouchClose().Show();
                    }

                }
            });
        }
        private int GetBatchUseNumber(int seconds)
        {
            int num = Mathf.FloorToInt((m_speedData.finishTime - ServerTimeModule.Instance.GetServerTime()) / (float)seconds)-1;
            return num<=0?1:num;
        }
        
        private int OverTime(int totalSeconds)
        {
//            if(!canSendSproto)
//            {
//                return 0;
//            }
            long diffTime = m_speedData.finishTime - ServerTimeModule.Instance.GetServerTime();
            if (diffTime < 0)
            {
                Debug.LogFormat("m_speedData.finishTime:{0}", m_speedData.finishTime);
                return 0;
            }
            long overTime = totalSeconds - diffTime;
            return (int)overTime/60;
        }

        private bool IsFinished(int seconds)
        {
            return m_speedData.finishTime - ServerTimeModule.Instance.GetServerTime() <= seconds;
        }

        //5分钟加速功能引导
        private void FuncGuideFiveSpeedProcess()
        {
            if (m_itemIndex == null)
            {
                return;
            }
            int findIndex = -1;
            int count = m_itemList.Count;
            for (int i = 0; i < m_itemIndex.Count; i++)
            {
                int index = m_itemIndex[i].Value;
                if (index < count)
                {
                    if (m_itemList[index].ID == 201010013)
                    {
                        findIndex = i;
                        break;
                    }
                }
            }
            if (findIndex < 0)
            {
                return;
            }
            m_speedItemIndex = findIndex;
            view.m_sv_list_ListView.MovePanelToItemIndex(findIndex);
            AppFacade.GetInstance().SendNotification(CmdConstant.FuncGuideId, 1003);
        }

        public int GetSpeedItemIndex()
        {
            return m_speedItemIndex;
        }
    }
}