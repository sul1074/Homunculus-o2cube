using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class BossController : MonoBehaviour
{
    //Component
    Rigidbody2D rigid;
    Animator anim;
    SpriteRenderer spriteRenderer;
    BoxCollider2D boxCollider;

    //Gameobject
    public GameObject attack_1;
    public GameObject attack_2;
    private GameObject player;

    //Status
    private float moveSpeed;
    private float atk;
    private float exp;
    private float def;
    private float evasion;
    private float hpRegen;
    private float hpMax;
    private float hp;

    //Script
    private float timer = 0.0f;
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player");

        //Status
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
        timer += Time.deltaTime;
        if (timer >= 5.0f)
        {
            timer = 0.0f;
            int patternRandom = Random.Range(1, 5);
            switch (patternRandom)
            {
                case 1:
                    StartCoroutine(AttackPattern_1());
                    break;
                case 2:
                    StartCoroutine(AttackPattern_2());
                    break;
                case 3:
                    AttackPattern_3();
                    break;
                case 4:
                    StartCoroutine(AttackPattern_4());
                    break;
            }
        }

        HpRegenOvertime();
    }

    public bool OnDamaged(float playerAtkDamage)
    {
        DetectPlayer();
        if (Random.Range(0.0f, 100.0f) <= evasion)
        {
            return true;
        }
        hp -= playerAtkDamage - (playerAtkDamage * def * 0.01f);
        gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * 3, ForceMode2D.Impulse);

        if (hp <= 0)
        {
            Die();
            GameManager.globalGameManager.adjustExp(exp);
            return true;
        }
        else return false;
    }

    void HpRegenOvertime()
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

    void Die()
    {
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);

        spriteRenderer.flipY = true;

        boxCollider.enabled = false;

        Destroy(gameObject, 3f);
    }

    //Animations
    void DetectPlayer()
    {
        anim.SetTrigger("detectPlayer");
    }

    //Attack
    IEnumerator AttackPattern_1()
    {
        GameObject attack;
        for (int i = 0; i < 5; i++)
        {
            attack = Instantiate(attack_1);
            attack.transform.position = gameObject.transform.position + new Vector3(-5.0f -3.5f * i, 0.0f, 0.0f);
            attack.GetComponent<BossAttack>().pattenNum = 0;
            attack = null;
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator AttackPattern_2()
    {
        GameObject attack;
        for (int i = 0; i < 5; i++)
        {
            attack = Instantiate(attack_1);
            attack.transform.position = gameObject.transform.position + new Vector3(-2.5f - 5.0f * i, 0.0f, 0.0f);
            attack.GetComponent<BossAttack>().pattenNum = 1;
            attack = null;
        }
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < 5; i++)
        {
            attack = Instantiate(attack_1);
            attack.transform.position = gameObject.transform.position + new Vector3(- 5.0f * i, 0.0f, 0.0f);
            attack.GetComponent<BossAttack>().pattenNum = 1;
            attack = null;
        }
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < 5; i++)
        {
            attack = Instantiate(attack_1);
            attack.transform.position = gameObject.transform.position + new Vector3(-2.5f - 5.0f * i, 0.0f, 0.0f);
            attack.GetComponent<BossAttack>().pattenNum = 1;
            attack = null;
        }
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < 5; i++)
        {
            attack = Instantiate(attack_1);
            attack.transform.position = gameObject.transform.position + new Vector3(-5.0f * i, 0.0f, 0.0f);
            attack.GetComponent<BossAttack>().pattenNum = 1;
            attack = null;
        }
    }

    void AttackPattern_3()
    {
        GameObject attack;
        attack = Instantiate(attack_1);
        attack.GetComponent<BossAttack>().pattenNum = 2;
        attack = null;
        attack = Instantiate(attack_1);
        attack.GetComponent<BossAttack>().pattenNum = 3;
    }

    IEnumerator AttackPattern_4()
    {
        for (int i = 0; i < 7; i++)
        {
            GameObject attack;
            attack = Instantiate(attack_1);
            attack.GetComponent<BossAttack>().pattenNum = 4;
            attack = null;
            yield return new WaitForSeconds(0.3f);
        }
    }
}