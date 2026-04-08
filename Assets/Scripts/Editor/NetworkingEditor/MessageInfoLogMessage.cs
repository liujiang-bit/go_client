using UnityEngine;

namespace IGG.Networking
{
    public class MessageInfoLogMessage : MessageInfo
    {
        public override string Type => Message;
        public override string Name => "Message";
        public string Message { get; private set; }

        public MessageInfoLogMessage(string message)
        {
            Message = message;
            m_createdFrame = Time.frameCount;
        }

        protected override void OnDrawHeader()
        {
            base.OnDrawHeader();
            GUILayout.Label($"{Type}", GUILayout.ExpandWidth(false));
        }

        protected override void OnDraw()
        {
            DrawBoxContent($"Data [{m_createdFrame}]", Message);
        }

        public override string ToString()
        {
            return base.ToString() + ": " + Message;
        }

        [SerializeField]
        private int m_createdFrame = 0;
    }
}