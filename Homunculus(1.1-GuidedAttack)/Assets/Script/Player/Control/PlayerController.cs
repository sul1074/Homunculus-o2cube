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
    private float evasionPoint;
    private float hpRegenPoint;
    private bool isAlive;

    private bool canDodge = true;
    private float dodgePower = 7.0f;
    private float dodgeTime = 0.2f;
    private float dodgeCooldown = 1f;

    public FadeInOut fadeInOut;
    public GameManager gameManager;
    public TalkPrinter talkPrinter;
    private GameObject interactingNPC = null;
    private GameObject interactingITEM = null;
    public GameObject player;
    public Transform attackRangeBoxTransform;

    public GameObject myWeapon;
    private bool canRangedAttack = false;
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
        isAlive = true;
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
        evasionPoint = playerStatus.getEvasionPoint();
        moveSpeed = playerStatus.getMoveSpeed();
        hpRegenPoint = playerStatus.getHpRegen();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;

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
        hpRegen();
    }

    void SetPlayerImmortal() { gameObject.layer = 9; }
    void SetPlayerMortal() { gameObject.layer = 8; }

    void Move()
    {
        Vector2 moveDir;
        float horizontalInput = talkPrinter.isTalking ? 0 : Input.GetAxis("Horizontal");

        Debug.DrawRay(new Vector3(transform.position.x, transform.position.y, transform.position.z), -0.4625f * transform.right, new Color(1, 0, 0));

        //if (horizontalInput != 0)
        // 레이를 쏴서 벽이 앞에 있으면 이동하지 못함
        if ((horizontalInput < 0.0f && !(Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -1 * transform.right, 0.4625f, LayerMask.GetMask("Platform"))) && !(Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), -1 * transform.right, 0.4625f, LayerMask.GetMask("Wall")))) 
            || (horizontalInput > 0.0f && !(Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.right, 0.4625f, LayerMask.GetMask("Platform"))) && !(Physics2D.Raycast(new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.right, 0.4625f, LayerMask.GetMask("Wall")))))
        {
            moveDir = new Vector2(horizontalInput, 0);

            transform.Translate(moveDir * moveSpeed * Time.deltaTime);
            if(!anim.GetBool("isJumping"))
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

        float randomVelocity = spriteRenderer.flipX ? 1.0f : -1.0f;
        RaycastHit2D checkPlatform1 = Physics2D.Raycast(new Vector2(transform.position.x - (randomVelocity * 0.408f), transform.position.y - 0.90513f), Vector2.left * randomVelocity * (-1), 0.816f, LayerMask.GetMask("Wall"));
        RaycastHit2D checkPlatform2 = Physics2D.Raycast(new Vector2(transform.position.x - (randomVelocity * 0.408f), transform.position.y - 0.90513f), Vector2.left * randomVelocity * (-1), 0.816f, LayerMask.GetMask("Platform"));
        if ((checkPlatform1.collider != null || checkPlatform2.collider != null) && jumpTime < 2)
        {
            anim.SetBool("isJumping", false);
            jumpTime = 2;
        }

        if (Input.GetKeyDown(KeyCode.Space) && jumpTime > 0 && !talkPrinter.isTalking)
        {
            rigid.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
            anim.SetBool("isWalking", false);
            anim.SetBool("isJumping", true);
            if (jumpTime > 0) jumpTime--;
        }
    }
    void Attack()
    {
        if (curTime <= 0)   // Attack
        {
            if (Input.GetMouseButtonDown(0)) // melee attack
            {
                if (myWeapon.activeSelf == false) return;

                attack.DoAttack(playerStatus.getAtkPoint());
                curTime = coolTime;
            }
            else if (Input.GetKeyDown(KeyCode.X)) // ranged attack
            {
                if (!canRangedAttack) return;

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
            talkPrinter.Talk(interactingNPC);
        }
    }
    public void forcedTalk(GameObject interactingNPC) { talkPrinter.Talk(interactingNPC); }

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

            rangedAttack.changePosition(1.0f);
        }
        else
        {
            // 캐릭터 스프라이트 반대로 전환
            spriteRenderer.flipX = true;

            // 근접공격 범위 박스 위치 반대로 전환
            if (attackBoxLocalPosition.x > 0) return;
            attackRangeBoxTransform.localPosition = attackBoxLocalPosition;

            rangedAttack.changePosition(-1.0f);
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
        anim.SetBool("isJumping", false);
        jumpTime = 2;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position, collision.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Npc")
        {
            interactingNPC = collision.gameObject;
        }

        if (collision.gameObject.tag == "Item")
        {
            interactingITEM = collision.gameObject;
        }

        if (collision.gameObject.tag == "Enemy")
        {
            OnDamaged(collision.transform.position, collision.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Item" && Input.GetKey(KeyCode.E))
        {
            if (interactingITEM != null) {
                if (interactingITEM.gameObject.name == "Sword") {
                    myWeapon.SetActive(true);
                    attack = GetComponentInChildren<Attack>();
                }
                else if (interactingITEM.gameObject.name == "Throwing Stars") {
                    canRangedAttack = true;
                }
                    

                Destroy(interactingITEM.gameObject);
                interactingITEM = null;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == interactingNPC)
        {
            interactingNPC = null;
        }

        if (collision.gameObject == interactingITEM)
        {
            interactingITEM = null;
        }
    }

    void OnAttack(Transform enemy)
    {
        EnemyController enemyController = enemy.GetComponent<EnemyController>();
        enemyController.OnDamaged(playerStatus.getAtkPoint());
        rigid.AddForce(Vector2.up * 11, ForceMode2D.Impulse);
    }

    void OnDamaged(Vector2 targetPos,GameObject target)
    {
        gameObject.layer = 9;
        float enemyCritPoint = target.GetComponent<EnemyStatus>().critPoint;
        float enemyCritAtk = target.GetComponent<EnemyStatus>().critAtk;
        Debug.Log(target.GetComponent<EnemyStatus>().hpMax);

        if (Random.Range(0.0f, 100.0f) <= evasionPoint)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.4f);
            Debug.Log("회피!");
        }
        else
        {
            if (Random.Range(0.0f,100.0f) <= enemyCritPoint)
            {
                gameManager.HealthDown(target.GetComponent<EnemyStatus>().atkPoint*enemyCritAtk * 0.01f);
                anim.SetTrigger("doDamaged");
                int physicDirection = transform.position.x - targetPos.x > 0 ? 1 : -1;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(physicDirection * 3, 3), ForceMode2D.Impulse);
            }
            else
            {
                gameManager.HealthDown(target.GetComponent<EnemyStatus>().atkPoint);
                anim.SetTrigger("doDamaged");
                int physicDirection = transform.position.x - targetPos.x > 0 ? 1 : -1;
                gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(physicDirection * 3, 3), ForceMode2D.Impulse);
            }
        }

        Invoke("OffDamaged", 2);
    }

    void OffDamaged()
    {
        gameObject.layer = 8;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }
    public void OnDie() { StartCoroutine(die()); }
    IEnumerator die()
    {
        isAlive = false;
        myWeapon.SetActive(false);
        spriteRenderer.color = new Color(1, 1, 1, 1);
        gameObject.GetComponent<Rigidbody2D>().simulated = false;
        gameObject.GetComponent<BoxCollider2D>().enabled = false;
        anim.SetTrigger("isDead");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Main");
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

    private void hpRegen()
    {
        gameManager.HealthUp(hpRegenPoint * Time.deltaTime);
    }
}
