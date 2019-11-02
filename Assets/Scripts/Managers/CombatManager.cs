using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class CombatManager : MonoBehaviour
{

    public GameObject heroPrefab;
    public TeamManager teamManager;

    public Character[] teamCharacters;
    public GameObject[] allies;

    public GameObject[] enemies;

    public GameObject[] alliesSpawnPoints=new GameObject[3];
    public GameObject[] enemiesSpawnPoints=new GameObject[3];

    public GameObject[] entitiesOrder;

    public GameObject currentEntity;
    public int currentEntityId;

    public GameObject portraitPos;

    public GameObject portraitPrefab;
    public GameObject[] portraits;

    public GameObject stateText;

    public string[] States=new String []{"Select","Apply","CheckForEnd"};

    public GameObject[] spellBtns;

    public Spells selectedSpell;

    public Spells[] charSpells;

    public GameObject[] target;

    public GameObject textDamages;

    public GameObject EndCombatText;

    public GameObject endPanel;

    public bool victory;

    public int deadAllies,deadEnemies;

    public bool isFightOver;

    public Character character;
    string currentState;
    // Start is called before the first frame update
    void Awake()
    {
        teamManager = GameObject.Find("TeamManager").GetComponent<TeamManager>();
        teamCharacters = teamManager.team;
        spawnEntities();
        entitiesOrder=defineOrderTurn();
        Debug.Log("ORDER:");
        Array.ForEach(entitiesOrder,Debug.Log);
        setupPortrait();

        charSpells=new Spells[3];
        
        currentEntity=entitiesOrder[0];
        currentEntityId=0;
        currentState=States[0];
        deadAllies=0;
        deadEnemies=0;
        target=new GameObject[3];

        
        UpdatePortraits();
        UpdateActionBar();
        SelectSpell(0);
        //EndCombatText.SetActive(false);
        endPanel.SetActive(false);

        isFightOver = false;
    }

    private void nextChar(){
        currentState=States[0];     // reset state
        SelectSpell(0);             // reset spell
        target=new GameObject[3];       // reset target
        if(currentEntityId == entitiesOrder.Length){
            currentEntityId=0;
            currentEntity=entitiesOrder[currentEntityId];
        }
        else{
            currentEntityId++;
            currentEntity=entitiesOrder[currentEntityId];
        }
        UpdatePortraits();
        UpdateActionBar();
    }

    void Update(){
        if(!isFightOver){
            character=currentEntity.GetComponent<Entity>().character;
            displayState(currentState);
            if(character.isAlly){
                switch(currentState){
                    case "Select":
                        // Selecting action and target(s)
                        if (Input.GetMouseButtonDown(0)) {
                            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                            
                            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
                            if (hit.collider != null) {
                                //Debug.Log(hit.collider.gameObject.GetComponent<Entity>().charName);

                                // if dmg spell and target is hostile
                                if(selectedSpell.spellType == "dmg" && !hit.collider.gameObject.GetComponent<Entity>().character.isAlly && !hit.collider.gameObject.GetComponent<Entity>().character.isDead){
                                    if(selectedSpell.target == "single"){
                                        target[0]=hit.collider.gameObject;
                                    }else if(selectedSpell.target == "aoe"){
                                        target=GameObject.FindGameObjectsWithTag("Enemy");
                                    }
                                    //Debug.Log(target.Length);
                                    currentState="Apply";
                                }

                                if(selectedSpell.spellType == "heal" && hit.collider.gameObject.GetComponent<Entity>().character.isAlly && !hit.collider.gameObject.GetComponent<Entity>().character.isDead){
                                    if(selectedSpell.target == "single"){
                                        target[0]=hit.collider.gameObject;
                                    }else if(selectedSpell.target == "aoe"){
                                        target=GameObject.FindGameObjectsWithTag("Ally");
                                    }
                                    //Debug.Log(target.Length);
                                    currentState="Apply";
                                }


                            }
                        }

                        break;

                    case "Apply":
                        for (int i = 0; i < target.Length; i++)
                        {
                            if(selectedSpell.spellType == "dmg"){
                                if(target[i] != null) {
                                    Entity tempEntity=target[i].GetComponent<Entity>();
                                    Character tempCharacter=tempEntity.character;
                                    float damages=tempEntity.takeDamages(selectedSpell.value);
                                    Debug.Log("POS: "+target[i].gameObject.transform.position);
                                    GameObject txtDmg=Instantiate(textDamages,target[i].gameObject.transform.position,Quaternion.identity);
                                    txtDmg.GetComponent<textDmg>().setDamages(damages,"dmg");
                                    Debug.Log(currentEntity.GetComponent<Entity>().character.charName+ " dealt "+damages+" damages to "+ tempCharacter.charName);
                                    if(tempCharacter.isDead){
                                        target[i].SetActive(false);
                                        Debug.Log(tempCharacter.charName.ToUpper()+" died.");
                                        if(tempCharacter.isAlly) deadAllies++;
                                        else deadEnemies++;
                                    }
                                }
                            }else if(selectedSpell.spellType == "heal"){
                                if(target[i] != null){
                                    Entity tempEntity=target[i].GetComponent<Entity>();
                                    Character tempCharacter=tempEntity.character;
                                    float heal=selectedSpell.value;
                                    //Debug.Log("POS: "+target[i].gameObject.transform.position);
                                    GameObject txtDmg=Instantiate(textDamages,target[i].gameObject.transform.position,Quaternion.identity);
                                    txtDmg.GetComponent<textDmg>().setDamages(selectedSpell.value,"heal");
                                    Debug.Log(currentEntity.GetComponent<Entity>().character.charName+ " healed "+heal+" HP to "+ tempCharacter.charName);
                                }
                            }
                        }
                        currentState="CheckForEnd";
                        break;

                    case "CheckForEnd":
                        Debug.Log("Dead allies: "+deadAllies);
                        Debug.Log("Dead enemies: "+deadEnemies);

                        if(deadEnemies==enemies.Length){
                            EndScreen("Victory");
                            victory = true;
                            isFightOver = true;
                        }
                        if(deadAllies==allies.Length){
                            EndScreen("Defeat");
                            victory = false;
                            isFightOver = true;
                        }
                        nextChar();
                        currentState=States[0];
                        break;

                    
                }
            }else{
                Debug.Log(character.charName+ "'s turn. It is a bot. Skiping to next char.");
                nextChar();
            }
        }else{
            endPanel.SetActive(true);
            if(Input.GetKeyDown("space")){
                if(victory) SceneManager.LoadScene("Game");
                else SceneManager.LoadScene("TeamPrep");
            }
        }

    }


    private void EndScreen(string result){
        EndCombatText.GetComponent<TextMeshProUGUI>().text=result;
        EndCombatText.SetActive(true);
    }
    private void displayState(String state){
        stateText.GetComponent<Text>().text=state;
    }
    private void setupPortrait()
    {
        int textureWidth=50;
        portraits=new GameObject[entitiesOrder.Length];
        int startPos= -1 * entitiesOrder.Length/2 * textureWidth;   // center portraits
        for (int i = 0; i < entitiesOrder.Length; i++)
        {
            GameObject image=Instantiate(portraitPrefab,portraitPos.transform.position+new Vector3(startPos+i*textureWidth,0,0),Quaternion.identity);
            image.transform.SetParent(portraitPos.transform);
            string name=entitiesOrder[i].GetComponent<Entity>().character.charName.ToLower();
            Texture texture=Resources.Load("portraits/"+name) as Texture;
            //Debug.Log(image.transform.Find("icon").gameObject.GetComponent<Image>());
            image.transform.Find("icon").gameObject.GetComponent<RawImage>().texture=texture;
            portraits[i]=image;
            //Instantiate(portrait,portraitPos.transform.position+new Vector3(1*i,0,0), Quaternion.identity);
        }
    }

    private void UpdatePortraits(){
        for(int i=0;i<portraits.Length;i++){
            portraits[i].GetComponent<RawImage>().color=Color.white;
        }
        portraits[currentEntityId].GetComponent<RawImage>().color=Color.grey;
    }

    private void UpdateActionBar(){
        Debug.Log("Updating action bar");
        character=currentEntity.GetComponent<Entity>().character;
        if(character.isAlly){
            Debug.Log("SPELL LENGTH: "+character.spells.Length);
            for (int i = 0; i < character.spells.Length; i++)
            {
                charSpells[i] = character.spells[i];
                Sprite texture=Resources.Load<Sprite>("spells/"+character.spells[i].icon_name.ToLower()) as Sprite;
                Debug.Log("spell img: " +texture);
                spellBtns[i].GetComponent<Image>().sprite=texture;
            }
        }
    }


    public void SelectSpell(int id){
        Debug.Log("Selected spell n°"+id);
        for(int i=0;i<spellBtns.Length;i++){
            spellBtns[i].GetComponent<Image>().color=Color.white;
        }
        spellBtns[id].GetComponent<Image>().color=Color.blue;

        selectedSpell = charSpells[id];
        //selectedSpell= spellBtns[id].GetComponent<Spells>();
    }

    void displayState(){
        
    }
    void spawnEntities(){
        allies= new GameObject[teamCharacters.Length];
        for(int i=0;i<teamCharacters.Length;i++){
            if(teamCharacters[i] != null){
                GameObject gameObject=Instantiate(heroPrefab,alliesSpawnPoints[i].transform.position, Quaternion.identity);
                gameObject.GetComponent<Entity>().character=teamCharacters[i];
                Sprite sprite = Resources.Load<Sprite>("char_texture/"+teamCharacters[i].charName.ToLower());
                Debug.Log(Resources.Load("char_texture/"+teamCharacters[i].charName.ToLower()));
                Debug.Log("SPRITE: "+sprite);
                gameObject.transform.Find("texture").GetComponent<SpriteRenderer>().sprite = sprite;

                gameObject.GetComponent<Entity>().character.isAlly = true;
                gameObject.name = teamCharacters[i].charName;
                allies[i] = gameObject;
            }
        }
/*         for(int i=0;i<allies.Length;i++){
            Instantiate(allies[i],alliesSpawnPoints[i].transform.position, Quaternion.identity);
        } */

        for(int i=0;i<enemies.Length;i++){
            Instantiate(enemies[i],enemiesSpawnPoints[i].transform.position, Quaternion.identity);
        }
    }

    public GameObject[] defineOrderTurn(){
        // Order each entities ( allies + enemies) by DESC speed

        // Concatenating two arrays
        GameObject[] temp=new GameObject[allies.Length+enemies.Length];
        allies.CopyTo(temp,0);
        enemies.CopyTo(temp,allies.Length);

        float max=temp[0].GetComponent<Entity>().character.initiative;
        List<GameObject> order=new List<GameObject>();

        for (int i = 0; i < temp.Length; i++)
        {
            if(temp[i]!=null){
                //Debug.Log("TEMP: " + temp[i]);
                float maxSpeed=temp[i].GetComponent<Entity>().character.initiative;
                int maxId=i;
                for (int j = 0; j < temp.Length; j++)
                {
                    if(temp[j]!=null){
                        //Debug.Log("Comparing: "+temp[i]+" ("+maxSpeed+") and "+temp[j]+" ("+temp[j].GetComponent<Entity>().character.speed+")");
                        if(maxSpeed<=temp[j].GetComponent<Entity>().character.initiative){
                            maxSpeed=temp[j].GetComponent<Entity>().character.initiative;
                            //Debug.Log("New max: "+maxSpeed+" by "+temp[j]);
                            maxId=j;
                        }
                    }
                }
                order.Add(temp[maxId]);
                //Debug.Log("Max found , removing: "+temp[maxId]);
                temp[maxId]=null;
            }
        }
       // Debug.Log(getNonNullArray(temp));
        order.Add(getNonNullArray(temp)); // adding remaining entity
        return order.ToArray();
    }
    
    public GameObject getNonNullArray(GameObject[] array){

        for (int i = 0; i < array.Length; i++)
        {
            if(array[i] != null) return array[i];

        }
        return array[array.Length-1];
    }
}
