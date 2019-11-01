using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayRoom : MonoBehaviour
{

    public int[][] room;

    public int sizeX;
    public int sizeY;

    public int currentLevel;
    public GameObject[] rooms;

    public GameObject tile;

    public GameObject teamSprite;
    public GameObject endingPoint;
    // Start is called before the first frame update
    void Start()
    {
        sizeX=10;
        sizeY=9;
        int[] randDirection=new int[]{20,60,20};

        //room=new int[sizeY,sizeX];

        room= new int[9][];

        room[0]= new int[]{0,0,0,0,0,0,0,0,0,0};
        room[1]= new int[]{0,0,0,0,0,0,0,0,0,0};
        room[2]= new int[]{0,0,0,1,1,1,1,0,0,0};
        room[3]= new int[]{0,0,0,1,0,0,1,1,1,0};
        room[4]= new int[]{1,1,1,1,1,1,1,0,1,0};
        room[5]= new int[]{0,0,0,0,0,0,0,0,1,1};
        room[6]= new int[]{0,0,0,0,0,0,0,0,0,0};
        room[7]= new int[]{0,0,0,0,0,0,0,0,0,0};
        room[8]= new int[]{0,0,0,0,0,0,0,0,0,0};

        currentLevel=0;

/*         for (int y = 0; y < sizeY; y++)
        {
            string temp="";
            for (int x = 0; x < sizeX; x++)
            {
                int rand = Random.Range(0,100);

                if(rand< randDirection[0]){
                    room[x,y-1] = 1;
                }
                else if(rand>=randDirection[0] && rand < 100-randDirection[2]){
                    room[x,y] = 1;
                }
                else if(rand>=100-randDirection[2]){
                    room[x,y+1] = 1;;
                }
                temp+=" "+room[x,y];

            }
            Debug.Log(temp);
        }  */

        rooms=new GameObject[18];
        //spawnMap();
        //RandomPath(20,0,0,randDirection);
        //rooms[0].GetComponent<Room>().wasDiscovered = true;
        teamSprite.transform.position=GameObject.FindGameObjectWithTag("spawnPoint").transform.position;


    }

/*      void spawnMap(){
        int i=0;
        for (int y = 0; y < sizeY; y++)
        {
            string temp="";
            for (int x = 0; x < sizeX; x++)
            {
                if(room[y][x] == 1){
                    GameObject go=Instantiate(tile,new Vector3(x,sizeY-y,0),Quaternion.identity);
                    go.GetComponent<Room>().roomId = i;
                    go.name = ""+i;
                    rooms[i]=go;
                    i++;
                }
                    

                temp +=room[y][x]+" ";
            }
            Debug.Log(temp);
        }
    }  */

    void Update(){
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                if(hit.transform.tag == "Rooms" || hit.transform.tag =="endPoint"){
                    if(hit.collider.gameObject.GetComponent<Room>().roomLevel == currentLevel+1){

                        hit.collider.transform.Find("bg").GetComponent<SpriteRenderer>().color = Color.white;
                        teamSprite.transform.position=hit.collider.transform.position;
                        Debug.Log("ok");
                        currentLevel++;
                        if(hit.transform.tag == "endPoint"){
                            Debug.Log("Finished");
                        }
                    }
                }
/*                 Debug.Log(hit.collider.gameObject.GetComponent<Room>().roomId);
                int roomId = hit.collider.gameObject.GetComponent<Room>().roomId;
                Debug.Log("Current room: "+rooms[roomId].GetComponent<Room>().wasDiscovered+" | Previous one: "+rooms[roomId-1].GetComponent<Room>().wasDiscovered);
                if(rooms[roomId-1].GetComponent<Room>().wasDiscovered == true){
                    rooms[roomId].GetComponent<Room>().wasDiscovered = true;
                    rooms[roomId].transform.Find("bg").GetComponent<SpriteRenderer>().color = Color.blue;
                    Debug.Log("Moved forward");
                } */
            }
        }
    }

    void RandomPath(int pathLength,int startX,int startY,int[] randDirection){
        //int pathLength= 100;
        rooms = new GameObject[pathLength];
        rooms[0] = Instantiate(tile,new Vector3(startX,startY,0),Quaternion.identity);
        rooms[0].transform.Find("bg").GetComponent<SpriteRenderer>().color = Color.red;
        int[] probas=new int[]{0,0,0};

        for (int i = 1; i < pathLength; i++)
        {
            // if(i == 50) RandomPath(pathLength-10,(int)gameObjects[i-1].transform.position.x,(int)gameObjects[i-1].transform.position.y,new int[]{60,20,20});
            // 60% forward / 20% up / 20% down
            int rand = Random.Range(0,100);

            if(rand< randDirection[0]){
                rooms[i] = Instantiate(tile,rooms[i-1].transform.position+new Vector3(0,1,0),Quaternion.identity);
                probas[0]++;
            }
            else if(rand>=randDirection[0] && rand < 100-randDirection[2]){
                rooms[i] = Instantiate(tile,rooms[i-1].transform.position+new Vector3(1,0,0),Quaternion.identity);
                probas[1]++;
            }
            else if(rand>=100-randDirection[2]){
                rooms[i] = Instantiate(tile,rooms[i-1].transform.position+new Vector3(0,-1,0),Quaternion.identity);
                probas[2]++;
            }

            //rooms[i].GetComponent<Room>().roomId = i;

            if(i == pathLength - 1){
                Instantiate(endingPoint,rooms[i].transform.position,Quaternion.identity);
                rooms[i].transform.Find("bg").GetComponent<SpriteRenderer>().color = Color.green;
            }
        }

        Debug.Log("Haut: "+((double)probas[0]/(double)pathLength)*100+"% Droit: "+((double)probas[1]/(double)pathLength)*100+"% Bas "+((double)probas[2]/(double)pathLength)*100+"%");
    }
}
