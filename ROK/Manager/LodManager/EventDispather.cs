using System;
using System.Collections.Generic;

namespace ROK
{
    public class EventDispather
    {
        public delegate void EventHandler();

        public List<EventDispather.EventHandler> _handlers = new List<EventDispather.EventHandler>();

        public void AddHandler(EventDispather.EventHandler handler)
        {
            if (!this._handlers.Contains(handler))
            {
                this._handlers.Add(handler);
            }
        }

        public void RemoveHandler(EventDispather.EventHandler handler)
        {
            if (this._handlers.Contains(handler))
            {
                this._handlers.Remove(handler);
            }
        }

        public void Dispath()
        {
            for (int i = 0; i < this._handlers.Count; i++)
            {
                this._handlers[i]();
            }
        }
    }
}