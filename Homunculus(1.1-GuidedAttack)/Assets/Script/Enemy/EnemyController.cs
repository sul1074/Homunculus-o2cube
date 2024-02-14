using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    int randomVelocity;
    private float hpMax = 25;
    public Slider hpBar;
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    static GameManager gameManager;
    public float moveSpeed = 1f;
    float exp;
    private float hp;
    private float def;
    private float evasion;
    private float hpRegen;


    private int value = 0;
    private bool playerDetected = false;
    private Coroutine valueResetCoroutine;
    private bool isRandomVelocityFixed = false;
    private int switchvalue = 0;

    private int jumpTime = 1;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        StartCoroutine(randomMove());

        //스탯 받아오기
        hpMax = GetComponent<EnemyStatus>().hpMax;
        moveSpeed = GetComponent<EnemyStatus>().moveSpeed;
        exp = GetComponent<EnemyStatus>().exp;
        def = GetComponent<EnemyStatus>().defPoint;
        evasion = GetComponent<EnemyStatus>().evasionPoint;
        hpRegen = GetComponent<EnemyStatus>().hpRegen;

        hp = hpMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (!((Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.63f), Vector2.left * randomVelocity, 0.7f, LayerMask.GetMask("Platform"))) || (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.63f), Vector2.left * randomVelocity, 0.7f, LayerMask.GetMask("Wall")))))
            transform.Translate(Vector2.left * randomVelocity * Time.deltaTime);

        Vector2 frontVec = new Vector2(transform.position.x - (randomVelocity * 0.64f), transform.position.y);
        Debug.DrawRay(frontVec, Vector2.down * 2.0f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(frontVec, Vector2.down, 2.0f, LayerMask.GetMask("Platform"));
        RaycastHit2D anotherRayHit = Physics2D.Raycast(frontVec, Vector2.down, 2.0f, LayerMask.GetMask("Wall"));
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 0.1f), Vector2.left * randomVelocity, new Color(0, 0, 1));
        RaycastHit2D rayHit2 = Physics2D.Raycast(transform.position, Vector2.left * randomVelocity, 1.0f, LayerMask.GetMask("Platform"));

        RaycastHit2D checkPlatform1 = Physics2D.Raycast(new Vector2(transform.position.x - (randomVelocity * 0.64f), transform.position.y - 0.64f), Vector2.left * randomVelocity * (-1), 1.28f, LayerMask.GetMask("Wall"));
        RaycastHit2D checkPlatform2 = Physics2D.Raycast(new Vector2(transform.position.x - (randomVelocity * 0.64f), transform.position.y - 0.64f), Vector2.left * randomVelocity * (-1), 1.28f, LayerMask.GetMask("Platform"));
        if ((checkPlatform1.collider != null || checkPlatform2.collider != null) && jumpTime < 1)
        {
            jumpTime = 1;  // 점프 횟수 초기화
        }

        if (rayHit2.collider != null && boxCollider.enabled != false && jumpTime > 0)//jump
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 0.6f, ForceMode2D.Impulse);
            jumpTime -= 1;
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

        RaycastHit2D rayHitf1 = Physics2D.Raycast(transform.position, Vector3.left * randomVelocity, rayfrontDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitf2 = Physics2D.Raycast(transup, Vector3.left * randomVelocity, rayfrontDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitf3 = Physics2D.Raycast(transdown, Vector3.left * randomVelocity, rayfrontDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitb1 = Physics2D.Raycast(transform.position, Vector3.left * randomVelocity * (-1), raybackDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitb2 = Physics2D.Raycast(transup, Vector3.left * randomVelocity * (-1), raybackDistance, LayerMask.GetMask("Player"));
        RaycastHit2D rayHitb3 = Physics2D.Raycast(transdown, Vector3.left * randomVelocity * (-1), raybackDistance, LayerMask.GetMask("Player"));

        if (rayHitf1.collider != null || rayHitf2.collider != null || rayHitf3.collider != null || rayHitb1.collider != null || rayHitb2.collider != null || rayHitb3.collider != null)
        {//if enemy find player
            if (rayHitf1.collider != null || rayHitf2.collider != null || rayHitf3.collider != null)
            {
                //moveSpeed = 2f;
                if (spriteRenderer.flipX == false)
                {
                    switchvalue = 1;

                    if (!((Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.63f), Vector2.left * randomVelocity, 0.7f, LayerMask.GetMask("Platform"))) || (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.63f), Vector2.left * randomVelocity, 0.7f, LayerMask.GetMask("Wall")))))
                        rigid.velocity = new Vector2(Mathf.Abs(randomVelocity) * moveSpeed, rigid.velocity.y);
                }
                else if (spriteRenderer.flipX == true)
                {
                    switchvalue = 2;

                    if (!((Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.63f), Vector2.left * randomVelocity, 0.7f, LayerMask.GetMask("Platform"))) || (Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.63f), Vector2.left * randomVelocity, 0.7f, LayerMask.GetMask("Wall")))))
                        rigid.velocity = new Vector2(Mathf.Abs(randomVelocity) * moveSpeed * (-1), rigid.velocity.y);
                }

                //FixRandomVelocity(true);
            }
            else
            {
                //FixRandomVelocity(false);
            }
            if (rayHitb1.collider != null || rayHitb2.collider != null || rayHitb3.collider != null)
            {// find player back, turn
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
            //moveSpeed = 1f;
            switchvalue = 0;
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
        if (!rayHit.collider && !anotherRayHit && jumpTime > 0)// if enemy go end of field
        {
            Turn();
        }
        if (randomVelocity != 0) spriteRenderer.flipX = randomVelocity != -1;

        //체력 표기
        hpBar.value = hp / hpMax;
        hpRegenOvertime();
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
            else if (switchvalue == 1)
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

    public bool OnDamaged(float playerAtkDamage)
    {
        if (Random.Range(0.0f,100.0f) <= evasion)
        {
            return true;
        }
        hp -= playerAtkDamage - (playerAtkDamage * def * 0.01f);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3, ForceMode2D.Impulse);

        if (hp <= 0)
        {
            die();
            GameManager.globalGameManager.adjustExp(exp);
            return true;
        }
        else return false;
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

    void hpRegenOvertime()
    {
        if (hp < hpMax && hp > 0)
        {
            if (hp + hpRegen * Time.deltaTime > hpMax)
            {
                hp = hpMax;
            }
            else hp += hpRegen * Time.deltaTime;
        }
        return;
    }
}
