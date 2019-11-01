using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapManager : MonoBehaviour
{

    

    public GameObject teamSprite;
    public int currentMap;

    public int currentLevel;


    // Start is called before the first frame update
    void Start()
    {
        currentMap=0;
        currentLevel=0;

        teamSprite.transform.position=GameObject.FindGameObjectWithTag("spawnPoint").transform.position+new Vector3(0,0,-1);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                if(hit.transform.tag == "Rooms" || hit.transform.tag =="endPoint"){
                    if(hit.collider.gameObject.GetComponent<Room>().roomLevel == currentLevel+1){

                        hit.collider.transform.Find("bg").GetComponent<SpriteRenderer>().color = Color.white;
                        teamSprite.transform.position=hit.collider.transform.position+new Vector3(0,0,-1);
                        Debug.Log("ok");
                        currentLevel++;
                        if(hit.transform.tag == "endPoint"){
                            Debug.Log("Finished");
                        }

                        if(hit.collider.GetComponent<Room>().encounterType == "mob"){
                            Debug.Log("Mob encounter! Loading fight scene");
                            SceneManager.LoadScene("CombatScene");
                        }
                    }
                }
            }
        }
    }
}
