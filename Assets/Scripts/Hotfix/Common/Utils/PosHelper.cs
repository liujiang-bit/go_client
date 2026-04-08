using Skyunion;
using SprotoType;
using UnityEngine;

namespace Game
{
    
    public class PosHelper
    {
        private const float Const6 = 6.0f;
        private const float Const30 = 30.0f;
        private const float Const100 = 100.0f;
        private const int ServerToClientShow = 600;

        public const int TIlE_BASE_SIZE = 18;

        // 客户端单位与服务器单位为 1:100 的关系。
        // 客户端单位与客户端坐标为 1:6 的关系
        // 客户端单位与客户端格子单位为 1:30 的关系

        #region 服务器转客户端

        /// <summary>
        /// 服务器单位转成客户端单位。   协议不支持浮点型  所以服务器 *100
        /// </summary>
        /// <param name="serverValue"></param>
        /// <returns></returns>
        public static int ServerUnitToClientUnit(int serverValue)
        {
            return (int)(serverValue / Const100);
        }
        
        /// <summary>
        /// 服务器单位转成客户端单位。   协议不支持浮点型  所以服务器 *100
        /// </summary>
        /// <param name="serverValue"></param>
        /// <returns></returns>
        public static float ServerUnitToClientUnit(float serverValue)
        {
            return serverValue / Const100;
        }

        /// <summary>
        /// 服务器单位转成客户端单位。   协议不支持浮点型  所以服务器 *100
        /// </summary>
        /// <param name="serverValue"></param>
        /// <returns></returns>
        public static Vector3 ServerUnitToClientUnit_Vec3(Vector2 serverValue)
        {
            return new Vector3(serverValue.x / Const100, 0, serverValue.y / Const100);
        }

        /// <summary>
        /// 服务器单位转成客户端单位。   协议不支持浮点型  所以服务器 /100
        /// </summary>
        /// <param name="serverValue"></param>
        /// <returns></returns>
        public static Vector2 ServerUnitToClientUnit_Vec2(Vector2 serverValue)
        {
            return new Vector2(serverValue.x / Const100,  serverValue.y / Const100);
        }
        
        /// <summary>
        /// 服务器单位转成客户端坐标。  6个单位一个坐标
        /// </summary>
        /// <param name="serverValue"></param>
        /// <returns></returns>
        public static int ServerUnitToClientPos(int serverValue)
        {
            return (int)(serverValue / Const6 / Const100);
        }
        
        /// <summary>
        /// 服务器单位转成客户端坐标。  6个单位一个坐标
        /// </summary>
        /// <param name="serverValue"></param>
        /// <returns></returns>
        public static int ServerUnitToClientPos(long serverValue)
        {
            return (int) (serverValue / Const6 / Const100);
        }
        
        /// <summary>
        /// 服务器单位转成客户端坐标。  6个单位一个坐标
        /// </summary>
        /// <param name="serverValue"></param>
        /// <returns></returns>
        public static int ServerUnitToClientPos(float serverValue)
        {
            return (int)(serverValue / Const6 / Const100);
        }

        /// <summary>
        /// 服务器单位转成客户端格子单位。  1:100/30
        /// </summary>
        /// <returns></returns>
        public static int ServerUnitToClientGrid(int serverValue)
        {
            return (int)(serverValue / Const100 / Const30);
        }

        /// <summary>
        /// 服务器单位转成客户端格子单位。  1:100/30
        /// </summary>
        /// <returns></returns>
        public static int ServerUnitToClientGrid(float serverValue)
        {
            return (int)(serverValue / Const100 / Const30);
        }

        /// <summary>
        /// 服务器单位转成客户端单位。 1:100
        /// </summary>
        /// <param name="posInfo"></param>
        /// <returns></returns>
        public static Vector3 ServerUnitToClientUnit(PosInfo posInfo)
        {
            return new Vector3(ServerUnitToClientUnit(posInfo.x), 0, ServerUnitToClientUnit(posInfo.y));
        }
        /// <summary>
        /// 服务器单位转成客户端单位。 1:100
        /// </summary>
        /// <param name="posInfo"></param>
        /// <returns></returns>
        public static Vector2 ServerUnitToClientUnit_v2(PosInfo posInfo)
        {
            return new Vector2(ServerUnitToClientUnit(posInfo.x), ServerUnitToClientUnit(posInfo.y));
        }
        /// <summary>
        /// 服务器单位转成客户端坐标。 1:600
        /// </summary>
        /// <param name="posInfo"></param>
        /// <returns></returns>
        public static Vector2Int ServerUnitToClientPos(PosInfo posInfo)
        {
            return new Vector2Int(ServerUnitToClientPos(posInfo.x), ServerUnitToClientPos(posInfo.y));
        }

