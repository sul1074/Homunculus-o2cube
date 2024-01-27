using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engraving : MonoBehaviour
{
    private bool visible;
    public GameObject EngravingPanel;

    private int numOfButtons;
    public EngravingButton[] Buttons;

    private float tatalMaxHp;


    // Start is called before the first frame update
    void Start()
    {
        visible = false;
        numOfButtons = Buttons.Length;

        tatalMaxHp = 0;
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

    public float getToTalMaxHp() { return this.tatalMaxHp; }  // getter
    public void setToTalMaxHp()
    {
        tatalMaxHp = 0;

        for (int i = 0; i < numOfButtons; i++)
        {
            if (Buttons[i].actived == true)
                tatalMaxHp += Buttons[i].hpStatus;
        }
    }
}
