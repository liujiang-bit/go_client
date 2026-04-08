using Client;
using Skyunion;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game
{
    public class InputSliderControl
    {
        public UnityAction<int, int> m_numChangeCallback;

        private GameSlider sd_count_GameSlider;
        private GameInput ipt_count_GameInput;
        private LanguageText show_text;

        private bool m_isChangeInput;
        private bool m_isChangeSilder;

        private int m_index;
        private int MinVal;
        private int MaxVal;

        private float m_lastPlaySoundTime;

        public void Init(GameInput input, GameSlider slider, UnityAction<int, int> callback)
        {
            ipt_count_GameInput = input;
            sd_count_GameSlider = slider;

            m_numChangeCallback = callback;

            ipt_count_GameInput.onValueChanged.AddListener(OnInputChange);
            sd_count_GameSlider.onValueChanged.AddListener(OnSilderChange);
        }

        public void Init(GameInput input, GameSlider slider, LanguageText text,UnityAction<int, int> callback)
        {
            ipt_count_GameInput = input;
            sd_count_GameSlider = slider;
            show_text = text;

            m_numChangeCallback = callback;

            ipt_count_GameInput.onValueChanged.AddListener(OnInputChange);
            sd_count_GameSlider.onValueChanged.AddListener(OnSilderChange);
        }

        public void UpdateMinMax(int minVal, int maxVal)
        {
            if (minVal > maxVal)
            {
                Debug.LogErrorFormat("minVal:{0} > maxVal:{1}", minVal, maxVal);
                maxVal = minVal;
            }
            MinVal = minVal;
            MaxVal = maxVal;
        }

        public void UpdateIndex(int num)
        {
            m_index = num;
        }

        public void SetInputVal(int val)
        {
            //如果输入的文本 与 当前ipt控件显示的文本一致 会导致OnInputChange不会调用到 所以这边要强制执行更新Slider
            if (val.ToString() == ipt_count_GameInput.text)
            {
                SetSilderVal((float)val / MaxVal);
            }
            m_isChangeInput = true;
            ipt_count_GameInput.text = val.ToString();
        }

        public void SetText(int val)
        {
            if (show_text != null)
            {
                show_text.text = ClientUtils.FormatComma(val);
            }
        }

        public void SetSilderVal(float val)
        {
            m_isChangeSilder = true;
            sd_count_GameSlider.value = val;
        }

        private void NumChangeCallback(int num)
        {
            m_numChangeCallback(num, m_index);
        }

        public void OnInputChange(string val)
        {
            if (!m_isChangeInput)
            {
                return;
            }
            int num = 0;
            bool isRest = false;
            if (val.Length < 1)
            {
                num = MinVal;
                isRest = true;
            }
            else
            {
                try
                {
                    num = int.Parse(val);
                }
                catch
                {
                    num = MinVal;
                    isRest = true;
                }
                if (val.Length != num.ToString().Length)
                {
                    SetInputVal(num);
                    return;
                }
            }
            if (num < MinVal)
            {
                num = MinVal;
                isRest = true;
            }
            else if (num > MaxVal)
            {
                num = MaxVal;
                isRest = true;
            }
            if (isRest)
            {
                SetInputVal(num);
                return;
            } 
            m_isChangeSilder = false;
            sd_count_GameSlider.value = (float)num / MaxVal;
            m_isChangeSilder = true;
            SetText(num);
            NumChangeCallback(num);
        }

        private void OnSilderChange(float val)
        {
            if (!m_isChangeSilder)
            {
                return;
            }
            int num = (int)(val * MaxVal);
            if (num == 0)
            {
                if (val > 0)
                {
                    sd_count_GameSlider.value = (float)1 / MaxVal;
                    return;
                }
                num = MinVal;
            }
            m_isChangeInput = false;
            ipt_count_GameInput.text = num.ToString();
            m_isChangeInput = true;
            SetText(num);
            if (Time.realtimeSinceStartup - m_lastPlaySoundTime > 0.02f)
            {
                m_lastPlaySoundTime = Time.realtimeSinceStartup;
                CoreUtils.audioService.PlayOneShot(RS.SoundUiCommonSlider);
            }
            NumChangeCallback(num);
        }
    }

    public class ResCostModel
    {
        private PlayerProxy m_playerProxy;
        private Dictionary<int, Transform> m_resCostNodeMap = new Dictionary<int, Transform>();
        private Dictionary<int, LanguageText> m_resCostTextMap = new Dictionary<int, LanguageText>();
        public Dictionary<int, Int64> ResCostData = new Dictionary<int, Int64>();
        public int m_resCostCount;
        private int m_enumFood = (int)EnumCurrencyType.food;
        private int m_enumWood = (int)EnumCurrencyType.wood;
        private int m_enumStone = (int)EnumCurrencyType.stone;
        private int m_enumGold = (int)EnumCurrencyType.gold;
        private bool m_isInit;
        private bool m_isInitTransform;
        private bool m_isInitText;
        private Color m_normalColor = Color.white;

        public void SetTransform(Transform food, Transform wood, Transform stone, Transform gold)
        {
            m_resCostNodeMap.Add((int)EnumCurrencyType.food, food);
            m_resCostNodeMap.Add((int)EnumCurrencyType.wood, wood);
            m_resCostNodeMap.Add((int)EnumCurrencyType.stone, stone);
            m_resCostNodeMap.Add((int)EnumCurrencyType.gold, gold);
            m_isInitTransform = true;
        }

        public void SetText(LanguageText food, LanguageText wood, LanguageText stone, LanguageText gold)
        {
            m_normalColor = food.color;
            m_resCostTextMap.Add((int)EnumCurrencyType.food, food);
            m_resCostTextMap.Add((int)EnumCurrencyType.wood, wood);
            m_resCostTextMap.Add((int)EnumCurrencyType.stone, stone);
            m_resCostTextMap.Add((int)EnumCurrencyType.gold, gold);
            m_isInitText = true;
        }

        public bool UpdateResCost(Int64 food, Int64 wood, Int64 stone, Int64 gold)
        {
            if (!m_isInit)
            {
                if (!m_isInitTransform)
                {
                    Debug.LogError("请先设置Transform");
                    return false;
                }
                if (!m_isInitText)
                {
                    Debug.LogError("请先设置Text");
                    return false;
                }
                m_isInit = true;
                m_playerProxy = AppFacade.GetInstance().RetrieveProxy(PlayerProxy.ProxyNAME) as PlayerProxy;
            }

            ResCostData[m_enumFood] = food;
            ResCostData[m_enumWood] = wood;
            ResCostData[m_enumStone] = stone;
            ResCostData[m_enumGold] = gold;

            int notEnoughNum = 0;
            bool isEnough = false;
            foreach (var data in m_resCostNodeMap)
            {
                if (ResCostData.ContainsKey(data.Key) && ResCostData[data.Key] > 0)
                {
                    data.Value.gameObject.SetActive(true);
                    if (m_playerProxy.GetResNumByType(data.Key) >= ResCostData[data.Key])
                    {
                        isEnough = true;
                    }
                    else
                    {
                        isEnough = false;
                        notEnoughNum = notEnoughNum + 1;
                    }
                    m_resCostTextMap[data.Key].text = ClientUtils.CurrencyFormat(ResCostData[data.Key]);
                    if (isEnough)
                    {
                        m_resCostTextMap[data.Key].color = m_normalColor;
                    }
                    else
                    {
                        m_resCostTextMap[data.Key].color = Color.red;
                    }
                }
                else
                {
                    data.Value.gameObject.SetActive(false);
                }
            }

            if (notEnoughNum > 0)
            {
                return false;
            }
            return true;
        }
    }
}