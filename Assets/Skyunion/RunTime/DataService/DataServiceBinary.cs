using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Skyunion
{
    // SQLite的到时候自己写个 DataService_SQLite的实现类
    public class DataServiceBinary : DataService
    {
        protected override ITable<T> CreateTable<T>(Type type)
        {
            return new TableBinary<T>(type);
        }
        public override DataMode GetDataMode()
        {
            return DataMode.Binary;
        }
    }
    public class TableBinary<T> : TableBase<T>
    {
        public TableBinary(Type type) : base(type)
        {
        }
        public TableBinary() : base(typeof(T))
        {
        }
        protected override void LoadTable()
        {
            var path = Path.Combine("Config/Bin/", $"{tableName}.bin");
            byte[] content;
#if UNITY_EDITOR
            if(!Application.isPlaying || CoreUtils.assetService == null)
            {
                content = File.ReadAllBytes(Path.Combine(Application.streamingAssetsPath, path));
            }
            else
#endif
            {
                content = CoreUtils.assetService.LoadFile(Path.Combine(Application.streamingAssetsPath, path));
            }
            CoreUtils.Decrypt(content);
            int nIndex = 0;
            int textNum = BitConverter.ToInt32(content, nIndex);
            nIndex = nIndex + 4;
            var texs = new string[textNum];
            var utf8 = Encoding.UTF8;
            for (int i = 0; i < textNum; i++)
            {
                int nSize = BitConverter.ToUInt16(content, nIndex);
                nIndex = nIndex + 2;
                texs[i] = utf8.GetString(content, nIndex, nSize);
                nIndex = nIndex + nSize;
            }
            int nRow = BitConverter.ToInt32(content, nIndex);
            nIndex = nIndex + 4;
            ushort nCol = BitConverter.ToUInt16(content, nIndex);
            nIndex = nIndex + 2;

            rows = new string[nRow][];
            records = new T[nRow];
            for (int row = 0; row < nRow; row++)
            {
                var cols = new string[nCol];
                for (int col = 0; col < nCol; col++)
                {
                    int nIdx = BitConverter.ToInt32(content, nIndex);
                    nIndex = nIndex + 4;
                    cols[col] = texs[nIdx];
                }
                try
                {
                    int id = Convert.ToInt32(cols[0]);
                    mapIdx.Add(id, row);
                }
                catch (Exception e)
                {
                    mapIdx.Add(mapIdx.Count, row);
                }
                rows[row] = cols;
            }
        }
    }
}
