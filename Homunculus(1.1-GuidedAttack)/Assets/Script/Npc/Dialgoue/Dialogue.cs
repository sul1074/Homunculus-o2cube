using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//인스펙터창에서 수정가능
[System.Serializable]
public class Dialogue
{
    [Tooltip("캐릭터 이름")]
    public string name;

    [Tooltip("대사 내용")]
    public string[] contexts;

    [Tooltip("이벤트 번호")]
    public int number;

    [Tooltip("스킵라인")]
    public int skipnum;

    [Tooltip("플레이어 여부")]
    public bool isplayer;
}

[System.Serializable]
public class DialogueEvent
{
    //이벤트 이름
    public string name;

    //public Vector2 line;
    public Dialogue[] dialogues;
}