using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public GameObject @object;

    /// <summary>
    /// �̹��� ������Ʈ��
    /// </summary>
    public List<Image> images;

    /// <summary>
    /// ����� �����۰���
    /// </summary>
    public List<int> ints;

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

    // Start is called before the first frame update
    void Start()
    {
        //Ŭ���� �ʱ�ȭ
        itemClass = new ItemClass("3��", 100, 50);
        Debug.Log(itemClass.stringItemName );
        Debug.Log(itemClass.hp);
        Debug.Log(itemClass.mp);


        itemClasses = new List<ItemClass>();
        itemClasses.Add(itemClass);
        Debug.Log(itemClasses[0].stringItemName);
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
        for (int i = 0; i < images.Count; i++)
        {
            if (ints[i] == 0)
            {
                images[i].gameObject.transform.GetChild(1).GetComponent<Image>().sprite = sprites[0];
            }
            else if (ints[i] == 1)
            {
                images[i].transform.GetChild(1).GetComponent<Image>().sprite = sprites[1];
            }
            else
            {
                images[i].transform.GetChild(1).GetComponent<Image>().sprite = sprites[2];
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
        for (int i = 0; i < images.Count; i++)
        {
            images[i].transform.GetChild(1).GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }

        // ���â ����
        for (int i = 0; i < listImageEquip.Count; i++)
        {
            listImageEquip[i].color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }

        if (index != -1)
        {

            images[index].transform.GetChild(1).GetComponent<Image>().color = new Color(0 / 255f, 0 / 255f, 0 / 255f, 255/ 255f);
        }


        if(index != -1)
        {
            GetText.text = "���� �� ���ڴ� " + ints[index];
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
                int t = ints[index];
                ints[index] = ints[inpus];
                ints[inpus] = t;
                index = -1;
            }

        }
        // ������ ��������
        else
        {
            index = -1;
        }

    }
}

public class ItemClass
{
    /// <summary>
    /// ������ �̸�
    /// </summary>
    public string stringItemName;

    public float hp;
    public float mp;


    public ItemClass(string input1, float input2, float input3)
    {
        stringItemName = input1;
        hp = input2;
        mp = input3;
    }

    ~ItemClass()
    {

    }
}

enum enumItemType
{
    Head, Ring, Weapon
}
