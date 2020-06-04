using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFiring : MonoBehaviour
{
    public Transform firePoint;


    public GameObject primaryBulletPrefab;
    public float PrimaryBulletSpeed;

    public GameObject secondaryBulletPrefab;
    public float SecondaryBulletSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootPrimary();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ShootSecondary();
        }
    }

    void ShootPrimary()
    {
        Instantiate(primaryBulletPrefab, firePoint.position, firePoint.rotation);
    }

    void ShootSecondary()
    {
        Instantiate(secondaryBulletPrefab, firePoint.position, firePoint.rotation);
    }
}
