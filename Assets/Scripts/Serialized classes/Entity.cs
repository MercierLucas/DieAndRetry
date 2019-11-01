using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages gameobject components of each entity (either ally or enemy)
// Stats are stored in Character
public class Entity : MonoBehaviour
{

    public GameObject HealthBar;

    public Character character;

    public void Start(){
        HealthBar = transform.Find("Healthbar").gameObject;
        //Debug.Log(character.charName+"'s healthbar: "+HealthBar);
    }

    public float takeDamages(float damages){
        float realDamages=character.takeDamages(damages);
        HealthBar.GetComponent<HealthBar>().SetSize(character.getLife()/character.getMaxLife());
        return realDamages;
    }
/*     public Entity(float life,float speed){
        this.life=life;
        this.maxLife=life;

        this.speed=speed;
    } */

}
