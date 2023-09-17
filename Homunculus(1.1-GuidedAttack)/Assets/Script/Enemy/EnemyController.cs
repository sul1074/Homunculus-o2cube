using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    int randomVelocity;
    public Slider hp;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;
    public float moveSpeed = 1f;
    private int value = 0;
    private bool playerDetected = false;
    private Coroutine valueResetCoroutine;
    private bool isRandomVelocityFixed = false;
    private int switchvalue = 0;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(randomMove());
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left* randomVelocity * Time.deltaTime);

        Vector2 frontVec = new Vector2(transform.position.x - randomVelocity, transform.position.y);
        Debug.DrawRay(frontVec, Vector2.down*4f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down*4f, 1, LayerMask.GetMask("Platform"));
        Debug.DrawRay(frontVec, Vector2.left*randomVelocity, new Color(0, 0, 1));
        RaycastHit2D rayHit2 = Physics2D.Raycast(transform.position,Vector2.left * randomVelocity, 2f, LayerMask.GetMask("Platform"));
        
        if (rayHit2.collider != null && boxCollider.enabled != false)//jump
        {
           gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.4f, ForceMode2D.Impulse);
        }

        float rayfrontDistance = 10f;
        float raybackDistance = 2f;
        Vector3 transup = transform.position + (Vector3)Vector2.up;
        Vector3 transdown = transform.position + (Vector3)Vector2.down;
        Debug.DrawRay(transform.position, Vector2.left * randomVelocity * rayfrontDistance, new Color(1, 0, 0));
        Debug.DrawRay(transup, Vector2.left * randomVelocity * rayfrontDistance, new Color(1, 0, 0));
        Debug.DrawRay(transdown, Vector2.left * randomVelocity * rayfrontDistance, new Color(1, 0, 0));

        Debug.DrawRay(transform.position, Vector2.left * randomVelocity * raybackDistance * (-1), new Color(1, 0, 0));
        Debug.DrawRay(transup, Vector2.left * randomVelocity * raybackDistance * (-1), new Color(1, 0, 0));
        Debug.DrawRay(transdown, Vector2.left * randomVelocity * raybackDistance * (-1), new Color(1, 0, 0));

        RaycastHit2D rayHitf1 = Physics2D.Raycast(transform.position, Vector3.left* randomVelocity, rayfrontDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitf2 = Physics2D.Raycast(transup, Vector3.left* randomVelocity, rayfrontDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitf3 = Physics2D.Raycast(transdown, Vector3.left* randomVelocity, rayfrontDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitb1 = Physics2D.Raycast(transform.position, Vector3.left* randomVelocity * (-1), raybackDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitb2 = Physics2D.Raycast(transup, Vector3.left* randomVelocity * (-1), raybackDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitb3 = Physics2D.Raycast(transdown, Vector3.left* randomVelocity * (-1), raybackDistance, LayerMask.GetMask("Player"));

        if (rayHitf1.collider != null || rayHitf2.collider != null || rayHitf3.collider != null || rayHitb1.collider != null || rayHitb2.collider != null || rayHitb3.collider != null)
        {//if enemy find player
            if (rayHitf1.collider != null || rayHitf2.collider != null || rayHitf3.collider != null){
                moveSpeed = 2f;
                if (spriteRenderer.flipX == true){
                    switchvalue = 1;
                    rigid.velocity = new Vector2(Mathf.Abs(randomVelocity)* moveSpeed, rigid.velocity.y);
                }
                else if (spriteRenderer.flipX == false){
                    switchvalue = 2;
                    rigid.velocity = new Vector2(Mathf.Abs(randomVelocity)* moveSpeed * (-1), rigid.velocity.y);
                }

                //FixRandomVelocity(true);
            }
            else {
                //FixRandomVelocity(false);
            }
            if  (rayHitb1.collider != null || rayHitb2.collider != null || rayHitb3.collider != null) {// find player back, turn
                Turn();
            }
            CancelInvoke();
            Invoke("randomMove", 5);
            if (!playerDetected)
            {
                playerDetected = true;
                UpdateValue(1);

                // Start or restart the coroutine to reset the value after 5 seconds
                if (valueResetCoroutine != null)
                {
                    StopCoroutine(valueResetCoroutine);
                }
            }
        }
        else
        {
            // Player not detected
            moveSpeed = 1f;
            if (playerDetected)
            {
                playerDetected = false;
                UpdateValue(2);
                if (valueResetCoroutine != null)
                {
                    StopCoroutine(valueResetCoroutine);
                }
                valueResetCoroutine = StartCoroutine(ResetValueAfterDelay(5f));
            }
        }
        if (value == 2)
        {
            Invoke("Turn", 1);
        }
        if (!rayHit.collider)// if enemy go end of field
        {
            //Turn();
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
        if (switchvalue == 0)
        {
            // randomVelocity를 무작위로 설정 (고정되지 않음)
            randomVelocity = Random.Range(-1, 2);
            anim.SetInteger("walkSpeed", randomVelocity);
        }
        else if ( switchvalue == 1)
        {
            // randomVelocity를 원하는 고정값으로 설정
            randomVelocity = -1; // 예시로 1로 고정
            anim.SetInteger("walkSpeed", randomVelocity); // 애니메이션 업데이트
        }
        else if (switchvalue == 2)
        {
            randomVelocity = 1; // 예시로 1로 고정
            anim.SetInteger("walkSpeed", randomVelocity); // 애니메이션 업데이트
        }

        yield return wait;
        }
    }

    public void OnDamaged()
    {
        hp.value -= 0.5f;
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3, ForceMode2D.Impulse);
    }

    void Turn()
    {
        randomVelocity *= -1;
        CancelInvoke();
        Invoke("randomMove", 2);
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

    private void UpdateValue(int newValue)
    {
        value = newValue;
        Debug.Log("Value updated to: " + value);
    }

    private IEnumerator ResetValueAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UpdateValue(0);
    }

    public void FixRandomVelocity(bool fix)
    {
    isRandomVelocityFixed = fix;
    }
}
