using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    Dictionary<int, string[]> talkData;
    Dictionary<int, Sprite> portraitData;

    public Sprite[] portraitArr;

    void Awake()
    {
        talkData = new Dictionary<int, string[]>();
        portraitData = new Dictionary<int, Sprite>();
        GenerateData();
    }

    void GenerateData()
    {
        
        talkData.Add(1000, new string[] { "�����, ȣ�� \n������ ���� ������?:0", "�׷� �����ֵ��� ����:0" });
        talkData.Add(1001, new string[] { "��ȭ1:0" });
        talkData.Add(1, new string[] { "��" });

        portraitData.Add(1000 + 0, portraitArr[0]);
        portraitData.Add(1001 + 0, portraitArr[1]);
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length)
            return null;
        else
            return talkData[id][talkIndex];
    }

    public Sprite GetPortrait(int id, int portraitIndex)
    {
        return portraitData[id + portraitIndex];
    }
}
