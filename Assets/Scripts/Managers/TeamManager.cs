using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeamManager : MonoBehaviour
{
    public Character[] characterCollection;
    public Spells[] spellsCollection;

    public Character[] team;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        team=new Character[3];  // fixed size

        TextAsset heroAsset=Resources.Load("data/player") as TextAsset;
        TextAsset spellAsset=Resources.Load("data/spells") as TextAsset;

        characterCollection = JsonHelper.FromJson<Character>(heroAsset.text);
        spellsCollection = JsonHelper.FromJson<Spells>(spellAsset.text);

        setupSpells();
    }

    void setupSpells(){
        foreach (Character character in characterCollection)
        {
            character.spells=new Spells[character.spellsIds.Length];
            for (int i = 0; i < character.spellsIds.Length; i++)
            {
                character.spells[i]=spellsCollection[character.spellsIds[i]];
            }
        }
    }

}
