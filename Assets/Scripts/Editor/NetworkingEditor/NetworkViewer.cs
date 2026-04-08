using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Path = System.IO.Path;
using Sproto;

namespace IGG.Networking
{
    public class NetworkViewer : EditorWindow
    {
        public int MaxShowCount { get; private set; } = 0;
        private int NotificationsCount { get; set; } = 0;
        private int RequestsCount { get; set; } = 0;

        private bool ShowNotifications { get; set; } = true;
        private bool ShowRequests { get; set; } = true;
        private bool IsIgnorgeMapOjbectMessage { get; set; } = true;

        private int ViewTargetObjectId { get; set; } = 0;

        [MenuItem("Tools/Networking/Network Viewer %#v")]
        public static void ShowWindow()
        {
            CreateInstance<NetworkViewer>().Show();
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void AutoInitNetViewer()
        {
            var typeGameView = typeof(EditorWindow).Assembly.GetType("UnityEditor.GameView");
            var gameView = GetWindow(typeGameView);
            /*bool maximizeOnPlay = false;
            if (gameView != null)
            {
                maximizeOnPlay = (bool)GetInstanceField(typeGameView, gameView, "m_MaximizeOnPlay");
            }
            if (maximizeOnPlay)
            {
                return;
            }*/
            var type2 = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
            GetWindow<NetworkViewer>(null, false, type2);
        }

        internal static object GetInstanceField(Type type, object instance, string fieldName)
        {
            BindingFlags bindFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic
                                     | BindingFlags.Static;
            FieldInfo field = type.GetField(fieldName, bindFlags);
            return field.GetValue(instance);
        }

        private void OnEnable()
        {
            m_messages.Clear();
            m_filteredMessages.Clear();
            NotificationsCount = 0;
            RequestsCount = 0;
            titleContent = EditorGUIUtility.TrTextContentWithIcon("Net Viewer", "network_viewer");
            wantsMouseMove = true;
            autoRepaintOnSceneChange = true;
            EditorApplication.playModeStateChanged += PlayModeStateChangedHandler;
        }

        private void OnDisable()
        {
            EditorApplication.playModeStateChanged -= PlayModeStateChangedHandler;
            RemoveNetEventListener();
        }        
        private void OnDestroy()
        {
            EditorApplication.playModeStateChanged -= PlayModeStateChangedHandler;
            RemoveNetEventListener();
        }

        private void RemoveNetEventListener()
        {
            if (m_netClient != null)
            {
                m_netClient.OnConnection -= AddNetEventListener;
                if (m_netClient.netClient != null)
                {
                    m_netClient.netClient.IsNetworkingViewerEnable = true;
                    m_netClient.netClient.OnLoginStateChange += OnLoginStateChange;
                    m_netClient.netClient.OnNetStateChange += OnNetStateChange;
                    m_netClient.netClient.OnSendMessage += OnRequestSentHandler;
                    m_netClient.netClient.OnReceiveMessage += OnReceiveMessage;
                    m_netClient.netClient.OnReceiveNotification += OnNotificationReceived;
                }
            }
            m_netClient = null;
        }

        private void Update()
        {
            if (m_netClient == null && EditorApplication.isPlaying)
            {
                FindAndInitNetworkManager();
            }
            Repaint();
        }

        private void OnGUI()
        {
            if (m_netClient == null && EditorApplication.isPlaying)
            {
                FindAndInitNetworkManager();
            }
            DrawHeaderMenu();

            if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Works only in playmode", MessageType.Warning);
            }
            if (m_filteredMessages.Count == 0)
            {
                EditorGUILayout.HelpBox("No messages for show", MessageType.Info);
            }
            else if (!EditorApplication.isPlaying)
            {
                EditorGUILayout.HelpBox("Was showed old messages", MessageType.Warning);
            }

            m_location = EditorGUILayout.BeginScrollView(m_location);
            int counter = 0;
            int targetStartIndex = m_filteredMessages.Count - MaxShowCount;
            foreach (var info in m_filteredMessages)
            {
                if (MaxShowCount > 0 && counter < targetStartIndex)
                {
                    counter++;
                    continue;
                }
                counter++;
                info.Draw();
            }
            EditorGUILayout.EndScrollView();

        }

        private void DrawHeaderMenu()
        {
            //GUILayout.BeginArea(new Rect(0, 0, position.width, 30));
            GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandWidth(true));
            var buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
            buttonStyle.padding = new RectOffset(buttonStyle.padding.left + 2, buttonStyle.padding.right + 2, buttonStyle.padding.top, buttonStyle.padding.bottom);
            if (GUILayout.Button("Clear", buttonStyle, GUILayout.ExpandWidth(false)))
            {
                m_messages.Clear();
                UpdateCounters();
                m_filteredMessages.Clear();
            }
            if (GUILayout.Button("Save", buttonStyle, GUILayout.ExpandWidth(false)))
            {
                SaveToFile();
            }
            //GUILayout.Space(12);
            //MaxShowCount = EditorGUILayout.IntField("MaxShowCount", MaxShowCount, buttonStyle, GUILayout.ExpandWidth(false));

