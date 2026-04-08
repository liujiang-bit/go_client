using Skyunion;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Client
{
    public class WarFogMgr : MonoBehaviour
    {
        private class FadeGroup
        {
            public int groupId;

            public int fadeType;

            public int tileX;

            public int tileY;

            public float fadeTimeHigh;

            public float fadeRateHigh = 0.08f;

            public static float fadeTimeLow = 1f;

            public static float fadeRateLow = 0.012f;

            public float hightColor;

            public int groupSize = WarFogMgr.GROUP_SIZE;

            private Color curColor;

            private Color diffColor;

            public FadeGroup(int id, int type)
            {
                this.groupId = id;
                this.fadeType = type;
                this.ResetFade();
                this.hightColor = 0.5f;
                this.diffColor = WarFogMgr.exploreSelected - Color.white;
                this.curColor = Color.white;
            }

            public void ResetFade()
            {
            }

            private void UpdateLowLevel()
            {
                bool flag = this.fadeType == WarFogMgr.FADE_TYPE_EXPLORE;
                for (int i = this.tileX; i < this.tileX + this.groupSize; i++)
                {
                    for (int j = this.tileY; j < this.tileY + this.groupSize; j++)
                    {
                        if (WarFogMgr.HasFogAt(i, j, true))
                        {
                            WarFogMgr.SetFogTileColor(i, j, (!flag) ? Color.white : WarFogMgr.exploreSelected, WarFogMgr.FadeGroup.fadeTimeLow);
                        }
                    }
                }
            }

            private void UpdateHighLevel()
            {
                if (WarFogMgr.IsAllFogClear())
                {
                    return;
                }
                int num = 0;
                if (this.fadeType == WarFogMgr.FADE_TYPE_EXPLORE)
                {
                    this.curColor += this.diffColor * this.fadeRateHigh;
                    num = Mathf.RoundToInt((this.curColor.r - 1f) / this.diffColor.r / Mathf.Abs(this.fadeRateHigh));
                }
                Color color = new Color(1f, 1f, 1f, (float)num / 255f);
                this.hightColor += this.fadeRateHigh * 0.08f;
                this.curColor.a = this.hightColor;
                this.fadeTimeHigh += this.fadeRateHigh;
                if (this.fadeTimeHigh >= 1.5f || this.fadeTimeHigh <= 0f)
                {
                    this.fadeRateHigh = -this.fadeRateHigh;
                }
                for (int i = this.tileX; i < this.tileX + this.groupSize; i++)
                {
                    for (int j = this.tileY; j < this.tileY + this.groupSize; j++)
                    {
                        if (WarFogMgr.HasFogAt(i, j, true))
                        {
                            WarFogMgr.SetPixel(WarFogMgr.LodMask, i, j, color, false);
                            WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, i, j, this.curColor, false);
                        }
                    }
                }
                WarFogMgr.LodMask.Apply();
                WarFogMgr.LodMaskAlpha.Apply();
            }

            public void Update()
            {
                if (WarFogMgr.IsHighLevel())
                {
                    this.UpdateHighLevel();
                }
                else
                {
                    this.UpdateLowLevel();
                }
            }
        }

        private class FadeTile
        {
            Vector2Int[] m_tiles;
            float m_fStartTime;
            float m_fFadeTime;
            Color m_color = Color.white;
            bool m_bWillDelete;
            public FadeTile(Vector2Int [] tiles, float fadeTime)
            {
                m_bWillDelete = false;
                m_tiles = tiles;
                m_fStartTime = Time.realtimeSinceStartup;
                m_fFadeTime = fadeTime;
                m_color = MaskHas;
            }
            public void Update()
            {
                float elapse = Time.realtimeSinceStartup - m_fStartTime;
                if (elapse > m_fFadeTime)
                {
                    elapse = m_fFadeTime;
                    m_bWillDelete = true;
                }

                float alpha = elapse / m_fFadeTime;
                m_color.a = (1-alpha)* MaskHas.a;
                for (int i = 0; i < m_tiles.Length; i++)
                {
                    WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, m_tiles[i].x, m_tiles[i].y, m_color, false);
                }
                WarFogMgr.LodMaskAlpha.Apply();
            }
            public bool WilllDelete()
            {
                return m_bWillDelete;
            }
        }

        private class FogTileCache
        {
            public GameObject go;

            public MeshRenderer render;
        }

        private static Color MaskClear = new Color(1f, 1f, 1f, 0f);

        private static Color MaskHas = new Color(1f, 1f, 1f, 0.5f);

        private static Color MaskColor = new Color(1f, 1f, 1f, 0f);

        public static int FogNumber = 0;

        public static int LodLevel = 0;

        private static int MapSize;

        private static byte[] FogUnlockData;

        private static byte[] FogDisabledData;

        private static bool IsApplyMask = false;

        private static Dictionary<int, int> TempOpenList = new Dictionary<int, int>();

        private static Texture2D LodMask = null;

        private static Texture2D LodMaskAlpha = null;

        private static GameObject HighLevelFog;

        private static GameObject MountainLevelFog;

        private static Dictionary<int, WarFogMgr.FadeGroup> Groups = new Dictionary<int, WarFogMgr.FadeGroup>();

        private static int ID_FORMATER = 100000;

        public static int GROUP_SIZE = 5;

        private static Transform LowLevelFogTrans;

        private static int ScaleSize = 2;

        private static bool AllFogClear = false;

        private static short[] connectionData = null;

        private static short NotConnection = 0;

        private static short ConnectionOpen = 1;

        private static short ConnectionClose = 2;

        private static List<int> LoopStack;

        private static MaterialPropertyBlock props;

        private static float FADE_DEFAULT_TIME = 1.2f;

        private static float FADE_DEFAULT_RATE = 0.01f;

        private static int FADE_TYPE_NULL = 0;

        private static int FADE_TYPE_CLICK = 1;

        private static int FADE_TYPE_EXPLORE = 2;

        public static Color exploreSelected = new Color(0.7372549f, 0.956862748f, 1f, 0.8f);

        private static Dictionary<string, WarFogMgr.FogTileCache> FogTileMap = new Dictionary<string, WarFogMgr.FogTileCache>();

        private static List<WarFogMgr.FogTileCache> FogTileBuffer = new List<WarFogMgr.FogTileCache>();

        private static List<FadeTile> FadeTileFog = new List<FadeTile>();

        private void Awake()
        {
            WarFogMgr.HighLevelFog = base.transform.Find("highLevelFog").gameObject;
            WarFogMgr.MountainLevelFog = base.transform.Find("mountainLevelFog").gameObject;
            WarFogMgr.props = new MaterialPropertyBlock();

            //byte[] unlock = new byte[400 * 400 * 8];
            //for (int i = 0; i < unlock.Length; i++)
            //{
            //    unlock[i] = 0;
            //}
            //InitFogSystem(400, unlock, null);
            //for (int i = 0; i < 2; i++)
            //{
            //    for (int j = 0; j < 2; j++)
            //    {
            //        OpenFog(i, j);
            //    }
            //}
            //ChangeLevel(2);
            //CreateFadeGroup(0, FADE_TYPE_NULL);
            //CreateFadeGroup(FadeTile2Id(0, 1), FADE_TYPE_CLICK);
        }

        private void Destroy()
        {
            WarFogMgr.Groups.Clear();
            WarFogMgr.TempOpenList.Clear();
        }

        public static Vector3 GetWorldHitPosition(int x, int y)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3((float)x, (float)y, 0f));
            int num = LayerMask.NameToLayer("FogCollider");
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit, float.PositiveInfinity, 1 << num))
            {
                return raycastHit.point;
            }
            return Vector3.zero;
        }

        public static void SaveDisabled(string path, byte[] disabledData)
        {
            try
            {
                File.WriteAllBytes(path, disabledData);
            }
            catch (Exception var_0_0C)
            {
            }
            WarFogMgr.FogDisabledData = disabledData;
        }

        public static bool LoadDisabled(string path, int minSize = 0)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            WarFogMgr.FogDisabledData = File.ReadAllBytes(path);
            return WarFogMgr.FogDisabledData != null && WarFogMgr.FogDisabledData.Length > minSize;
        }

        public static void InitFogSystem(int mapSize, Int64[] unlockedData, Transform lowLevelTrans)
        {
            byte[] data = new byte[unlockedData.Length*8];
            for (int i = 0; i < unlockedData.Length; i++)
            {
                var bytes = BitConverter.GetBytes(unlockedData[i]);
                Array.Copy(bytes, 0, data, i * 8, 8);
            }
            InitFogSystem(mapSize, data, lowLevelTrans);
        }

        public static void InitFogSystem(int mapSize, byte[] unlockedData, Transform lowLevelTrans)
        {
            WarFogMgr.FadeTileFog.Clear();
            WarFogMgr.Groups.Clear();
            WarFogMgr.TempOpenList.Clear();
            WarFogMgr.ClearAllFogTileCache();
            if (mapSize > 0)
            {
                WarFogMgr.MapSize = mapSize;
            }
            else
            {
                WarFogMgr.MapSize = (int)Math.Sqrt((double)(unlockedData.Length * 8));
            }
            WarFogMgr.AllFogClear = true;
            for (int i = 0; i < unlockedData.Length; i++)
            {
                if (unlockedData[i] != 255)
                {
                    WarFogMgr.AllFogClear = false;
                    break;
                }
            }
            if (WarFogMgr.LodMask == null && !WarFogMgr.AllFogClear)
            {
                WarFogMgr.LodMask = new Texture2D(WarFogMgr.MapSize * WarFogMgr.ScaleSize, WarFogMgr.MapSize * WarFogMgr.ScaleSize, TextureFormat.Alpha8, false);
                WarFogMgr.LodMaskAlpha = new Texture2D(WarFogMgr.MapSize * WarFogMgr.ScaleSize, WarFogMgr.MapSize * WarFogMgr.ScaleSize, TextureFormat.Alpha8, false);
            }
            WarFogMgr.FogUnlockData = unlockedData;
            WarFogMgr.LowLevelFogTrans = lowLevelTrans;
            WarFogMgr.FogNumber = 0;
            if (WarFogMgr.AllFogClear)
            {
                return;
            }
            for (int j = 0; j < WarFogMgr.MapSize; j++)
            {
                for (int k = 0; k < WarFogMgr.MapSize; k++)
                {
                    if (WarFogMgr.HasFogAt(j, k, true))
                    {
                        WarFogMgr.FogNumber++;
                        WarFogMgr.SetPixel(WarFogMgr.LodMask, j, k, WarFogMgr.MaskColor, false);
                        WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, j, k, WarFogMgr.MaskHas, false);
                    }
                    else
                    {
                        WarFogMgr.SetPixel(WarFogMgr.LodMask, j, k, WarFogMgr.MaskColor, false);
                        WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, j, k, WarFogMgr.MaskClear, false);
                    }
                }
            }
            WarFogMgr.LodMask.Apply();
            WarFogMgr.LodMaskAlpha.Apply();
            WarFogMgr.HighLevelFog.GetComponent<MeshRenderer>().material.SetTexture("_MaskTex", WarFogMgr.LodMask);
            WarFogMgr.HighLevelFog.GetComponent<MeshRenderer>().material.SetTexture("_MaskTexAlpha", WarFogMgr.LodMaskAlpha);
            WarFogMgr.MountainLevelFog.GetComponent<MeshRenderer>().material.SetTexture("_MaskTex", WarFogMgr.LodMask);
        }

        private static void AddStack(int x, int y)
        {
            if (x >= WarFogMgr.MapSize || x < 0 || y >= WarFogMgr.MapSize || y < 0)
            {
                return;
            }
            WarFogMgr.LoopStack.Add(WarFogMgr.Tile2Id(x, y));
        }

        public static void BuildConnections(int x, int y, bool clear = true)
        {
            if (WarFogMgr.connectionData == null)
            {
                WarFogMgr.connectionData = new short[WarFogMgr.MapSize * WarFogMgr.MapSize];
                WarFogMgr.LoopStack = new List<int>();
            }
            if (clear)
            {
                Array.Clear(WarFogMgr.connectionData, 0, WarFogMgr.connectionData.Length);
            }
            if (WarFogMgr.LoopStack.Count > 0)
            {
                Debug.LogError("LoopStack must empty!");
            }
            WarFogMgr.AddStack(x, y);
            while (WarFogMgr.LoopStack.Count > 0)
            {
                int index = WarFogMgr.LoopStack.Count - 1;
                int id = WarFogMgr.LoopStack[index];
                int num;
                int num2;
                WarFogMgr.Id2Tile(id, out num, out num2);
                WarFogMgr.LoopStack.RemoveAt(index);
                if (WarFogMgr.FindConnection(num, num2))
                {
                    WarFogMgr.AddStack(num - 1, num2);
                    WarFogMgr.AddStack(num + 1, num2);
                    WarFogMgr.AddStack(num, num2 - 1);
                    WarFogMgr.AddStack(num, num2 + 1);
                }
            }
        }

        private static bool FindConnection(int x, int y)
        {
            if (x >= WarFogMgr.MapSize || x < 0 || y >= WarFogMgr.MapSize || y < 0)
            {
                return false;
            }
            if (WarFogMgr.connectionData[WarFogMgr.Tile2Id(x, y)] != WarFogMgr.NotConnection)
            {
                return false;
            }
            if (WarFogMgr.HasFogAt(x, y, true))
            {
                WarFogMgr.connectionData[WarFogMgr.Tile2Id(x, y)] = WarFogMgr.ConnectionClose;
                return false;
            }
            WarFogMgr.connectionData[WarFogMgr.Tile2Id(x, y)] = WarFogMgr.ConnectionOpen;
            return true;
        }

        public static short GetConnection(int x, int y)
        {
            if (x >= WarFogMgr.MapSize || x < 0 || y >= WarFogMgr.MapSize || y < 0)
            {
                return 0;
            }
            return WarFogMgr.connectionData[WarFogMgr.Tile2Id(x, y)];
        }

        public static int CanExploreTile(int x, int y, bool IsJumpHasFog = false)
        {
            if (!IsJumpHasFog && !WarFogMgr.HasFogAt(x, y, true))
            {
                return (int)WarFogMgr.ConnectionOpen;
            }
            if (WarFogMgr.connectionData == null)
            {
                return (int)WarFogMgr.ConnectionOpen;
            }
            
            int num = Mathf.FloorToInt((float)(x / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            int num2 = Mathf.FloorToInt((float)(y / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            for (int i = num; i < num + WarFogMgr.GROUP_SIZE; i++)
            {
                for (int j = num2; j < num2 + WarFogMgr.GROUP_SIZE; j++)
                {
                    if (WarFogMgr.GetConnection(i, j) == WarFogMgr.ConnectionClose)
                    {
                        int id = WarFogMgr.Tile2Id(i, j);
                        return (int)WarFogMgr.ConnectionClose;
                    }
                }
            }
            return (int)WarFogMgr.NotConnection;
        }

        private static void Id2Tile(int id, out int x, out int y)
        {
            y = Mathf.FloorToInt((float)(id / WarFogMgr.MapSize));
            x = id % WarFogMgr.MapSize;
        }

        public static int Tile2Id(int x, int y)
        {
            return y * WarFogMgr.MapSize + x;
        }

        public static int GetGroupIdByTile(int x, int y)
        {
            int tileX = Mathf.FloorToInt(x / WarFogMgr.GROUP_SIZE) * WarFogMgr.GROUP_SIZE;
            int tileY = Mathf.FloorToInt(y / WarFogMgr.GROUP_SIZE) * WarFogMgr.GROUP_SIZE;
            return WarFogMgr.Tile2Id(tileX, tileY);
        }

        private static void Offset(ref int x, ref int y)
        {
            if (x >= WarFogMgr.MapSize || y >= WarFogMgr.MapSize)
            {
                x = 0;
                y = 0;
                return;
            }
            int num = WarFogMgr.Tile2Id(x, y);
            x = Mathf.FloorToInt((float)(num / 8));
            y = num % 8;
        }

        public static bool HasFogAt(int x, int y, bool withTemp = true)
        {
            int key = WarFogMgr.Tile2Id(x, y);
            if (withTemp && WarFogMgr.TempOpenList.ContainsKey(key))
            {
                return false;
            }
            if (x >= WarFogMgr.MapSize || x < 0 || y < 0 || y >= WarFogMgr.MapSize)
            {
                return false;
            }
            WarFogMgr.Offset(ref x, ref y);
            return ((int)WarFogMgr.FogUnlockData[x] & 1 << y) == 0;
        }

        public static bool HasGroupOpen(int x, int y)
        {
            int num = Mathf.FloorToInt((float)(x / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            int num2 = Mathf.FloorToInt((float)(y / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            for (int i = num; i < num + WarFogMgr.GROUP_SIZE; i++)
            {
                for (int j = num2; j < num2 + WarFogMgr.GROUP_SIZE; j++)
                {
                    if (WarFogMgr.HasFogAt(i, j, false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool HasDisabledAt(int x, int y)
        {
            WarFogMgr.Offset(ref x, ref y);
            return ((int)WarFogMgr.FogDisabledData[x] & 1 << y) > 0;
        }

        public static void OpenFog(int x, int y)
        {
            int x2 = x;
            int y2 = y;
            WarFogMgr.Offset(ref x, ref y);
            byte b = (byte)(1 << y);
            WarFogMgr.FogUnlockData[x] = (byte)(WarFogMgr.FogUnlockData[x] | b);
            WarFogMgr.SetPixel(WarFogMgr.LodMask, x2, y2, WarFogMgr.MaskColor, true);
            WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, x2, y2, WarFogMgr.MaskClear, true);
            WarFogMgr.FogNumber--;
            if (WarFogMgr.connectionData != null)
            {
                if ((int)WarFogMgr.NotConnection != CanExploreTile(x2, y2, true)) //当迷雾块属于已开启或可探索时 才能进行BuildConnections
                {
                    WarFogMgr.connectionData[WarFogMgr.Tile2Id(x2, y2)] = WarFogMgr.NotConnection;
                    WarFogMgr.BuildConnections(x2, y2, false);
                }
            }
        }

        /// <summary>
        /// 关闭一大块雾
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void CloseGroupFog(int x, int y)
        {
            int num = Mathf.FloorToInt((float)(x / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            int num2 = Mathf.FloorToInt((float)(y / WarFogMgr.GROUP_SIZE)) * WarFogMgr.GROUP_SIZE;
            for (int i = num; i < num + WarFogMgr.GROUP_SIZE; i++)
            {
                for (int j = num2; j < num2 + WarFogMgr.GROUP_SIZE; j++)
                {
                    int fixedX = i;
                    int fixedY = j;
                    int x2 = i;
                    int y2 = j;
                    
                    Offset(ref fixedX, ref fixedY);
                    byte b = (byte)(1 << y);
                    b = (byte)~b;
                    WarFogMgr.FogUnlockData[x] = (byte)(WarFogMgr.FogUnlockData[x] & b);
                    SetPixel(LodMask, x2, y2, MaskColor, true);
                    SetPixel(LodMaskAlpha, x2, y2, MaskHas, true);
                    
                    FogNumber++;
                    if (connectionData != null)
                    {
                        connectionData[Tile2Id(x2, y2)] = NotConnection;
                    }
                }
            }
        }

        
        /// <summary>
        /// 关闭一小块雾
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public static void CloseFog(int x, int y)
        {
            int x2 = x;
            int y2 = y;
            WarFogMgr.Offset(ref x, ref y);
            byte b = (byte)(1 << y);
            b = (byte)~b;
            WarFogMgr.FogUnlockData[x] = (byte)(WarFogMgr.FogUnlockData[x] & b);
            WarFogMgr.SetPixel(WarFogMgr.LodMask, x2, y2, WarFogMgr.MaskColor, true);
            WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, x2, y2, WarFogMgr.MaskHas, true);
            WarFogMgr.FogNumber++;
            if (WarFogMgr.connectionData != null)
            {
                WarFogMgr.connectionData[WarFogMgr.Tile2Id(x2, y2)] = WarFogMgr.NotConnection;
            }
        }

        public static void AddFog(int x, int y)
        {
            int x2 = x;
            int y2 = y;
            WarFogMgr.Offset(ref x, ref y);
            byte b = (byte)(1 << y);
            b = (byte)~b;
            WarFogMgr.FogUnlockData[x] = (byte)(WarFogMgr.FogUnlockData[x] & b);
            WarFogMgr.SetPixel(WarFogMgr.LodMask, x2, y2, WarFogMgr.MaskColor, true);
            WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, x2, y2, WarFogMgr.MaskHas, true);
        }

        private static void SetPixel(Texture2D tex, int x, int y, Color color, bool apply = true)
        {
            if (WarFogMgr.IsAllFogClear())
            {
                return;
            }
            x *= WarFogMgr.ScaleSize;
            y *= WarFogMgr.ScaleSize;
            for (int i = 0; i < WarFogMgr.ScaleSize; i++)
            {
                for (int j = 0; j < WarFogMgr.ScaleSize; j++)
                {
                    tex.SetPixel(x + i, y + j, color);
                }
            }
            WarFogMgr.IsApplyMask = apply;
        }

        public static bool AddTempOpenFog(int x, int y)
        {
            if (!WarFogMgr.HasFogAt(x, y, true))
            {
                return false;
            }
            int num = WarFogMgr.Tile2Id(x, y);
            if (WarFogMgr.TempOpenList.ContainsKey(num))
            {
                return false;
            }
            WarFogMgr.TempOpenList.Add(num, num);
            WarFogMgr.SetPixel(WarFogMgr.LodMask, x, y, WarFogMgr.MaskColor, true);
            WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, x, y, WarFogMgr.MaskClear, true);
            return true;
        }

        public static void RemoveTempOpenFog(int x, int y)
        {
            int key = WarFogMgr.Tile2Id(x, y);
            if (!WarFogMgr.TempOpenList.ContainsKey(key))
            {
                return;
            }
            WarFogMgr.TempOpenList.Remove(key);
            if (WarFogMgr.HasFogAt(x, y, true))
            {
                WarFogMgr.SetPixel(WarFogMgr.LodMask, x, y, WarFogMgr.MaskColor, true);
                WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, x, y, WarFogMgr.MaskHas, true);
            }
        }

        public static bool IsAllFogOpen()
        {
            return WarFogMgr.FogNumber <= 0;
        }

        public static bool IsAllFogClear()
        {
            return WarFogMgr.AllFogClear;
        }

        public static Vector2 FindFogClosestAt(int x, int y, Dictionary<int, bool> ignoreGroupDic = null)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, WarFogMgr.FadeGroup> current in WarFogMgr.Groups)
            {
                int tileX = current.Value.tileX;
                int tileY = current.Value.tileY;
                for (int i = tileX; i < tileX + WarFogMgr.GROUP_SIZE; i++)
                {
                    for (int j = tileY; j < tileY + WarFogMgr.GROUP_SIZE; j++)
                    {
                        if (WarFogMgr.AddTempOpenFog(i, j))
                        {
                            num = i;
                            num2 = j;
                            int item = WarFogMgr.Tile2Id(i, j);
                            list.Add(item);
                        }
                    }
                }
            }
            int num5 = 999999999;
            for (int k = 0; k < WarFogMgr.MapSize; k++)
            {
                for (int l = 0; l < WarFogMgr.MapSize; l++)
                {
                    if (WarFogMgr.HasFogAt(k, l, true) && (ignoreGroupDic == null || !ignoreGroupDic.ContainsKey(WarFogMgr.GetGroupIdByTile(k, l)) ))
                    {
                        num = k;
                        num2 = l;
                        int num6 = k - x;
                        int num7 = l - y;
                        int num8 = num6 * num6 + num7 * num7;
                        if (num5 > num8)
                        {
                            num5 = num8;
                            num3 = k;
                            num4 = l;
                        }
                    }
                }
            }
            foreach (int current2 in list)
            {
                int x2;
                int y2;
                WarFogMgr.Id2Tile(current2, out x2, out y2);
                WarFogMgr.RemoveTempOpenFog(x2, y2);
            }
            if (num3 == 0 && num4 == 0)
            {
                num3 = num;
                num4 = num2;
            }
            return new Vector2((float)num3, (float)num4);
        }

        private static int FindGroup(int row, int col, int groupSize)
        {
            int num = row * groupSize;
            int num2 = row * groupSize + groupSize - 1;
            int num3 = col * groupSize;
            int num4 = col * groupSize + groupSize - 1;
            int num5 = 0;
            for (col = num3; col < num4; col++)
            {
                for (row = num; row < num2; row++)
                {
                    if (WarFogMgr.HasFogAt(row, col, false))
                    {
                        num5++;
                    }
                }
            }
            return num5;
        }

        public static Vector2 FindGroupForUseItem(int groupSize, float ratio, int x, int y)
        {
            int openNumber = Mathf.CeilToInt((float)(groupSize * groupSize) * ratio);
            int fogSize = Mathf.CeilToInt((float)(WarFogMgr.MapSize / groupSize));
            int fogCenterX = Mathf.CeilToInt((float)(x / groupSize));
            int fogCenterY = Mathf.CeilToInt((float)(y / groupSize));
            int openSize = 1;
            int fogX = -1;
            int fogY = -1;
            int fogX2 = -1;
            int fogY2 = -1;
            if (WarFogMgr.FindGroup(fogCenterX, fogCenterY, groupSize) > openNumber)
            {
                fogX = fogCenterX;
                fogY = fogCenterY;
            }
            else
            {
                int max = fogSize - 1;
                while (fogCenterX - openSize >= 0 || fogCenterX + openSize < fogSize || fogCenterY - openSize >= 0 || fogCenterY + openSize < fogSize)
                {
                    int fogTop = Mathf.Clamp(fogCenterY + openSize, 0, max);
                    int fogLeft = Mathf.Clamp(fogCenterX - openSize, 0, max);
                    int fogRight = Mathf.Clamp(fogCenterX + openSize, 0, max);
                    int j;
                    for (j = fogLeft; j <= fogRight; j++)
                    {
                        int count = WarFogMgr.FindGroup(j, fogTop, groupSize);
                        if (count > openNumber)
                        {
                            fogX = j;
                            fogY = fogTop;
                            break;
                        }
                        if (count > 0 && fogX2 == -1)
                        {
                            fogX2 = j;
                            fogY2 = fogTop;
                        }
                    }
                    if (fogX != -1)
                    {
                        break;
                    }
                    int fogBottom = Mathf.Clamp(fogCenterY - openSize, 0, max);
                    for (j = fogLeft; j <= fogRight; j++)
                    {
                        int count = WarFogMgr.FindGroup(j, fogBottom, groupSize);
                        if (count > openNumber)
                        {
                            fogX = j;
                            fogY = fogBottom;
                            break;
                        }
                        if (count > 0 && fogX2 == -1)
                        {
                            fogX2 = j;
                            fogY2 = fogBottom;
                        }
                    }
                    if (fogX != -1)
                    {
                        break;
                    }
                    j = Mathf.Clamp(fogCenterX - openSize, 0, max);
                    int num14 = Mathf.Clamp(fogCenterY - (openSize - 1), 0, max);
                    int num15 = Mathf.Clamp(fogCenterY + (openSize - 1), 0, max);
                    for (fogBottom = num14; fogBottom <= num15; fogBottom++)
                    {
                        int count = WarFogMgr.FindGroup(j, fogBottom, groupSize);
                        if (count > openNumber)
                        {
                            fogX = j;
                            fogY = fogBottom;
                            break;
                        }
                        if (count > 0 && fogX2 == -1)
                        {
                            fogX2 = j;
                            fogY2 = fogBottom;
                        }
                    }
                    if (fogX != -1)
                    {
                        break;
                    }
                    j = Mathf.Clamp(fogCenterX + openSize, 0, max);
                    for (fogTop = num14; fogTop <= num15; fogTop++)
                    {
                        int num17 = WarFogMgr.FindGroup(j, fogTop, groupSize);
                        if (num17 > openNumber)
                        {
                            fogX = j;
                            fogY = fogTop;
                            break;
                        }
                        if (num17 > 0 && fogX2 == -1)
                        {
                            fogX2 = j;
                            fogY2 = fogTop;
                        }
                    }
                    if (fogX != -1)
                    {
                        break;
                    }
                    openSize++;
                }
            }
            if (fogX != -1)
            {
                return new Vector2((float)(fogX * groupSize), (float)(fogY * groupSize));
            }
            return new Vector2((float)(fogX2 * groupSize), (float)(fogY2 * groupSize));
        }

        public static bool IsAllOpenedArround(int x, int y, int size)
        {
            int num = x - size / 2;
            int num2 = x + size / 2;
            int num3 = y - size / 2;
            int num4 = y + size / 2;
            for (int i = num; i < num2; i++)
            {
                for (int j = num3; j < num4; j++)
                {
                    if (WarFogMgr.HasFogAt(i, j, false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void ChangeLevel(int level)
        {
            WarFogMgr.LodLevel = level;
        }

        private static bool IsHighLevel()
        {
            return WarFogMgr.LodLevel > 1;
        }

        public static bool CreateFadeGroup(int id, int type, int groupSize = 5)
        {
            if (WarFogMgr.Groups.ContainsKey(id))
            {
                if (type == WarFogMgr.FADE_TYPE_EXPLORE && WarFogMgr.Groups[id].fadeType != type)
                {
                    WarFogMgr.Groups[id].fadeType = type;
                    WarFogMgr.RemoveFadeGroupById(id);
                    WarFogMgr.CreateFadeGroup(id, type, groupSize);
                }
                return true;
            }
            WarFogMgr.Groups[id] = new WarFogMgr.FadeGroup(id, type);
            Vector2 vector = WarFogMgr.FadeId2Tile(id);
            WarFogMgr.Groups[id].tileX = (int)vector.x;
            WarFogMgr.Groups[id].tileY = (int)vector.y;
            WarFogMgr.Groups[id].groupSize = groupSize;
            return true;
        }

        public static int FadeTile2Id(int tx, int ty)
        {
            return ty * WarFogMgr.ID_FORMATER + tx;
        }

        public static Vector2 FadeId2Tile(int id)
        {
            return new Vector2((float)(id % WarFogMgr.ID_FORMATER), (float)Mathf.FloorToInt((float)(id / WarFogMgr.ID_FORMATER)));
        }

        public static List<int> FadeGroupRemoveList = new List<int>();
        public static void RemoveFadeGroupByType(int type)
        {
            FadeGroupRemoveList.Clear();
            foreach (KeyValuePair<int, WarFogMgr.FadeGroup> current in WarFogMgr.Groups)
            {
                if (current.Value.fadeType == type)
                {
                    FadeGroupRemoveList.Add(current.Key);
                }
            }
            for (int i = 0; i < FadeGroupRemoveList.Count; i++)
            {
                WarFogMgr.RemoveFadeGroupById(FadeGroupRemoveList[i]);
            }
        }

        public static void RemoveFadeGroupById(int id)
        {
            if (WarFogMgr.IsAllFogClear())
            {
                return;
            }
            if (!WarFogMgr.Groups.ContainsKey(id))
            {
                return;
            }
            for (int i = WarFogMgr.Groups[id].tileX; i < WarFogMgr.Groups[id].tileX + WarFogMgr.Groups[id].groupSize; i++)
            {
                for (int j = WarFogMgr.Groups[id].tileY; j < WarFogMgr.Groups[id].tileY + WarFogMgr.Groups[id].groupSize; j++)
                {
                    WarFogMgr.SetPixel(WarFogMgr.LodMask, i, j, WarFogMgr.MaskColor, false);
                    WarFogMgr.SetPixel(WarFogMgr.LodMaskAlpha, i, j, (!WarFogMgr.HasFogAt(i, j, true)) ? WarFogMgr.MaskClear : WarFogMgr.MaskHas, false);
                    WarFogMgr.SetFogTileColor(i, j, Color.white, 1f);
                }
            }
            if (WarFogMgr.IsHighLevel())
            {
                WarFogMgr.LodMask.Apply();
                WarFogMgr.LodMaskAlpha.Apply();
            }
            WarFogMgr.Groups.Remove(id);
        }

        private static void SetFogTileColor(int x, int y, Color color, float fadeTime)
        {
            string key = x + "_" + y;
            if (!WarFogMgr.FogTileMap.ContainsKey(key))
            {
                return;
            }
            color *= fadeTime;
            WarFogMgr.props.SetColor("_CloudColor", color);
            WarFogMgr.FogTileMap[key].render.SetPropertyBlock(WarFogMgr.props);
        }

        public static void ResetFadeObject(MeshRenderer render)
        {
            WarFogMgr.props.SetColor("_CloudColor", Color.white);
            render.SetPropertyBlock(WarFogMgr.props);
        }

        private void Update()
        {
            try
            {
                bool flag = false;
                foreach (KeyValuePair<int, WarFogMgr.FadeGroup> current in WarFogMgr.Groups)
                {
                    current.Value.Update();
                    flag = true;
                }
                if (flag && !WarFogMgr.IsHighLevel())
                {
                    WarFogMgr.FadeGroup.fadeTimeLow += WarFogMgr.FadeGroup.fadeRateLow;
                    if (WarFogMgr.FadeGroup.fadeTimeLow >= 1.2f || WarFogMgr.FadeGroup.fadeTimeLow <= 1f)
                    {
                        WarFogMgr.FadeGroup.fadeRateLow = -WarFogMgr.FadeGroup.fadeRateLow;
                    }
                }

                for(int i = 0; i < FadeTileFog.Count; i++)
                {
                    var fadeTile = FadeTileFog[i];
                    fadeTile.Update();
                    if(fadeTile.WilllDelete())
                    {
                        FadeTileFog.RemoveAt(i);
                        i--;
                    }
                }

                if (WarFogMgr.IsApplyMask && WarFogMgr.LodMask != null && WarFogMgr.LodMaskAlpha != null && !WarFogMgr.IsAllFogClear())
                {
                    WarFogMgr.IsApplyMask = false;
                    WarFogMgr.LodMask.Apply();
                    WarFogMgr.LodMaskAlpha.Apply();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void CreateFogAt(int tileX, int tileY, Action<GameObject> action)
        {
            int num = WarFogMgr.FogTileBuffer.Count - 1;
            WarFogMgr.FogTileCache fogTileCache;
            if (num >= 0)
            {
                fogTileCache = WarFogMgr.FogTileBuffer[num];
                WarFogMgr.FogTileBuffer.RemoveAt(num);
                WarFogMgr.FogTileMap[tileX + "_" + tileY] = fogTileCache;
                WarFogMgr.ResetFadeObject(fogTileCache.render);
                fogTileCache.go.SetActive(true);
                fogTileCache.go.transform.parent = WarFogMgr.LowLevelFogTrans;
                action?.Invoke(fogTileCache.go);
            }
            else
            {
                CoreUtils.assetService.Instantiate("fog_tile", (GameObject obj) =>
                {
                    fogTileCache = new WarFogMgr.FogTileCache();
                    fogTileCache.go = obj;
                    fogTileCache.render = fogTileCache.go.GetComponent<MeshRenderer>();
                    WarFogMgr.FogTileMap[tileX + "_" + tileY] = fogTileCache;
                    WarFogMgr.ResetFadeObject(fogTileCache.render);
                    fogTileCache.go.SetActive(true);
                    fogTileCache.go.transform.parent = WarFogMgr.LowLevelFogTrans;
                    action?.Invoke(fogTileCache.go);
                });
            }
        }

        public static void ReleaseFogAt(int tileX, int tileY)
        {
            string key = tileX + "_" + tileY;
            WarFogMgr.FogTileMap[key].go.SetActive(false);
            Transform transform = WarFogMgr.FogTileMap[key].go.transform.Find("shadow");
            if (transform != null)
            {
                transform.gameObject.SetActive(false);
            }
            WarFogMgr.FogTileBuffer.Add(WarFogMgr.FogTileMap[key]);
            WarFogMgr.FogTileMap.Remove(key);
        }
        public static void CrateFadeFog(Vector2Int [] tiles)
        {
            FadeTileFog.Add(new FadeTile(tiles, 1.0f));
        }

        public static void ClearFogMap()
        {
            foreach (KeyValuePair<string, WarFogMgr.FogTileCache> current in WarFogMgr.FogTileMap)
            {
                current.Value.go.SetActive(false);
                WarFogMgr.FogTileBuffer.Add(current.Value);
            }
            WarFogMgr.FogTileMap.Clear();
        }

        public static void ClearAllFogTileCache()
        {
            foreach (KeyValuePair<string, WarFogMgr.FogTileCache> current in WarFogMgr.FogTileMap)
            {
                CoreUtils.assetService.Destroy(current.Value.go);
            }
            foreach (WarFogMgr.FogTileCache current2 in WarFogMgr.FogTileBuffer)
            {
                CoreUtils.assetService.Destroy(current2.go);
            }
            WarFogMgr.FogTileMap.Clear();
            WarFogMgr.FogTileBuffer.Clear();
        }

        private void OnDestroy()
        {
            WarFogMgr.ClearAllFogTileCache();
        }
    }
}