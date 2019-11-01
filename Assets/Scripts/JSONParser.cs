using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class JSONParser : MonoBehaviour
{

    string path, jsonString;
    public Hero[] heroes;

    public Spells[] spells;

    // Start is called before the first frame update
    void Start()
    {
        TextAsset heroAsset=Resources.Load("data/player") as TextAsset;
        TextAsset spellAsset=Resources.Load("data/player") as TextAsset;

        heroes = JsonHelper.FromJson<Hero>(heroAsset.text);
        spells = JsonHelper.FromJson<Spells>(spellAsset.text);

        foreach(Hero hero in heroes){

            Debug.Log(hero.name);
            Debug.Log(hero.title);

        }
        //string jsonString = "{\r\n    \"Items\": [{\"Name\":\"Bob\",\"Title\":\"Lost Inkeeper\"}]}";
    }
}


public static class JsonHelper
{
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

    public static string ToJson<T>(T[] array, bool prettyPrint)
    {
        Wrapper<T> wrapper = new Wrapper<T>();
        wrapper.Items = array;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}



