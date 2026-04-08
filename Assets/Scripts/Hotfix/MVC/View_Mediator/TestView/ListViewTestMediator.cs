// =============================================================================== 
// Author              :    Gen By Tools
// Create Time         :    2019年12月9日
// Update Time         :    2019年12月9日
// Class Description   :    ListViewTestMediator
// Copyright IGG All rights reserved.
// ===============================================================================

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PureMVC.Patterns;
using Skyunion;
using Client;
using PureMVC.Interfaces;

namespace Game {
    public class ListViewTestMediator : GameMediator {
        #region Member
        public static string NameMediator = "ListViewTestMediator";


        private enum ListType
        {
            ListView = 0,
            ScrollView = 1,
            PageView = 2,
        }

        private ListType m_type = ListType.ListView;

        private int currentIndex;

        private bool m_isInitPageView;
        public List<Color> pageColorList = new List<Color>();
        private Dictionary<string, GameObject> m_assetDic = new Dictionary<string, GameObject>();

        #endregion

        //IMediatorPlug needs
        public ListViewTestMediator(object viewComponent ):base(NameMediator, viewComponent ) {}


        public ListViewTestView view;

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
            AppFacade.GetInstance().SendNotification(CmdConstant.ReturnToFullView);
        }

        public override void PrewarmComplete(){
            
        }   

        public override void Update()
        {
            
        }        

        protected override void InitData()
        {
            pageColorList.Add(Color.gray);
            pageColorList.Add(Color.yellow);
            pageColorList.Add(Color.green);
            pageColorList.Add(Color.blue);
            pageColorList.Add(Color.cyan);

            List<string> prefabNames = new List<string>();
            prefabNames.AddRange(view.m_sv_listview_ListView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_sv_scrollview_ScrollView.ItemPrefabDataList);
            prefabNames.AddRange(view.m_sv_page_view_PageView.ItemPrefabDataList);
            ClientUtils.PreLoadRes(view.gameObject, prefabNames, LoadFinish);
        }

        protected override void BindUIEvent()
        {
            view.m_btn_listview_Button.onClick.AddListener(OnListViewBtn);
            view.m_btn_scrollview_Button.onClick.AddListener(OnScrollViewBtn);
            view.m_btn_pageview_Button.onClick.AddListener(OnPageViewBtn);

            view.m_btn_func0_Button.onClick.AddListener(OnFunction0);
            view.m_btn_func1_Button.onClick.AddListener(OnFunction1);
            view.m_btn_func2_Button.onClick.AddListener(OnFunction2);
            view.m_btn_func3_Button.onClick.AddListener(OnFunction3);
            view.m_btn_func4_Button.onClick.AddListener(OnFunction4);
            view.m_btn_func5_Button.onClick.AddListener(OnFunction5);
            view.m_btn_func6_Button.onClick.AddListener(OnFunction6);
            view.m_btn_func7_Button.onClick.AddListener(OnFunction7);

            view.m_btn_pageJump_Button.onClick.AddListener(OnPageJump);

            view.m_ipt_field_InputField.onValueChanged.AddListener(OnValueChanged);
        }

        protected override void BindUIData()
        {

        }
       
        #endregion

        private void LoadFinish(Dictionary<string, IAsset> dic)
        {
            foreach (var data in dic)
            {
                m_assetDic[data.Key] = data.Value.asset() as GameObject;
            }
        }

        private void OnListViewBtn()
        {
            m_type = ListType.ListView;
            view.m_img_listview_Image.gameObject.SetActive(false);
            view.m_img_scrollview_Image.gameObject.SetActive(true);
            view.m_sv_listview_ScrollRect.gameObject.SetActive(true);
            view.m_sv_scrollview_ScrollRect.gameObject.SetActive(false);

            view.m_img_pageview_Image.gameObject.SetActive(true);
            view.m_sv_page_view_PageView.gameObject.SetActive(false);
            view.m_sv_functions_ScrollRect.gameObject.SetActive(true);
            view.m_btn_pageJump_Button.gameObject.SetActive(false);
        }

        private void OnScrollViewBtn()
        {
            m_type = ListType.ScrollView;
            view.m_img_listview_Image.gameObject.SetActive(true);
            view.m_img_scrollview_Image.gameObject.SetActive(false);
            view.m_sv_listview_ScrollRect.gameObject.SetActive(false);
            view.m_sv_scrollview_ScrollRect.gameObject.SetActive(true);

            view.m_img_pageview_Image.gameObject.SetActive(true);
            view.m_sv_page_view_PageView.gameObject.SetActive(false);
            view.m_sv_functions_ScrollRect.gameObject.SetActive(true);
            view.m_btn_pageJump_Button.gameObject.SetActive(false);
        }

