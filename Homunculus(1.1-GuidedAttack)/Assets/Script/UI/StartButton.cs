using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public LoadingSceneManager loadingSceneManager;
    public GameObject loadingCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        loadingCanvas.SetActive(true);
        loadingSceneManager.load(1);
    }
}
