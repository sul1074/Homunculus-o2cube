using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public Attack attack;
    public SpriteRenderer weaponRenderer, playerRenderer;
    public Vector2 PointerPosition { get; set; }

    public Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!attack.gameObject.activeSelf) return;

        Vector2 direction = (PointerPosition - (Vector2)playerPos.position);
        //rotateWeapon(direction);
        flipWeaponTowardMousePointer(direction);
        adjustRenderingLayerOrderDependingWeaponAngle();
    }

    void rotateWeapon(Vector2 direction) { transform.right = direction; }

    void flipWeaponTowardMousePointer(Vector2 direction)
    {
        Vector2 pos = transform.localPosition;

        // 마우스가 왼쪽을 가리킬 때
        if (direction.x < 0)
        {
            pos.x = -0.29f;
            transform.rotation = Quaternion.Euler(0, 0, 23.02f);
            weaponRenderer.flipX = true;
            attack.setDirection(false);
        }
        // 마우스가 오른쪽을 가리킬 때
        else if (direction.x > 0)
        {
            pos.x = 0.29f;
            transform.rotation = Quaternion.Euler(0, 0, -23.02f);
            weaponRenderer.flipX = false;
            attack.setDirection(true);
        }
        transform.localPosition = pos;
    }

    void adjustRenderingLayerOrderDependingWeaponAngle()
    {
        if (transform.eulerAngles.z > 0 && transform.eulerAngles.z < 180)
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder - 1;
        }

        else
        {
            weaponRenderer.sortingOrder = playerRenderer.sortingOrder + 1;
        }
    }
}
