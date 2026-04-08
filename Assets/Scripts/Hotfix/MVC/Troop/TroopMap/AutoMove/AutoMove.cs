using System;
using System.Collections.Generic;
using UnityEngine;

namespace Hotfix
{
    public class AutoMoveData
    {
        public int id;
        public List<Vector2> path = new List<Vector2>();
        public Action<int,int> moveCallBack;
        public GameObject go;
        public int startIndex;
    }

    public sealed class AutoMove : IAutoMove
    {
        private readonly Dictionary<int, AutoMoveData> _dicMove = new Dictionary<int, AutoMoveData>();
        private int index = 1;
        private bool isPlay = true;

        public void Init(object pamr)
        {
            AutoMoveData autoMoveData = pamr as AutoMoveData;
//            Debug.Log("++++AutoMoveData Init初始化 index" + autoMoveData.startIndex);
            if (autoMoveData != null)
            {
                index = autoMoveData.startIndex + 1;
                _dicMove[autoMoveData.id] = autoMoveData;
            }
        }

        public void Update(int id)
        {
            if (!isPlay)
            {
                return;
            }

            AutoMoveData moveData;
            if (_dicMove.TryGetValue(id, out moveData))
            {
                if (moveData.go == null)
                {
                    return;
                }

                Vector2 v2= new Vector2(moveData.go.transform.position.x,moveData.go.transform.position.z);  
                if (index >= 0 && index < moveData.path.Count)
                {
                    float distance =Vector2.Distance(v2, moveData.path[index]);
                    if (distance < 0.2f)
                    {
                        index += 1;
                        if (moveData.moveCallBack != null)
                        {
                            moveData.moveCallBack.Invoke(id,index-1);
                        }
                    }
                }
            }   
        }

        public void Remove(int id)
        {
            if (_dicMove.ContainsKey(id))
            {
                _dicMove.Remove(id);
            }
        }


        public object GetData(int id)
        {
            AutoMoveData autoMoveData;
            if (_dicMove.TryGetValue(id, out autoMoveData))
            {
                return autoMoveData;
            }

            return null;
        }
    }
}