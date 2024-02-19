using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tutorialDoor : MonoBehaviour
{
    PlayerController playerController;
    public Transform roomA;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Vector2 nextRoomPos = (Vector2)roomA.position;
            playerController = collision.gameObject.GetComponent<PlayerController>();
            StartCoroutine(playerController.TeleportInDungeon(nextRoomPos));
        }
    }
}
