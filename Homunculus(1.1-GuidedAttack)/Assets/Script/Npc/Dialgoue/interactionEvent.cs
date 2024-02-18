using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactionEvent : MonoBehaviour
{
    public int id;
    public bool isNpc;

    public int lineY;
    public int s_lineY;
    public DialogueEvent dialogue;
    //[SerializeField] SelectEvent select;

    /*public Dialogue[] GetDialogues()
    {
        dialogue.dialogues = DatabaseManager.instance.GetDialogue(1, 5);
        return dialogue.dialogues;
    }*/

    /*public SelectDialogue[] GetSelectes()
    {
        select.Selecter = DatabaseManager.instance.GetSelects(1, s_lineY);
        return select.Selecter;
    }*/

}