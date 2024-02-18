using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    bool start = true;
    bool isPlayerTracking = true;
    public int pattenNum = 0;
    GameObject player;
    Animator animator;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");   
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (pattenNum)
        {
            case 0:
                if (start)
                { //0.3,2
                    boxCollider.size = new Vector2(0.3f, 2.0f);
                    animator.SetInteger("patternNum", 0);
                    start = false;
                    StartCoroutine(DestroyObject(0.8f));
                    StartCoroutine(ColorChange(0.37f));
                }
                break;
            case 1:
                if (start)
                {
                    boxCollider.size = new Vector2(0.3f, 2.0f);
                    animator.SetInteger("patternNum", 1);
                    start = false;
                    StartCoroutine(DestroyObject(0.8f));
                    StartCoroutine(ColorChange(0.37f));
                }
                break;
            case 2:
                if (start)
                {
                    //boxCollider.size = new Vector2(0.5f,32.0f);
                    animator.SetInteger("patternNum", 3);
                    spriteRenderer.color = new Color(1, 0.2f, 0.1f, 0.3f);
                    transform.localScale = new Vector3(10.0f, 80.0f, 10.0f);
                    start = false;
                    StartCoroutine(DestroyObject(4.8f));
                    StartCoroutine(DisablePlayerTracking(2.0f));
                    StartCoroutine(ColorChange(2.5f));
                }
                if (isPlayerTracking)
                {
                    transform.position = player.transform.position;
                }                break;
            case 3:
                if (start)
                {
                    animator.SetInteger("patternNum", 3);
                    spriteRenderer.color = new Color(1, 0.2f, 0.1f, 0.3f);
                    transform.localScale = new Vector3(10.0f, 80.0f, 10.0f);
                    transform.rotation = Quaternion.Euler(0, 0, 90.0f);
                    start = false;
                    StartCoroutine(DestroyObject(4.8f));
                    StartCoroutine(DisablePlayerTracking(2.0f));
                    StartCoroutine(ColorChange(2.5f));
                }
                if (isPlayerTracking)
                {
                    transform.position = player.transform.position;
                }
                break;
            case 4:
                if (start)
                {
                    animator.SetInteger("patternNum", 3);
                    spriteRenderer.color = new Color(1, 0.2f, 0.1f, 0.3f);
                    transform.localScale = new Vector3(10.0f, 80.0f, 10.0f);
                    start = false;
                    StartCoroutine(DestroyObject(2.0f));
                    transform.position = player.transform.position;
                    StartCoroutine(ColorChange(1.0f));
                }
                break;
        }
        
    }

    IEnumerator DestroyObject(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    IEnumerator DisablePlayerTracking(float time)
    {
        yield return new WaitForSeconds(time);
        isPlayerTracking = false;
    }
    IEnumerator ColorChange(float time)
    {
        yield return new WaitForSeconds(time);
        spriteRenderer.color = new Color(1, 1.0f, 1.0f, 1.0f);
        gameObject.tag = "Enemy";
    }

}
