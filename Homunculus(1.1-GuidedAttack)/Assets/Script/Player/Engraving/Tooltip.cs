using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        setPosition();
    }

    public void ShowTooltip()
    {
        gameObject.SetActive(true);
    }
    public void CloseTooltip()
    {
        gameObject.SetActive(false);
    }

    private void setPosition()
    {
        Vector3 newPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y - 50f, 0f);
        transform.position = newPos; 
    }
}
