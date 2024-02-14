using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engraving : MonoBehaviour
{
    private bool visible;
    public GameObject EngravingPanel;

    private int numOfButtons;
    public EngravingButton[] Buttons;

    public int cost;
    public Text costText;

    private float totalMaxHp;
    private float totalHpHealing;
    private float totalMaxMp;
    private float totalMpHealing;
    private float totalAttackSpeed;
    private float totalDefenseStatus;
    private float totalCriticalPercent;
    private float totalCriticalDamage;
    private float totalAvoidPercent;
    private float totalMoveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        numOfButtons = Buttons.Length;
        setCostText();

        totalMaxHp = 0;
        totalHpHealing = 0;
        totalMaxMp = 0;
        totalMpHealing = 0;
        totalAttackSpeed = 0;
        totalDefenseStatus = 0;
        totalCriticalPercent = 0;
        totalCriticalDamage = 0;
        totalAvoidPercent = 0;
        totalMoveSpeed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            openAndClosePanel();
        }
    }

    public void openAndClosePanel()
    {
        if (visible == false)
            visible = true;
        else if (visible == true)
            visible = false;

        EngravingPanel.SetActive(visible);
    }

    public int getCost() { return this.cost; }
    public void subtractCost(int x) { this.cost -= x; }


    public void setTotalEngravingStatus()
    {
        setToTalMaxHp();
        setToTalHpHealing();
        setToTalMaxMp();
        setToTalMpHealing();
        setToTalAttackSpeed();
        setToTalDefenseStatus();
        setToTalCriticalPercent();
        setToTalCriticalDamage();
        setToTalAvoidPercent();
        setToTalMoveSpeed();
    }

    public float getToTalMaxHp() { return this.totalMaxHp; }  // getter
    private void setToTalMaxHp()
    {
        totalMaxHp = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalMaxHp += Buttons[i].maxHp;
        }
    }

    public float getToTalHpHealing() { return this.totalHpHealing; }  // getter
    private void setToTalHpHealing()
    {
        totalHpHealing = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalHpHealing += Buttons[i].hpHealing;
        }
    }

    public float getToTalMaxMp() { return this.totalMaxMp; }  // getter
    private void setToTalMaxMp()
    {
        totalMaxMp = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalMaxMp += Buttons[i].maxMp;
        }
    }

    public float getToTalMpHealing() { return this.totalMpHealing; }  // getter
    private void setToTalMpHealing()
    {
        totalMpHealing = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalMpHealing += Buttons[i].mpHealing;
        }
    }

    public float getToTalAttackSpeed() { return this.totalAttackSpeed; }  // getter
    private void setToTalAttackSpeed()
    {
        totalAttackSpeed = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalAttackSpeed += Buttons[i].attackSpeed;
        }
    }

    public float getToTalDefenseStatus() { return this.totalDefenseStatus; }  // getter
    private void setToTalDefenseStatus()
    {
        totalDefenseStatus = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalDefenseStatus += Buttons[i].defenseStatus;
        }
    }

    public float getToTalCriticalPercent() { return this.totalCriticalPercent; }  // getter
    private void setToTalCriticalPercent()
    {
        totalCriticalPercent = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalCriticalPercent += Buttons[i].criticalPercent;
        }
    }

    public float getToTalCriticalDamage() { return this.totalCriticalDamage; }  // getter
    private void setToTalCriticalDamage()
    {
        totalCriticalDamage = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalCriticalDamage += Buttons[i].criticalDamage;
        }
    }

    public float getToTalAvoidPercent() { return this.totalAvoidPercent; }  // getter
    private void setToTalAvoidPercent()
    {
        totalAvoidPercent = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalAvoidPercent += Buttons[i].avoidPercent;
        }
    }

    public float getToTalMoveSpeed() { return this.totalMoveSpeed; }  // getter
    private void setToTalMoveSpeed()
    {
        totalMoveSpeed = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                totalMoveSpeed += Buttons[i].moveSpeed;
        }
    }

    public void setCostText() { costText.text = "Cost : " + this.cost; }
}
