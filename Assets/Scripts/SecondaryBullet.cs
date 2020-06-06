using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryBullet : MonoBehaviour
{
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Pulls bullet speed value from Gun object and applies it to the bullet
        GameObject go = GameObject.Find("Gun");
        WeaponFiring cs = go.GetComponent<WeaponFiring>();
        rb.velocity = transform.right * cs.SecondaryBulletSpeed;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.name != "Player" && hitInfo.name != "PrimaryBullet(Clone)" && hitInfo.name != "SecondaryBullet(Clone)")
        {
            //Debug.Log("Secondary Hit " + hitInfo.name);

            IO_Collision io = hitInfo.GetComponent<IO_Collision>();
            if (io != null)
            {
                //Debug.Log("Secondary Hit interaction object!");
                io.ToggleFrozen();
            }

            Destroy(gameObject);
        }
    }
}
