using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool[] nodeUsable;
    private Character virtualCharacter;
    private Node[] nodes;

    void Awake()
    {
        Instance = this;
        InitializeNodes();

        virtualCharacter = new Character(10); 
    }

    void InitializeNodes()
    {
        nodeUsable = new bool[7];
        for (int i = 0; i < 7; i++)
        {
            nodeUsable[i] = false;
        }

        nodes[0].SubNodes.Add(nodes[1]);
        nodes[1].SubNodes.Add(nodes[2]);
        nodes[2].SubNodes.Add(nodes[3]);
        nodes[3].SubNodes.Add(nodes[5]);
        nodes[5].SubNodes.Add(nodes[6]);
        nodes[6].SubNodes.Add(nodes[4]);

        // 1번 노드는 항상 사용 가능 상태
        nodeUsable[0] = true;
    }

    public bool IsNodeUsable(int nodeIndex)
    {
        return nodeUsable[nodeIndex];
    }

    public void UpdateNodeUsability(int nodeIndex, bool isActive)
    {
        if (isActive)
        {
            CharacterAbility(nodeIndex);

            if (nodeIndex > 0 && nodeIndex < nodes.Length)
            {
                Node parentNode = nodes[nodeIndex - 1];
                parentNode.SubNodes.ForEach(subNode =>
                {
                    nodeUsable[subNode.Index] = true;
                });
            }
        }
    }

    void CharacterAbility(int nodeIndex)
    {
        virtualCharacter.MaxHp = (nodeIndex + 1);
    }
}

public class Character
{
    public int MaxHp { get; set; }

    public Character(int initialMaxHp)
    {
        MaxHp = initialMaxHp;
    }
}

