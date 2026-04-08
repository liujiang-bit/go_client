using System.Collections.Generic;
using Skyunion;
using UnityEngine;

namespace Game
{
    public class BaseWorldEffectMediator : IBaseWorldEffectMediator
    {
        Dictionary<string, string> loadingEffect = new Dictionary<string, string>();
        Dictionary<string, GameObject> loadedEffect = new Dictionary<string, GameObject>();
        Dictionary<string, string> wantDestroyedLoadingEffect = new Dictionary<string, string>();
        Dictionary<string, bool> wantSetEffectActive = new Dictionary<string, bool>();
        
        protected void CreateEffect(MapObjectInfoEntity objectInfoEntity, string effectName)
        {
            if (loadingEffect.ContainsKey(effectName) || loadedEffect.ContainsKey(effectName))
            {
                return;
            }
            
            loadingEffect.Add(effectName,effectName);
            CoreUtils.assetService.Instantiate(effectName, (obj) =>
            {
                if (obj==null)
                {
                    Debug.Log("null effect:"+effectName);
                    return;
                }
                loadingEffect.Remove(effectName);
                loadedEffect.Add(effectName, obj);

                if (wantSetEffectActive.ContainsKey(effectName))
                {
                    obj.SetActive(wantSetEffectActive[effectName]);
                    wantSetEffectActive.Remove(effectName);
                }
                
                if (wantDestroyedLoadingEffect.ContainsKey(effectName))
                {
                    wantDestroyedLoadingEffect.Remove(effectName);
                    DestroyEffect(effectName);
                }
                else
                {
                    OnLoadedEffect(objectInfoEntity, effectName, obj);    
                }
            });
        }

        protected void SetEffectActive(MapObjectInfoEntity objectInfoEntity, string effectName, bool value)
        {
            if (loadedEffect.ContainsKey(effectName))    // 已创建过特效
            {
                if (loadedEffect[effectName] != null)
                {
                    loadedEffect[effectName].SetActive(value);    
                }
            }
            else if(value)    // 未创建过特效，且需要显示特效，则创建
            {
                if (!wantSetEffectActive.ContainsKey(effectName))
                {
                    wantSetEffectActive.Add(effectName, value);    
                }

                wantSetEffectActive[effectName] = value;
                CreateEffect(objectInfoEntity, effectName);
            }
        }

        protected void DestroyEffect(string effectName)
        {
            if (loadedEffect.ContainsKey(effectName))
            {
                GameObject obj = loadedEffect[effectName];
                loadedEffect.Remove(effectName);
                CoreUtils.assetService.Destroy(obj);
            }
            else
            {
                wantDestroyedLoadingEffect.Add(effectName,effectName);
            }
        }

        protected virtual void OnLoadedEffect(MapObjectInfoEntity objectInfoEntity, string effectName, GameObject obj)
        {
            if (obj != null && objectInfoEntity != null && objectInfoEntity.gameobject != null)
            {
                obj.transform.SetParent(objectInfoEntity.gameobject.transform);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localScale = Vector3.one;
                obj.transform.localRotation = Quaternion.identity;
            }
        }

        public virtual void UpdateEffects(MapObjectInfoEntity objectInfoEntity)
        {
            
        }

        public void DisposeEffect()
        {
            foreach (var kv in loadedEffect)
            {
                GameObject obj = kv.Value;
                CoreUtils.assetService.Destroy(obj);
            }
            loadedEffect.Clear();
        }
    }
}