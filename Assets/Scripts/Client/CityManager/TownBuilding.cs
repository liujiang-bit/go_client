using UnityEngine;
using UnityEngine.Serialization;

namespace Client
{
    /// <summary>
    /// 挂载在城市建筑物上  显示升级图标  播放建筑动画 触碰等
    /// </summary>
    public class TownBuilding : MonoBehaviour
    {
        [Header("可升级图标")]
        [FormerlySerializedAs("m_upgradeBoard")]
        public GameObject upgradeIcon;

        [Header("建筑点击区域")]
        [FormerlySerializedAs("m_Colliders")]
        public BoxCollider[] colliders;

        [Header("可变色图片")]
        [FormerlySerializedAs("m_changeColorObjs")]
        public SpriteRenderer[] colorSprites;

        public void EnableUpgrade(bool enable)
        {
            if (upgradeIcon != null)
            {
                upgradeIcon.SetActive(enable);
            }
        }
        public void SetColliderOrder(float y)
        {
            if (colliders.Length > 0)
            {
                return;
            }
            foreach (BoxCollider boxCollider in colliders)
            {
                Vector3 localPosition = boxCollider.transform.localPosition;
                localPosition.y = y;
                boxCollider.transform.localPosition = localPosition;
            }
        }

        public void SetColor(Color c)
        {
            for (int i = 0; i < colorSprites.Length; i++)
            {
                var spriteRenderer = colorSprites[i];
                if (spriteRenderer != null)
                {
                    spriteRenderer.color = c;
                    MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
                    spriteRenderer.GetPropertyBlock(materialPropertyBlock);
                    materialPropertyBlock.SetColor("_Color_1", c);
                    spriteRenderer.SetPropertyBlock(materialPropertyBlock);
                    spriteRenderer.color = Color.white;
                }
            }
        }
    }
}
