using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossRoom : MonoBehaviour
{
    public BossController Boss;
    public PlayerController Player;
    public GameObject BossKillEvent;
    public Image npc;
    public Image npcback;
    private bool isTalking;

    // Start is called before the first frame update
    void Start()
    {
        isTalking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!Boss.getIsAlive() && !isTalking) {
            talk();
        }

        if(isTalking && Input.GetKeyDown(KeyCode.E))
            Player.forcedTalk(BossKillEvent);
    }


    private void talk()
    {
        isTalking = true;
        Player.forcedTalk(BossKillEvent);
    }


}
