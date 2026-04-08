using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Skyunion;
using Data;
using System.IO;
using Client;

public class CreateMapRssSprite : MonoBehaviour
{
    public static int SpriteSize = 40;
    
    
    

    [MenuItem("Tools/Sprite/导出地图的资源富饶度分布图")]
    public static void OnCreateMapRss()
    {

        string[] lc = new[] {"#D6FFDC","#71EA83","#74D135","#21AE21","#2D9131","#0F651B"};
        
        
        Color[] level = new Color[6];

        for (int i = 0; i < lc.Length; i++)
        {
            Color c;//= new Color(Color.green.r * (i+1) / 6, Color.green.g * (i+1)  / 6, Color.green.b * (i+1) / 6, 0.3f);

            ColorUtility.TryParseHtmlString(lc[i], out c);
            c.a = 0.8f;
            level[i] = c;
        }
        
        TableBinary<ResourceZoneLevelDefine> m_table = null;
        if (m_table == null)
        {
            m_table = new TableBinary<ResourceZoneLevelDefine>();
        }
        var table = m_table.QueryRecords();
        if (table != null)
        {
            Texture2D texture = new Texture2D(SpriteSize, SpriteSize);
            foreach (var item in table)
            {
                int x = (item.ID-1) % SpriteSize;
                int y = (item.ID-1) / SpriteSize;
                Color color = level[item.zoneLevel-1];
                texture.SetPixel(x, y, color);
            }
            texture.Apply();

            byte[] pngShot = texture.EncodeToPNG();         
            File.WriteAllBytes(Application.dataPath+ "/BundleAssets/Map/Env/Ground/resource_minimap.png", pngShot);
        }
    }
}
