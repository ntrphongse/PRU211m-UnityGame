using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JsonHelper
{
    public List<QuestionBase> Items { get; set; }
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string ToJson<T>(T[] array)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper);
    }

    public static string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}
