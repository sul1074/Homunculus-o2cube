using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager globalGameManager;

    public int totalPoint;
    public int stagePoint;
    public int stageIndex;

    public float playerHp;
    private float playerHpMax;
    private float playerMpMax;
    private float playerExpMax;

    public GameObject[] Stages;
    public PlayerController playerController;
    public PlayerStatus playerStatus;

    public Image[] UIHealth;
    public GameObject RestartButton;
    public Slider hpSlider;
    public Slider mpSlider;
    public Slider expSlider;

    void Awake()
    {
        if (globalGameManager == null)
        {
            globalGameManager = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
        // Start is called before the first frame update
        void Start()
    {
        totalPoint = 0;
        stagePoint = 0;
        stageIndex = 0;
        playerHpMax = playerStatus.getHpMax();
        playerMpMax = playerStatus.getMpMax();
        playerExpMax = playerStatus.getExpMax();
        hpSlider.interactable = false;
        mpSlider.interactable = false;
        expSlider.interactable = false;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBarOn();
    }

    public void NextStage()
    {
        if(stageIndex < Stages.Length - 1)
        {
            Stages[stageIndex].SetActive(false);
            stageIndex++;
            Stages[stageIndex].SetActive(true);
            PlayerReposition();
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

    public void HealthDown(float hitDamage)
    {
        float calculatedDamage = playerStatus.getHitDamage(hitDamage);

        if (playerHp - calculatedDamage > 0) playerHp -= calculatedDamage;

        else
        {
            playerHp = 0;
            playerController.OnDie();
        }
    }

    public void HealthUp(float regenPoint)
    {
        if (playerHp >= playerHpMax)
        {
            hpSlider.value = 1.0f;
        }
        else
        {
            playerHp += regenPoint;
        }
    }

    public void adjustExp(float exp) 
    { 
        expSlider.value += (exp / playerExpMax);

        if (expSlider.value >= 1)
        {
            playerStatus.levelUp(1);
            updateStatus();
        }
    }

    public void updateStatus()
    {
        playerHpMax = playerStatus.getHpMax();
        playerMpMax = playerStatus.getMpMax();
        playerExpMax = playerStatus.getExpMax();
        playerHp = playerHpMax;
        mpSlider.value = 1;
        expSlider.value = 0;
    }
    /*
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
    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            /*
            if(health > 1)
            {
                PlayerReposition();
            }
            */

            HealthDown(50.0f);
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

    private void HealthBarOn()
    {
        hpSlider.value = playerHp/playerHpMax;
    }
}
