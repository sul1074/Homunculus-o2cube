using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputButton : MonoBehaviour
{
    public GameObject escCanvas;
    public GameObject inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) EscButton();

        else if (Input.GetKeyDown(KeyCode.I)) OpenInventory();
    }

    public void Pause() { Time.timeScale = 0; }
    public void Resume() { Time.timeScale = 1; }
    public void ExitFunction() { Application.Quit(); }

    public void EscButton()
    {
        if (escCanvas.activeSelf == false)  // esc창이 꺼져있다면 esc창 활성화
        {
            escCanvas.SetActive(true);
            Pause();
        }
        else  // esc창이 켜져있다면 esc창 비활성화
        {
            Resume();
            escCanvas.SetActive(false);
        }
    }

    public void OpenInventory()
    {
        if (inventory.activeSelf == false)
        {
            inventory.SetActive(true);
            Pause();
        }
        else
        {
            Resume();
            inventory.SetActive(false);
        }
    }
}
