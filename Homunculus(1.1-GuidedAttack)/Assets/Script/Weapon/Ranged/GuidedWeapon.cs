using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidedWeapon : MonoBehaviour
{
    public float speed = 10f;
    private Transform target;
    Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        target = null;
        rigid = GetComponent<Rigidbody2D>();
        Destroy(this.gameObject, 3f);
        rigid.velocity = transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        TrackTarget();
    }
    void FixTarget()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10f, LayerMask.GetMask("Enemy"));
        if (colliders.Length > 0)
        {
            target = colliders[0].transform;
        }
    }

    void TrackTarget()
    {
        FixTarget();
        if(target != null)
        {
            Vector2 dir = (target.position - transform.position).normalized;
            //transform.Translate(dir * speed * Time.deltaTime);
            rigid.velocity = dir * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().OnDamaged(10.0f);
            Destroy(this.gameObject);
        }
    }
}