            GUILayout.Space(12);
            GUILayout.FlexibleSpace();
            GUILayout.Space(12);

            Texture2D icon;
            GUIContent content;
            bool newVal;

            const int maxToggleFilter = 60;

            ViewTargetObjectId = EditorGUILayout.IntField("FocusMapObjectId", ViewTargetObjectId, buttonStyle, GUILayout.ExpandWidth(false));

            newVal = GUILayout.Toggle(IsIgnorgeMapOjbectMessage, "IgnorgeMapOjbectMessage", buttonStyle, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(150));
            if (newVal != IsIgnorgeMapOjbectMessage)
            {
                IsIgnorgeMapOjbectMessage = newVal;
            }

            icon = EditorGUIUtility.LoadRequired("icons/request.png") as Texture2D;
            content = new GUIContent(RequestsCount > 999 ? "999+" : RequestsCount.ToString(), icon, "ShowRequests");
            newVal = GUILayout.Toggle(ShowRequests, content, buttonStyle, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(maxToggleFilter));
            if (newVal != ShowRequests)
            {
                ShowRequests = newVal;
                RefilterMessages();
            }

            icon = EditorGUIUtility.LoadRequired("icons/notification.png") as Texture2D;
            content = new GUIContent(NotificationsCount > 999 ? "999+" : NotificationsCount.ToString(), icon, "ShowNotifications");
            newVal = GUILayout.Toggle(ShowNotifications, content, buttonStyle, GUILayout.ExpandWidth(false), GUILayout.MaxWidth(maxToggleFilter));
            if (newVal != ShowNotifications)
            {
                ShowNotifications = newVal;
                RefilterMessages();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button(new GUIContent("X", null, "Clear filter"), buttonStyle, GUILayout.ExpandWidth(false)))
            {
                SetNewFilter(string.Empty);
            }
            GUILayout.Label("Filter by type:", GUILayout.ExpandWidth(false));
            string newFilterString = GUILayout.TextField(m_filterString, GUILayout.ExpandWidth(true));
            if (m_filterString != newFilterString)
            {
                SetNewFilter(newFilterString);
            }
            GUILayout.EndHorizontal();
        }

        private void SaveToFile()
        {
            if (m_msgString.Length == 0) return;
            string filePath = Path.Combine(Application.dataPath, "../msgLog.log");
            File.WriteAllText(filePath, m_msgString.ToString());
        }

        private void SetNewFilter(string newFilterString)
        {
            m_filterString = newFilterString;
            m_filterSeparatedLowerStrings = GetFilterSeparatedLowerStrings(m_filterString);
            RefilterMessages();
        }

        private List<List<string>> GetFilterSeparatedLowerStrings(string filterString)
        {
            List<List<string>> res = new List<List<string>>();
            var orSeparated = filterString.Split('|');
            foreach (var orMsg in orSeparated)
            {
                var list = new List<string>();
                res.Add(list);
                var separated = orMsg.Split(' ');
                foreach (var s in separated)
                {
                    if (string.IsNullOrWhiteSpace(s))
                    {
                        continue;
                    }
                    list.Add(s.ToLower());
                }
            }

            return res;
        }

        private void RefilterMessages()
        {
            m_filteredMessages.Clear();
            foreach (var messageInfo in m_messages)
            {
                if (IsPassedByFilter(messageInfo))
                {
                    m_filteredMessages.Enqueue(messageInfo);
                }
            }
        }

        private bool IsPassedByFilter(MessageInfo messageInfo)
        {
            if (messageInfo.Name == "Notification" && !ShowNotifications)
            {
                return false;
            }
            if (messageInfo.Name == "Request" && !ShowRequests)
            {
                return false;
            }

            if (!IsPassedByFilter(messageInfo.Type))
            {
                return false;
            }
            return true;
        }

        private bool IsPassedByFilter(string typeName)
        {
            var msgType = typeName.ToLower();
            int countOfBad = 0;
            foreach (var orFilterList in m_filterSeparatedLowerStrings)
            {
                foreach (var filterPart in orFilterList)
                {
                    if (!msgType.Contains(filterPart))
                    {
                        countOfBad++;
                        break;
                    }
                }
            }

            return countOfBad < m_filterSeparatedLowerStrings.Count;
        }

        private void OnLoginStateChange(Game.ELoginState loginState, int errorCode)
        {
            var info = MessageInfo.Create($"Login State Changed[{loginState}] errorCode[{errorCode}]", this);
            Add(info);
            m_msgString.Append(info).AppendLine().AppendLine();
        }
        
