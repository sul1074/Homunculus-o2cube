using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcWeapon : MonoBehaviour
{
    private float launchAngle;
    private float launchForce;
    private float inputDirection = 0f;

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
        checkDirection();
        Vector2 moveDirection = new Vector2(inputDirection, 0);
        float attackAngle = (moveDirection.x > 0) ? launchAngle : (180f - launchAngle);

        float angleRad = attackAngle * Mathf.Deg2Rad;
        Vector2 launchDir = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
        GetComponent<Rigidbody2D>().velocity = launchDir * launchForce;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyController>().OnDamaged(10.0f);
            Destroy(this.gameObject);
        }
    }

    private Vector2 getPointerInput()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    private void checkDirection()
    {
        Vector2 mousePointer = getPointerInput();
        Vector2 direction = (mousePointer - (Vector2)transform.position).normalized;

        // 마우스가 왼쪽을 가리킬 때
        if (direction.x < 0)
            inputDirection = -1f;
        else if (direction.x > 0) // 마우스가 오른쪽을 가리킬 때
            inputDirection = 1f;
    }
}
