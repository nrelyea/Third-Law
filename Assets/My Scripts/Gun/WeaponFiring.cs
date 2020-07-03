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
        // ignore all gun firing if game is paused
        if (GlobalVars.GameIsPaused) return;

        if (Input.GetKey(GlobalVars.PrimaryFireKey) && UpdatesBeforeReadyToFire == 0)
        {
            ShootPrimary();
            UpdatesBeforeReadyToFire = FramesBetweenFiring;
        }
        else if (Input.GetKeyDown(GlobalVars.SecondaryFireKey) && UpdatesBeforeReadyToFire == 0)
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
