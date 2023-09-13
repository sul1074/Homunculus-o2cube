using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int totalPoint;
    public int stagePoint;
    public int stageIndex;
    public int health;
    public GameObject[] Stages;
    public PlayerController playerController;

    public Image[] UIHealth;
    public Text UIPoint;
    public Text UIStage;
    public GameObject RestartButton;

    // Start is called before the first frame update
    void Start()
    {
        totalPoint = 0;
        stagePoint = 0;
        stageIndex = 0;
        health = 3;
    }

    // Update is called once per frame
    void Update()
    {
        UIPoint.text = (totalPoint + stagePoint).ToString();
    }

    public void NextStage()
    {
        if(stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();

            UIStage.text = "Stage" + (stageIndex + 1);
        }
        else
        {
            Time.timeScale = 0;
            
            Text btnText = RestartButton.GetComponentInChildren<Text>();
            btnText.text = "Clear!";
            RestartButton.SetActive(true);
        }
        totalPoint += stagePoint;
        stagePoint = 0;
    }

    public void HealthDown()
    {
        if (health > 1)
        { 
            health--;
            UIHealth[health].color = new Color(1, 0, 0, 0.4f);
        }
        else
        {
            RestartButton.SetActive(true);
            UIHealth[0].color = new Color(1, 0, 0, 0.4f);
            playerController.OnDie();        
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(health > 1)
            {
                PlayerReposition();
            }

            HealthDown();
        }
    }

    void PlayerReposition()
    {
        playerController.transform.position = new Vector3(0.5f, 4, -1);
        playerController.VelocityZero();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Main");
        Time.timeScale = 1;
    }
}
