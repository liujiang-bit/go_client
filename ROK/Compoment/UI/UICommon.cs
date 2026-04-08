using Skyunion;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ROK
{
    public static class UICommon
    {
        public static Camera s_UICamera;

        public static bool enableTextWrap;

        public static float GetUIDirOffset(GameObject go)
        {
            return (Vector3.Dot(go.transform.forward, -Vector3.forward) + 1f) / 2f;
        }

        public static void SetAnchoredPosition(RectTransform rt, float x, float y)
        {
            rt.anchoredPosition = new Vector2(x, y);
        }

        public static void SetLocalEulerAngles(GameObject go, float z)
        {
            go.transform.localEulerAngles = new Vector3(0f, 0f, z);
        }

        public static void SetRectTransformSizeDelta(RectTransform rt, float x, float y)
        {
            rt.sizeDelta = new Vector2(x, y);
        }

        public static Vector3[] GetRectTransformWorldCorners(RectTransform rt)
        {
            Vector3[] array = new Vector3[4];
            rt.GetWorldCorners(array);
            return array;
        }

        public static Vector3[] GetRectTransformLocalCorners(RectTransform rt)
        {
            Vector3[] array = new Vector3[4];
            rt.GetLocalCorners(array);
            return array;
        }

        public static Vector2 GetRectTransformWorldSize(RectTransform rt, Vector2 sizeDelta)
        {
            Vector3 vector = rt.TransformPoint(Vector3.zero);
            Vector3 vector2 = rt.TransformPoint(new Vector3(sizeDelta.x, sizeDelta.y, 0f));
            return new Vector2(vector2.x - vector.x, vector2.y - vector.y);
        }

        public static float GetRectTransformWorldWidth(RectTransform rt, float width)
        {
            Vector3 vector = rt.TransformPoint(Vector3.zero);
            return rt.TransformPoint(new Vector3(width, 0f, 0f)).x - vector.x;
        }

        public static float GetRectTransformWorldHeight(RectTransform rt, float height)
        {
            Vector3 vector = rt.TransformPoint(Vector3.zero);
            return rt.TransformPoint(new Vector3(0f, height, 0f)).y - vector.y;
        }

        public static Vector2 GetRectTransformLocalSize(RectTransform rt, Vector2 sizeDelta)
        {
            Vector3 vector = rt.InverseTransformPoint(Vector3.zero);
            Vector3 vector2 = rt.InverseTransformPoint(new Vector3(sizeDelta.x, sizeDelta.y, 0f));
            return new Vector2(vector.x - vector2.x, vector.y - vector2.y);
        }

        public static float GetRectTransformLocalWidth(RectTransform rt, float width)
        {
            Vector3 vector = rt.InverseTransformPoint(Vector3.zero);
            Vector3 vector2 = rt.InverseTransformPoint(new Vector3(width, 0f, 0f));
            return vector.x - vector2.x;
        }

        public static float GetRectTransformLocalHeight(RectTransform rt, float height)
        {
            Vector3 vector = rt.InverseTransformPoint(Vector3.zero);
            Vector3 vector2 = rt.InverseTransformPoint(new Vector3(0f, height, 0f));
            return vector.y - vector2.y;
        }

        public static Canvas GetRootCanvas(RectTransform rt)
        {
            Canvas canvas = rt.GetComponent<Canvas>();
            if (canvas == null)
            {
                canvas = rt.GetComponentInParent<Canvas>();
            }
            if (canvas != null && !canvas.isRootCanvas)
            {
                canvas = canvas.rootCanvas;
            }
            return canvas;
        }

        public static Vector2 ScreenToRectTransformLocalPos(RectTransform rt, Vector2 screenPos)
        {
            Vector2 zero = Vector2.zero;
            Canvas rootCanvas = UICommon.GetRootCanvas(rt);
            if (rootCanvas != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(rt, screenPos, rootCanvas.worldCamera, out zero);
            }
            return zero;
        }

        public static Vector2 RectTransformLocalToScreenPos(RectTransform rt, Vector2 pos)
        {
            Vector2 result = Vector2.zero;
            Vector3 position = rt.TransformPoint(pos);
            Canvas rootCanvas = UICommon.GetRootCanvas(rt);
            if (rootCanvas != null)
            {
                result = rootCanvas.worldCamera.WorldToScreenPoint(position);
            }
            return result;
        }

        public static Vector2 GetRectTransformScreenPos(RectTransform rt)
        {
            return UICommon.RectTransformLocalToScreenPos(rt, Vector2.zero);
        }

        public static float GetCanvasGroupAlpha(GameObject obj)
        {
            CanvasGroup component = obj.GetComponent<CanvasGroup>();
            if (component != null)
            {
                return component.alpha;
            }
            return 0f;
        }

        public static void SetCanvasGroupAlpha(GameObject obj, float alpha)
        {
            CanvasGroup component = obj.GetComponent<CanvasGroup>();
            if (component != null)
            {
                component.alpha = alpha;
            }
        }

        public static void WorldToUIPos(GameObject go_canvas, float x, float y, float z, out float rx, out float ry)
        {
            Vector3 vector = WorldCamera.camera.WorldToScreenPoint(new Vector3(x, y, z));
            RectTransform component = go_canvas.GetComponent<RectTransform>();
            float x2 = component.sizeDelta.x;
            float y2 = component.sizeDelta.y;
            rx = vector.x * (x2 / (float)Screen.width);
            ry = vector.y * (y2 / (float)Screen.height);
        }

        public static void SetTextContent(UnityEngine.Object obj, string content, bool replacetxtandremovebft)
        {
            Text text = null;
            if (obj is GameObject)
            {
                text = (obj as GameObject).GetComponent<Text>();
            }
            else if (obj is Text)
            {
                text = (obj as Text);
            }
            if (text != null)
            {
                if (replacetxtandremovebft)
                {
                    text.resizeTextForBestFit = false;
                    text.text = UICommon.NonBreakingSpaceText(content);
                }
                else
                {
                    text.text = content;
                }
            }
        }

        public static Component GetComponent(GameObject go, string target_path, Type type)
        {
            Transform transform = go.transform.Find(target_path);
            if (transform != null)
            {
                return transform.GetComponent(type);
            }
            return null;
        }

        public static Vector3 UITo3DWorldPos(Vector3 uiPos, Vector3 targetPanelPos)
        {
            if (UICommon.s_UICamera == null)
            {
                GameObject gameObject = GameObject.Find("Canvas");
                Transform transform = gameObject.transform.Find("Camera");
                UICommon.s_UICamera = transform.gameObject.GetComponent<Camera>();
            }
            Camera camera = WorldCamera.camera;
            Vector3 vector = UICommon.s_UICamera.WorldToScreenPoint(uiPos);
            float z = Vector3.Distance(camera.transform.position, targetPanelPos);
            return camera.ScreenToWorldPoint(new Vector3(vector.x, vector.y, z));
        }

        public static void SetScale(GameObject go, float scale)
        {
            if (go != null && scale != float.PositiveInfinity)
            {
                go.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }

        public static string AllToUpper(string txt)
        {
            return txt.ToUpper();
        }

        public static string AllToLower(string txt)
        {
            return txt.ToLower();
        }

        public static string NonBreakingSpaceText(string txt)
        {
            if (txt.Contains(" "))
            {
                return txt.Replace(" ", "\u00a0");
            }
            return txt;
        }

        public static string InitialToUpper(string txt, bool right_to_left)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return txt;
            }
            string text = string.Empty;
            int num = 0;
            if (right_to_left)
            {
                num = txt.Length - 1;
            }
            text = txt[num].ToString();
            StringBuilder stringBuilder = new StringBuilder(txt);
            stringBuilder.Replace(text, text.ToUpper(), num, 1);
            return stringBuilder.ToString();
        }

        public static string EachWordInitialToUpper(string txt, bool right_to_left)
        {
            if (string.IsNullOrEmpty(txt))
            {
                return txt;
            }
            string[] array = txt.Split(new char[]
            {
            ' '
            });
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < array.Length; i++)
            {
                if (i != 0 && i != array.Length)
                {
                    stringBuilder.Append(" ");
                }
                string text = array[i];
                if (text != null && text.Length > 0)
                {
                    StringBuilder stringBuilder2 = new StringBuilder(text);
                    int num = 0;
                    if (right_to_left)
                    {
                        num = text.Length - 1;
                    }
                    string text2 = text[num].ToString();
                    stringBuilder2.Replace(text2, text2.ToUpper(), num, 1);
                    stringBuilder.Append(stringBuilder2);
                }
            }
            return stringBuilder.ToString();
        }

        public static void ReplaceMaterial(GameObject go, string material_path)
        {
            MaskableGraphic component = go.GetComponent<MaskableGraphic>();
            if (component != null)
            {
                if (string.IsNullOrEmpty(material_path))
                {
                    component.material = component.defaultMaterial;
                }
                else
                {
                    CoreUtils.assetService.LoadAssetAsync<Material>(material_path, (IAsset asset) =>
                    {
                        Material material = asset.asset() as Material;
                        if (material != null)
                        {
                            component.material = material;
                        }
                    });
                }
            }
        }

        public static void ResetAnimation(GameObject go, string ani_name)
        {
            Animation component = go.GetComponent<Animation>();
            if (component != null && component[ani_name] != null)
            {
                component[ani_name].time = 0f;
                component.Sample();
                component.Stop(ani_name);
            }
        }

        public static void StopAnimation(GameObject go, string ani_name)
        {
            Animation component = go.GetComponent<Animation>();
            if (component != null)
            {
                if (string.IsNullOrEmpty(ani_name))
                {
                    component.Stop();
                }
                else if (component[ani_name] != null)
                {
                    component.Stop(ani_name);
                }
            }
        }

        public static void PlayAnimationBackwards(GameObject go, string ani_name)
        {
            Animation component = go.GetComponent<Animation>();
            if (component != null)
            {
                AnimationState animationState = component[ani_name];
                if (animationState != null)
                {
                    animationState.time = animationState.length;
                    animationState.speed = -1f;
                    component.Play(ani_name);
                }
            }
        }

        public static void PlayAnimationNormal(GameObject go, string ani_name)
        {
            Animation component = go.GetComponent<Animation>();
            if (component != null)
            {
                AnimationState animationState = component[ani_name];
                if (animationState != null)
                {
                    animationState.time = 0f;
                    animationState.speed = 1f;
                    component.Play(ani_name);
                }
            }
        }

        public static int GetTextLength(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return str.Length;
            }
            return 0;
        }

        public static string Substring(string str, int idx)
        {
            return str.Substring(idx);
        }

        public static int GetTextByteNum(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return Encoding.Default.GetByteCount(str);
            }
            return 0;
        }

        public static string GetTextByByteNum(string str, int num)
        {
            int byteCount = Encoding.Default.GetByteCount(str);
            if (num >= byteCount)
            {
                return str;
            }
            StringBuilder stringBuilder = new StringBuilder(string.Empty);
            if (!string.IsNullOrEmpty(str))
            {
                for (int i = 0; i < str.Length; i++)
                {
                    string text = str.Substring(i, 1);
                    num -= Encoding.Default.GetByteCount(text);
                    if (num < 0)
                    {
                        break;
                    }
                    stringBuilder.Append(text);
                }
            }
            return stringBuilder.ToString();
        }

        public static string GetShowBestFitText(GameObject go_txt, string str)
        {
            if (go_txt == null)
            {
                return string.Empty;
            }
            string text = string.Empty;
            Text component = go_txt.GetComponent<Text>();
            float num = component.rectTransform.sizeDelta.x;
            component.text = str;
            if (component.preferredWidth <= num)
            {
                return str;
            }
            component.text = "...";
            num -= component.preferredWidth;
            for (int i = 1; i <= str.Length; i++)
            {
                string text2 = str.Substring(0, i);
                component.text = text2;
                if (component.preferredWidth > num)
                {
                    text += "...";
                    break;
                }
                text = text2;
            }
            return text;
        }

        public static void AddUnderLine(Text text)
        {
            if (text == null)
            {
                return;
            }
            Transform transform = text.transform.Find("TextUnderline");
            Text text2;
            if (transform != null)
            {
                text2 = transform.GetComponent<Text>();
            }
            else
            {
                GameObject gameObject = new GameObject();
                text2 = gameObject.AddComponent<Text>();
                text2.font = text.font;
                text2.fontSize = text.fontSize;
                text2.raycastTarget = text.raycastTarget;
                text2.alignment = text.alignment;
                text2.fontStyle = text.fontStyle;
                text2.lineSpacing = text.lineSpacing;
                text2.supportRichText = text.supportRichText;
                text2.resizeTextForBestFit = text.resizeTextForBestFit;
                text2.color = text.color;
                text2.name = "TextUnderline";
                text2.transform.SetParent(text.transform);
                text2.transform.localScale = new Vector3(1f, 1f, 1f);
            }
            RectTransform rectTransform = text2.rectTransform;
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.offsetMax = Vector2.zero;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.anchorMin = Vector2.zero;
            text2.text = "_";
            float preferredWidth = text2.preferredWidth;
            float preferredWidth2 = text.preferredWidth;
            int num = (int)Mathf.Ceil(preferredWidth2 / preferredWidth);
            for (int i = 1; i < num; i++)
            {
                Text expr_15B = text2;
                expr_15B.text += "_";
            }
        }

        public static void SetTextMeshesText(GameObject obj, string text)
        {
            TextMesh[] componentsInChildren = obj.GetComponentsInChildren<TextMesh>();
            for (int i = 0; i < componentsInChildren.Length; i++)
            {
                TextMesh textMesh = componentsInChildren[i];
                textMesh.text = text;
            }
        }

        public static void SetUILinePointNum(GameObject obj, int num)
        {
            if (obj == null)
            {
                return;
            }
            UILineRenderer component = obj.GetComponent<UILineRenderer>();
            component.Points = new Vector2[num];
        }

        public static void AddUILinePoint(GameObject obj, int index, int x, int y)
        {
            if (obj == null)
            {
                return;
            }
            UILineRenderer component = obj.GetComponent<UILineRenderer>();
            component.Points[index] = new Vector2((float)x, (float)y);
        }

        private static void SetParticleSystemColor(GameObject go, Color color)
        {
            ParticleSystem component = go.GetComponent<ParticleSystem>();
            if (!component)
            {
                return;
            }
            component.startColor = color;
        }

        private static void SetParticleSystemSortorder(GameObject go, int order)
        {
            Renderer component = go.GetComponent<Renderer>();
            if (!component)
            {
                return;
            }
            component.sortingOrder = order;
        }

        public static void SetParticleSystemSortorder(GameObject go, int order, bool withChild)
        {
            if (withChild)
            {
                Transform transform = go.transform;
                for (int i = 0; i < transform.childCount; i++)
                {
                    UICommon.SetParticleSystemSortorder(transform.GetChild(i).gameObject, order);
                }
            }
            UICommon.SetParticleSystemSortorder(go, order);
        }

        public static void SetParticleSystemColor(GameObject go, Color color, bool withChild)
        {
            if (withChild)
            {
                Transform transform = go.transform;
                for (int i = 0; i < transform.childCount; i++)
                {
                    UICommon.SetParticleSystemColor(transform.GetChild(i).gameObject, color);
                }
            }
            UICommon.SetParticleSystemColor(go, color);
        }

        public static bool IsClickChatBanner()
        {
            EventSystem current = EventSystem.current;
            if (current != null)
            {
                PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> list = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, list);
                return (list.Count == 1 && list[0].gameObject.name == "btn_chat") || (list.Count == 2 && list[0].gameObject.name == "txt_chat_content1") || (list.Count == 2 && list[0].gameObject.name == "txt_chat_content2") || (list.Count == 2 && list[0].gameObject.name == "txt_chat_content3");
            }
            return false;
        }

        public static void ForceRebuildLayout(GameObject obj)
        {
            if (obj != null)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate(obj.transform.GetComponent<RectTransform>());
            }
        }

        public static void SetSpineSkeletonSortOrder(GameObject obj, int order)
        {
            obj.GetComponent<MeshRenderer>().sortingOrder = order;
        }

        public static void SetTextHorizontalOverflow(GameObject go, int type)
        {
            if (type == 1)
            {
                go.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Wrap;
            }
            else if (type == 2)
            {
                go.GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
            }
        }

        //public static void EnableTextWrap(bool isEnable)
        //{
        //	Text.EnableWrap(isEnable);
        //	UICommon.enableTextWrap = isEnable;
        //}

        //public static void EnableTextArabic(bool isEnable)
        //{
        //	Text.EnableArabic(isEnable);
        //}
    }
}