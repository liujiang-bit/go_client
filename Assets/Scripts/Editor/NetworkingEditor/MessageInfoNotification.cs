using UnityEngine;
using Sproto;

namespace IGG.Networking
{
    public class MessageInfoNotification : MessageInfo
    {
        public SprotoTypeBase Notification { get; internal set; }

        public override string Type => Notification.ToString();
        public override string Name => "Notification";

        private string JsonData
        {
            get
            {
                if (string.IsNullOrEmpty(m_jsonData))
                {
                    m_notifBytes = Notification.encode().Length;
                    m_jsonData = SprotoExtension.ToJson(Notification);
                }
                return m_jsonData;
            }
        }

        public MessageInfoNotification(SprotoTypeBase notification)
        {
            Notification = notification;
            m_createdFrame = Time.frameCount;
        }

        protected override void OnDrawHeader()
        {
            base.OnDrawHeader();
            GUILayout.Label($"{Type}", GUILayout.ExpandWidth(false));
        }

        protected override void OnDraw()
        {
            DrawBoxContent($"Data [{m_createdFrame}] [{m_notifBytes}b]", JsonData);
        }

        public override string ToString()
        {
            return base.ToString() + "\r\n" + JsonData;
        }

        private string m_jsonData = null;
        private int m_createdFrame = 0;
        private int m_notifBytes = -1;
    }
}