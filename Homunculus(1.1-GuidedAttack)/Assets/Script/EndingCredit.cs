using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingCredit : MonoBehaviour
{
    public float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadMain());
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 move = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y + (moveSpeed * Time.deltaTime));
        gameObject.transform.position = move;
    }

    IEnumerator LoadMain() 
    {
        yield return new WaitForSeconds(10f);
        SceneManager.LoadScene("Main"); 
    }
}
