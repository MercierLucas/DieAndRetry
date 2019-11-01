using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject border;

    public bool wasDiscovered;

    public string encounterType;
    public int roomLevel;

    // Start is called before the first frame update
    void Start()
    {
        border=gameObject.transform.Find("border").gameObject;
        border.SetActive(false);
    }
    void OnMouseOver()
    {
        border.SetActive(true);
    }
    void OnMouseExit()
    {
        border.SetActive(false);
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
