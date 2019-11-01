using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{

    public int openingDirection; 
    // 1 bottom
    // 2 top
    // 3 left
    // 4 right

    private int rand;
    private RoomTemplate templates;

    private bool spawned = false;

    void Start(){
        templates = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplate>();
        //Invoke("Spawn",0.1f);
    }


    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("spawnPoint")){
            Destroy(gameObject);
        }
    }
    void Spawn(){
        if(spawned == false){
            switch(openingDirection){
                case 1:
                    // need to spawn BOTTOM door

                    rand = Random.Range(0,templates.bottomRooms.Length);
                    Instantiate(templates.bottomRooms[rand],transform.position,Quaternion.identity);
                    break;

                case 2:
                    // need to spawn TOP door

                    rand = Random.Range(0,templates.topRooms.Length);
                    Instantiate(templates.topRooms[rand],transform.position,Quaternion.identity);
                    
                    break;

                case 3:
                    // need to spawn LEFT door
                    rand = Random.Range(0,templates.leftRooms.Length);
                    Instantiate(templates.leftRooms[rand],transform.position,Quaternion.identity);
                    
                    break;

                case 4:
                    // need to spawn RIGHT door

                    rand = Random.Range(0,templates.rightRooms.Length);
                    Instantiate(templates.rightRooms[rand],transform.position,Quaternion.identity);
                    break;

            }
            spawned=true;
        }
    }
}
