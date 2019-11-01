using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TeamPreparation : MonoBehaviour
{

    public GameObject TeamManager;
    public GameObject[] panels;
    public GameObject characterFrame;
    public GameObject charSelectionAnchor;
    public GameObject charFrameContainer;
    public GameObject teamFrameContainer;

    public Character[] heroes;
    public Spells[] spells;
    public GameObject teamPanel;

    public Character selectedCharacter;


    public GameObject spellsSubPanel;

    // Start is called before the first frame update
    void Start()
    {
        

        heroes=TeamManager.GetComponent<TeamManager>().characterCollection;
        spells=TeamManager.GetComponent<TeamManager>().spellsCollection;


        Panels(0);
        heroesList();
        selectHero(0);

        placeCharInTeam(-1);
    }

    void heroesList(){
        int startingPos=-30;
        for (int i = 0; i < heroes.Length; i++)
        {
            GameObject obj=Instantiate(characterFrame, new Vector3(0,startingPos-i*60,0),Quaternion.identity) as GameObject;
            int index=i;
            obj.GetComponent<Button>().onClick.AddListener(() => selectHero(index));
            Texture texture=Resources.Load("portraits/"+heroes[i].charName.ToLower()) as Texture;
            obj.transform.Find("icon").GetComponent<RawImage>().texture=texture;
            //Debug.Log(obj.transform.Find("subPanel").Find("charName"));
            obj.transform.Find("subPanel").Find("charName").GetComponent<TextMeshProUGUI>().text=heroes[i].charName;
            obj.transform.Find("subPanel").Find("charTitle").GetComponent<TextMeshProUGUI>().text=heroes[i].title;
            //obj.transform.localScale=new Vector3(0.65f,0.6f,1f);
            obj.transform.SetParent(charFrameContainer.transform,false);
        }
    }

    public void Panels(int panelId){
        foreach ( GameObject panel in panels)
        {
            panel.SetActive(false);
        }
        panels[panelId].SetActive(true);
    }

    
    void selectHero(int index){
        selectedCharacter=heroes[index];
        //Debug.Log(index);
        teamPanel.transform.Find("charName").GetComponent<TextMeshProUGUI>().text=heroes[index].charName;
        teamPanel.transform.Find("title").GetComponent<TextMeshProUGUI>().text=heroes[index].title;
        teamPanel.transform.Find("history").GetComponent<TextMeshProUGUI>().text=heroes[index].history;

        // stats
        teamPanel.transform.Find("Stats").Find("initiative").GetComponent<TextMeshProUGUI>().text="INI: "+heroes[index].initiative;
        teamPanel.transform.Find("Stats").Find("life").GetComponent<TextMeshProUGUI>().text="HP: "+heroes[index].life+"/"+heroes[index].maxLife;
        teamPanel.transform.Find("Stats").Find("crit").GetComponent<TextMeshProUGUI>().text="CRIT: "+heroes[index].critRate+" %";
        teamPanel.transform.Find("Stats").Find("str").GetComponent<TextMeshProUGUI>().text="STR: "+heroes[index].strength+"";
        teamPanel.transform.Find("Stats").Find("res").GetComponent<TextMeshProUGUI>().text="RES: "+heroes[index].resistance+"";
        teamPanel.transform.Find("Stats").Find("moral").GetComponent<TextMeshProUGUI>().text="MORAL: "+heroes[index].moral+"";

        // spells
        Debug.Log(heroes[index].spells.Length);
        for (int i = 0; i < heroes[index].spells.Length; i++)
        {
            Texture texture=Resources.Load("spells/"+heroes[index].spells[i].icon_name.ToLower()) as Texture;
            spellsSubPanel.transform.Find("spell_"+(i+1)).GetComponent<RawImage>().texture=texture;
        }
    }

    public void run(){
        int nullCounter=0;
        Character[] team=TeamManager.GetComponent<TeamManager>().team;
        for (int i = 0; i < team.Length; i++)
        {
            if(team[i] == null) nullCounter++;
            //if(team[i].charName == null) nullCounter++;
        }
        Debug.Log(nullCounter);
        if(nullCounter==3) panels[2].transform.Find("warning").gameObject.SetActive(true);
        else{
            SceneManager.LoadScene("Game");
        }
    }

    public void placeCharInTeam(int index){
        Character[] team=TeamManager.GetComponent<TeamManager>().team;
        for (int i = 0; i < team.Length; i++)
        {
            //Debug.Log("TEAM ["+i+"]: "+team[i]);
            if(team[i] == selectedCharacter){
                team[i]=null;
            }
            if(index != -1) team[index]=selectedCharacter;  // init
        }

        //update UI

        string charName;
        string charTitle;
        Texture texture;

        for (int i = 0; i < team.Length; i++)
        {
            if(team[i] == null){
                charName="";
                charTitle="";
                texture=Resources.Load("portraits/frame") as Texture;
            }else{
                charName=team[i].charName;
                charTitle=team[i].title;
                Debug.Log(charName);
                texture=Resources.Load("portraits/"+charName.ToLower()) as Texture;
            }

            //Debug.Log(teamFrameContainer.transform.Find("teamMember"+(i+1)).Find("icon"));
            teamFrameContainer.transform.Find("teamMember"+(i+1)).Find("icon").GetComponent<RawImage>().texture=texture;
            teamFrameContainer.transform.Find("teamMember"+(i+1)).Find("subPanel").Find("charName").GetComponent<TextMeshProUGUI>().text=charName;
            teamFrameContainer.transform.Find("teamMember"+(i+1)).Find("subPanel").Find("charTitle").GetComponent<TextMeshProUGUI>().text=charTitle;
        }

    }
}
