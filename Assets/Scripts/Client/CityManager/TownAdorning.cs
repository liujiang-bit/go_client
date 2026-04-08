using UnityEngine;
using UnityEngine.Serialization;

namespace Client
{
    /// <summary>
    ///马路物件 挂载在Road_1_1上
    /// </summary>
    public class TownAdorning : MonoBehaviour
    {
        [Header("装饰物图片")]
        [FormerlySerializedAs("m_encode_sprite")]
        public Sprite[] m_sprite;

        [Header("装饰物蒙版")]
        [FormerlySerializedAs("m_encode_sprite_mask")]
        public Sprite[] m_sprite_mask;

        private void SetIndex(int index)
        {
            if (index >= 0 && index <= 15)
            {
                Sprite sprite = m_sprite[index];
                GetComponent<SpriteRenderer>().sprite = sprite;
                Sprite mask_sprite = m_sprite_mask[index];
                var component = GetComponent<MaskSprite>();
                component.m_mask_sprite = mask_sprite;
            }
        }
    }
}