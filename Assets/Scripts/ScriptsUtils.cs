using System;
using System.IO;
using UnityEngine;

public class SampleToJson
{
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
        // return JsonConvert.SerializeObject(this, new JsonSerializerSettings{ NullValueHandling = NullValueHandling.Ignore});
    }
}