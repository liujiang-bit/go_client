using System;
using System.Collections.Generic;
using Skyunion;
using UnityEngine;

namespace Hotfix
{
    
    public sealed class BattleUIEffectHandler:  IBattleUIEffectHandler
    {
        private  readonly  Dictionary<int, GameObject> dicUIEffect= new Dictionary<int, GameObject>();

        public void Init()
        {
            
        }

        public void Play(int id, string name, Transform transform)
        {
            Remove(id);

            AddEffectr(name, transform, (go) =>
            {
                if (!dicUIEffect.ContainsKey(id))
                {
                    dicUIEffect.Add(id,go);
                }
            });
            
        }

        public void Stop(int id)
        {
            GameObject eff;
            if (dicUIEffect.TryGetValue(id, out eff))
            {
                eff.SetActive(false);
            }
        }

        public void Remove(int id)
        {
            GameObject eff;
            if (dicUIEffect.TryGetValue(id, out eff))
            {
                if (eff != null)
                {                
                    CoreUtils.assetService.Destroy(eff); 
                }

                dicUIEffect.Remove(id);
            }
        }        

        private void AddEffectr(string name,Transform transform, Action<GameObject> callBack)
        {
            CoreUtils.assetService.Instantiate(name, (go) =>
            {
                if (transform == null)
                {
                    CoreUtils.assetService.Destroy(go);
                }           
                go.name = name;
                go.SetActive(true);
                go.transform.SetParent(transform);
                go.transform.position = Vector3.zero;
                go.transform.localPosition= Vector3.zero;
                go.transform.localScale = Vector3.one;
                callBack?.Invoke(go);
            });
        }

        public void Clear()
        {
            foreach(var effect in dicUIEffect.Values)
            {
                if (effect != null)
                {
                    CoreUtils.assetService.Destroy(effect);
                }                
            }
            dicUIEffect.Clear();
        }
    }
}