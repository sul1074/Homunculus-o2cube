using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpPower;
    private int jumpTime;
    private float curTime;
    private float coolTime;
    private bool restrictMoving;

    private bool canDodge = true;
    private float dodgePower = 7.0f;
    private float dodgeTime = 0.2f;
    private float dodgeCooldown = 1f;

    public FadeInOut fadeInOut;
    public GameManager gameManager;
    Attack attack;
    RangedAttack rangedAttack;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigid;
    Animator anim;
    PlayerStatus playerStatus;

    // Start is called before the first frame update
    void Start()
    {
        jumpTime = 2;
        restrictMoving = false;
        attack = GetComponent<Attack>();
        rangedAttack = GetComponentInChildren<RangedAttack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
        gameManager.updateStatus();
        coolTime = playerStatus.getAtkSpeed();
    }

    // Update is called once per frame
    void Update()
    {
        if (restrictMoving || anim.GetBool("isDodging")) return;

        Move();
        Jump();
        Attack();
        ChangeWeapon();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDodge) StartCoroutine(DodgeRoll());
    }

    void SetPlayerImmortal() { gameObject.layer = 9; }
    void SetPlayerMortal() { gameObject.layer = 8; }

    void Move()
    {
        Vector2 moveDir;
        float horizontalInput = Input.GetAxis("Horizontal");

        if (horizontalInput != 0)
        {
            moveDir = new Vector2(horizontalInput, 0);
            
            // 플레이어 이동
            transform.Translate(moveDir * (horizontalInput > 0 ? moveSpeed : -moveSpeed) * Time.deltaTime);

            // 플레이어 방향전환
            transform.eulerAngles = new Vector2(0, horizontalInput > 0 ? 0 : 180);
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }   
    }
    void Jump()
    {
        if (anim.GetBool("isDodging")) return;

        if (Input.GetKeyDown(KeyCode.Space) && jumpTime > 0)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isJumping", true);
            if (jumpTime > 0) jumpTime--;
        }

        // y축 속도와 가속도가 모두 0일 때 공중에 있는 상태가 아님으로 판정
        if (rigid.velocity.y == 0 && rigid.velocity.y / Time.deltaTime == 0)
        {
            anim.SetBool("isJumping", false);
            jumpTime = 2;
        }
    }
    void Attack()
    {
        if (curTime <= 0)   // Attack
        {
            if (Input.GetKeyDown(KeyCode.Z)) // melee attack
            {
                attack.DoAttack(playerStatus.getAtkPoint());
                curTime = coolTime;
            }

            else if (Input.GetKeyDown(KeyCode.X)) // ranged attack
            {
                rangedAttack.Fire();
                curTime = coolTime;
            }
        }
        else
        {
            curTime -= Time.deltaTime;
        }
    }

    IEnumerator DodgeRoll()
    {
        if (anim.GetBool("isJumping")) yield break;

        // 굴러서 나아갈 방향 설정
        Vector2 moveDir;
        float horizontalInput = Input.GetAxis("Horizontal");
        moveDir = new Vector2(horizontalInput, 0).normalized;

        // 구르기
        anim.SetBool("isDodging", true);
        canDodge = false;
        rigid.velocity = new Vector2(moveDir.x * dodgePower, 0f);

        // 구르기 판정 지속시간
        yield return new WaitForSeconds(dodgeTime);
        anim.SetBool("isDodging", false);
  
        // 구르기 쿨타임
        yield return new WaitForSeconds(dodgeCooldown);
        canDodge = true;
    }

    void ChangeWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            rangedAttack.ChangeRangedWeapon();
        }
    }

    public IEnumerator TeleportInDungeon(Vector2 teleportPos) 
    {
        restrictMoving = true;
        fadeInOut.Fade();
        yield return new WaitForSeconds(fadeInOut.GetFadeDurationTime() * 1.25f);
        transform.position = teleportPos;
        yield return new WaitForSeconds(fadeInOut.GetFadeDurationTime() * 1.25f);
        restrictMoving = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if(rigid.velocity.y < 0 && transform.position.y > collision.transform.position.y)
            {
                OnAttack(collision.transform);
            }
            else OnDamaged(collision.transform.position);

        }

        else if(collision.gameObject.tag == "Spring" && transform.position.y > collision.transform.position.y + (collision.transform.localScale.y / 2f))
        {
            rigid.AddForce(Vector2.up * 16, ForceMode2D.Impulse);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Item")
        {
            bool isBronze = collision.gameObject.name.Contains("Bronze");
            bool isSilver= collision.gameObject.name.Contains("Silver");
            bool isGold = collision.gameObject.name.Contains("Gold");

            if(isBronze) gameManager.stagePoint += 50;
            else if(isSilver) gameManager.stagePoint += 100;
            else if(isGold) gameManager.stagePoint += 200;

            collision.gameObject.SetActive(false);
        }

        else if(collision.gameObject.tag == "Finish")
        {
            gameManager.NextStage();
        }
    }

    void OnAttack(Transform enemy)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDamaged(playerStatus.getAtkPoint());
        gameManager.stagePoint += 100;
        rigid.AddForce(Vector2.up * 11, ForceMode2D.Impulse);
    }

    void OnDamaged(Vector2 targetPos)
    {
        gameObject.layer = 9;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
        gameManager.HealthDown(20.0f);

        int physicDirection = transform.position.x - targetPos.x > 0 ? 1 : -1;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(physicDirection, 7), ForceMode2D.Impulse);

        anim.SetTrigger("doDamaged");
        Invoke("OffDamaged", 2);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    public void OnDie()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        gameObject.SetActive(false);
    }
    public void VelocityZero()
    {
        rigid.velocity = Vector2.zero;
    }

    public void expUp(float exp) { gameManager.adjustExp(exp); }
}
