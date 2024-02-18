using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnable() { Invoke("invisible", 5f); }
    private void invisible() { gameObject.SetActive(false); }
}
