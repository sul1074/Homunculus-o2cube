using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_atk : MonoBehaviour
{
    Animator anim;
    BoxCollider2D boxCollider;
    private float detectionRange = 1f;
    SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 attackDirection = transform.right;
        if (spriteRenderer.flipX == false) attackDirection *= -1;
        RaycastHit2D hit = Physics2D.Raycast(boxCollider.bounds.center, attackDirection, detectionRange, LayerMask.GetMask("Player"));
        Debug.DrawRay(boxCollider.bounds.center, attackDirection * detectionRange, Color.blue);

        if (hit.collider != null)
        {
            
            // Debug.Log("Player find. Do attack.");
            Attack();
        }
    }

    void Attack()
    {
        anim.SetTrigger("enemy_atk");
    }
}
