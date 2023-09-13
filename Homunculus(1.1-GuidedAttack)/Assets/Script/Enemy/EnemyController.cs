using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    int randomVelocity;
    public Slider hp;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        StartCoroutine(randomMove());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left* randomVelocity * Time.deltaTime);

        Vector2 frontVec = new Vector2(transform.position.x - randomVelocity, transform.position.y);
        Debug.DrawRay(frontVec, Vector2.down, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 1, LayerMask.GetMask("Platform"));

        if (!rayHit.collider)
        {
            randomVelocity *= -1;
        }

        if (randomVelocity != 0) spriteRenderer.flipX = randomVelocity == -1;

        if (hp.value <= 0) die();
    }

    IEnumerator randomMove()
    {
        float interval = Random.Range(2f, 5f);
        WaitForSeconds wait = new WaitForSeconds(interval);
        while (true)
        {       
            randomVelocity = Random.Range(-1, 2);    
            anim.SetInteger("walkSpeed", randomVelocity);     

            yield return wait;
        }
    }

    public void OnDamaged()
    {
        hp.value -= 0.5f;
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3, ForceMode2D.Impulse);
    }

    void die()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        boxCollider.enabled = false;

        Destroy(gameObject, 3f);
    }

    void DeActive()
    {
        gameObject.SetActive(false);
    }
}
