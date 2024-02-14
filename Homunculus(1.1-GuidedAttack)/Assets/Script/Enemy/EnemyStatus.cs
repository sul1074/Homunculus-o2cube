using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour
{
    // level & exp
    public int lv;
    public float expMax;
    public float exp;

    // hp & mp status
    public float hpMax;
    public float hp;
    public float hpRegen;
    public float mpMax;
    public float mp;
    public float mpRegen;

    // attack & defend & critical atk
    public float atkPoint;
    public float atkSpeed;
    public float defPoint;
    public float critPoint;
    public float critAtk;

    // evade & speed
    public float evasionPoint;
    public float moveSpeed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