        private void OnNetStateChange(Skyunion.NetEvent netEvent, int error)
        {
            var info = MessageInfo.Create($"Net State Changed[{netEvent}] errorCode[{error}]", this);
            Add(info);
            m_msgString.Append(info).AppendLine().AppendLine();
        }

        private void OnRequestSentHandler(SprotoTypeBase msg, long session)
        {
            if (msg.Tag() == 18 || msg.Tag() == 15) return;
            AsyncRequestToken token = new AsyncRequestToken()
            {
                SessionId = session,
                Request = msg,
                State = AsyncRequestState.WaitingForResponse,
            };
            var info = MessageInfo.Create(token, this);
            Add(info);
            m_msgString.Append(msg.ToString());
            m_msgString.Append(SprotoExtension.ToJson(msg)).AppendLine();
        }

        private void OnReceiveMessage(SprotoTypeBase msg, long session)
        {
            MessageInfoRequest request = null;
            if(m_dictRequest.TryGetValue(session, out request))
            {
                request.OnResponseHandler(msg);
                m_msgString.Append(msg.ToString());
                m_msgString.Append(SprotoExtension.ToJson(msg)).AppendLine();
            }
        }

        private void OnNotificationReceived(SprotoTypeBase notify)
        {
            if (IsIgnoreMapObject(notify)) return;           

            var info = MessageInfo.Create(notify, this);
            Add(info);
            m_msgString.Append(notify.ToString());
            m_msgString.Append(SprotoExtension.ToJson(notify)).AppendLine().AppendLine();
        }

        private bool IsIgnoreMapObject(SprotoTypeBase notify)
        {
            if (notify.Tag() == 30001 || notify.Tag() == 30002)
            {
                if (IsIgnorgeMapOjbectMessage) return true;
                if (ViewTargetObjectId > 0)
                {
                    if (notify.Tag() == 30001)
                    {
                        var mapObjectInfo = notify as SprotoType.Map_ObjectInfo.request;
                        if (mapObjectInfo == null || mapObjectInfo.mapObjectInfo.objectId != ViewTargetObjectId) return true;
                    }
                    if(notify.Tag() == 30002)
                    {
                        var mapObjectInfo = notify as SprotoType.Map_ObjectDelete.request;
                        if (mapObjectInfo == null || mapObjectInfo.objectId != ViewTargetObjectId) return true;
                    }
                }
            }
            return false;
        }

        private void Add(MessageInfo info)
        {
            m_messages.Enqueue(info);
            UpdateCounters();
            if (IsPassedByFilter(info))
            {
                m_filteredMessages.Enqueue(info);
            }
            if(info is MessageInfoRequest)
            {
                var request = info as MessageInfoRequest;
                m_dictRequest[request.SessionId] = request;
            }
        }

        private void PlayModeStateChangedHandler(PlayModeStateChange state)
        {
            switch (state)
            {
                case PlayModeStateChange.EnteredPlayMode:
                    m_messages.Clear();
                    m_dictRequest.Clear();
                    UpdateCounters();
                    m_filteredMessages.Clear();
                    if (m_netClient == null)
                    {
                        FindAndInitNetworkManager();
                    }
                    break;
            }
        }

        private void UpdateCounters()
        {
            RequestsCount = 0;
            NotificationsCount = 0;
            foreach (var messageInfo in m_messages)
            {
                if (messageInfo.Name == "Request")
                {
                    RequestsCount++;
                }
                if (messageInfo.Name == "Notification")
                {
                    NotificationsCount++;
                }
            }
        }

        private void FindAndInitNetworkManager()
        {
            m_netClient = Game.AppFacade.GetInstance().RetrieveProxy(Game.NetProxy.ProxyNAME) as Game.NetProxy;
            if (m_netClient == null) return;
            m_netClient.OnConnection += AddNetEventListener;

            SetNewFilter(m_filterString);
            m_msgString.Clear();
        }

        private void AddNetEventListener(Game.SprotoSocketAp sprotoSocket)
        {
            sprotoSocket.IsNetworkingViewerEnable = true;
            sprotoSocket.OnLoginStateChange += OnLoginStateChange;
            sprotoSocket.OnNetStateChange += OnNetStateChange;
            sprotoSocket.OnSendMessage += OnRequestSentHandler;
            sprotoSocket.OnReceiveMessage += OnReceiveMessage;
            sprotoSocket.OnReceiveNotification += OnNotificationReceived;
        }

        private Game.NetProxy m_netClient;
        private Queue<MessageInfo> m_messages = new Queue<MessageInfo>();
        private Queue<MessageInfo> m_filteredMessages = new Queue<MessageInfo>();
        private Dictionary<long, MessageInfoRequest> m_dictRequest = new Dictionary<long, MessageInfoRequest>();

        private Vector2 m_location = Vector2.zero;
        private string m_filterString = string.Empty;
        private List<List<string>> m_filterSeparatedLowerStrings = new List<List<string>>();

        private StringBuilder m_msgString = new StringBuilder();
    }
}
