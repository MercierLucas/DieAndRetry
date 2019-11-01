using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class textDmg : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
       Object.Destroy(gameObject, 2.0f); 
    }

    public void setDamages(float damages,string type){
        gameObject.GetComponent<TextMeshPro>().text=""+damages;

        if(type == "dmg") gameObject.GetComponent<TextMeshPro>().color = new Color32(212, 206, 49, 255);
        
        else if(type == "heal") gameObject.GetComponent<TextMeshPro>().color = new Color32(82, 154, 81, 255);

    }

    void Update(){
        gameObject.transform.position += new Vector3(0,0.01f,0);
    }

}
