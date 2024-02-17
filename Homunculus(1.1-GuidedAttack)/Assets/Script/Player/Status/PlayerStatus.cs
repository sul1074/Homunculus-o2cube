using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    // level & exp
    private int lv;
    private float expMax;
    private float exp;

    // hp & mp status
    private float hpMax;
    private float hp;
    private float hpRegen;
    private float mpMax;
    private float mp;
    private float mpRegen;

    // attack & defend & critical atk
    private float atkPoint;
    private float atkSpeed;
    private float defPoint;
    private float critPoint;
    private float critAtk;

    // evade & speed
    private float evasionPoint;
    private float moveSpeed;

    public Engraving engraving;
    public GameObject levelupText;
   
    // Start is called before the first frame update
    void Start()
    {
        lv = 1;
        exp = 0;
        setAtkSpeed(0.3f);
        setCritPoint(20.0f);
        setCritAtk(200.0f);
        setMoveSpeed(6.0f);
        updateStatusAffectedByLevel();
        setEvasionPoint(20.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // setter & getter

    // level & exp
    public int getCurrLevel() { return lv; }
    public void updateExpMax() { expMax = lv * 10; }
    public float getExpMax() { return expMax; }
    public float getCurrExp() { return exp; }
    public void adjustExp(float exp) { this.exp += exp; }
    public float getCurrExpPercentage() { return (getCurrExp() / getExpMax() * 100.0f); }
    public void levelUp(int level)
    {
        lv += level;
        exp = 0;

        // adjust status concered with level
        updateStatusAffectedByLevel();
        engraving.subtractCost((-1) * (lv - 1));
        engraving.setCostText();

        // restore Hp & Mp
        setCurrHp(getHpMax());
        setCurrHp(getMpMax());

        levelupText.SetActive(true);
    }

    // hp
    public void updateHpMax() { hpMax = 100.0f + (float)(lv * 20); }
    public void adjustHpMax(float hp) { hpMax += hp; }
    public float getHpMax() { return hpMax; }
    public void setCurrHp(float hp) { this.hp += hp; }
    public float getCurrHp() { return hp; }
    public void updateHpRegen() { hpRegen = hpMax * 0.01f; }
    public void adjustHpRegen(float hpRegen) { this.hpRegen += hpRegen; }
    public float getHpRegen() { return hpRegen; }
    public void updateMpMax() { mpMax = 20.0f + (float)(lv * 4); }

    // mp
    public void adjustMpMax(float mp) { mpMax += mp; } 
    public float getMpMax() { return mpMax; }
    public void adjustMp(float mp) { this.mp += mp; }
    public float getCurrMp() { return mp; }
    public void updateMpRegen() { mpRegen = mpMax * 0.005f; }
    public void adjustMpRegen(float mpRegen) { this.mpRegen += mpRegen; }
    public float getMpRegen() { return mpRegen; }

    // attack & defense
    public void updateAtkPoint() { atkPoint = 10.0f + (float)lv; }
    public void adjustAtkPoint(float atk) { atkPoint += atk; }
    public float getAtkPoint() { return atkPoint; }
    public void setAtkSpeed(float atkSpeed) { this.atkSpeed = atkSpeed; }
    public float getAtkSpeed() { return atkSpeed; }
    public void setDefPoint(float defPoint) { this.defPoint = defPoint; }
    public float getDefPoint() { return defPoint; }

    // critical
    public void setCritPoint(float critPoint) { this.critPoint = critPoint; }
    public float getCritPoint() { return critPoint; }
    public void setCritAtk(float critAtk) { this.critAtk = critAtk; }
    public float getCritAtk() { return critAtk; }

    // evasion & moveSpeed
    public void setEvasionPoint(float evasionPoint) { this.evasionPoint = evasionPoint; }
    public float getEvasionPoint() { return evasionPoint; }
    public void setMoveSpeed(float moveSpeed) { this.moveSpeed = moveSpeed; }
    public float getMoveSpeed() { return moveSpeed; }
    
    // calculated atkDamage & hitDamage
    public float getAtkDamage(float enemyDefPoint) {
        return (this.atkPoint - (this.atkPoint * enemyDefPoint * 0.01f)); 
    }
    public float getCritAtkDamage(float enemyDefPoint) {
        return (atkPoint - (this.atkPoint * enemyDefPoint * 0.01f) * critAtk * 0.01f);
    }
    public float getHitDamage(float enemyAtkPoint) {
        return (enemyAtkPoint - (enemyAtkPoint * this.defPoint * 0.01f)); 
    }

    // update status affected by player level
    public void updateStatusAffectedByLevel()
    {
        updateExpMax();
        updateHpMax();
        updateHpRegen();
        updateMpMax();
        updateMpRegen();
        updateAtkPoint();
    }
}
