using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    private Transform bar;

    // Start is called before the first frame update
    void Start()
    {
        bar = transform.Find("bar");
    }

    public void SetSize(float newVal){
        bar.localScale= new Vector3(newVal,1f);
    }
}
