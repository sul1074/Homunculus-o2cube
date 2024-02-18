using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    Animator anim;
    public Transform pos;
    public Vector2 boxSize;
    GameObject player;
    private PlayerStatus playerStatus;
    private float critPoint;
    private float critAtk;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.Find("Player");
        playerStatus = player.GetComponent<PlayerStatus>();
        critPoint = playerStatus.getCritPoint();
        critAtk = playerStatus.getCritAtk();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void DoAttack(float damage)
    {
        //크리티컬 데미지, 확률 가져옴
        critPoint = playerStatus.getCritPoint();
        critAtk = playerStatus.getCritAtk();

        if (true /* 현재 무기가 근접 공격일 경우(추후 추가)*/)
        {
            anim.SetTrigger("Attack");
            Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(pos.position, boxSize, 0);
            foreach (Collider2D collider in collider2Ds)
            {
                if (collider.tag == "Enemy")
                {
                    if (Random.Range(0.0f, 100.0f) <= critPoint)
                    {
                        if (collider.GetComponent<EnemyController>() != null)
                        {
                            collider.GetComponent<EnemyController>().OnDamaged(damage * critAtk * 0.01f);
                            Debug.Log("Crit!");
                        }
                        else if (collider.GetComponent<BossController>() != null)
                        {
                            collider.GetComponent<BossController>().OnDamaged(damage * critAtk * 0.01f);
                            Debug.Log("Crit!");
                        }
                    }
                    else
                    {
                        if (collider.GetComponent<EnemyController>() != null)
                        {
                            collider.GetComponent<EnemyController>().OnDamaged(damage);
                        }
                        else if (collider.GetComponent<BossController>() != null)
                        {
                            collider.GetComponent<BossController>().OnDamaged(damage);
                        }
                    }
                    break;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(pos.position, boxSize);
    }

    public void setDirection(bool x) {

        anim.SetBool("isRight", x);
    }
}
