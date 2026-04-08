// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月5日
// Update Time         :    2019年12月5日
// Class Description   :    TestUICompomentMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using System;
using UnityEngine;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using PureMVC.Interfaces;
using Client;
using static Client.ListView;
using UnityEngine.EventSystems;

namespace Game {
    public class TestUICompomentMediator : GameMediator {
        #region Member
        public static string NameMediator = "TestUICompomentMediator";

        public List<Color> pageColorList = new List<Color>();

        private int m_listType;

        private Dictionary<int, bool> m_initedList = new Dictionary<int, bool>();

        private Dictionary<int, ListView> m_listView = new Dictionary<int, ListView>();

        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        private ListView.ListItem m_deleteItem;
        private float m_deleteTime;
        private float m_height;

        #endregion

        //IMediatorPlug needs
        public TestUICompomentMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public TestUICompomentView view;

        public override string[] ListNotificationInterests()
        {
            return new List<string>(){
                
            }.ToArray();
        }

        public override void HandleNotification(INotification notification)
        {
            switch (notification.Name)
            {
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
            if (m_deleteItem != null)
            {
                m_deleteTime = m_deleteTime - Time.deltaTime;
                view.m_ls_top_view_ListView.UpdateItemSize(m_deleteItem.index, m_height * m_deleteItem.go.transform.localScale.y);
                if (m_deleteTime <= 0)
                {
                    view.m_ls_top_view_ListView.RemoveAt(m_deleteItem.index);
                    m_deleteItem = null;
                }
            }
        }        

        protected override void InitData()
        {
            IsOpenUpdate = true;
            view.m_lbl_linkImageText_LinkImageText.text = "测试超链接<href=www.baidu.com><color=#24ACCC> 跳转链接</color></href>";
            view.m_lbl_linkImageText_LinkImageText.onHrefClick.RemoveAllListeners();
            view.m_lbl_linkImageText_LinkImageText.onHrefClick.AddListener((str)=> {
                Debug.LogError("str:"+str);
            });

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_ls_top_view_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_ls_bottom_view_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_ls_right_view_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_ls_left_view_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_page_view_PageView.ItemPrefabDataList);

            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);

        }

        #region ListView

