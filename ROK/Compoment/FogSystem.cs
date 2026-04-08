using Skyunion;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace ROK
{
    public class FogSystem : MonoBehaviour
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

            public int groupSize = FogSystem.GROUP_SIZE;

            private Color curColor;

            private Color diffColor;

            public FadeGroup(int id, int type)
            {
                this.groupId = id;
                this.fadeType = type;
                this.ResetFade();
                this.hightColor = 0.5f;
                this.diffColor = FogSystem.exploreSelected - Color.white;
                this.curColor = Color.white;
            }

            public void ResetFade()
            {
            }

            private void UpdateLowLevel()
            {
                bool flag = this.fadeType == FogSystem.FADE_TYPE_EXPLORE;
                for (int i = this.tileX; i < this.tileX + this.groupSize; i++)
                {
                    for (int j = this.tileY; j < this.tileY + this.groupSize; j++)
                    {
                        if (FogSystem.HasFogAt(i, j, true))
                        {
                            FogSystem.SetFogTileColor(i, j, (!flag) ? Color.white : FogSystem.exploreSelected, FogSystem.FadeGroup.fadeTimeLow);
                        }
                    }
                }
            }

            private void UpdateHighLevel()
            {
                if (FogSystem.IsAllFogClear())
                {
                    return;
                }
                int num = 0;
                if (this.fadeType == FogSystem.FADE_TYPE_EXPLORE)
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
                        if (FogSystem.HasFogAt(i, j, true))
                        {
                            FogSystem.SetPixel(FogSystem.LodMask, i, j, color, false);
                            FogSystem.SetPixel(FogSystem.LodMaskAlpha, i, j, this.curColor, false);
                        }
                    }
                }
                FogSystem.LodMask.Apply();
                FogSystem.LodMaskAlpha.Apply();
            }

            public void Update()
            {
                if (FogSystem.IsHighLevel())
                {
                    this.UpdateHighLevel();
                }
                else
                {
                    this.UpdateLowLevel();
                }
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

        private static Dictionary<int, FogSystem.FadeGroup> Groups = new Dictionary<int, FogSystem.FadeGroup>();

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

        public static Color exploreSelected = new Color(0.7372549f, 0.956862748f, 1f, 1f);

        private static Dictionary<string, FogSystem.FogTileCache> FogTileMap = new Dictionary<string, FogSystem.FogTileCache>();

        private static List<FogSystem.FogTileCache> FogTileBuffer = new List<FogSystem.FogTileCache>();

        private void Start()
        {
            FogSystem.HighLevelFog = base.transform.Find("highLevelFog").gameObject;
            FogSystem.MountainLevelFog = base.transform.Find("mountainLevelFog").gameObject;
            FogSystem.props = new MaterialPropertyBlock();

            byte[] unlock = new byte[800 * 800 * 8];
            for (int i = 0; i < unlock.Length; i++)
            {
                unlock[i] = 0;
            }
            InitFogSystem(800, unlock, null);
            OpenFog(0, 0);
            OpenFog(1, 1);
            OpenFog(2, 2);
            OpenFog(3, 3);
            OpenFog(4, 4);
            OpenFog(5, 5);
            OpenFog(6, 6);
            OpenFog(7, 7);
            OpenFog(6, 7);
        }

        private void Destroy()
        {
            FogSystem.Groups.Clear();
            FogSystem.TempOpenList.Clear();
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
            FogSystem.FogDisabledData = disabledData;
        }

        public static bool LoadDisabled(string path, int minSize = 0)
        {
            if (!File.Exists(path))
            {
                return false;
            }
            FogSystem.FogDisabledData = File.ReadAllBytes(path);
            return FogSystem.FogDisabledData != null && FogSystem.FogDisabledData.Length > minSize;
        }

        public static void InitFogSystem(int mapSize, byte[] unlockedData, Transform lowLevelTrans)
        {
            FogSystem.Groups.Clear();
            FogSystem.TempOpenList.Clear();
            FogSystem.ClearAllFogTileCache();
            if (mapSize > 0)
            {
                FogSystem.MapSize = mapSize;
            }
            else
            {
                FogSystem.MapSize = (int)Math.Sqrt((double)(unlockedData.Length * 8));
            }
            FogSystem.AllFogClear = true;
            for (int i = 0; i < unlockedData.Length; i++)
            {
                if (unlockedData[i] != 255)
                {
                    FogSystem.AllFogClear = false;
                    break;
                }
            }
            if (FogSystem.LodMask == null && !FogSystem.AllFogClear)
            {
                FogSystem.LodMask = new Texture2D(FogSystem.MapSize * FogSystem.ScaleSize, FogSystem.MapSize * FogSystem.ScaleSize, TextureFormat.Alpha8, false);
                FogSystem.LodMaskAlpha = new Texture2D(FogSystem.MapSize * FogSystem.ScaleSize, FogSystem.MapSize * FogSystem.ScaleSize, TextureFormat.Alpha8, false);
            }
            FogSystem.FogUnlockData = unlockedData;
            FogSystem.LowLevelFogTrans = lowLevelTrans;
            FogSystem.FogNumber = 0;
            if (FogSystem.AllFogClear)
            {
                return;
            }
            for (int j = 0; j < FogSystem.MapSize; j++)
            {
                for (int k = 0; k < FogSystem.MapSize; k++)
                {
                    if (FogSystem.HasFogAt(j, k, true))
                    {
                        FogSystem.FogNumber++;
                        FogSystem.SetPixel(FogSystem.LodMask, j, k, FogSystem.MaskColor, false);
                        FogSystem.SetPixel(FogSystem.LodMaskAlpha, j, k, FogSystem.MaskHas, false);
                    }
                    else
                    {
                        FogSystem.SetPixel(FogSystem.LodMask, j, k, FogSystem.MaskColor, false);
                        FogSystem.SetPixel(FogSystem.LodMaskAlpha, j, k, FogSystem.MaskClear, false);
                    }
                }
            }
            FogSystem.LodMask.Apply();
            FogSystem.LodMaskAlpha.Apply();
            FogSystem.HighLevelFog.GetComponent<MeshRenderer>().material.SetTexture("_MaskTex", FogSystem.LodMask);
            FogSystem.HighLevelFog.GetComponent<MeshRenderer>().material.SetTexture("_MaskTexAlpha", FogSystem.LodMaskAlpha);
            //FogSystem.MountainLevelFog.GetComponent<MeshRenderer>().material.SetTexture("_MaskTex", FogSystem.LodMask);
        }

        private static void AddStack(int x, int y)
        {
            if (x >= FogSystem.MapSize || x < 0 || y >= FogSystem.MapSize || y < 0)
            {
                return;
            }
            FogSystem.LoopStack.Add(FogSystem.Tile2Id(x, y));
        }

        public static void BuildConnections(int x, int y, bool clear = true)
        {
            if (FogSystem.connectionData == null)
            {
                FogSystem.connectionData = new short[FogSystem.MapSize * FogSystem.MapSize];
                FogSystem.LoopStack = new List<int>();
            }
            if (clear)
            {
                Array.Clear(FogSystem.connectionData, 0, FogSystem.connectionData.Length);
            }
            if (FogSystem.LoopStack.Count > 0)
            {
                Debug.LogError("LoopStack must empty!");
            }
            FogSystem.AddStack(x, y);
            while (FogSystem.LoopStack.Count > 0)
            {
                int index = FogSystem.LoopStack.Count - 1;
                int id = FogSystem.LoopStack[index];
                int num;
                int num2;
                FogSystem.Id2Tile(id, out num, out num2);
                FogSystem.LoopStack.RemoveAt(index);
                if (FogSystem.FindConnection(num, num2))
                {
                    FogSystem.AddStack(num - 1, num2);
                    FogSystem.AddStack(num + 1, num2);
                    FogSystem.AddStack(num, num2 - 1);
                    FogSystem.AddStack(num, num2 + 1);
                }
            }
        }

        private static bool FindConnection(int x, int y)
        {
            if (x >= FogSystem.MapSize || x < 0 || y >= FogSystem.MapSize || y < 0)
            {
                return false;
            }
            if (FogSystem.connectionData[FogSystem.Tile2Id(x, y)] != FogSystem.NotConnection)
            {
                return false;
            }
            if (FogSystem.HasFogAt(x, y, true))
            {
                FogSystem.connectionData[FogSystem.Tile2Id(x, y)] = FogSystem.ConnectionClose;
                return false;
            }
            FogSystem.connectionData[FogSystem.Tile2Id(x, y)] = FogSystem.ConnectionOpen;
            return true;
        }

        public static short GetConnection(int x, int y)
        {
            if (x >= FogSystem.MapSize || x < 0 || y >= FogSystem.MapSize || y < 0)
            {
                return 0;
            }
            return FogSystem.connectionData[FogSystem.Tile2Id(x, y)];
        }

        public static int CanExploreTile(int x, int y)
        {
            if (!FogSystem.HasFogAt(x, y, true))
            {
                return (int)FogSystem.ConnectionOpen;
            }
            if (FogSystem.connectionData == null)
            {
                return (int)FogSystem.ConnectionOpen;
            }
            int num = Mathf.FloorToInt((float)(x / FogSystem.GROUP_SIZE)) * FogSystem.GROUP_SIZE;
            int num2 = Mathf.FloorToInt((float)(y / FogSystem.GROUP_SIZE)) * FogSystem.GROUP_SIZE;
            for (int i = num; i < num + FogSystem.GROUP_SIZE; i++)
            {
                for (int j = num2; j < num2 + FogSystem.GROUP_SIZE; j++)
                {
                    if (FogSystem.GetConnection(i, j) == FogSystem.ConnectionClose)
                    {
                        return (int)FogSystem.ConnectionClose;
                    }
                }
            }
            return (int)FogSystem.NotConnection;
        }

        private static void Id2Tile(int id, out int x, out int y)
        {
            x = Mathf.FloorToInt((float)(id / FogSystem.MapSize));
            y = id % FogSystem.MapSize;
        }

        private static int Tile2Id(int x, int y)
        {
            return x * FogSystem.MapSize + y;
        }

        private static void Offset(ref int x, ref int y)
        {
            if (x >= FogSystem.MapSize || y >= FogSystem.MapSize)
            {
                x = 0;
                y = 0;
                return;
            }
            int num = FogSystem.Tile2Id(x, y);
            x = Mathf.FloorToInt((float)(num / 8));
            y = num % 8;
        }

        public static bool HasFogAt(int x, int y, bool withTemp = true)
        {
            int key = FogSystem.Tile2Id(x, y);
            if (withTemp && FogSystem.TempOpenList.ContainsKey(key))
            {
                return false;
            }
            if (x >= FogSystem.MapSize || x < 0 || y < 0 || y >= FogSystem.MapSize)
            {
                return false;
            }
            FogSystem.Offset(ref x, ref y);
            return ((int)FogSystem.FogUnlockData[x] & 1 << y) == 0;
        }

        public static bool HasGroupOpen(int x, int y)
        {
            int num = Mathf.FloorToInt((float)(x / FogSystem.GROUP_SIZE)) * FogSystem.GROUP_SIZE;
            int num2 = Mathf.FloorToInt((float)(y / FogSystem.GROUP_SIZE)) * FogSystem.GROUP_SIZE;
            for (int i = num; i < num + FogSystem.GROUP_SIZE; i++)
            {
                for (int j = num2; j < num2 + FogSystem.GROUP_SIZE; j++)
                {
                    if (FogSystem.HasFogAt(i, j, false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static bool HasDisabledAt(int x, int y)
        {
            FogSystem.Offset(ref x, ref y);
            return ((int)FogSystem.FogDisabledData[x] & 1 << y) > 0;
        }

        public static void OpenFog(int x, int y)
        {
            int x2 = x;
            int y2 = y;
            FogSystem.Offset(ref x, ref y);
            byte b = (byte)(1 << y);
            FogSystem.FogUnlockData[x] = (byte)(FogSystem.FogUnlockData[x] | b);
            FogSystem.SetPixel(FogSystem.LodMask, x2, y2, FogSystem.MaskColor, true);
            FogSystem.SetPixel(FogSystem.LodMaskAlpha, x2, y2, FogSystem.MaskClear, true);
            FogSystem.FogNumber--;
            if (FogSystem.connectionData != null)
            {
                FogSystem.connectionData[FogSystem.Tile2Id(x2, y2)] = FogSystem.NotConnection;
                FogSystem.BuildConnections(x2, y2, false);
            }
        }

        public static void AddFog(int x, int y)
        {
            int x2 = x;
            int y2 = y;
            FogSystem.Offset(ref x, ref y);
            byte b = (byte)(1 << y);
            b = (byte)~b;
            FogSystem.FogUnlockData[x] = (byte)(FogSystem.FogUnlockData[x] & b);
            FogSystem.SetPixel(FogSystem.LodMask, x2, y2, FogSystem.MaskColor, true);
            FogSystem.SetPixel(FogSystem.LodMaskAlpha, x2, y2, FogSystem.MaskHas, true);
        }

        private static void SetPixel(Texture2D tex, int x, int y, Color color, bool apply = true)
        {
            if (FogSystem.IsAllFogClear())
            {
                return;
            }
            x *= FogSystem.ScaleSize;
            y *= FogSystem.ScaleSize;
            for (int i = 0; i < FogSystem.ScaleSize; i++)
            {
                for (int j = 0; j < FogSystem.ScaleSize; j++)
                {
                    tex.SetPixel(x + i, y + j, color);
                }
            }
            FogSystem.IsApplyMask = apply;
        }

        public static bool AddTempOpenFog(int x, int y)
        {
            if (!FogSystem.HasFogAt(x, y, true))
            {
                return false;
            }
            int num = FogSystem.Tile2Id(x, y);
            if (FogSystem.TempOpenList.ContainsKey(num))
            {
                return false;
            }
            FogSystem.TempOpenList.Add(num, num);
            FogSystem.SetPixel(FogSystem.LodMask, x, y, FogSystem.MaskColor, true);
            FogSystem.SetPixel(FogSystem.LodMaskAlpha, x, y, FogSystem.MaskClear, true);
            return true;
        }

        public static void RemoveTempOpenFog(int x, int y)
        {
            int key = FogSystem.Tile2Id(x, y);
            if (!FogSystem.TempOpenList.ContainsKey(key))
            {
                return;
            }
            FogSystem.TempOpenList.Remove(key);
            if (FogSystem.HasFogAt(x, y, true))
            {
                FogSystem.SetPixel(FogSystem.LodMask, x, y, FogSystem.MaskColor, true);
                FogSystem.SetPixel(FogSystem.LodMaskAlpha, x, y, FogSystem.MaskHas, true);
            }
        }

        public static bool IsAllFogClear()
        {
            return FogSystem.AllFogClear;
        }

        public static Vector2 FindFogClosestAt(int x, int y)
        {
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int num4 = 0;
            List<int> list = new List<int>();
            foreach (KeyValuePair<int, FogSystem.FadeGroup> current in FogSystem.Groups)
            {
                int tileX = current.Value.tileX;
                int tileY = current.Value.tileY;
                for (int i = tileX; i < tileX + FogSystem.GROUP_SIZE; i++)
                {
                    for (int j = tileY; j < tileY + FogSystem.GROUP_SIZE; j++)
                    {
                        if (FogSystem.AddTempOpenFog(i, j))
                        {
                            num = i;
                            num2 = j;
                            int item = FogSystem.Tile2Id(i, j);
                            list.Add(item);
                        }
                    }
                }
            }
            int num5 = 999999999;
            for (int k = 0; k < FogSystem.MapSize; k++)
            {
                for (int l = 0; l < FogSystem.MapSize; l++)
                {
                    if (FogSystem.HasFogAt(k, l, true))
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
                FogSystem.Id2Tile(current2, out x2, out y2);
                FogSystem.RemoveTempOpenFog(x2, y2);
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
                    if (FogSystem.HasFogAt(row, col, false))
                    {
                        num5++;
                    }
                }
            }
            return num5;
        }

        public static Vector2 FindGroupForUseItem(int groupSize, float ratio, int x, int y)
        {
            int num = Mathf.CeilToInt((float)(groupSize * groupSize) * ratio);
            int num2 = Mathf.CeilToInt((float)(FogSystem.MapSize / groupSize));
            int num3 = Mathf.CeilToInt((float)(x / groupSize));
            int num4 = Mathf.CeilToInt((float)(y / groupSize));
            int num5 = 1;
            int num6 = -1;
            int num7 = -1;
            int num8 = -1;
            int num9 = -1;
            if (FogSystem.FindGroup(num3, num4, groupSize) > num)
            {
                num6 = num3;
                num7 = num4;
            }
            else
            {
                int max = num2 - 1;
                while (num3 - num5 >= 0 || num3 + num5 < num2 || num4 - num5 >= 0 || num4 + num5 < num2)
                {
                    int i = Mathf.Clamp(num4 + num5, 0, max);
                    int num10 = Mathf.Clamp(num3 - num5, 0, max);
                    int num11 = Mathf.Clamp(num3 + num5, 0, max);
                    int j;
                    for (j = num10; j <= num11; j++)
                    {
                        int num12 = FogSystem.FindGroup(j, i, groupSize);
                        if (num12 > num)
                        {
                            num6 = j;
                            num7 = i;
                            break;
                        }
                        if (num12 > 0 && num8 == -1)
                        {
                            num8 = j;
                            num9 = i;
                        }
                    }
                    if (num6 != -1)
                    {
                        break;
                    }
                    i = Mathf.Clamp(num4 - num5, 0, max);
                    for (j = num10; j <= num11; j++)
                    {
                        int num13 = FogSystem.FindGroup(j, i, groupSize);
                        if (num13 > num)
                        {
                            num6 = j;
                            num7 = i;
                            break;
                        }
                        if (num13 > 0 && num8 == -1)
                        {
                            num8 = j;
                            num9 = i;
                        }
                    }
                    if (num6 != -1)
                    {
                        break;
                    }
                    j = Mathf.Clamp(num3 - num5, 0, max);
                    int num14 = Mathf.Clamp(num4 - (num5 - 1), 0, max);
                    int num15 = Mathf.Clamp(num4 + (num5 - 1), 0, max);
                    for (i = num14; i <= num15; i++)
                    {
                        int num16 = FogSystem.FindGroup(j, i, groupSize);
                        if (num16 > num)
                        {
                            num6 = j;
                            num7 = i;
                            break;
                        }
                        if (num16 > 0 && num8 == -1)
                        {
                            num8 = j;
                            num9 = i;
                        }
                    }
                    if (num6 != -1)
                    {
                        break;
                    }
                    j = Mathf.Clamp(num3 + num5, 0, max);
                    for (i = num14; i <= num15; i++)
                    {
                        int num17 = FogSystem.FindGroup(j, i, groupSize);
                        if (num17 > num)
                        {
                            num6 = j;
                            num7 = i;
                            break;
                        }
                        if (num17 > 0 && num8 == -1)
                        {
                            num8 = j;
                            num9 = i;
                        }
                    }
                    if (num6 != -1)
                    {
                        break;
                    }
                    num5++;
                }
            }
            if (num6 != -1)
            {
                return new Vector2((float)(num6 * groupSize), (float)(num7 * groupSize));
            }
            return new Vector2((float)(num8 * groupSize), (float)(num9 * groupSize));
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
                    if (FogSystem.HasFogAt(i, j, false))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public static void ChangeLevel(int level)
        {
            FogSystem.LodLevel = level;
        }

        private static bool IsHighLevel()
        {
            return FogSystem.LodLevel > 1;
        }

        public static bool CreateFadeGroup(int id, int type, int groupSize = 5)
        {
            if (FogSystem.Groups.ContainsKey(id))
            {
                if (type == FogSystem.FADE_TYPE_EXPLORE && FogSystem.Groups[id].fadeType != type)
                {
                    FogSystem.Groups[id].fadeType = type;
                    FogSystem.RemoveFadeGroupById(id);
                    FogSystem.CreateFadeGroup(id, type, groupSize);
                }
                return true;
            }
            FogSystem.Groups[id] = new FogSystem.FadeGroup(id, type);
            Vector2 vector = FogSystem.FadeId2Tile(id);
            FogSystem.Groups[id].tileX = (int)vector.x;
            FogSystem.Groups[id].tileY = (int)vector.y;
            FogSystem.Groups[id].groupSize = groupSize;
            return true;
        }

        public static int FadeTile2Id(int tx, int ty)
        {
            return ty * FogSystem.ID_FORMATER + tx;
        }

        public static Vector2 FadeId2Tile(int id)
        {
            return new Vector2((float)(id % FogSystem.ID_FORMATER), (float)Mathf.FloorToInt((float)(id / FogSystem.ID_FORMATER)));
        }

        public static void RemoveFadeGroupByType(int type)
        {
            foreach (KeyValuePair<int, FogSystem.FadeGroup> current in FogSystem.Groups)
            {
                if (current.Value.fadeType == type)
                {
                    FogSystem.RemoveFadeGroupById(current.Key);
                    break;
                }
            }
        }

        public static void RemoveFadeGroupById(int id)
        {
            if (FogSystem.IsAllFogClear())
            {
                return;
            }
            if (!FogSystem.Groups.ContainsKey(id))
            {
                return;
            }
            for (int i = FogSystem.Groups[id].tileX; i < FogSystem.Groups[id].tileX + FogSystem.Groups[id].groupSize; i++)
            {
                for (int j = FogSystem.Groups[id].tileY; j < FogSystem.Groups[id].tileY + FogSystem.Groups[id].groupSize; j++)
                {
                    FogSystem.SetPixel(FogSystem.LodMask, i, j, FogSystem.MaskColor, false);
                    FogSystem.SetPixel(FogSystem.LodMaskAlpha, i, j, (!FogSystem.HasFogAt(i, j, true)) ? FogSystem.MaskClear : FogSystem.MaskHas, false);
                    FogSystem.SetFogTileColor(i, j, Color.white, 1f);
                }
            }
            if (FogSystem.IsHighLevel())
            {
                FogSystem.LodMask.Apply();
                FogSystem.LodMaskAlpha.Apply();
            }
            FogSystem.Groups.Remove(id);
        }

        private static void SetFogTileColor(int x, int y, Color color, float fadeTime)
        {
            string key = x + "_" + y;
            if (!FogSystem.FogTileMap.ContainsKey(key))
            {
                return;
            }
            color *= fadeTime;
            FogSystem.props.SetColor("_CloudColor", color);
            FogSystem.FogTileMap[key].render.SetPropertyBlock(FogSystem.props);
        }

        public static void ResetFadeObject(MeshRenderer render)
        {
            FogSystem.props.SetColor("_CloudColor", Color.white);
            render.SetPropertyBlock(FogSystem.props);
        }

        private void Update()
        {
            try
            {
                bool flag = false;
                foreach (KeyValuePair<int, FogSystem.FadeGroup> current in FogSystem.Groups)
                {
                    current.Value.Update();
                    flag = true;
                }
                if (flag && !FogSystem.IsHighLevel())
                {
                    FogSystem.FadeGroup.fadeTimeLow += FogSystem.FadeGroup.fadeRateLow;
                    if (FogSystem.FadeGroup.fadeTimeLow >= 1.2f || FogSystem.FadeGroup.fadeTimeLow <= 1f)
                    {
                        FogSystem.FadeGroup.fadeRateLow = -FogSystem.FadeGroup.fadeRateLow;
                    }
                }
                if (FogSystem.IsApplyMask && FogSystem.LodMask != null && FogSystem.LodMaskAlpha != null && !FogSystem.IsAllFogClear())
                {
                    FogSystem.IsApplyMask = false;
                    FogSystem.LodMask.Apply();
                    FogSystem.LodMaskAlpha.Apply();
                }
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }

        public static void CreateFogAt(int tileX, int tileY, Action<GameObject> action)
        {
            int num = FogSystem.FogTileBuffer.Count - 1;
            FogSystem.FogTileCache fogTileCache;
            if (num >= 0)
            {
                fogTileCache = FogSystem.FogTileBuffer[num];
                FogSystem.FogTileBuffer.RemoveAt(num);
                FogSystem.FogTileMap[tileX + "_" + tileY] = fogTileCache;
                FogSystem.ResetFadeObject(fogTileCache.render);
                fogTileCache.go.SetActive(true);
                fogTileCache.go.transform.parent = FogSystem.LowLevelFogTrans;
                action?.Invoke(fogTileCache.go);
            }
            else
            {
                CoreUtils.assetService.Instantiate("fog_tile", (GameObject obj) =>
                {
                    fogTileCache = new FogSystem.FogTileCache();
                    fogTileCache.go = obj;
                    fogTileCache.render = fogTileCache.go.GetComponent<MeshRenderer>();
                    FogSystem.FogTileMap[tileX + "_" + tileY] = fogTileCache;
                    FogSystem.ResetFadeObject(fogTileCache.render);
                    fogTileCache.go.SetActive(true);
                    fogTileCache.go.transform.parent = FogSystem.LowLevelFogTrans;
                    action?.Invoke(fogTileCache.go);
                });
            }
        }

        public static void ReleaseFogAt(int tileX, int tileY)
        {
            string key = tileX + "_" + tileY;
            FogSystem.FogTileMap[key].go.SetActive(false);
            Transform transform = FogSystem.FogTileMap[key].go.transform.Find("shadow");
            if (transform != null)
            {
                transform.gameObject.SetActive(false);
            }
            FogSystem.FogTileBuffer.Add(FogSystem.FogTileMap[key]);
            FogSystem.FogTileMap.Remove(key);
        }

        public static void ClearFogMap()
        {
            foreach (KeyValuePair<string, FogSystem.FogTileCache> current in FogSystem.FogTileMap)
            {
                current.Value.go.SetActive(false);
                FogSystem.FogTileBuffer.Add(current.Value);
            }
            FogSystem.FogTileMap.Clear();
        }

        public static void ClearAllFogTileCache()
        {
            foreach (KeyValuePair<string, FogSystem.FogTileCache> current in FogSystem.FogTileMap)
            {
                CoreUtils.assetService.Destroy(current.Value.go);
            }
            foreach (FogSystem.FogTileCache current2 in FogSystem.FogTileBuffer)
            {
                CoreUtils.assetService.Destroy(current2.go);
            }
            FogSystem.FogTileMap.Clear();
            FogSystem.FogTileBuffer.Clear();
        }

        private void OnDestroy()
        {
            FogSystem.ClearAllFogTileCache();
        }
    }
}