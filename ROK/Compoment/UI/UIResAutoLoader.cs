using Skyunion;
using System;
using UnityEngine;

namespace ROK
{
    public class UIResAutoLoader : MonoBehaviour
    {
        public string m_path;

        private GameObject m_obj;

        private void Awake()
        {
            if (this.m_path != null && this.m_path != string.Empty && this.m_obj == null)
            {
                CoreUtils.assetService.Instantiate(m_path, (GameObject obj) =>
                {
                    m_obj = obj;
                    if (this.m_obj != null)
                    {
                        this.m_obj.transform.SetParent(base.transform, false);
                    }
                });
            }
        }

        private void OnDestroy()
        {
            if (this.m_obj != null)
            {
                CoreUtils.assetService.Destroy(this.m_obj);
                this.m_obj = null;
            }
        }
    }
}