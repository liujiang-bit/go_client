using System;
using System.Text;
using UnityEngine;
using Sproto;

namespace IGG.Networking
{
    public enum AsyncRequestState
    {
        WaitingForResponse,
        Success,       
    }

    public class AsyncRequestToken
    {
        public long SessionId { get; set; }
        public SprotoTypeBase Request { get; set; }

        public SprotoTypeBase Reponse { get; set; }

        public AsyncRequestState State { get; set; }
    }

    public class MessageInfoRequest : MessageInfo
    {
        public long SessionId { get { return Token.SessionId; } }

        private AsyncRequestToken Token { get; set; }

        public override string Name => "Request";
        public override string Type => Token.Request.ToString();

        private string JsonRequest
        {
            get
            {
                if (string.IsNullOrEmpty(m_jsonRequest))
                {
                    m_requestBytes = Token.Request.encode().Length;
                    m_jsonRequest = SprotoExtension.ToJson(Token.Request);
                }
                return m_jsonRequest;
            }
        }

        private string JsonResponse
        {
            get
            {
                if (string.IsNullOrEmpty(m_jsonResponse))
                {
                    m_responseBytes = Token.Reponse.encode().Length;
                    m_jsonResponse = SprotoExtension.ToJson(Token.Reponse);
                }
                return m_jsonResponse;
            }
        }

        public MessageInfoRequest(AsyncRequestToken token)
        {
            Token = token;
            m_createdFrame = Time.frameCount;
            m_responseFrame = -1;
        }

        ~MessageInfoRequest()
        {
        }

        public void OnResponseHandler(SprotoTypeBase response)
        {
            Token.Reponse = response;
            Token.State = AsyncRequestState.Success;
            m_responseFrame = Time.frameCount;
        }

        protected override void OnDrawHeader()
        {
            base.OnDrawHeader();
            GUILayout.Label($"ID={Token.Request.Tag()}\t", GUILayout.ExpandWidth(false));
            GUILayout.Label($"{Type}", GUILayout.ExpandWidth(false));

            GUILayout.FlexibleSpace();
            var coloredLabel = new GUIStyle(GUI.skin.label);
            var backColor = GUI.backgroundColor;
            GUI.backgroundColor = Color.black;
            var color = GetColorByState(Token.State);
            coloredLabel.normal.textColor = color;
            GUILayout.Label($"State={Token.State}", coloredLabel, GUILayout.ExpandWidth(false));
            GUI.backgroundColor = backColor;
        }

        private Color GetColorByState(AsyncRequestState tokenState)
        {
            switch (tokenState)
            {
                case AsyncRequestState.WaitingForResponse:
                    return Color.yellow * Color.gray;
                case AsyncRequestState.Success:
                    return Color.green * 0.7f;
                default:
                    return Color.gray;
            }
        }

        protected override void OnDraw()
        {
            ReadOnlyTextField("Current State", Token.State.ToString());

            DrawBoxContent($"Request [{m_createdFrame}] [{m_requestBytes}b]", JsonRequest);
            if (Token.Reponse != null)
            {
                if (m_responseFrame == -1)
                {
                    Debug.LogWarning("Found responseFrame = -1");
                    m_responseFrame = Time.frameCount;
                }
                DrawBoxContent($"Response [{m_responseFrame}] [{m_responseBytes}b] ", JsonResponse);
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(base.ToString());
            sb.Append("Type: ").AppendLine(Type);
            sb.Append("State: ").AppendLine(Token.State.ToString());
            sb.Append("Request: ").AppendLine(JsonRequest);
            sb.Append("Response: ").AppendLine(Token.Reponse == null ? "NULL" : JsonResponse);
            return sb.ToString();
        }

        private string m_jsonRequest = null;
        private string m_jsonResponse = null;
        private int m_createdFrame = 0;
        private int m_responseFrame = -1;
        private int m_requestBytes = -1;
        private int m_responseBytes = -1;
    }
}