using System;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    public class UIPrimitiveBase : MaskableGraphic, ILayoutElement, ICanvasRaycastFilter
    {
        [SerializeField]
        private Sprite m_Sprite;

        [NonSerialized]
        private Sprite m_OverrideSprite;

        internal float m_EventAlphaThreshold = 1f;

        public Sprite sprite
        {
            get
            {
                return this.m_Sprite;
            }
            set
            {
                this.m_Sprite = value;
            }
        }

        public Sprite overrideSprite
        {
            get
            {
                return (!(this.m_OverrideSprite == null)) ? this.m_OverrideSprite : this.sprite;
            }
            set
            {
                this.m_OverrideSprite = value;
            }
        }

        public float eventAlphaThreshold
        {
            get
            {
                return this.m_EventAlphaThreshold;
            }
            set
            {
                this.m_EventAlphaThreshold = value;
            }
        }

        public override Texture mainTexture
        {
            get
            {
                if (!(this.overrideSprite == null))
                {
                    return this.overrideSprite.texture;
                }
                if (this.material != null && this.material.mainTexture != null)
                {
                    return this.material.mainTexture;
                }
                return Graphic.s_WhiteTexture;
            }
        }

        public float pixelsPerUnit
        {
            get
            {
                float num = 100f;
                if (this.sprite)
                {
                    num = this.sprite.pixelsPerUnit;
                }
                float num2 = 100f;
                if (base.canvas)
                {
                    num2 = base.canvas.referencePixelsPerUnit;
                }
                return num / num2;
            }
        }

        public virtual float minWidth
        {
            get
            {
                return 0f;
            }
        }

        public virtual float preferredWidth
        {
            get
            {
                if (this.overrideSprite == null)
                {
                    return 0f;
                }
                return this.overrideSprite.rect.size.x / this.pixelsPerUnit;
            }
        }

        public virtual float flexibleWidth
        {
            get
            {
                return -1f;
            }
        }

        public virtual float minHeight
        {
            get
            {
                return 0f;
            }
        }

        public virtual float preferredHeight
        {
            get
            {
                if (this.overrideSprite == null)
                {
                    return 0f;
                }
                return this.overrideSprite.rect.size.y / this.pixelsPerUnit;
            }
        }

        public virtual float flexibleHeight
        {
            get
            {
                return -1f;
            }
        }

        public virtual int layoutPriority
        {
            get
            {
                return 0;
            }
        }

        protected UIVertex[] SetVbo(Vector2[] vertices, Vector2[] uvs)
        {
            UIVertex[] array = new UIVertex[4];
            for (int i = 0; i < vertices.Length; i++)
            {
                UIVertex simpleVert = UIVertex.simpleVert;
                simpleVert.color = this.color;
                simpleVert.position = vertices[i];
                simpleVert.uv0 = uvs[i];
                array[i] = simpleVert;
            }
            return array;
        }

        public virtual void CalculateLayoutInputHorizontal()
        {
        }

        public virtual void CalculateLayoutInputVertical()
        {
        }

        public virtual bool IsRaycastLocationValid(Vector2 screenPoint, Camera eventCamera)
        {
            if (this.m_EventAlphaThreshold >= 1f)
            {
                return true;
            }
            Sprite overrideSprite = this.overrideSprite;
            if (overrideSprite == null)
            {
                return true;
            }
            Vector2 local;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(base.rectTransform, screenPoint, eventCamera, out local);
            Rect pixelAdjustedRect = base.GetPixelAdjustedRect();
            local.x += base.rectTransform.pivot.x * pixelAdjustedRect.width;
            local.y += base.rectTransform.pivot.y * pixelAdjustedRect.height;
            local = this.MapCoordinate(local, pixelAdjustedRect);
            Rect textureRect = overrideSprite.textureRect;
            Vector2 vector = new Vector2(local.x / textureRect.width, local.y / textureRect.height);
            float u = Mathf.Lerp(textureRect.x, textureRect.xMax, vector.x) / (float)overrideSprite.texture.width;
            float v = Mathf.Lerp(textureRect.y, textureRect.yMax, vector.y) / (float)overrideSprite.texture.height;
            bool result;
            try
            {
                result = (overrideSprite.texture.GetPixelBilinear(u, v).a >= this.m_EventAlphaThreshold);
            }
            catch (UnityException ex)
            {
                Debug.LogError("Using clickAlphaThreshold lower than 1 on Image whose sprite texture cannot be read. " + ex.Message + " Also make sure to disable sprite packing for this sprite.", this);
                result = true;
            }
            return result;
        }

        private Vector2 MapCoordinate(Vector2 local, Rect rect)
        {
            Rect rect2 = this.sprite.rect;
            return new Vector2(local.x * rect2.width / rect.width, local.y * rect2.height / rect.height);
        }

        private Vector4 GetAdjustedBorders(Vector4 border, Rect rect)
        {
            for (int i = 0; i <= 1; i++)
            {
                float num = border[i] + border[i + 2];
                if (rect.size[i] < num && num != 0f)
                {
                    float num2 = rect.size[i] / num;
                    int index;
                    border[index = i] = border[index] * num2;
                    int index2;
                    border[index2 = i + 2] = border[index2] * num2;
                }
            }
            return border;
        }
    }
}