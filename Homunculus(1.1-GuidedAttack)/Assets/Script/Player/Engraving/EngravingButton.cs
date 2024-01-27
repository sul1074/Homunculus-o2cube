using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EngravingButton : MonoBehaviour
{
    public Engraving engravingSystem;
    public EngravingButton parent;
    private int numOfChild;
    public EngravingButton[] child;
    private Image image;
    public Text text;
    public bool actived;
    public bool actable;

    public float hpStatus;

    private float AlphaThreshold;


    // Start is called before the first frame update
    void Start()
    {
        AlphaThreshold = 0.1f;
        GetComponent<Image>().alphaHitTestMinimumThreshold = AlphaThreshold;


        image = GetComponent<Image>();

        numOfChild = child.Length;

        if (parent)
            actable = parent.actived;
        else
            actable = true;
        actived = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckActable();

        ChangeColor();
    }

    private void ChangeColor()
    {
        if (actived == true)
        {
            image.color = new Color(1, 1, 1, 1);
            text.color = new Color(0, 0, 0, 1);
        }
        else if (actived == false && actable == true)
        {
            image.color = new Color(0.5f, 0.5f, 0.5f, 1);
            text.color = new Color(1, 1, 1, 1);
        }
        else if (actived == false && actable == false)
        {
            image.color = new Color(0, 0, 0, 1);
            text.color = new Color(1, 1, 1, 1);
        }
    }

    public void OnClick()
    {
        if (actived == false && actable == true)
        {
            actived = true;
        }
        else if (actived == true)
        {
            UnactivateChild();
        }

        engravingSystem.setToTalMaxHp();
    }

    private void CheckActable()
    {
        if (parent)
        {
            if (parent.actived == false)
            {
                this.actived = false;
                this.actable = false;
            }
            else if (parent.actived == true)
            {
                this.actable = true;
            }
        }
    }

    private void UnactivateChild()
    {
        this.actived = false;
        if (numOfChild > 0)
        {
            for (int i = 0; i < numOfChild; i++)
                child[i].UnactivateChild();
        }
    }
}