        public void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }

            AssetLoadFinish();
        }

        public void AssetLoadFinish()
        {
            InitListView();
            InitScrollView();
            InitPageView();
        }

        public void InitListView()
        {
            m_listView[0] = view.m_ls_top_view_ListView;
            m_listView[1] = view.m_ls_bottom_view_ListView;
            m_listView[2] = view.m_ls_right_view_ListView;
            m_listView[3] = view.m_ls_left_view_ListView;

            m_listType = 0;

            OnInitList();
        }

        //更新item内容
        void ListItemByIndex(ListView.ListItem listItem)
        {
            //Debug.LogError("Index:" + listItem.index);
            ItemListTestView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemListTestView>(listItem.go);

            itemView.m_lbl_text_Text.text = listItem.index.ToString();
            itemView.m_btn_test_Button.onClick.RemoveAllListeners();
            itemView.m_btn_test_Button.onClick.AddListener(() => {
                if (m_listType == 0)
                {
                    m_deleteItem = listItem;
                    Animation ani = listItem.go.GetComponent<Animation>();
                    AnimationClip clip = ani.GetClip("MissionItemRemove");
                    m_deleteTime = clip.length;
                    m_height = view.m_ls_top_view_ListView.GetItemSizeByIndex(m_deleteItem.index);
                    if (ani != null)
                    {
                        ani.Play("MissionItemRemove");
                    }
                }
            });

            itemView.m_btn_print_Button.onClick.RemoveAllListeners();
            itemView.m_btn_print_Button.onClick.AddListener(() => {

            });
        }

        #endregion

        #region ScrollView

        public void InitScrollView()
        {
            //string itemPrefabName = "UI_Item_ListTest";
            //view.m_sv_scroll_view_ScrollView.LoadAsset(itemPrefabName, () =>
            //{
            //    view.m_sv_scroll_view_ScrollView.SetInitData(itemPrefabName, ScrollItemByIndex);

            //    for (int i = 0; i < 50; i++)
            //    {
            //        view.m_sv_scroll_view_ScrollView.AddItem(100f, i.ToString());
            //    }
            //});
        }

        //更新item内容
        void ScrollItemByIndex(ScrollView.ScrollItem scrollItem)
        {
            ItemListTestView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemListTestView>(scrollItem.GetGameObject());

            itemView.m_lbl_text_Text.text = scrollItem.index.ToString();

            itemView.m_btn_test_Button.onClick.RemoveAllListeners();
            itemView.m_btn_test_Button.onClick.AddListener(() => {
                ScrollView.ScrollItem item = view.m_sv_scroll_view_ScrollView.GetItemByIndex(scrollItem.index);
                view.m_sv_scroll_view_ScrollView.RemoveItem(item);
            });

            itemView.m_btn_print_Button.onClick.RemoveAllListeners();
            itemView.m_btn_print_Button.onClick.AddListener(() => {
                Debug.LogError("print index:" + scrollItem.tag);
            });
        }

        public void InitPageView()
        {
            pageColorList.Add(Color.gray);
            pageColorList.Add(Color.yellow);
            pageColorList.Add(Color.green);
            pageColorList.Add(Color.blue);
            pageColorList.Add(Color.cyan);

            Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
            prefabDic["UI_Item_PageView"] = m_assetDic["UI_Item_PageView"];
            PageView.FuncTab funcTab = new PageView.FuncTab();
            funcTab.ItemEnter = PageViewItemByIndex;

            view.m_page_view_PageView.SetInitData(prefabDic, funcTab);
            view.m_page_view_PageView.FillContent(6);
        }

        //更新item内容
        void PageViewItemByIndex(PageView.ListItem scrollItem)
        {
            ItemPageViewView itemView = MonoHelper.GetHotFixViewComponent<ItemPageViewView>(scrollItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<ItemPageViewView>(scrollItem.go);
                itemView.m_sv_list_view_ListView.SetParentView(view.m_page_view_PageView);
                itemView.m_sv_list_view_ListView.SetParent(view.m_page_view_ScrollRect);

                string itemPrefabName = "UI_Item_PageView";
                List<string> list = new List<string>();
                list.Add(itemPrefabName);

            }
            int index = GetResidue(scrollItem.index, 4);

            itemView.m_img_bg_Image.color = pageColorList[index];
            itemView.m_lbl_text_Text.text = scrollItem.index.ToString();
            //itemView.m_lbl_text_Text.text = scrollItem.index.ToString();

            //itemView.m_btn_test_Button.onClick.RemoveAllListeners();
            //itemView.m_btn_test_Button.onClick.AddListener(() => {
            //    ScrollView.ScrollItem item = view.m_sv_scroll_view_ScrollView.GetItemByIndex(scrollItem.index);
            //    view.m_sv_scroll_view_ScrollView.RemoveItem(item);
            //});

            //itemView.m_btn_print_Button.onClick.RemoveAllListeners();
            //itemView.m_btn_print_Button.onClick.AddListener(() => {
            //    Debug.LogError("print index:" + scrollItem.tag);
            //});
        }

        public int GetResidue(int num, int num2)
        {
            num = (num + 1) % num2;
            if (num == 0)
            {
                return (num2 - 1);
            }
            else
            {
                return (num - 1);
            }
        }

        #endregion

        protected override void BindUIEvent()
        {
            view.m_btn_demo_other_Button.onClick.AddListener(OtherDemo);
            view.m_btn_demo_list_Button.onClick.AddListener(ListDemo);
            view.m_btn_demo_page_Button.onClick.AddListener(PageViewDemo);
            view.m_btn_sd_ani_test_Button.onClick.AddListener(SmoothBarDemo);

            view.m_btn_top_GameButton.onClick.AddListener(ClickTop);
            view.m_btn_bottom_GameButton.onClick.AddListener(ClickBottom);
            view.m_btn_right_GameButton.onClick.AddListener(ClickRight);
            view.m_btn_left_GameButton.onClick.AddListener(ClickLeft);

            view.m_btn_initList_GameButton.onClick.AddListener(OnInitList);
            view.m_btn_delete_GameButton.onClick.AddListener(DeleteIndex);
            view.m_btn_insert_GameButton.onClick.AddListener(InsertIndex);
            view.m_btn_changeHeight_GameButton.onClick.AddListener(ChangeHeight);
            view.m_btn_scrollIndex_GameButton.onClick.AddListener(ScrollIndex);
            view.m_btn_switchIndex_GameButton.onClick.AddListener(SwitchIndex);
            view.m_btn_centerIndex_GameButton.onClick.AddListener(CenterIndex);
            view.m_btn_refreshIndex_GameButton.onClick.AddListener(RefreshIndex);


            view.m_btn_jumpPage_GameButton.onClick.AddListener(PageJump);

            LongClickDemoMul();
            LongClickDemoOnlyOne();
        }

        protected override void BindUIData()
        {

        }

        #endregion

        private void ClickTop()
        {
            m_listType = 0;
            OnInitList();
        }

        private void ClickBottom()
        {
            m_listType = 1;
            OnInitList();
        }

        private void ClickRight()
        {
            m_listType = 2;
            OnInitList();
        }

        private void ClickLeft()
        {
            m_listType = 3;
            OnInitList();
        }

        private void OnInitList()
        {
            if (m_listType == 0)
            {
                if (m_initedList.ContainsKey(0))
                {
                    view.m_ls_top_view_ListView.RefreshAndRestPos(5);
                }
                else
                {
                    m_initedList[0] = true;
                 
                    Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
                    prefabNameList["UI_Item_ListTopTest"] = m_assetDic["UI_Item_ListTopTest"];

                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ListItemByIndex;

                    view.m_ls_top_view_ListView.SetInitData(prefabNameList, funcTab);
                    view.m_ls_top_view_ListView.FillContent(5);
                }
            }
            else if (m_listType == 1)
            {
                if (m_initedList.ContainsKey(1))
                {
                    view.m_ls_bottom_view_ListView.RefreshAndRestPos(10);
                }
                else
                {
                    m_initedList[1] = true;

                    Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
                    prefabNameList["UI_Item_ListBottomTest"] = m_assetDic["UI_Item_ListBottomTest"];
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ListItemByIndex;
                    view.m_ls_bottom_view_ListView.SetInitData(prefabNameList, funcTab);
                    view.m_ls_bottom_view_ListView.FillContent(10);
                 
                }
            }
            else if (m_listType == 2)
            {
                if (m_initedList.ContainsKey(2))
                {
                    view.m_ls_right_view_ListView.RefreshAndRestPos(10);
                }
                else
                {
                    m_initedList[2] = true;

                    Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
                    prefabNameList["UI_Item_ListRightTest"] = m_assetDic["UI_Item_ListRightTest"];
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ListItemByIndex;
                    view.m_ls_right_view_ListView.SetInitData(prefabNameList, funcTab);
                    view.m_ls_right_view_ListView.FillContent(10);
                }
            }
            else
            {
                if (m_initedList.ContainsKey(3))
                {
                    view.m_ls_left_view_ListView.RefreshAndRestPos(10);
                }
                else
                {
                    m_initedList[3] = true;

                    Dictionary<string, GameObject> prefabNameList = new Dictionary<string, GameObject>();
                    prefabNameList["UI_Item_ListLeftTest"] = m_assetDic["UI_Item_ListLeftTest"];
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = ListItemByIndex;

                    view.m_ls_left_view_ListView.SetInitData(prefabNameList, funcTab);
                    view.m_ls_left_view_ListView.FillContent(10);
                }
            }

        }

        private void DeleteIndex()
        {
            int index = ParseInt(view.m_ipt_index_GameInput.text);
            m_listView[m_listType].RemoveAt(index);
        }

        private void InsertIndex()
        {
            int index = ParseInt(view.m_ipt_index_GameInput.text);
            m_listView[m_listType].Insert(index);
        }

        private void ChangeHeight()
        {
            int index = ParseInt(view.m_ipt_index_GameInput.text);          
            m_listView[m_listType].UpdateItemSize(index, m_listView[m_listType].GetItemSizeByIndex(index) + 30);
        }

        private void ScrollIndex()
        {
            int index = ParseInt(view.m_ipt_index_GameInput.text);
            m_listView[m_listType].ScrollPanelToItemIndex(index);
        }

        private void SwitchIndex()
        {
            int index = ParseInt(view.m_ipt_index_GameInput.text);
            m_listView[m_listType].MovePanelToItemIndex(index);
        }

        private void CenterIndex()
        {
            int index = ParseInt(view.m_ipt_index_GameInput.text);
            m_listView[m_listType].ScrollList2IdxCenter(index);
        }

        private void RefreshIndex()
        {
            int index = ParseInt(view.m_ipt_index_GameInput.text);
            m_listView[m_listType].RefreshItem(index);
        }

        private void OtherDemo()
        {
            view.m_pl_demo_page_Image.gameObject.SetActive(false);
            view.m_pl_demo_other_Image.gameObject.SetActive(true);
            view.m_pl_demo_list_Image.gameObject.SetActive(false);
        }

        private void ListDemo()
        {
            view.m_pl_demo_page_Image.gameObject.SetActive(false);
            view.m_pl_demo_other_Image.gameObject.SetActive(false);
            view.m_pl_demo_list_Image.gameObject.SetActive(true);

        }

        private void PageViewDemo()
        {
            view.m_pl_demo_page_Image.gameObject.SetActive(true);
            view.m_pl_demo_other_Image.gameObject.SetActive(false);
            view.m_pl_demo_list_Image.gameObject.SetActive(false);
        }

        private void SmoothBarDemo()
        {
            view.m_sd_demo_SmoothBar.SetValue(view.m_sd_demo_Slider.value + 0.5f);
        }

        //多次触发长按事件
        private void LongClickDemoMul()
        {
            view.m_btn_long_mul_LongClickButton.reqHoldTimeFristTime = 1f;
            view.m_btn_long_mul_LongClickButton.reqHoldTimeOtherTime = 0.1f;
            //监听长按事件
            view.m_btn_long_mul_LongClickButton.action = () =>
            {
                int num = int.Parse(view.m_lbl_long_mul_Text.text);
                view.m_lbl_long_mul_Text.text = (num + 1).ToString();
            };
            UIClickListener listener = view.m_btn_long_mul_Button.gameObject.AddComponent<UIClickListener>();
            //监听按下事件
            listener.onPointerDown = (data)=> {
                Debug.Log("按下");
            };
            //监听放开事件
            listener.onPointerUp = (data) => {
                Debug.Log("放开");
            };
        }

        //仅触发一次长按事件
        private void LongClickDemoOnlyOne()
        {
            view.m_btn_long_onlyone_LongClickButton.reqHoldTimeFristTime = 1f;
            view.m_btn_long_onlyone_LongClickButton.action = () =>
            {
                int num = int.Parse(view.m_lbl_long_onlyone_Text.text);
                view.m_lbl_long_onlyone_Text.text = (num + 1).ToString();

            };
        }

        private void PageJump()
        {
            string text = view.m_ipt_languageInputField_GameInput.text;
            int index = int.Parse(text);
            view.m_page_view_PageView.ScrollPanelToItemIndex(index);
        }

        /// <summary>
        /// 字符串转int
        /// </summary>
        /// <param name="intStr">要进行转换的字符串</param>
        /// <param name="defaultValue">默认值，默认：0</param>
        /// <returns></returns>
        public static int ParseInt(string intStr, int defaultValue = 0)
        {
            int parseInt;
            if (int.TryParse(intStr, out parseInt))
                return parseInt;
            return defaultValue;
        }
    }
}