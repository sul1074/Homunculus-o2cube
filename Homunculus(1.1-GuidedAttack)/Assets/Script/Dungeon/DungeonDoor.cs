using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonDoor : MonoBehaviour
{
    //private bool isInDoorArea; // 해당 변수는 사용되지 않음, 아래 수정사항 참고
    float maxRayDistance;
    PlayerController playerController;
    DoorManager roomScanner;

    // Start is called before the first frame update
    void Start()
    {
        //isInDoorArea = false;
        maxRayDistance = 70f;
        roomScanner = GetComponentInParent<DoorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (isInDoorArea && Input.GetKeyDown(KeyCode.F))
        {
            if(isEnemyInTheRoom() == true || playerController == null) return;

            StartCoroutine(playerController.TeleportInDungeon(getNextRoomPos()));
        }  던전 간의 이동 시에 화면이 버벅이는 문제 때문에 해당 코드를 OnTriggerEnter2D에 옮겼음 */
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            playerController = collision.GetComponent<PlayerController>();
            //isInDoorArea = true;

            // 여기서부터 Update에서 옮긴 코드
            if (isEnemyInTheRoom() == true || playerController == null) return;

            StartCoroutine(playerController.TeleportInDungeon(getNextRoomPos()));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            //isInDoorArea = false;
        }
    }

    public bool isEnemyInTheRoom()
    {
        if (roomScanner.getNumOfEnemyInRoom() > 0) return true;
        else return false;
    }

    Vector2 getNextRoomPos()
    {
        RaycastHit2D hitData;
        Vector2 nextRoomPos;

        if (this.gameObject.name == "East Door")
        {
            hitData = Physics2D.Raycast(new Vector2(transform.position.x + 10f, transform.position.y), transform.right, maxRayDistance, LayerMask.GetMask("RoomScanner"));
            nextRoomPos = hitData.collider.gameObject.transform.Find("West Door").transform.position;
            nextRoomPos = new Vector2(nextRoomPos.x + 2.5f, nextRoomPos.y);
            return nextRoomPos;
        }

        else if(this.gameObject.name == "West Door")
        {
            hitData = Physics2D.Raycast(new Vector2(transform.position.x - 10f, transform.position.y), -1*transform.right, maxRayDistance, LayerMask.GetMask("RoomScanner"));
            nextRoomPos = hitData.collider.gameObject.transform.Find("East Door").transform.position;
            nextRoomPos = new Vector2(nextRoomPos.x - 2.5f, nextRoomPos.y);
            return nextRoomPos;
        }

        else if(this.gameObject.name == "South Door")
        {
            hitData = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 10f), -1*transform.up, maxRayDistance, LayerMask.GetMask("RoomScanner"));
            nextRoomPos = hitData.transform.Find("North Door").transform.position;
            nextRoomPos = new Vector2(nextRoomPos.x, nextRoomPos.y - 2.5f);
            return nextRoomPos;
        }

        else
        {
            hitData =  Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 10f), transform.up, maxRayDistance, LayerMask.GetMask("RoomScanner"));
            nextRoomPos = hitData.transform.Find("South Door").transform.position;
            nextRoomPos = new Vector2(nextRoomPos.x + 2f, nextRoomPos.y + 2.5f);
            return nextRoomPos;
        }
    }
}
