using System;
using System.Collections.Generic;
using Client;
using Skyunion;
using UnityEngine;

namespace Game.WorldObjs
{
   

    public class WorldMapViewObjFactory
    {
        
        
        private static HashSet<long> m_isLoading = new HashSet<long>();

        public static void SysLoadWorldObj(MapObjectInfoEntity data,string prefabname,Action<GameObject,MapObjectInfoEntity> callback)
        {

            if (data.isLoading == false && data.gameobject == null && data.delTime <=0 )
            {
                data.isLoading = true;


                if (Common.IsInViewPort2DS(WorldCamera.Instance().GetCamera(), data.objectPos.x/100, data.objectPos.y/100, String.Empty))
                {
                    CoreUtils.assetService.Instantiate(prefabname, (obj) =>
                    {

                        if (data.delTime>0)
                        {
                            CoreUtils.assetService.Destroy(obj);
                            return;
                        }
                        data.gameobject = obj;
                        if (obj!=null)
                        {
                            callback?.Invoke(obj,data);
                        }
                    });
                }
                else
                {
                    CoreUtils.assetService.Instantiate(prefabname, (obj) =>
                    {
                        if (data.delTime>0)
                        {
                            CoreUtils.assetService.Destroy(obj);
                            return;
                        }
                        data.gameobject = obj;
                        if (obj!=null)
                        {
                            callback?.Invoke(obj,data);
                        }
                    });
                }


            }
        }
        
        
        public static void SysLoadBlockObj(MapObjectInfoEntity data,string prefabname,Action<GameObject,MapObjectInfoEntity> callback)
        {
            if ( data.delTime <= 0)
            {
                CoreUtils.assetService.Instantiate(prefabname, (obj) =>
                {
                    if (data.delTime > 0)
                    {
                        CoreUtils.assetService.Destroy(obj);
                        return;
                    }

                    if (obj != null)
                    {
                        callback?.Invoke(obj, data);
                    }
                });
            }
        }
        
        
        public static void SysLoadBlockArmyObj(GameObject data,string prefabname,Action<GameObject,GameObject,float> callback,float rd)
        {
            if (!m_isLoading.Contains(data.GetHashCode()))
            {
                m_isLoading.Add(data.GetHashCode());
                CoreUtils.assetService.Instantiate(prefabname, (obj) =>
                {
                    if (data == null)
                    {
                        CoreUtils.assetService.Destroy(obj);
                        return;
                    }

                    if (obj != null)
                    {
                        callback?.Invoke(obj, data,rd);
                    }
                });
            }
        }

        public static void ClearLoading()
        {
            m_isLoading.Clear();
        }


    }
}