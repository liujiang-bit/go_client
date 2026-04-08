using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using Debug = UnityEngine.Debug;
using Sproto;

namespace IGG.Networking
{
    public abstract class MessageInfo
    {
        public NetworkViewer NetworkViewer { get; private set; }

        /// <summary>
        /// Name of 
        /// </summary>
        public abstract string Name { get; }
        public abstract string Type { get; }

        public StackTrace StackTrace { get; private set; }

        private bool Expanded { get; set; }
        private bool StackTraceExpanded { get; set; }

        public GUIStyle StackTraceButtonStyle
        {
            get
            {
                if (m_stackTraceButtonStyle == null)
                {
                    m_stackTraceButtonStyle = new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleLeft };
                }
                return m_stackTraceButtonStyle;
            }
        }

        [DebuggerStepThrough]
        public static MessageInfo Create(object message, NetworkViewer networkViewer)
        {
            MessageInfo info = null;
            var notification = message as SprotoTypeBase;
            if (notification != null)
            {
                info = new MessageInfoNotification(notification);
            }

            var token = message as AsyncRequestToken;
            if (token != null)
            {
                info = new MessageInfoRequest(token);
            }

            var msg = message as String;
            if (msg != null)
            {
                info = new MessageInfoLogMessage(msg);
            }

            if (info == null)
            {
                Debug.LogAssertion("Unknown message type " + message);
                return null;
            }

            info.NetworkViewer = networkViewer;
            return info;
        }

        [DebuggerStepThrough]
        protected MessageInfo()
        {
            // Skip frames, which is:
            // 1) ctor()
            // 2) upper.ctor()
            // 3) MessageInfo.Create()
            // 4) NetworkViewer.Handler()
            // 5) ActionExt.SafeInvoke
            StackTrace = new StackTrace(5, true);
        }

        public void Draw()
        {
            EditorGUILayout.BeginVertical(GUI.skin.box);

            GUILayout.BeginHorizontal();
            Expanded = GUILayout.Toggle(Expanded, string.Empty, EditorStyles.foldout, GUILayout.ExpandWidth(false));
            OnDrawHeader();
            GUILayout.EndHorizontal();

            if (Expanded)
            {
                ReadOnlyTextField(Name + " Type", Type);
                OnDraw();
                DrawStackTrace();
            }

            EditorGUILayout.EndVertical();
        }

        protected virtual void OnDrawHeader()
        {
            //GUILayout.Label($"{Name}\t", GUILayout.ExpandWidth(false));
            if (GUILayout.Button($"{Name}\t", GUI.skin.label, GUILayout.ExpandWidth(false)))
            {
                EditorGUIUtility.systemCopyBuffer = ToString();
            }
        }

        protected abstract void OnDraw();

        protected void DrawBoxContent(string label, string content, bool withCopyToClipboardButton = true, int trunkateHeight = 55)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{label}:", GUILayout.ExpandWidth(false));
            GUILayout.FlexibleSpace();
            if (withCopyToClipboardButton)
            {
                if (GUILayout.Button("Copy"))
                {
                    EditorGUIUtility.systemCopyBuffer = content;
                }
            }
            GUILayout.EndHorizontal();
            var showedContent = content;
            if (trunkateHeight != -1)
            {
                var lines = content.Split(new[] { '\r', '\n' }).ToList();
                lines.RemoveAll(string.IsNullOrWhiteSpace);
                if (lines.Count >= trunkateHeight)
                {
                    var lines2 = new String[trunkateHeight];
                    Array.Copy(lines.ToArray(), lines2, trunkateHeight - 1);
                    lines2[trunkateHeight - 1] = "<message truncated>";
                    lines = lines2.ToList();
                    showedContent = string.Join(Environment.NewLine, lines);
                }
            }
            GUILayout.TextArea(showedContent, EditorStyles.textArea, CalcMaxHeight(showedContent));
        }

        private GUILayoutOption[] CalcMaxHeight(string content)
        {
            var size = EditorStyles.textArea.CalcSize(new GUIContent(content));
            var minHeight = GUILayout.MinHeight(size.y);
            var maxHeight = GUILayout.MaxWidth(size.y);
            return new GUILayoutOption[]
            {
                minHeight,
                maxHeight,
                GUILayout.ExpandWidth(true)
            };
        }
        
        private void DrawStackTrace()
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginVertical(GUI.skin.box);
            StackTraceExpanded = EditorGUILayout.Foldout(StackTraceExpanded, "StackTrace", EditorStyles.foldout);
            if (!StackTraceExpanded)
            {
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
                return;
            }
            EditorGUILayout.TextArea(StackTrace.ToString());

            for (int i = 0; i < StackTrace.FrameCount; i++)
            {
                var frame = StackTrace.GetFrame(i);
                if (IsNotVisibleFrame(frame))
                {
                    continue;
                }

                var method = frame.GetMethod();
                int line = frame.GetFileLineNumber();
                string showName = method.DeclaringType.Name + "." + method.Name + ":" + line;
                if (GUILayout.Button(showName, StackTraceButtonStyle))
                {
                    Debug.Log(frame.GetFileName() + "; " + showName);
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(frame.GetFileName(), line);
                }
            }
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
        }


        private bool IsNotVisibleFrame(StackFrame frame)
        {
            var fileName = frame.GetFileName();
            if (string.IsNullOrEmpty(fileName))
            {
                return true;
            }
            fileName = fileName.Replace("\\", "/").ToLower();
            var dataPath = Application.dataPath.Replace("\\", "/").ToLower();
            if (!fileName.StartsWith(dataPath))
            {
                return true;
            }
            foreach (var attr in frame.GetMethod().GetCustomAttributes())
            {
                if (attr is DebuggerStepThroughAttribute)
                {
                    return true;
                }
            }

            return false;
        }

        protected void ReadOnlyTextField(string label, string text)
        {
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.LabelField(label, GUILayout.Width(EditorGUIUtility.labelWidth - 4));
                EditorGUILayout.SelectableLabel(text, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            }
            EditorGUILayout.EndHorizontal();
        }

        public override string ToString()
        {
            return Name;
        }

        private GUIStyle m_stackTraceButtonStyle = null;
    }
}