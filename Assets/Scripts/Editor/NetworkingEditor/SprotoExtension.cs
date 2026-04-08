using System.Collections;
using System.Collections.Generic;
using Sproto;
using UnityEngine;
using LitJson;

public static class SprotoExtension 
{
    private static JsonWriter s_jsonWrite = null;
    public static string ToJson<T>(T obj) where T : SprotoTypeBase
    {
        if(s_jsonWrite == null)
        {
            s_jsonWrite = new JsonWriter();
            s_jsonWrite.PrettyPrint = true;
        }
        s_jsonWrite.Reset();
        JsonMapper.ToJson(obj, s_jsonWrite);
        string strJson = s_jsonWrite.ToString();
        return strJson;
    }
}
