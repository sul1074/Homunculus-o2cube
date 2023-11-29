using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using TMPro;

public class NodeController : MonoBehaviour
{
    private bool isActive = false;
    private int nodeIndex;

    public Camera GetCamera;
    private TMP_Text nodeInfoText;
    

    // Start is called before the first frame update
    void Start()
    {
        string nodeNumberString = System.Text.RegularExpressions.Regex.Replace(gameObject.name, @"[^0-9]", "");

        if (!string.IsNullOrEmpty(nodeNumberString) && int.TryParse(nodeNumberString, out int result))
        {
            nodeIndex = result - 1;
        }

        nodeInfoText = GetComponentInChildren<TMP_Text>();
        if (nodeInfoText != null)
        {
            // 기본적으로 텍스트 숨기기
            nodeInfoText.gameObject.SetActive(false);
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Renderer clickedRenderer = hit.collider.GetComponent<Renderer>();

                isActive = true;
                Color whiteColor = Color.white;
                clickedRenderer.material.color = whiteColor;
              
            }
        }
    }

    void OnMouseEnter()
    {
        ShowNodeInfo();
    }

    void OnMouseExit()
    {
        HideNodeInfo();
    }

    void ShowNodeInfo()
    {
        if (nodeInfoText != null)
        {
            nodeInfoText.gameObject.SetActive(true);
            nodeInfoText.text = $"+{nodeIndex}";
        }
    }

    void HideNodeInfo()
    {
        if (nodeInfoText != null)
        {
            nodeInfoText.gameObject.SetActive(false);
        }
    }
}

public class Node
{
    public int Index { get; }
    public List<Node> SubNodes { get; } = new List<Node>();

    public Node(int index)
    {
        Index = index;
    }
}
