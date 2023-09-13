using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    private bool isPlaced;

    // Start is called before the first frame update
    void Start()
    {
        isPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setIsPlaced(bool placed = true) { isPlaced = placed; }
    public bool getIsPlaced() { return isPlaced; }
    public void setRoomPos(Vector2 pos) 
    {
        if (isPlaced == true) return;   // 방의 위치가 이미 설정되었다면 더 이상 설정하지 않음.

        transform.position = pos;
        setIsPlaced();
    }

    public Vector2 getRoomPos() { return transform.position; }
}
