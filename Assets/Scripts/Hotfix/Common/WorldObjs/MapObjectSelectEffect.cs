using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Skyunion;

namespace Game
{
    public class MapObjectSelectEffect 
    {
        public MapObjectSelectEffect(string assetName)
        {
            m_assetName = assetName;
        }

        public void AttachTransform(Transform parent, float scale)
        {
            if (parent == m_parent) return;
            m_parent = parent;
            m_scale = scale;
            if(m_selectEffect == null)
            {               
                if(m_status == Status.Loading)
                {
                    return;
                }
                InstanceObject();                
            }
            else
            {
                InternalAttach(parent);
            }            
        }

        public void UnAttach()
        {
            if (m_parent == null) return;
            m_parent = null;
            if(m_selectEffect != null)
            {
                m_selectEffect.transform.SetParent(null);
                m_selectEffect.SetActive(false);
            }
        }

        public void Destroy()
        {
            m_status = Status.Destroy;
            if(m_selectEffect != null)
            {
                CoreUtils.assetService.Destroy(m_selectEffect);
                m_selectEffect = null;
            }
        }

        public void SetActiveState(bool state)
        {
            if (m_selectEffect != null)
            {
                m_selectEffect.SetActive(state);
            }
        }

        private void InstanceObject()
        {
            m_status = Status.Loading;
            CoreUtils.assetService.Instantiate(m_assetName, (go) =>
            {
                if(m_status == Status.Destroy)
                {
                    CoreUtils.assetService.Destroy(go);
                    return;
                }
                m_selectEffect = go;
                if(m_parent != null)
                {
                    InternalAttach(m_parent);
                }
            });
        }

        private void InternalAttach(Transform parent)
        {
            m_selectEffect.SetActive(true);
            m_selectEffect.transform.SetParent(parent);
            m_selectEffect.transform.localPosition = Vector3.zero;
            m_selectEffect.transform.localScale = new Vector3(m_scale, m_scale, m_scale);
        }

        private enum Status
        {
            None,
            Loading,
            Loaded,
            Destroy
        }

        private Status m_status = Status.None;
        private Transform m_parent = null;
        private string m_assetName = string.Empty;
        private GameObject m_selectEffect = null;
        private float m_scale = 1;
    }
}


