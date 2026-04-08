using System.Collections.Generic;
using Data;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    public sealed class BattleBuffHandler:  IBattleBuffHandler
    {
        // 对象身上的bug只可能有一个，所以用一个字典就够了
        Dictionary<int, GameObject> m_objectBufGoDic = new Dictionary<int, GameObject>();
        Dictionary<int, int> m_objectBufIdDic = new Dictionary<int, int>();
        public void Init()
        {
        }
        // 重登清理buf
        public void Clear()
        {
            m_objectBufIdDic.Clear();

            foreach(var value in m_objectBufGoDic.Values)
            {
                if (value !=  null)
                {
                    CoreUtils.assetService.Destroy(value);
                }                
            }
            m_objectBufGoDic.Clear();
        }

        // 地图对象添加Buf
        public void CreateBuffGo(int id, int buffid, Transform parent)
        {
            // 先销毁旧的buf
            ClearBuff(id);
            SkillStatusDefine statusDefine = CoreUtils.dataService.QueryRecord<SkillStatusDefine>(buffid);
            if(statusDefine != null && !string.IsNullOrEmpty(statusDefine.showEffect))
            {
                m_objectBufIdDic.Add(id, buffid);
                // 添加buf到对象上面
                CoreUtils.assetService.Instantiate(statusDefine.showEffect, (go) =>
                {
                    // 保证只有最后一次的buf才会被添加进来
                    int nBufId;
                    if (m_objectBufIdDic.TryGetValue(id, out nBufId))
                    {
                        if (nBufId == buffid)
                        {
                            go.transform.SetParent(parent);
                            go.transform.localPosition = Vector3.zero;
                            go.transform.localRotation = Quaternion.identity;
                            go.transform.localScale = Vector3.one;
                            m_objectBufGoDic.Add(id, go);
                            m_objectBufIdDic.Remove(id);
                            return ;
                        }
                    }
                    // 不符合条件就放回去回收池
                    CoreUtils.assetService.Destroy(go);
                });
            }
        }

        // 状态变更清理Buf
        public void ClearBuff(int id)
        {
            if (m_objectBufIdDic.ContainsKey(id))
            {
                m_objectBufIdDic.Remove(id);
            }
            ClearGo(id);
        }

        // 删除指定地图对象的buf
        private void ClearGo(int id)
        {
            GameObject gameObject;
            if(m_objectBufGoDic.TryGetValue(id, out gameObject))
            {
                if (gameObject.activeSelf == true)
                {
                    CoreUtils.assetService.Destroy(gameObject);
                }
                m_objectBufGoDic.Remove(id);
            }
        }
    }
}