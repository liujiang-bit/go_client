using System.Collections.Generic;
using UnityEngine;

namespace Client
{
    public class TextTool
    {
        private static TextTool tg;

        private Dictionary<char, string> texturePathLookup;

        private Camera editorCamera;

        private const int CHAR_TEXTURE_HEIGHT = 8;

        private const int CHAR_TEXTURE_WIDTH = 6;

        private const string characters = "abcdefghijklmnopqrstuvwxyz0123456789-.";

        private TextTool()
        {
            editorCamera = Camera.current;
            texturePathLookup = new Dictionary<char, string>();
            for (int i = 0; i < "abcdefghijklmnopqrstuvwxyz0123456789-.".Length; i++)
            {
                texturePathLookup.Add("abcdefghijklmnopqrstuvwxyz0123456789-."[i], "text_" + "abcdefghijklmnopqrstuvwxyz0123456789-."[i] + ".png");
            }
        }

        public static void Init()
        {
            tg = new TextTool();
        }

        public static void Draw(Vector3 position, string text)
        {
            if (tg == null)
            {
                Init();
            }
            string text2 = text.ToLower();
            Vector3 vector = tg.editorCamera.WorldToScreenPoint(position);
            int num = 20;
            for (int i = 0; i < text2.Length; i++)
            {
                if (tg.texturePathLookup.ContainsKey(text2[i]))
                {
                    Vector3 center = tg.editorCamera.ScreenToWorldPoint(new Vector3(vector.x + (float)num, vector.y, vector.z));
                    Gizmos.DrawIcon(center, tg.texturePathLookup[text2[i]], allowScaling: false);
                    num += 6;
                }
                else if (text2[i] == ' ')
                {
                    num += 6;
                }
            }
        }
    }
}