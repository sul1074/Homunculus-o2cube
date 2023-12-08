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
            /* <�� ��ġ ����ǥ>  
             * +-6 �� +-1 ���̴� ����� ��.
             * ������ 0���� �۰ų� 35���� ũ�ٸ� ó�� X
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

            selectedOrder[roomOrder] = true; // ���޹��� ��ġ������ �̹� �����Ǿ����Ƿ�
            for (int i = 0; i < 4; i++)
            {
                if (posOrder[i] < 0 || posOrder[i] > size*size - 1) continue;
                if (roomOrder % size == 0 && i == 0) continue;  // �簢�� �� ���� ��ġ���� �ڱ⺸�� 1���� ��ġ������ ������ִٰ� �����Ǿ �ȵ� ex) 05�� 06�� ���� ������ �ƴ�
                else if (roomOrder % size == size - 1 && i == 1) continue;  // �簢�� �� ������ ��ġ���� �ڱ⺸�� 1ū ��ġ������ ������ִٰ� �����Ǿ �ȵ�
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
        //roomNums = 20; "���� A~G������ �����ϱ� ���� �ּ�ó�� �� �ν����Ϳ��� ������ �� �ְ� ��������"
        standardPos = 60;   // DungeonDoor.cs�� DoorManager.cs�� maxRayDistance�� �Բ� ��������� ���� ���������� ���� ��.
        rootOfRoomPosNum = 7;   // ������ �� �ִ� �� ��ġ�� ������ = (�ش� ���� * �ش� ����)��, ���簢�� ������.
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
        /* <�� ��ġ ����ǥ>  
        * +-6 �� +-1 ���̴� ����� ��.
         * ������ 0���� �۰ų� 35���� ũ�ٸ� ó�� X
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
        rooms[0].setRoomPos(roomPosCollections[n].getRoomPos()); // �÷��̾ ó�� �����ϴ� �Ա��� ��ġ���� ���� ����
        connectedRoomPosGroup.arrangeConnectedPosOrder(n);
        remainRoomNums--;

        while (remainRoomNums > 0)
        {   
            // �Ա��� �����ϰ� ��ġ�� ������ ���� ����
            n = Random.Range(1, roomNums);
            if (rooms[n].getIsPlaced() == true) continue;

            // �������� �濡 ���� ���� ����Ǿ� �ִ� ��ġ������ ������
            posOrder = connectedRoomPosGroup.getConnectedPosOrder();
            rooms[n].setRoomPos(roomPosCollections[posOrder].getRoomPos());
            connectedRoomPosGroup.arrangeConnectedPosOrder(posOrder); // ������ ��ġ�� ���� ������ ��ġ���� ����Ǿ� �ִ� ��ġ������ �߰�
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
        yield return new WaitForSeconds(0.3f);  // ���� ��ġ�� ���� ������ �Ϸ�Ǿ�� ������ �� ��ġ�� �ľ��Ͽ� ���������� ���� ���� �� �ֱ� ����. ������ �Ϸ�Ǳ���� ��ٸ�
                                                // Update�� �ش� ����� �α⿡��, �� ���� �۵��ϸ� �Ǵ� ����� �� �����Ӹ��� ȣ���ϴ� ���� ���ʿ��ϴ� �����Ͽ� �� ���� ȣ���ϱ� ����.
        for(int i = 0; i < roomNums; i++)
        {
            gameObjectRoom[i].GetComponentInChildren<DoorManager>().CreateAllDoors();
        }

        assignExitDoor();
    }
}