        #endregion

        #region 客户端转服务器

        
        /// <summary>
        /// 客户端单位转成服务器单位。 100:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientUnitToServerUnit(int clientValue)
        {
            return (int) (clientValue * Const100);
        }

        /// <summary>
        /// 客户端单位转成服务器单位。 100:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientUnitToServerUnit(float clientValue)
        {
            return (int) (clientValue * Const100);
        }

        /// <summary>
        /// 客户端坐标转成服务器单位。 600:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientPosToServerUnit(int clientValue)
        {
            return (int) (clientValue * Const100 * Const6);
        }
        
        /// <summary>
        /// 客户端坐标转成服务器单位。 600:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientPosToServerUnit(float clientValue)
        {
            return (int) (clientValue * Const100 * Const6);
        }
        
        /// <summary>
        /// 客户端单位转成服务器坐标。 6:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientPosToServerPos(float clientValue)
        {
            return (int) (clientValue * Const6);
        }
        
        /// <summary>
        /// 客户端单位转成服务器坐标。 6:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientPosToServerPos(int clientValue)
        {
            return (int) (clientValue * Const6);
        }

        #endregion


        #region 客户端自转
       
        /// <summary>
        /// 客户端单位转成客户端坐标。 1:6
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static Vector2Int ClientUnitToClientPos(Vector3 clientValue)
        {
            return new Vector2Int(ClientUnitToClientPos(clientValue.x), ClientUnitToClientPos(clientValue.z));
        }
        
        public static Vector2Int ClientUnitToClientPos(Vector2 clientValue)
        {
            return new Vector2Int(ClientUnitToClientPos(clientValue.x), ClientUnitToClientPos(clientValue.y));
        }

        /// <summary>
        /// 客户端单位转成客户端坐标。 1:6
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientUnitToClientPos(int clientValue)
        {
            return (int)(clientValue / Const6);
        }
        
        /// <summary>
        /// 客户端单位转成客户端坐标。  1:6
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientUnitToClientPos(float clientValue)
        {
            return (int)(clientValue / Const6);
        }
        
        /// <summary>
        /// 客户端单位转成客户端坐标。  6:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientPosToClientUnit(int clientValue)
        {
            return (int)(clientValue * Const6);
        }
        
        /// <summary>
        /// 客户端单位转成客户端坐标。  6:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientPosToClientUnit(float clientValue)
        {
            return (int)(clientValue * Const6);
        }
        
        /// <summary>
        /// 客户端单位转成客户端格子单位。 1:30
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static Vector2Int ClientUnitToClientGrid(Vector3 clientValue)
        {
            return new Vector2Int(ClientUnitToClientGrid(clientValue.x), ClientUnitToClientGrid(clientValue.z));
        }
        
        /// <summary>
        /// 客户端单位转成客户端格子单位。 1:30
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientUnitToClientGrid(int clientValue)
        {
            return (int) (clientValue / Const30);
        }

        /// <summary>
        /// 客户端单位转成客户端格子单位。 1:30
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientUnitToClientGrid(float clientValue)
        {
            return (int)(clientValue / Const30);
        }
        

        /// <summary>
        /// 客户端格子单位转成客户端单位。 30:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientGridToClientUnit(int clientValue)
        {
            return (int)(clientValue * Const30);
        }
        
        /// <summary>
        /// 客户端格子单位转成客户端单位。 30:1
        /// </summary>
        /// <param name="clientValue"></param>
        /// <returns></returns>
        public static int ClientGridToClientUnit(float clientValue)
        {
            return (int)(clientValue * Const30);
        }


        #endregion
        

        public static string FormatServerPos(PosInfo pos)
        {
            return LanguageUtils.getTextFormat(300032, pos.x / ServerToClientShow, pos.y / ServerToClientShow);
        }

        public static Vector3 ServerPosToClient(PosInfo pos)
        {
            return new Vector3(pos.x/Const100,0,pos.y/Const100);
        }

        public static Vector2 ServerPosToLocal(PosInfo pos)
        {
            return new Vector2(pos.x / ServerToClientShow, pos.y / ServerToClientShow);
        }

        #region 迷雾tile转化

        public static Vector3 TileToWorldPos(int x, int y)
        {
            return new Vector3(x * TIlE_BASE_SIZE + 9, 0, y * TIlE_BASE_SIZE + 9);
        }

        public static Vector2Int FogIndexToTile(int fog)
        {
            var y = (fog - 1) / 400;
            var x = (fog - 1) % 400;
            return new Vector2Int(x, y);
        }

        #endregion
    }

}


