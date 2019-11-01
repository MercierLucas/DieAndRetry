[System.Serializable]
public class Character 
{
    // Start is called before the first frame update
    public string charName;
    public string title;
    public string history;
    public string role;
    public float life;
    public float maxLife;
    public float resistance;
    public float moral;
    public float strength;
    public float mindpower;
    public float initiative;

    public float armor;

    public bool isAlly;
    
    public bool isDead;

    public float critRate;
    public int[] spellsIds;

    public Spells[] spells;

    public Character(){
        this.maxLife=this.life;
        this.isDead=false;
    }

    public float takeDamages(float damages){
        float realDamages=damages-armor;
        if(realDamages<0) realDamages=0;
        life-=realDamages;
        if(life<=0){
            life=0;
            isDead=true;
        }
        return realDamages;
    }

    public float getLife(){
        return this.life;
    }

    public float getMaxLife(){
        return this.maxLife;
    }
}
