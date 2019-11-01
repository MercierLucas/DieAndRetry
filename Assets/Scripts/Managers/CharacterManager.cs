using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{

    public Hero[] heroes;
    public Spells[] spells;

    // Start is called before the first frame update
    void Start()
    {

        TextAsset heroAsset=Resources.Load("data/player") as TextAsset;
        TextAsset spellAsset=Resources.Load("data/spells") as TextAsset;

        

        heroes = JsonHelper.FromJson<Hero>(heroAsset.text);
        spells = JsonHelper.FromJson<Spells>(spellAsset.text);

        Debug.Log(spells);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