        private void OnPageViewBtn()
        {
            view.m_img_pageview_Image.gameObject.SetActive(false);
            view.m_img_listview_Image.gameObject.SetActive(true);
            view.m_img_scrollview_Image.gameObject.SetActive(true);

            view.m_sv_listview_ScrollRect.gameObject.SetActive(true);
            view.m_sv_scrollview_ScrollRect.gameObject.SetActive(false);

            view.m_sv_page_view_PolygonImage.gameObject.SetActive(false);
            view.m_sv_page_view_PageView.gameObject.SetActive(true);
            view.m_sv_functions_ScrollRect.gameObject.SetActive(false);
            view.m_btn_pageJump_Button.gameObject.SetActive(true);

            if (m_isInitPageView)
            {

                view.m_sv_page_view_PageView.FillContent(6);
            }
            else
            {
                m_isInitPageView = true;

                Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
                prefabDic["UI_Item_PageView"] = m_assetDic["UI_Item_PageView"];
                PageView.FuncTab funcTab = new PageView.FuncTab();
                funcTab.ItemEnter = PageViewItemByIndex;

                view.m_sv_page_view_PageView.SetInitData(prefabDic, funcTab);
                view.m_sv_page_view_PageView.FillContent(6);
            }
        }

        //更新item内容
        void PageViewItemByIndex(PageView.ListItem scrollItem)
        {
            ItemPageViewView itemView = MonoHelper.GetHotFixViewComponent<ItemPageViewView>(scrollItem.go);
            if (itemView == null)
            {
                itemView = MonoHelper.AddHotFixViewComponent<ItemPageViewView>(scrollItem.go);
                itemView.m_sv_list_view_ListView.SetParentView(view.m_sv_page_view_PageView);
                itemView.m_sv_list_view_ListView.SetParent(view.m_sv_page_view_ScrollRect);

                string itemPrefabName = "UI_Item_PageView";
                List<string> list = new List<string>();
                list.Add(itemPrefabName);
                ListView.FuncTab funcTab = new ListView.FuncTab();
                itemView.m_sv_list_view_ListView.SetInitData(new Dictionary<string, GameObject>(), funcTab);
            }
            int index = GetResidue(scrollItem.index, 4);

            itemView.m_img_bg_Image.color = pageColorList[index];
            itemView.m_lbl_text_Text.text = scrollItem.index.ToString();
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

        private void OnValueChanged(string value)
        {
            currentIndex = int.Parse(value);
        }


        #region functions
        private bool m_type1Clear;
        private bool m_type2Clear;
        private void OnFunction0()
        {
            if (m_type == ListType.ListView)
            {
                if(!m_type1Clear)
                {
                    m_type1Clear = true;
                    view.m_lbl_fun0_Text.text = "删除列表";
                    ListView.FuncTab funcTab = new ListView.FuncTab();
                    funcTab.ItemEnter = OnInitItem;
                    view.m_sv_listview_ListView.SetInitData(m_assetDic, funcTab);
                    view.m_sv_listview_ListView.FillContent(10);
                }
                else
                {
                    m_type1Clear = false;
                    view.m_lbl_fun0_Text.text = "初始化列表";
                    view.m_sv_listview_ListView.Clear();
                }
            }
            else
            {
                if(!m_type2Clear)
                {
                    m_type2Clear = true;
                    view.m_lbl_fun0_Text.text = "删除列表";

                    ScrollView.FuncTab funcTab = new ScrollView.FuncTab();
                    funcTab.ItemEnter = OnInitScrollItem;

                    view.m_sv_scrollview_ScrollView.SetInitData(m_assetDic["UI_Item_ListViewChild"], funcTab);
                    for (int i = 0; i < 3; i++)
                    {
                        view.m_sv_scrollview_ScrollView.AddItem(120f, i.ToString());
                    }

                }
                else
                {
                    m_type2Clear = false;
                    view.m_lbl_fun0_Text.text = "初始化列表";
                    view.m_sv_scrollview_ScrollView.Clear(true);
                }
            }
        }

        private void OnInitItem(ListView.ListItem item)
        {
            ItemListViewChildView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemListViewChildView>(item.go);

            itemView.m_lbl_info_Text.text = "索引" + item.index;
            itemView.m_btn_click_Button.onClick.RemoveAllListeners();
            itemView.m_btn_click_Button.onClick.AddListener(()=>
            {
                view.m_ipt_field_InputField.text = item.index.ToString();
            });
        }

        private void OnInitScrollItem(ScrollView.ScrollItem item)
        {
            if (item.gameObject.isInit == false)
            {
                item.gameObject.isInit = true;
            }
            ItemListViewChildView itemView = MonoHelper.GetOrAddHotFixViewComponent<ItemListViewChildView>(item.GetGameObject());

            itemView.m_lbl_info_Text.text = "索引" + item.index;
            itemView.m_btn_click_Button.onClick.RemoveAllListeners();
            itemView.m_btn_click_Button.onClick.AddListener(() =>
            {
                view.m_ipt_field_InputField.text = item.index.ToString();
            });
        }

        private void OnFunction1()
        {
            if(m_type== ListType.ListView)
            {
                view.m_sv_listview_ListView.Insert(currentIndex);
                view.m_sv_listview_ListView.ForceRefresh();
            }
            else
            {
                view.m_sv_scrollview_ScrollView.InsertItem(currentIndex,120);
                view.m_sv_scrollview_ScrollView.RefreshShowRect();
            }
        }

        private void OnFunction2()
        {
            if(m_type ==ListType.ListView)
            {
                view.m_sv_listview_ListView.RemoveAt(currentIndex);
                view.m_sv_listview_ListView.ForceRefresh();
            }
            else
            {
                view.m_sv_scrollview_ScrollView.RemoveItem(view.m_sv_scrollview_ScrollView.GetItemByIndex(currentIndex));
            }
        }
        private void OnFunction3()
        {
            if(m_type==ListType.ListView)
            {
                view.m_sv_listview_ListView.MovePanelToItemIndex(currentIndex);
            }
            else
            {
                //view.m_sv_scrollview_ScrollView.LocateItemPosition(currentIndex);
                view.m_sv_scrollview_ScrollView.MovePanelToItemIndex(currentIndex);
            }
        }
        private void OnFunction4()
        {
            if (m_type == ListType.ListView)
            {
                view.m_sv_listview_ListView.ScrollPanelToItemIndex(currentIndex);
            }
            else
            {
                //CoreUtils.logService.Error(view.m_sv_scrollview_ScrollView.GetItemByIndex(currentIndex).position.y.ToString());
                view.m_sv_scrollview_ScrollView.ScrollPanelToItemInidex(currentIndex);
                //view.m_sv_scrollview_ScrollView.ScrollToPos(Mathf.Abs(view.m_sv_scrollview_ScrollView.GetItemByIndex(currentIndex).position.y));
            }
        }
        private void OnFunction5()
        {
            if (m_type == ListType.ListView)
            {
                view.m_sv_listview_ListView.ScrollList2IdxCenter(currentIndex);

            }
            else
            {
                view.m_sv_scrollview_ScrollView.LocateItemPosition(currentIndex);
            }
        }
        private void OnFunction6()
        {
            if (m_type == ListType.ListView)
            {
                ListView.ListItem item = view.m_sv_listview_ListView.GetItemByIndex(currentIndex);
                RectTransform rect = item.go.GetComponent<RectTransform>();
                rect.sizeDelta += new Vector2(0,50);
                view.m_sv_listview_ListView.UpdateItemSize(currentIndex, rect.sizeDelta.y);
            }
            else
            {
                Debug.LogError("变更高度 请使用ListView组件");
                //view.m_sv_scrollview_ScrollView.GetItemByIndex(currentIndex).height += 50;
                //view.m_sv_scrollview_ScrollView.RefreshAllItemPos(true);
            }
        }
        private void OnFunction7()
        {
            if (m_type == ListType.ListView)
            {
                ListView.ListItem item = view.m_sv_listview_ListView.GetItemByIndex(currentIndex);
                RectTransform rect = item.go.GetComponent<RectTransform>();
                rect.sizeDelta += new Vector2(0, -50);
                view.m_sv_listview_ListView.UpdateItemSize(currentIndex, rect.sizeDelta.y);
            }
            else
            {
                Debug.LogError("变更高度 请使用ListView组件");
                //view.m_sv_scrollview_ScrollView.GetItemByIndex(currentIndex).height -= 50;
                //view.m_sv_scrollview_ScrollView.RefreshAllItemPos(true);
            }
        }
        private void OnFunction8()
        {

        }

        private void OnPageJump()
        {
            view.m_sv_page_view_PageView.ScrollPanelToItemIndex(currentIndex);
        }
        #endregion
    }
}