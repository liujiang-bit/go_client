using System;
using UnityEngine;
using UnityEngine.UI;

namespace ROK
{
    public class ImageSheetAnimation : MonoBehaviour
    {
        public Image targetImage;

        public Sprite[] allSprites;

        private float _mTime;

        private float _mSpeed = 10f;

        private int m_curIndex;

        private void Start()
        {
        }

        private void Update()
        {
            if (this.targetImage != null && this.allSprites.Length > 0)
            {
                this._mTime += Time.deltaTime;
                int num = (int)(this._mTime * this._mSpeed) % this.allSprites.Length;
                if (this.m_curIndex != num)
                {
                    this.targetImage.sprite = this.allSprites[num];
                    this.m_curIndex = num;
                }
            }
        }
    }
}
