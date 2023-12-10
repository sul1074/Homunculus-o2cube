using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using static ItemClass;

public class test : MonoBehaviour
{
    public GameObject @object;

    /// <summary>
    /// �̹��� ������Ʈ��
    /// </summary>
    public List<Image> imagecomponents;

    /// <summary>
    /// ����� �����۰���
    /// </summary>
    public List<ItemClass> intitemvalues;

    /// <summary>
    /// ǥ���� �̹�����
    /// </summary>
    public List<Sprite> sprites;

    public Text GetText;

    /// <summary>
    /// ���� ������ ��ȣ
    /// </summary>
    public int index;

    /// <summary>
    /// ������ ��� ������Ʈ
    /// </summary>
    public List<Image> listImageEquip;

    /// <summary>
    /// ������ ����
    /// </summary>
    public List<int> listIntEquip;
    //Ŭ���� ����
    public ItemClass itemClass;
    public List<ItemClass> itemClasses;

    public List<ItemClass> equippedItems;

    // Start is called before the first frame update
    void Start()
    {
    
        //Ŭ���� �ʱ�ȭ
        itemClass = new ItemClass(001,"����",01, AttackType.Melee, HandType.TwoHanded ,true,4,5);
        


        itemClasses = new List<ItemClass>();
        itemClasses.Add(itemClass);
        //Debug.Log(itemClasses[0].stringItemName);
    }

    // Update is called once per frame
    void Update()
    {
        InputKey();
        ChangeImage();



    }

    public void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (@object.activeSelf == true)
            {
                @object.SetActive(false);
            }
            else
            {
                @object.SetActive(true);

            }

        }
    }

    public void ChangeImage()
    {
        // �Ϲ� �κ��丮 ������ ����
        for (int i = 0; i < imagecomponents.Count; i++)
        {
            if (intitemvalues[i].ItemID == 0)
            {
                imagecomponents[i].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = sprites[0];
            }
            else if (intitemvalues[i] == 1)
            {
                imagecomponents[i].transform.GetChild(1).GetComponent<Image>().sprite = sprites[1];
            }
            else
            {
                imagecomponents[i].transform.GetChild(1).GetComponent<Image>().sprite = sprites[2];
            }
        }

        // ��� ������ ������ ����
        for (int i = 0; i < listImageEquip.Count; i++)
        {
            if (listIntEquip[i] == 0)
            {
                listImageEquip[i].sprite = sprites[0];
            }
            else if (listIntEquip[i] == 1)
            {
                listImageEquip[i].sprite = sprites[1];
            }
            else
            {
                listImageEquip[i].sprite = sprites[2];
            }
        }

        //�κ��丮 ����
        for (int i = 0; i < imagecomponents.Count; i++)
        {
            imagecomponents[i].transform.GetChild(1).GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }

        // ���â ����
        for (int i = 0; i < listImageEquip.Count; i++)
        {
            listImageEquip[i].color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }

        if (index != -1)
        {

            imagecomponents[index].transform.GetChild(1).GetComponent<Image>().color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255/ 255f);
        }


        if(index != -1)
        {
            GetText.text = "���� �� ���ڴ� " + intitemvalues[index];
        }
        else
        {
            GetText.text = "���ڸ� ���ÿ�";
        }

    }

    /// <summary>
    /// ��ư�� Ŭ�� ����
    /// </summary>
    /// <param name="inpus"></param>
    public void teste(int inpus)
    {
        // �ٸ��� ��������
        if(index != inpus)
        {
            if(index == -1)
            {
                index = inpus;
            }
            else
            {
                int t = intitemvalues[index];
                intitemvalues[index] = intitemvalues[inpus];
                intitemvalues[inpus] = t;
                index = -1;
            }

        }
        // ������ ��������
        else
        {
            index = -1;
        }

    }

    /// <summary>
    /// ��� ��ü�մϴ�
    /// </summary>
    public void EquipItem(int index)
    {
        
        ItemClass t = equippedItems[(int)EnumOfType.head];

        // t = a;
        // a = b;
        switch (itemClasses[index].itemOfType)
        {
            case EnumOfType.head:
                t = equippedItems[(int)EnumOfType.head];
                equippedItems[(int)EnumOfType.head] = itemClasses[index];
                break;
            case EnumOfType.body:
                t = equippedItems[(int)EnumOfType.body];
                equippedItems[(int)EnumOfType.body] = itemClasses[index];
                break;
            case EnumOfType.leg:
                t = equippedItems[(int)EnumOfType.leg];
                equippedItems[(int)EnumOfType.leg] = itemClasses[index];
                break;
            case EnumOfType.foot:
                t = equippedItems[(int)EnumOfType.foot];
                equippedItems[(int)EnumOfType.foot] = itemClasses[index];
                break;

            default:
                t = equippedItems[(int)EnumOfType.head];
                equippedItems[(int)EnumOfType.head] = itemClasses[index];
                break;
        }

        // b = t;
        itemClasses[index] = t;






    }
}

public class ItemClass
{

    public enum HandType
    {
        OneHanded, TwoHanded
    }// �Ѽ�/��� ����
    public enum EnumOfType
    {
        head, body, leg, foot
    }// ���â ���� 

    public EnumOfType itemOfType;

    public enum AttackType
    {
        Melee , Range , Magic
    }//���� ���� �з�

    public bool AutoReuseType;
    

    public float ItemID;
    public string stringItemName;
    public float ItemLevel;
    public AttackType DamageType;
    public HandType BoolTwoHand;
    public bool AutoReuse;
    public float atkPoint;
    public float atkSpeed;



    public ItemClass(float input1, string input2, float input3, AttackType input4, HandType  input5, bool input6, float input7, float input8, EnumOfType input9 = EnumOfType.head)
    {
        ItemID = input1;
        stringItemName = input2;
        ItemLevel = input3;
        DamageType = input4;
        BoolTwoHand = input5;   
        AutoReuse = input6;
        atkPoint = input7;
        atkSpeed = input8;
        itemOfType = input9;
    }

    ~ItemClass()
    {

    }
}

enum enumItemType
{
    Head, Ring, Weapon
}

