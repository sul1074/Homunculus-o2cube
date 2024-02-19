using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && Input.GetKey(KeyCode.F))
        {
            StartCoroutine(collision.gameObject.GetComponent<PlayerController>().TeleportInDungeon(new Vector2(-146.91f, -126.55f)));
        }
    }
}
