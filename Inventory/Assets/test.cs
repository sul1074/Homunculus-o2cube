using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class test : MonoBehaviour
{
    public GameObject @object;

    /// <summary>
    /// 이미지 컴포넌트들
    /// </summary>
    public List<Image> images;

    /// <summary>
    /// 저장된 아이템값들
    /// </summary>
    public List<int> ints;

    /// <summary>
    /// 표시할 이미지들
    /// </summary>
    public List<Sprite> sprites;

    public Text GetText;

    /// <summary>
    /// 지금 선택한 번호
    /// </summary>
    public int index;

    /// <summary>
    /// 장착한 장비 오브젝트
    /// </summary>
    public List<Image> listImageEquip;

    /// <summary>
    /// 장착한 장비들
    /// </summary>
    public List<int> listIntEquip;
    //클래스 선언
    public ItemClass itemClass;
    public List<ItemClass> itemClasses;

    // Start is called before the first frame update
    void Start()
    {
        //클래스 초기화
        itemClass = new ItemClass("3뚝", 100, 50);
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
        // 일반 인벤토리 아이콘 변경
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

        // 장비 아이템 아이콘 변경
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

        //인벤토리 색상
        for (int i = 0; i < images.Count; i++)
        {
            images[i].transform.GetChild(1).GetComponent<Image>().color = new Color(255 / 255f, 255 / 255f, 255 / 255f, 255 / 255f);
        }

        // 장비창 색상
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
            GetText.text = "지금 고른 숫자는 " + ints[index];
        }
        else
        {
            GetText.text = "숫자를 고르시오";
        }

    }

    /// <summary>
    /// 버튼과 클릭 관련
    /// </summary>
    /// <param name="inpus"></param>
    public void teste(int inpus)
    {
        // 다른거 눌럿을때
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
        // 같은거 눌럿을떄
        else
        {
            index = -1;
        }

    }
}

public class ItemClass
{
    /// <summary>
    /// 아이템 이름
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
