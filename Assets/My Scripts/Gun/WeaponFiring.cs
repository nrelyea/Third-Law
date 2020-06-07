using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFiring : MonoBehaviour
{
    public Transform firePoint;


    public GameObject primaryBulletPrefab;
    public float PrimaryBulletSpeed;

    private int FramesBetweenFiring;
    private int UpdatesBeforeReadyToFire = 0;
    public double ShotsPerSecond;


    public GameObject secondaryBulletPrefab;
    public float SecondaryBulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        FramesBetweenFiring = (int)(50 / ShotsPerSecond);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && UpdatesBeforeReadyToFire == 0)
        {
            ShootPrimary();
            UpdatesBeforeReadyToFire = FramesBetweenFiring;
        }
        else if (Input.GetButtonDown("Fire2") && UpdatesBeforeReadyToFire == 0)
        {
            ShootSecondary();
            UpdatesBeforeReadyToFire = FramesBetweenFiring;
        }        
    }

    void FixedUpdate()
    {
        if (UpdatesBeforeReadyToFire > 0)
        {
            UpdatesBeforeReadyToFire--;
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
