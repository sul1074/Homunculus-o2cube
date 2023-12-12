using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TalkPrinter : MonoBehaviour
{
    public TalkManager talkManager;
    public GameObject talkPanel;
    public Image portraitImg;
    public TypingEffect talk;
    public GameObject scanObject;
    public bool isTalking;
    public int talkIndex;
    public GameObject blackBackGround;
    public GameObject playerSprite;
    public Image playerBlackBackground;
    public Image npcBlackBackground;
    Color color;

    bool playerTalking;

    private void Awake()
    {

        talkPanel.SetActive(false);
        blackBackGround.SetActive(false);
        portraitImg.color = new Color(1, 1, 1, 0);
        playerSprite.SetActive(false);

        playerBlackBackground.color = new Color(0, 0, 0, 0);
        npcBlackBackground.color = new Color(0, 0, 0, 0);

        playerTalking = true;
    }

    public void Talk(GameObject scanObj)
    {
        scanObject = scanObj;
        NpcData npcData = scanObject.GetComponent<NpcData>();
        Communicate(npcData.id, npcData.isNpc);

        talkPanel.SetActive(isTalking);
        blackBackGround.SetActive(isTalking);
        if (isTalking) {
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else {
            portraitImg.color = new Color(1, 1, 1, 0);
        }
        playerSprite.SetActive(isTalking);
    }

    void Communicate(int id, bool isNpc)
    {
        string talkData = talkManager.GetTalk(id, talkIndex);

        if (talkData == null) {
            isTalking = false;
            talkIndex = 0;
            playerTalking = true;

            
            SceneManager.LoadScene("Dungeon Generation");

            return;
        }

        if (isNpc) {
            talk.SetMsg(talkData.Split(':')[0]);

            portraitImg.sprite = talkManager.GetPortrait(id, int.Parse(talkData.Split(':')[1]));
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else {
            talk.SetMsg(talkData);

            portraitImg.color = new Color(1, 1, 1, 0);
        }

        isTalking = true;

        if (!playerTalking)
        {
            talkIndex++;
            color = npcBlackBackground.color;
            color.a = 0.2f;
            playerBlackBackground.color = new Color(0, 0, 0, 0);
            npcBlackBackground.color = color;
            playerTalking = true;
        }
        else
        {
            color = playerBlackBackground.color;
            color.a = 0.2f;
            playerBlackBackground.color = color;
            npcBlackBackground.color = new Color(0, 0, 0, 0);
            playerTalking = false;
        }
    }
}
