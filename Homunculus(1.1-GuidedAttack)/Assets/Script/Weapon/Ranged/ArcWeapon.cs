using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcWeapon : MonoBehaviour
{
    private float launchAngle;
    private float launchForce;

    // Start is called before the first frame update
    void Start()
    {
        launchAngle = 27.5f;
        launchForce = 11f;
        Destroy(gameObject, 4f);
        Fire();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void Fire()   
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        float attackAngle = (moveDirection.x > 0) ? launchAngle : (180f - launchAngle);

        float angleRad = attackAngle * Mathf.Deg2Rad;
        Vector2 launchDir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        GetComponent<Rigidbody2D>().velocity = launchDir * launchForce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().OnDamaged();
            Destroy(this.gameObject);
        }
    }
}
