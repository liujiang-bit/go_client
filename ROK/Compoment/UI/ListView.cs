using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ROK
{
    public class ListView : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IEventSystemHandler
    {
        public enum ListViewLayoutType
        {
            Horizontal,
            Vertical
        }

        public class ListItem
        {
            public GameObject go;

            public string tag;

            public string prefabName;

            public int index;

            public float startPos;

            public float endPos;

            public ListItem()
            {
                this.go = null;
                this.prefabName = string.Empty;
                this.tag = string.Empty;
                this.index = 0;
                this.startPos = 0f;
                this.endPos = 0f;
            }

            public bool HasGameObject()
            {
                return this.go != null;
            }

            public void ExportItem()
            {
                if (!this.go)
                {
                    return;
                }
            }
        }

        public ListView.ListViewLayoutType layoutType;

        public RectTransform listContainer;

        public float offset;

        public float spacing;

        public float cacheSize;

        private float autoScrollTime = 0.2f;

        private bool isVertical;

        private Dictionary<string, float> prefabSizeMap = new Dictionary<string, float>();

        private string defaultPrefabName;

        private List<ListView.ListItem> itemList = new List<ListView.ListItem>();

        private float totalSize;

        private float viewSize;

        private float viewStartPos;

        private float viewEndPos;

        private float containerLastPos;

        private ScrollRect parentScrollRect;

        private bool parentScrollEnable;

        private bool autoScroll;

        private Vector4 autoScrollParam;

        private float GetItemSize(ListView.ListItem item)
        {
            return this.prefabSizeMap[item.prefabName];
        }

        private string GetItemTag(ListView.ListItem item)
        {
            return string.Empty;
        }

        private string GetItemPrefabName(ListView.ListItem item)
        {
            return this.defaultPrefabName;
        }

        private void SetContainerSize(float size)
        {
            this.listContainer.sizeDelta = ((!this.isVertical) ? new Vector3(size, this.listContainer.rect.height) : new Vector3(this.listContainer.rect.width, size));
        }

        private float GetContainerSize()
        {
            return (!this.isVertical) ? this.listContainer.rect.width : this.listContainer.rect.height;
        }

        public void SetContainerPos(float pos)
        {
            this.listContainer.anchoredPosition = ((!this.isVertical) ? new Vector2(pos, 0f) : new Vector2(0f, pos));
        }

        public float GetContainerPos()
        {
            return (!this.isVertical) ? this.listContainer.anchoredPosition.x : this.listContainer.anchoredPosition.y;
        }

        private void SetViewRect(float viewStart)
        {
            this.viewStartPos = ((!this.isVertical) ? (-viewStart - this.cacheSize) : (-viewStart + this.cacheSize));
            this.viewEndPos = ((!this.isVertical) ? (this.viewStartPos + this.viewSize + 2f * this.cacheSize) : (this.viewStartPos - this.viewSize - 2f * this.cacheSize));
        }

        private bool ItemVisible(ListView.ListItem item)
        {
            return (!this.isVertical || (item.endPos <= this.viewStartPos && item.startPos >= this.viewEndPos)) && (this.isVertical || (item.endPos >= this.viewStartPos && item.startPos <= this.viewEndPos));
        }

        private bool ShowItem(ListView.ListItem item, bool force = false)
        {
            if (this.ItemVisible(item))
            {
                this.OnItemEnter(item, force);
                return true;
            }
            this.OnItemLeave(item);
            return false;
        }

        public void SetInitData(List<string> prefabNames)
        {
            if (this.listContainer == null)
            {
                Debug.LogError("Please set Container");
            }
            this.isVertical = (this.layoutType == ListView.ListViewLayoutType.Vertical);
            this.viewSize = ((!this.isVertical) ? base.GetComponent<RectTransform>().rect.width : base.GetComponent<RectTransform>().rect.height);
            foreach (var text in prefabNames)
            {
                if (!this.prefabSizeMap.ContainsKey(text))
                {
                    CoreUtils.assetService.Instantiate(text, (GameObject gameObject) =>
                    {
                        Rect rect = gameObject.GetComponent<RectTransform>().rect;
                        this.prefabSizeMap.Add(text, (!this.isVertical) ? rect.width : rect.height);
                        CoreUtils.assetService.Destroy(gameObject);
                        this.defaultPrefabName = text;
                    });
                }
            }
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.vertical = this.isVertical;
                component.horizontal = !this.isVertical;
                component.onValueChanged.RemoveAllListeners();
                component.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnValueChanged));
            }
        }

        public void SetParent(ScrollRect parent)
        {
            this.parentScrollRect = parent;
        }

        public void RemoveAt(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            ListView.ListItem listItem = this.itemList[index];
            float num = Mathf.Abs(listItem.endPos - listItem.startPos) + this.spacing;
            this.totalSize -= num;
            this.SetContainerSize(this.totalSize);
            this.OnItemLeave(listItem);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem2 = this.itemList[i];
                listItem2.index--;
                listItem2.startPos = ((!this.isVertical) ? (listItem2.startPos - num) : (listItem2.startPos + num));
                listItem2.endPos = ((!this.isVertical) ? (listItem2.endPos - num) : (listItem2.endPos + num));
                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                }
                this.ShowItem(listItem2, false);
            }
            this.itemList.RemoveAt(index);
        }

        public void Insert(int index)
        {
            if (index > this.itemList.Count || index < 0)
            {
                return;
            }
            ListView.ListItem listItem = new ListView.ListItem();
            listItem.index = index;
            listItem.prefabName = this.GetItemPrefabName(listItem);
            listItem.tag = this.GetItemTag(listItem);
            listItem.startPos = ((index != 0) ? (this.itemList[index - 1].endPos + this.spacing) : ((!this.isVertical) ? this.offset : (-this.offset)));
            float itemSize = this.GetItemSize(listItem);
            listItem.endPos = ((!this.isVertical) ? (listItem.startPos + itemSize) : (listItem.startPos - itemSize));
            this.itemList.Insert(index, listItem);
            this.ShowItem(listItem, false);
            this.totalSize += itemSize;
            this.SetContainerSize(this.totalSize);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                listItem = this.itemList[i];
                listItem.index++;
                listItem.startPos = ((!this.isVertical) ? (listItem.startPos + itemSize) : (listItem.startPos - itemSize));
                listItem.endPos = ((!this.isVertical) ? (listItem.endPos + itemSize) : (listItem.endPos - itemSize));
                if (listItem.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem);
                }
                this.ShowItem(listItem, false);
            }
        }

        public void UpdateItemSize(int index, float size)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            ListView.ListItem listItem = this.itemList[index];
            this.ShowItem(listItem, false);
            float num = Mathf.Abs(listItem.endPos - listItem.startPos);
            listItem.endPos = ((!this.isVertical) ? (listItem.startPos + size) : (listItem.startPos - size));
            float num2 = size - num;
            this.totalSize += num2;
            this.SetContainerSize(this.totalSize);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem2 = this.itemList[i];
                listItem2.startPos = ((!this.isVertical) ? (listItem2.startPos + num2) : (listItem2.startPos - num2));
                listItem2.endPos = ((!this.isVertical) ? (listItem2.endPos + num2) : (listItem2.endPos - num2));
                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                }
                this.ShowItem(listItem2, false);
            }
        }

        public void ForceRefresh()
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                this.ShowItem(this.itemList[i], true);
            }
        }

        public void RefreshItem(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            ListView.ListItem listItem = this.itemList[index];
            this.ShowItem(listItem, true);
            float num = Mathf.Abs(listItem.endPos - listItem.startPos);
            float itemSize = this.GetItemSize(listItem);
            listItem.endPos = ((!this.isVertical) ? (listItem.startPos + itemSize) : (listItem.startPos - itemSize));
            float num2 = itemSize - num;
            this.totalSize += num2;
            this.SetContainerSize(this.totalSize);
            for (int i = index + 1; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem2 = this.itemList[i];
                listItem2.startPos = ((!this.isVertical) ? (listItem2.startPos + num2) : (listItem2.startPos - num2));
                listItem2.endPos = ((!this.isVertical) ? (listItem2.endPos + num2) : (listItem2.endPos - num2));
                if (listItem2.go != null)
                {
                    this.UpdateItemGameObjectPosition(listItem2);
                }
                this.ShowItem(listItem2, false);
            }
        }

        public void ClearPostion()
        {
            this.containerLastPos = 0f;
        }

        public void FillContent(int listLength)
        {
            int count = this.itemList.Count;
            this.totalSize = ((!this.isVertical) ? this.offset : (-this.offset));
            for (int i = 0; i < listLength; i++)
            {
                ListView.ListItem listItem;
                if (i < count)
                {
                    listItem = this.itemList[i];
                    this.OnItemLeave(listItem);
                }
                else
                {
                    listItem = new ListView.ListItem();
                    this.itemList.Add(listItem);
                }
                listItem.index = i;
                listItem.prefabName = this.GetItemPrefabName(listItem);
                listItem.tag = this.GetItemTag(listItem);
                listItem.startPos = this.totalSize;
                float itemSize = this.GetItemSize(listItem);
                listItem.endPos = ((!this.isVertical) ? (this.totalSize + itemSize) : (this.totalSize - itemSize));
                this.totalSize += ((!this.isVertical) ? (itemSize + this.spacing) : (-(itemSize + this.spacing)));
            }
            for (int j = count - 1; j >= listLength; j--)
            {
                this.OnItemLeave(this.itemList[j]);
                this.itemList.RemoveAt(j);
            }
            this.totalSize = Mathf.Abs(this.totalSize);
            this.SetContainerSize(this.totalSize);
            this.SetContainerPos(this.containerLastPos);
            this.ShowContentAt(this.containerLastPos);
        }

        private void UpdateItemGameObjectPosition(ListView.ListItem item)
        {
            Vector3 v = (!this.isVertical) ? new Vector2(item.startPos, 0f) : new Vector2(0f, item.startPos);
            RectTransform component = item.go.GetComponent<RectTransform>();
            component.anchoredPosition = v;
            component.localScale = Vector3.one;
        }

        private void OnItemEnter(ListView.ListItem item, bool force = false)
        {
            if (item.go != null && !force)
            {
                return;
            }
            if (item.go == null)
            {
                CoreUtils.assetService.Instantiate(item.prefabName, (GameObject obj) =>
                {
                    item.go = obj;
                    item.go.transform.SetParent(this.listContainer);
                    if (!item.go.activeSelf)
                    {
                        item.go.SetActive(true);
                    }
                    item.ExportItem();
                    this.UpdateItemGameObjectPosition(item);
                });
            }
            else
            {
                this.UpdateItemGameObjectPosition(item);
            }
        }

        private void OnItemLeave(ListView.ListItem item)
        {
            if (item.go == null)
            {
                return;
            }
            CoreUtils.assetService.Destroy(item.go);
            item.go = null;
        }

        public void ShowContentAt(float pos)
        {
            this.SetViewRect(pos);
            for (int i = 0; i < this.itemList.Count; i++)
            {
                ListView.ListItem item = this.itemList[i];
                this.ShowItem(item, false);
            }
            this.containerLastPos = pos;
        }

        public void OnValueChanged(Vector2 vec2)
        {
            this.ShowContentAt(this.GetContainerPos());
        }

        public void Clear()
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                ListView.ListItem listItem = this.itemList[i];
                if (listItem.go != null)
                {
                    CoreUtils.assetService.Destroy(listItem.go);
                    listItem.go = null;
                }
            }
            this.prefabSizeMap.Clear();
            this.containerLastPos = 0f;
            this.itemList.Clear();
            this.totalSize = 0f;
            this.SetContainerSize(0f);
            ScrollRect component = base.GetComponent<ScrollRect>();
            if (component != null)
            {
                component.StopMovement();
            }
        }

        private void OnDestroy()
        {
            this.Clear();
        }

        public ListView.ListItem GetItemByIndex(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return null;
            }
            return this.itemList[index];
        }

        public ListView.ListItem GetItemByTag(string tag)
        {
            for (int i = 0; i < this.itemList.Count; i++)
            {
                if (this.itemList[i] != null && this.itemList[i].tag == tag)
                {
                    return this.itemList[i];
                }
            }
            return null;
        }

        public void ScrollList2Idx(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            float num = -this.itemList[index].startPos;
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((num - containerPos) / this.autoScrollTime, this.autoScrollTime, num, containerPos);
        }

        public void ScrollList2IdxImmediate(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            float num = -this.itemList[index].startPos;
            if (!this.isVertical)
            {
                num = Mathf.Clamp(num, -this.totalSize + this.viewSize, 0f);
            }
            else
            {
                num = Mathf.Clamp(num, 0f, this.totalSize - this.viewSize);
            }
            this.SetContainerPos(num);
            this.ShowContentAt(num);
        }

        public void ScrollList2IdxCenter(int index)
        {
            if (index >= this.itemList.Count || index < 0)
            {
                return;
            }
            float num = -this.itemList[index].startPos;
            ListView.ListItem item = this.itemList[index];
            float itemSize = this.GetItemSize(item);
            float containerSize = this.GetContainerSize();
            if (this.itemList[index].startPos < this.viewSize / 2f)
            {
                num = 0f;
            }
            else if (this.itemList[index].startPos > containerSize - this.viewSize / 2f)
            {
                num = this.viewSize - containerSize;
            }
            else
            {
                num = -this.itemList[index].startPos + this.viewSize / 2f - itemSize / 2f;
            }
            this.SetContainerPos(num);
            this.ShowContentAt(num);
        }

        public void ScrollToPos(float dest)
        {
            float containerPos = this.GetContainerPos();
            this.autoScroll = true;
            this.autoScrollParam = new Vector4((dest - containerPos) / this.autoScrollTime, this.autoScrollTime, dest, containerPos);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            this.autoScroll = false;
            if ((this.layoutType == ListView.ListViewLayoutType.Horizontal && Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y)) || (this.layoutType == ListView.ListViewLayoutType.Vertical && Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y)))
            {
                this.parentScrollEnable = false;
            }
            else
            {
                this.parentScrollEnable = true;
            }
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnBeginDrag(eventData);
                ScrollRect component = base.GetComponent<ScrollRect>();
                if (component != null)
                {
                    component.vertical = false;
                    component.horizontal = false;
                }
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnDrag(eventData);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (this.parentScrollRect != null && this.parentScrollEnable)
            {
                this.parentScrollRect.OnEndDrag(eventData);
                ScrollRect component = base.GetComponent<ScrollRect>();
                if (component != null)
                {
                    component.vertical = this.isVertical;
                    component.horizontal = !this.isVertical;
                }
            }
        }

        public void Update()
        {
            try
            {
                if (this.autoScroll)
                {
                    if (this.autoScrollParam.y <= 0f)
                    {
                        this.SetContainerPos(this.autoScrollParam.z);
                        this.ShowContentAt(this.autoScrollParam.z);
                        this.autoScroll = false;
                    }
                    else
                    {
                        float num = this.GetContainerPos();
                        num += this.autoScrollParam.x * Time.deltaTime;
                        this.autoScrollParam.y = this.autoScrollParam.y - Time.deltaTime;
                        if ((this.autoScrollParam.z > this.autoScrollParam.w && num > this.autoScrollParam.z) || (this.autoScrollParam.z < this.autoScrollParam.w && num < this.autoScrollParam.z))
                        {
                            num = this.autoScrollParam.z;
                            this.autoScroll = false;
                        }
                        if (!this.isVertical)
                        {
                            if (num > 0f || num < -this.totalSize + this.viewSize)
                            {
                                num = Mathf.Clamp(num, Mathf.Min(-this.totalSize + this.viewSize, 0f), 0f);
                                this.autoScroll = false;
                            }
                        }
                        else if (num < 0f || num > this.totalSize - this.viewSize)
                        {
                            num = Mathf.Clamp(num, 0f, Mathf.Max(0f, this.totalSize - this.viewSize));
                            this.autoScroll = false;
                        }
                        this.SetContainerPos(num);
                        this.ShowContentAt(num);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
}