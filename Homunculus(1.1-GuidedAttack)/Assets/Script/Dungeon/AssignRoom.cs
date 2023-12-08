using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignRoom : MonoBehaviour
{
    class RoomPosCollection
    {
        Vector2 pos;

        public RoomPosCollection(Vector2 pos)
        {
            this.pos = pos;
        }
        public void setRoomPos(Vector2 pos) { this.pos = pos; }
        public Vector2 getRoomPos() { return this.pos; }
        ~RoomPosCollection()
        {
            pos = new Vector2(0, 0);
        }
    }
    class ConnectedRoomPosGroup
    {
        private bool[] connectedRoomsOrder;
        private bool[] selectedOrder;
        int size;
        public ConnectedRoomPosGroup(int size)
        {
            connectedRoomsOrder = new bool[size * size];
            selectedOrder = new bool[size * size];
            this.size = size;
            for (int i = 0; i < size * size; i++)
            {
                connectedRoomsOrder[i] = false;
                selectedOrder[i] = false;
            }
        }
        public void setSelectedOrder(int n) { selectedOrder[n] = true; }
        public void arrangeConnectedPosOrder(int roomOrder)
        {
            /* <방 위치 순서표>  
             * +-6 과 +-1 차이는 연결된 것.
             * 범위가 0보다 작거나 35보다 크다면 처리 X
            30 31 32 33 34 35 
            24 25 26 27 28 29
            18 19 20 21 22 23
            12 13 14 15 16 17
            06 07 08 09 10 11
            00 01 02 03 04 05
            */

            int[] posOrder = new int[4];
            posOrder[0] = roomOrder - 1;
            posOrder[1] = roomOrder + 1;
            posOrder[2] = roomOrder - size;
            posOrder[3] = roomOrder + size;

            selectedOrder[roomOrder] = true; // 전달받은 위치순서는 이미 배정되었으므로
            for (int i = 0; i < 4; i++)
            {
                if (posOrder[i] < 0 || posOrder[i] > size*size - 1) continue;
                if (roomOrder % size == 0 && i == 0) continue;  // 사각형 맨 왼쪽 위치들은 자기보다 1작은 위치순서와 연결돼있다고 판정되어선 안됨 ex) 05와 06은 연결 판정이 아님
                else if (roomOrder % size == size - 1 && i == 1) continue;  // 사각형 맨 오른쪽 위치들은 자기보다 1큰 위치순서와 연결돼있다고 판정되어선 안됨
                connectedRoomsOrder[posOrder[i]] = true;
            }
        }
        public int getConnectedPosOrder()
        {
            int n;
            while(true)
            {
                n = Random.Range(0, size * size);
                if (connectedRoomsOrder[n] == false || selectedOrder[n] == true) continue;
                return n;
            }
        }
    }

    [SerializeField] private int roomNums;
    private int standardPos;
    private int rootOfRoomPosNum;
    private int remainRoomNums;

    public GameObject[] gameObjectRoom;
    public GameObject entrace;
    Room[] rooms;
    RoomPosCollection[] roomPosCollections;
    ConnectedRoomPosGroup connectedRoomPosGroup;

    // Start is called before the first frame update
    void Start()
    {
        //roomNums = 20; "던전 A~G까지만 생성하기 위해 주석처리 후 인스펙터에서 조정할 수 있게 변경했음"
        standardPos = 60;   // DungeonDoor.cs와 DoorManager.cs의 maxRayDistance도 함께 수정해줘야 문이 정상적으로 생성 됨.
        rootOfRoomPosNum = 7;   // 배정될 수 있는 방 위치의 갯수는 = (해당 변수 * 해당 변수)로, 정사각형 형태임.
        remainRoomNums = roomNums;
        roomPosCollections = new RoomPosCollection[rootOfRoomPosNum*rootOfRoomPosNum];
        rooms = new Room[roomNums];
        connectedRoomPosGroup = new ConnectedRoomPosGroup(rootOfRoomPosNum);

        for (int i = 0; i < roomNums; i++)
            rooms[i] = gameObjectRoom[i].GetComponent<Room>();

        initializeRoomPos();
        setAllRoomsPos();
        StartCoroutine(createDoorOfRooms());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    float getDistanceInterTwoRooms(Vector2 pos1, Vector2 pos2)
    {
        float x = pos1.x - pos2.x;
        float y = pos1.y - pos2.y;

        return Mathf.Sqrt(x * x + y * y);
    }

    void initializeRoomPos()
    {
        /* <방 위치 순서표>  
        * +-6 과 +-1 차이는 연결된 것.
         * 범위가 0보다 작거나 35보다 크다면 처리 X
        30 31 32 33 34 35 
        24 25 26 27 28 29
        18 19 20 21 22 23
        12 13 14 15 16 17
        06 07 08 09 10 11
        00 01 02 03 04 05
        */

        int n = 0;
        for (int i = 0; i < rootOfRoomPosNum; i++)
        {
            for (int j = 0; j < rootOfRoomPosNum; j++)
            {
                roomPosCollections[n] = new RoomPosCollection(new Vector2(standardPos * j, standardPos * i));
                n++;
            }
        }
    }

    void setAllRoomsPos()
    {
        int posOrder;
        int n = Random.Range(0, rootOfRoomPosNum * rootOfRoomPosNum);
        rooms[0].setRoomPos(roomPosCollections[n].getRoomPos()); // 플레이어가 처음 입장하는 입구의 위치부터 먼저 설정
        connectedRoomPosGroup.arrangeConnectedPosOrder(n);
        remainRoomNums--;

        while (remainRoomNums > 0)
        {   
            // 입구를 제외하고 위치를 배정할 방을 선택
            n = Random.Range(1, roomNums);
            if (rooms[n].getIsPlaced() == true) continue;

            // 배정받을 방에 대해 방들과 연결되어 있는 위치순서를 배정함
            posOrder = connectedRoomPosGroup.getConnectedPosOrder();
            rooms[n].setRoomPos(roomPosCollections[posOrder].getRoomPos());
            connectedRoomPosGroup.arrangeConnectedPosOrder(posOrder); // 배정된 위치에 대해 인접한 위치들을 연결되어 있는 위치순서로 추가
            remainRoomNums--;
        }
    }

    void assignExitDoor()
    {
        Vector2 entrancePos = entrace.transform.position;
        float furthestDistanceFromEntrance = getDistanceInterTwoRooms(entrancePos, rooms[0].getRoomPos());
        int furthestRoomNum = 0;

        for (int i = 1; i < roomNums; i++)
        {
            float distance = getDistanceInterTwoRooms(entrancePos, rooms[i].getRoomPos());
            if (distance > furthestDistanceFromEntrance)
            {
                furthestDistanceFromEntrance = distance;
                furthestRoomNum = i;
            }
        }

        gameObjectRoom[furthestRoomNum].GetComponentInChildren<DoorManager>().CreateExitDoor();
    }
    IEnumerator createDoorOfRooms()
    {
        yield return new WaitForSeconds(0.3f);  // 방의 위치가 랜덤 배정이 완료되어야 주위의 방 위치를 파악하여 정상적으로 문을 만들 수 있기 때문. 배정이 완료되기까지 기다림
                                                // Update에 해당 기능을 두기에는, 한 번만 작동하면 되는 기능을 매 프레임마다 호출하는 것은 불필요하다 생각하여 한 번만 호출하기 위함.
        for(int i = 0; i < roomNums; i++)
        {
            gameObjectRoom[i].GetComponentInChildren<DoorManager>().CreateAllDoors();
        }

        assignExitDoor();
    }
}
