using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�ν�����â���� ��������
[System.Serializable]
public class Dialogue
{
    [Tooltip("ĳ���� �̸�")]
    public string name;

    [Tooltip("��� ����")]
    public string[] contexts;

    [Tooltip("�̺�Ʈ ��ȣ")]
    public int number;

    [Tooltip("��ŵ����")]
    public int skipnum;

    [Tooltip("�÷��̾� ����")]
    public bool isplayer;
}

[System.Serializable]
public class DialogueEvent
{
    //�̺�Ʈ �̸�
    public string name;

    //public Vector2 line;
    public Dialogue[] dialogues;
}