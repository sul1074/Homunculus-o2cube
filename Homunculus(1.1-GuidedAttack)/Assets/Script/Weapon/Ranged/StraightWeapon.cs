using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightWeapon : MonoBehaviour
{
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        checkDirection();
        Destroy(this.gameObject, 3f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
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
            speed = -10f;
        else if (direction.x > 0) // 마우스가 오른쪽을 가리킬 때
            speed = 10f;
    }
}
