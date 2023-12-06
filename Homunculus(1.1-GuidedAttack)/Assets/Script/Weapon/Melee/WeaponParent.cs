using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParent : MonoBehaviour
{
    public SpriteRenderer weaponRenderer, playerRenderer;
    public Vector2 PointerPosition { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        rotateWeapon(direction);
        flipWeaponTowardMousePointer(direction);
        adjustRenderingLayerOrderDependingWeaponAngle();
    }

    void rotateWeapon(Vector2 direction) { transform.right = direction; }

    void flipWeaponTowardMousePointer(Vector2 direction)
    {
        Vector2 scale = transform.localScale;

        // 마우스가 왼쪽을 가리킬 때
        if (direction.x < 0)
        {
            scale.y = -1;
        }
        // 마우스가 오른쪽을 가리킬 때
        else if (direction.x > 0)
        {
            scale.y = 1;
        }
        transform.localScale = scale;
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
