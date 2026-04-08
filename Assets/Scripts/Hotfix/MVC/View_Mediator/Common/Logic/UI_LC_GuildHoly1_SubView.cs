// =============================================================================== 
// Author              :    林光志
// Create Time         :    2020年5月13日
// Update Time         :    2020年5月13日
// Class Description   :    UI_LC_GuildHoly1_SubView
// Copyright IGG All rights reserved.
// ===============================================================================

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Skyunion;
using Client;

namespace Game {
    
     
    // 奇观建筑卡片信息
    public class StrongHoldCardData
    {
        // 奇观建筑状态
        public enum StrongHoldState
        {
            Normal,
            Fighting,
        }
        
        public string imgShow;
        public int nameId;
        public Vector2 pos;
        public StrongHoldState state;
        public List<int> buffDataLst = new List<int>();
        public int descId;
    }
    
    public partial class UI_LC_GuildHoly1_SubView : UI_SubView
    {
        private List<UI_Model_GuildHolyItem_SubView> guildHolyItemPool = new List<UI_Model_GuildHolyItem_SubView>();
        private int usingCorsor = 0;
        public float preferredHeight;
        private Vector2 originSize;

        private GridLayoutGroup _gridLayoutGroup;

        public GridLayoutGroup gridLayoutGroup
        {
            get
            {
                if (_gridLayoutGroup == null)
                {
                    _gridLayoutGroup = gameObject.GetComponent<GridLayoutGroup>();
                }
                return _gridLayoutGroup;
            }
        }

        private ContentSizeFitter _contentSizeFitter;

        public ContentSizeFitter contentSizeFitter
        {
            get
            {
                if (_contentSizeFitter == null)
                {
                    _contentSizeFitter = gameObject.GetComponent<ContentSizeFitter>();
                }
                
                return _contentSizeFitter;
            }
        }

        private RectTransform _rectTransform;

        public RectTransform rectTransform
        {
            get
            {
                if (_rectTransform == null)
                {
                    _rectTransform = gameObject.GetComponent<RectTransform>();
                }
                
                return _rectTransform;
            }
        }

        protected override void BindEvent()
        {
            originSize = rectTransform.sizeDelta;
            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                guildHolyItemPool.Add(new UI_Model_GuildHolyItem_SubView(gameObject.transform.GetChild(i).GetComponent<RectTransform>()));
            }
            base.BindEvent();
        }

        public void Refresh(List<StrongHoldCardData> cardDatas)
        {
            usingCorsor = 0;
            for (int i = 0; i < guildHolyItemPool.Count; i++)
            {
                guildHolyItemPool[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < cardDatas.Count; i++)
            {
                GetGuildHolyItemSubView().Refresh(cardDatas[i]);
            }

            Vector2 newSize = originSize;
            int line = cardDatas.Count / gridLayoutGroup.constraintCount;
            newSize.y += line * (gridLayoutGroup.cellSize.y + gridLayoutGroup.spacing.y);
//            rectTransform.sizeDelta = newSize;
            preferredHeight = newSize.y;
        }

        private UI_Model_GuildHolyItem_SubView GetGuildHolyItemSubView()
        {
            UI_Model_GuildHolyItem_SubView result = null;

            if (usingCorsor >= guildHolyItemPool.Count)
            {
                  GameObject obj = CoreUtils.assetService.Instantiate(m_UI_Item_GuildHolyItem.gameObject);
                  if (obj == null)
                  {
                      CoreUtils.logService.Warn($"圣地  实例化圣地建筑报错! ");
                      return m_UI_Item_GuildHolyItem;
                  }

                  obj.transform.SetParent(gameObject.transform);
                  obj.transform.localPosition = Vector3.zero;
                  obj.transform.localScale = Vector3.one;
                  
                  UI_Model_GuildHolyItem_SubView tmp = new UI_Model_GuildHolyItem_SubView(obj.GetComponent<RectTransform>());
                  guildHolyItemPool.Add(tmp);
            }
            
            
            result = guildHolyItemPool[usingCorsor];
            result.gameObject.SetActive(true);
            usingCorsor++;
            return result;
        }

    }
}