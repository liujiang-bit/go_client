using System;
using UnityEngine.EventSystems;

namespace ROK
{
    public class UIEventTrigger : EventTrigger
    {
        public delegate void EventTriggerCB0();

        public delegate void EventTriggerCB1(PointerEventData data);

        public delegate void EventTriggerCB2(BaseEventData data);

        public delegate void EventTriggerCB3(AxisEventData data);

        public UIEventTrigger.EventTriggerCB1 onBeginDrag;

        public UIEventTrigger.EventTriggerCB2 onCancel;

        public UIEventTrigger.EventTriggerCB2 onDeselect;

        public UIEventTrigger.EventTriggerCB1 onDrag;

        public UIEventTrigger.EventTriggerCB1 onDrop;

        public UIEventTrigger.EventTriggerCB1 onEndDrag;

        public UIEventTrigger.EventTriggerCB1 onInitializePotentialDrag;

        public UIEventTrigger.EventTriggerCB3 onMove;

        public UIEventTrigger.EventTriggerCB1 onPointerClick;

        public UIEventTrigger.EventTriggerCB1 onPointerDown;

        public UIEventTrigger.EventTriggerCB1 onPointerEnter;

        public UIEventTrigger.EventTriggerCB1 onPointerExit;

        public UIEventTrigger.EventTriggerCB1 onPointerUp;

        public UIEventTrigger.EventTriggerCB1 onScroll;

        public UIEventTrigger.EventTriggerCB2 onSelect;

        public UIEventTrigger.EventTriggerCB2 onSubmit;

        public UIEventTrigger.EventTriggerCB2 onUpdateSelected;

        public UIEventTrigger.EventTriggerCB0 onEnable;

        public UIEventTrigger.EventTriggerCB0 onDisable;

        public void SetOnSelectCB(UIEventTrigger.EventTriggerCB2 cb)
        {
            if (this.onSelect != null)
            {
                this.onSelect = cb;
            }
        }

        public override void OnBeginDrag(PointerEventData data)
        {
            if (this.onBeginDrag != null)
            {
                this.onBeginDrag(data);
            }
        }

        public override void OnCancel(BaseEventData data)
        {
            if (this.onCancel != null)
            {
                this.onCancel(data);
            }
        }

        public override void OnDeselect(BaseEventData data)
        {
            if (this.onDeselect != null)
            {
                this.onDeselect(data);
            }
        }

        public override void OnDrag(PointerEventData data)
        {
            if (this.onDrag != null)
            {
                this.onDrag(data);
            }
        }

        public override void OnDrop(PointerEventData data)
        {
            if (this.onDrop != null)
            {
                this.onDrop(data);
            }
        }

        public override void OnEndDrag(PointerEventData data)
        {
            if (this.onEndDrag != null)
            {
                this.onEndDrag(data);
            }
        }

        public override void OnInitializePotentialDrag(PointerEventData data)
        {
            if (this.onInitializePotentialDrag != null)
            {
                this.onInitializePotentialDrag(data);
            }
        }

        public override void OnMove(AxisEventData data)
        {
            if (this.onMove != null)
            {
                this.onMove(data);
            }
        }

        public override void OnPointerClick(PointerEventData data)
        {
            if (this.onPointerClick != null)
            {
                this.onPointerClick(data);
            }
        }

        public override void OnPointerDown(PointerEventData data)
        {
            if (this.onPointerDown != null)
            {
                this.onPointerDown(data);
            }
        }

        public override void OnPointerEnter(PointerEventData data)
        {
            if (this.onPointerEnter != null)
            {
                this.onPointerEnter(data);
            }
        }

        public override void OnPointerExit(PointerEventData data)
        {
            if (this.onPointerExit != null)
            {
                this.onPointerExit(data);
            }
        }

        public override void OnPointerUp(PointerEventData data)
        {
            if (this.onPointerUp != null)
            {
                this.onPointerUp(data);
            }
        }

        public override void OnScroll(PointerEventData data)
        {
            if (this.onScroll != null)
            {
                this.onScroll(data);
            }
        }

        public override void OnSelect(BaseEventData data)
        {
            if (this.onSelect != null)
            {
                this.onSelect(data);
            }
        }

        public override void OnSubmit(BaseEventData data)
        {
            if (this.onSubmit != null)
            {
                this.onSubmit(data);
            }
        }

        public override void OnUpdateSelected(BaseEventData data)
        {
            if (this.onUpdateSelected != null)
            {
                this.onUpdateSelected(data);
            }
        }

        private void OnEnable()
        {
            if (this.onEnable != null)
            {
                this.onEnable();
            }
        }

        private void OnDisable()
        {
            if (this.onDisable != null)
            {
                this.onDisable();
            }
        }
    }
}