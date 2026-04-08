using System;
using UnityEngine;

namespace ROK
{
    public class GamerInfoSmooth : MonoBehaviour
    {
        public delegate void SmoothEndCallBack(int char_id);

        public AnimationCurve ani_cur;

        private int char_id;

        private float total_time;

        private float cur_time;

        private bool bPlay;

        private float max_offset_x;

        private float max_offset_y;

        private RectTransform rectTransform;

        private Vector2 cur_offset_pos;

        private SmoothEndCallBack onSmoothEnd;

        private Vector2 start_pos;

        private void Start()
        {
            this.cur_offset_pos = Vector2.zero;
            this.start_pos = Vector2.zero;
            this.rectTransform = base.gameObject.GetComponent<RectTransform>();
        }

        private void Update()
        {
            try
            {
                if (this.bPlay)
                {
                    float num = this.ani_cur.Evaluate(this.cur_time / this.total_time);
                    this.cur_offset_pos.x = num * this.max_offset_x;
                    this.cur_offset_pos.y = num * this.max_offset_y;
                    this.rectTransform.anchoredPosition = this.start_pos + this.cur_offset_pos;
                    this.cur_time += Time.deltaTime;
                    if (this.cur_time >= this.total_time)
                    {
                        this.bPlay = false;
                        this.onSmoothEnd(this.char_id);
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public void Initsmooth(int char_id, SmoothEndCallBack call_back)
        {
            this.char_id = char_id;
            this.onSmoothEnd = call_back;
        }

        public void StartSmoothMove(int x)
        {
            if (x == 0)
            {
                StartSmoothMove(100, 100, 2);
            }
            else if (x == 1)
            {
                StartSmoothMove(500, 300, 2);
            }
        }

        public void StartSmoothMove(float x, float y, float time)
        {
            if (this.rectTransform == null)
            {
                return;
            }
            this.bPlay = true;
            this.cur_time = 0f;
            this.total_time = time;
            this.max_offset_x = x - this.rectTransform.anchoredPosition.x;
            this.max_offset_y = y - this.rectTransform.anchoredPosition.y;
            this.start_pos = this.rectTransform.anchoredPosition;
        }

        public void StopSmoothMove()
        {
            this.bPlay = false;
        }
    }
}