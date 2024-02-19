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
    public int dialogueIndex;
    public GameObject blackBackGround;
    public GameObject playerSprite;
    public Image playerBlackBackground;
    public Image npcBlackBackground;
    Color color;


    private void Awake()
    {

        talkPanel.SetActive(false);
        blackBackGround.SetActive(false);
        portraitImg.color = new Color(1, 1, 1, 0);
        playerSprite.SetActive(false);

        playerBlackBackground.color = new Color(0, 0, 0, 0);
        npcBlackBackground.color = new Color(0, 0, 0, 0);
    }

    public void Talk(GameObject scanObj)
    {
        scanObject = scanObj;
        interactionEvent dialogue = scanObject.GetComponent<interactionEvent>();

        NpcData npcData = scanObject.GetComponent<NpcData>();
        Communicate(dialogue);

        talkPanel.SetActive(isTalking);
        blackBackGround.SetActive(isTalking);
        if (isTalking) {
            portraitImg.color = new Color(1, 1, 1, 1);
        }
        else {
            portraitImg.color = new Color(1, 1, 1, 0);
        }
        playerSprite.SetActive(isTalking);

        if(scanObj.GetComponent<interactionEvent>().dialogue.name == "독백")
        {
            npcBlackBackground.color = new Color(0, 0, 0, 0);
            portraitImg.color = new Color(0, 0, 0, 0);
        }
    }

    void Communicate(interactionEvent dialogue)
    {
        if (talkIndex > dialogue.dialogue.dialogues[dialogueIndex].contexts.Length - 1) {
            isTalking = false;
            talkIndex = 0;
            dialogueIndex++;
        }

        if (dialogueIndex > dialogue.dialogue.dialogues.Length - 1)
        {
            isTalking = false;
            talkIndex = 0;
            dialogueIndex = 0;
            playerBlackBackground.color = new Color(0, 0, 0, 0);
            npcBlackBackground.color = new Color(0, 0, 0, 0);

            if (dialogue.id == 1001) // 튜토리얼의 쓰러진 NPC를 처치
            {
                GameManager.globalGameManager.adjustExp(10f);
                dialogue.gameObject.GetComponent<PolygonCollider2D>().enabled = false;
            }
            else if (dialogue.id == 1002)
            {
                SceneManager.LoadScene("Ending Credit");
            }



            return;
        }

        if (dialogue.isNpc)
        {
            talk.SetMsg(dialogue.dialogue.dialogues[dialogueIndex].contexts[talkIndex]);

            portraitImg.sprite = talkManager.GetPortrait(dialogue.id, 0);
            portraitImg.color = new Color(1, 1, 1, 1);
        }

        isTalking = true;

        if (dialogue.dialogue.dialogues[dialogueIndex].isplayer)
        {
            talkIndex++;
            color = npcBlackBackground.color;
            color.a = 0.2f;
            playerBlackBackground.color = new Color(0, 0, 0, 0);
            npcBlackBackground.color = color;
        }
        else
        {
            talkIndex++;
            color = playerBlackBackground.color;
            color.a = 0.2f;
            playerBlackBackground.color = color;
            npcBlackBackground.color = new Color(0, 0, 0, 0);
        }


    }
}
