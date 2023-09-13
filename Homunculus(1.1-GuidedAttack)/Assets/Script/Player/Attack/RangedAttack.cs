using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{
    private int weaponTypeNum;
    public GameObject straightWeapon;
    public GameObject arcWeapon;
    public GameObject guidedWeapon;

    bool[] equipWeaponType;
    // Start is called before the first frame update
    void Start()
    {
        weaponTypeNum = 3;
        equipWeaponType = new bool[weaponTypeNum];
        for (int i = 0; i < weaponTypeNum; i++)
            equipWeaponType[i] = false;
        equipWeaponType[0] = true; // 초기 상태는 straight weapon
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeRangedWeapon()
    {
        for (int i = 0; i < weaponTypeNum; i++)
        {
            if (equipWeaponType[i] == false) continue;
            equipWeaponType[i] = false;
            
            if(i == weaponTypeNum - 1)
            {
                equipWeaponType[0] = true;
                return;
            }
            equipWeaponType[i + 1] = true;
            return;
        }
    }

    void StraightFire()
    {
        Instantiate(straightWeapon, transform.position, transform.rotation);
    }
    void ArcFire()
    {
        Instantiate(arcWeapon, transform.position, transform.rotation);
    }
    void GuidedFire()
    {
        Instantiate(guidedWeapon, transform.position, transform.rotation);
    }

    public void Fire()
    {
        if (equipWeaponType[0] == true) // straight
            StraightFire();
        else if (equipWeaponType[1] == true) // arc
            ArcFire();
        else if (equipWeaponType[2] == true) // guided
            GuidedFire();
    }
}
