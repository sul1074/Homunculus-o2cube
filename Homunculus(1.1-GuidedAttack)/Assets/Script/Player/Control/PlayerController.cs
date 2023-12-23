using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public TalkPrinter talkPrinter;
    private GameObject interactingNPC = null;
    public GameObject player;
    public Transform attackRangeBoxTransform;
    bool playertalking;

    private Attack attack;
    private RangedAttack rangedAttack;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigid;
    private Animator anim;
    private PlayerStatus playerStatus;
    private WeaponParent weaponParent;

    // Start is called before the first frame update
    void Start()
    {
        jumpTime = 2;
        restrictMoving = false;
        attack = GetComponentInChildren<Attack>();
        rangedAttack = GetComponentInChildren<RangedAttack>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStatus = GetComponent<PlayerStatus>();
        weaponParent = GetComponentInChildren<WeaponParent>();
        gameManager.updateStatus();
        coolTime = playerStatus.getAtkSpeed();
        playertalking = true;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePointer = getPointerInput();
        if (restrictMoving || anim.GetBool("isDodging")) return;

        Move();
        Jump();
        RotateWeapon(mousePointer);
        FlipPlayerAndAttackRangeTowardsMouse(mousePointer);
        Attack();
        ChangeWeapon();
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDodge && !talkPrinter.isTalking) StartCoroutine(DodgeRoll());
        Talk();

    }

    void SetPlayerImmortal() { gameObject.layer = 9; }
    void SetPlayerMortal() { gameObject.layer = 8; }

    void Move()
    {
        Vector2 moveDir;
        float horizontalInput = talkPrinter.isTalking ? 0 : Input.GetAxis("Horizontal");

        //if (horizontalInput != 0)
        // 레이를 쏴서 'Platform'이 앞에 있으면 이동하지 못함
        if (horizontalInput < 0.0f && !(Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -1 * transform.right, 0.4625f, LayerMask.GetMask("Platform"))) || horizontalInput > 0.0f && !(Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.right, 0.4625f, LayerMask.GetMask("Platform"))))
        {
            moveDir = new Vector2(horizontalInput, 0);

            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
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

        if (Input.GetKeyDown(KeyCode.Space) && jumpTime > 0 && !talkPrinter.isTalking)
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
            if (Input.GetMouseButtonDown(0)) // melee attack
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

    void Talk()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactingNPC != null)
        {
            if (playertalking == true)
            {
                talkPrinter.Talk(interactingNPC);
                playertalking = false;
            }
            else if (playertalking == false)
            {
                talkPrinter.Talk(player);
                playertalking = true;
            }
        }
    }

    private void RotateWeapon(Vector2 dir)
    {
        if (weaponParent == null) return;
        weaponParent.PointerPosition = dir;
    }
    private void FlipPlayerAndAttackRangeTowardsMouse(Vector2 dir)
    {
        // 플레이어의 현재 위치에서 마우스 포인터의 위치를 향하는 벡터를 계산
        Vector2 direction = dir - (Vector2)transform.position;

        Vector3 attackBoxLocalPosition = attackRangeBoxTransform.localPosition;
        attackBoxLocalPosition.x = -attackBoxLocalPosition.x;

        // 플레이어 방향전환
        if (direction.x > 0)
        {
            // 캐릭터 스프라이트 반대로 전환
            spriteRenderer.flipX = false;

            // 근접공격 범위 박스 위치 반대로 전환
            if (attackBoxLocalPosition.x < 0) return;
            attackRangeBoxTransform.localPosition = attackBoxLocalPosition;
        }
        else
        {
            // 캐릭터 스프라이트 반대로 전환
            spriteRenderer.flipX = true;

            // 근접공격 범위 박스 위치 반대로 전환
            if (attackBoxLocalPosition.x > 0) return;
            attackRangeBoxTransform.localPosition = attackBoxLocalPosition;
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
            OnDamaged(collision.transform.position);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Npc")
        {
            interactingNPC = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Exit Door" && Input.GetKey(KeyCode.F))
        {
            SceneManager.LoadScene("TalkDemo");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == interactingNPC)
        {
            interactingNPC = null;
        }
    }

    void OnAttack(Transform enemy)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDamaged(playerStatus.getAtkPoint());
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

    private Vector2 getPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }

    public void expUp(float exp) { gameManager.adjustExp(exp); }
}
