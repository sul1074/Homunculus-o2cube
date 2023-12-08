using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorManager : MonoBehaviour
{
    private float maxRayDistance;
    bool isClearedRoom;
    BoxCollider2D boxCollider2D;
    Vector2 boxColliderSize;
    GameObject[] wall;
    GameObject[] closedDoor;
    GameObject[] door;

    // Start is called before the first frame update
    void Start()
    {
        maxRayDistance = 60f;
        isClearedRoom = false;
        boxCollider2D = GetComponent<BoxCollider2D>();
        boxColliderSize = boxCollider2D.size;
        wall = new GameObject[4];
        closedDoor = new GameObject[4];
        door = new GameObject[4];
        InitGameObjects();
    }

    // Update is called once per frame
    void Update()
    {
        if (isClearedRoom != true) StartCoroutine(CreateAvailableDoor());
    }

    void InitGameObjects()  // East = 0, West = 1, South = 2, North = 3
    {
        string[] wallNames = { "East Wall", "West Wall", "South Wall", "North Wall" };
        string[] doorNames = { "East Door", "West Door", "South Door", "North Door" };
        string[] closedDoorNames = { "Closed East Door", "Closed West Door", "Closed South Door", "Closed North Door" };

        for (int i = 0; i < 4; i++)
        {
            wall[i] = transform.Find(wallNames[i]).gameObject;
            door[i] = transform.Find(doorNames[i]).gameObject;
            closedDoor[i] = transform.Find(closedDoorNames[i]).gameObject;
        }
    }

    public int getNumOfEnemyInRoom()
    {
        Collider2D[] hits = new Collider2D[1];

        return Physics2D.OverlapBoxNonAlloc(transform.position, boxColliderSize, 0, hits, LayerMask.GetMask("Enemy"));
    }

    void CreateEastDoor()
    {
        // 본인 scanner와 충돌을 피하기 위해 한칸 앞에서 발사
        if (Physics2D.Raycast(new Vector3(transform.position.x + 1 + boxColliderSize.x / 2, transform.position.y, transform.position.z), transform.right, maxRayDistance, LayerMask.GetMask("RoomScanner")))
        {
            wall[0].SetActive(false);
            closedDoor[0].SetActive(true);
        }
    }

    void CreateWestDoor()
    {
        if (Physics2D.Raycast(new Vector3(transform.position.x  - 1 - boxColliderSize.x / 2, transform.position.y, transform.position.z), -1 * (transform.right), maxRayDistance, LayerMask.GetMask("RoomScanner")))
        {
            wall[1].SetActive(false);
            closedDoor[1].SetActive(true);
        }
    }

    void CreateSouthDoor()
    {
        if (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y - 1 - boxColliderSize.y / 2, transform.position.z), -1*(transform.up), maxRayDistance, LayerMask.GetMask("RoomScanner")))
        {
            wall[2].SetActive(false);
            closedDoor[2].SetActive(true);
        }
    }
    void CreateNorthDoor()
    {
        if (Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y + 1 + boxColliderSize.y / 2, transform.position.z), transform.up, maxRayDistance, LayerMask.GetMask("RoomScanner")))
        {
            wall[3].SetActive(false);
            closedDoor[3].SetActive(true);
        }
    }

    public void CreateExitDoor()
    {
        transform.Find("Exit Door").gameObject.SetActive(true);
    }

    public void CreateAllDoors()
    {
        CreateEastDoor();
        CreateWestDoor();
        CreateSouthDoor();
        CreateNorthDoor();
    }
    void CreateAvailableDoors()
    {
        if (getNumOfEnemyInRoom() > 0 || isClearedRoom == true) return;

        for(int i = 0; i < 4; i++)
        {
            if (closedDoor[i].activeSelf == false) continue;
            closedDoor[i].SetActive(false);
            door[i].SetActive(true);
        }
        isClearedRoom = true;
    }

    IEnumerator CreateAvailableDoor()
    {
        yield return new WaitForSeconds(0.1f);

        CreateAvailableDoors();
    }
}
