using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_Collision : MonoBehaviour
{
    private const float MassVelocityModifier = 15;

    //public Sprite NormalSprite;
    public Material NormalMaterial;
    //public Sprite FrozenSprite;
    public Material FrozenMaterial;
    private SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;
    private Transform velocityIndicator;

    public bool Frozen;
    private Vector2 StoredLinearVelocity;
    private float StoredAngularVelocity;

    public float MaximumVelocity;

    //public bool Rideable;
    //private HashSet<string> ConnectedObjects;


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == null || !Frozen)
        {
            //spriteRenderer.sprite = NormalSprite;
            spriteRenderer.material = NormalMaterial;
        }
        else
        {
            //spriteRenderer.sprite = FrozenSprite;
            spriteRenderer.material = FrozenMaterial;
        }

        StoredLinearVelocity = new Vector2(0.0f, 0.0f);
        StoredAngularVelocity = 0.0f;

        velocityIndicator = this.gameObject.transform.GetChild(0);

        //ConnectedObjects = new HashSet<string> { };
    }

    // toggle whether object is frozen and make necessary changes to alter the state fully
    // called by SecondaryBullet upon collision with IO
    public void ToggleFrozen()
    {
        Frozen = !Frozen;

        if (Frozen)                                 
        {
            // Freeze the object

            StoredLinearVelocity = rb.velocity;
            StoredAngularVelocity = rb.angularVelocity;

            rb.bodyType = RigidbodyType2D.Static;

            //spriteRenderer.sprite = FrozenSprite;
            spriteRenderer.material = FrozenMaterial;


            // Render Velocity Indicator

            //float angle = IndicatorAngle(StoredLinearVelocity);
            UpdateIndicator();        

        }
        else                                        
        {
            // Un-Freeze the object

            rb.bodyType = RigidbodyType2D.Dynamic;

            rb.velocity = new Vector2(StoredLinearVelocity.x, StoredLinearVelocity.y);
            rb.angularVelocity = StoredAngularVelocity;

            //spriteRenderer.sprite = NormalSprite;
            spriteRenderer.material = NormalMaterial;

            UpdateIndicator();
        }
    }

    // update indicator arrow to reflect current / changes to store velocity
    public void UpdateIndicator()
    {
        SpriteRenderer indicatorSR = velocityIndicator.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        if (Frozen)
        {
            float angle = IndicatorAngle(StoredLinearVelocity);
            if (angle != 420.0f)    // if frozen object has movement stored, show the arror with appropriate angle and scale
            {
                velocityIndicator.eulerAngles = new Vector3(0.0f, 0.0f, angle);

                float magnitude = StoredLinearVelocity.magnitude;
                velocityIndicator.localScale = new Vector3(0.5f, magnitude / 10, 1.0f);

                indicatorSR.enabled = true;
            }          
        }
        else
        {
            indicatorSR.enabled = false;
        }
    }

    // Calculate angle of indicator angle
    public float IndicatorAngle(Vector2 vel)
    {
        float x = vel.x;
        float y = vel.y;

        if(x < 0.01f && x > -0.01f)     // if x velocity is 0, handle potential divide by zero cases
        {
            if (y > 0.01f) return 0.0f;
            else if (y < -0.01f) return 180.0f;
            else return 420.0f;                     // if object isn't moving, return special number to be interpreted as "no movement"
        }

        double angle = Math.Atan2(y, x) * (180 / Math.PI) - 90;

        return (float)angle;
    }

    // apply force to this object to either alter its velocity immediately or alter the stored value while frozen
    // called by PrimaryBullet upon collision with IO
    public void ApplyForce(Vector2 incomingVelocity)
    {
        float xModifier = incomingVelocity.x / rb.mass / MassVelocityModifier;
        float yModifier = incomingVelocity.y / rb.mass / MassVelocityModifier;

        if (Frozen)                                 // If object is frozen, modify its stored velocity
        {
            StoredLinearVelocity = new Vector2(StoredLinearVelocity.x + xModifier, StoredLinearVelocity.y + yModifier);

            float magnitude = StoredLinearVelocity.magnitude;

            if (magnitude > MaximumVelocity)       // If maximum magnitude of velocity has been exceeded, adjust velocity proportionally
            {
                float StoredToMaxRatio = magnitude / MaximumVelocity;
                Vector2 adjustedVelocity = new Vector2(StoredLinearVelocity.x / StoredToMaxRatio, StoredLinearVelocity.y / StoredToMaxRatio);
                StoredLinearVelocity = adjustedVelocity;
            }

            UpdateIndicator();
        }
        else                                        // If object isn't frozen, modify its velocity
        {
            rb.velocity = new Vector2(rb.velocity.x + xModifier, rb.velocity.y + yModifier);
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    // BELOW IS FAILED ATTEMPT AT RIDEABLE OBJECT IMPLEMENTATION, WILL COME BACK TO //
    //////////////////////////////////////////////////////////////////////////////////


    //private void OnCollisionEnter2D(Collision2D collisionInfo)
    //{
    //    if (Rideable)
    //    {
    //        string colliderName = collisionInfo.gameObject.name;
    //        IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();
    //        if(colliderName == "Player" || io != null)
    //        {
    //            if (!IsBullet(collisionInfo))
    //            {
    //                Debug.Log("Mounting " + colliderName + " on " + gameObject.name);

    //                //var middleManObj = new GameObject();
    //                //middleManObj.name = collisionInfo.gameObject.name + " Helper";
    //                //collisionInfo.collider.transform.SetParent(middleManObj.transform);
    //                //middleManObj.transform.SetParent(transform);

    //                ConnectedObjects.Add(colliderName);
    //            }
    //        }           
    //    }
    //}

    //private void OnCollisionStay2D(Collision2D collisionInfo)
    //{
    //    if (Rideable)
    //    {
    //        if (!IsBullet(collisionInfo) && ConnectedObjects.Contains(collisionInfo.gameObject.name))
    //        {
    //            //Debug.Log("Updating " + collisionInfo.gameObject.name + " in respect to " + gameObject.name);

    //            //Rigidbody2D riderRB = collisionInfo.gameObject.GetComponent<Rigidbody2D>();
    //            //Debug.Log("Object mass: " + rb.mass + "  Rider mass: " + riderRB.mass);
    //            //Debug.Log("Velocity of " + riderRB.velocity.x + ", " + riderRB.velocity.y + " being set to " + rb.velocity.x + ", " + riderRB.velocity.y);
    //            //riderRB.velocity = new Vector2(rb.velocity.x, riderRB.velocity.y);
    //            //riderRB.velocity = new Vector2(rb.velocity.x + xModifier, rb.velocity.y + yModifier);
    //        }
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collisionInfo)
    //{
    //    if (Rideable)
    //    {
    //        string colliderName = collisionInfo.gameObject.name;
    //        IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();
    //        if (colliderName == "Player" || io != null)
    //        {
    //            if (!IsBullet(collisionInfo))
    //            {
    //                if (collisionInfo.gameObject.transform.parent != null)
    //                {
    //                    Debug.Log("Disconnecting " + colliderName + " from " + gameObject.name);

    //                    ConnectedObjects.Remove(colliderName);

    //                    //var middleManObj = collisionInfo.collider.transform.parent.gameObject;
    //                    //middleManObj.transform.SetParent(null);
    //                    //collisionInfo.collider.transform.SetParent(null);
    //                    //Destroy(middleManObj);
    //                }
    //            }
    //        }
    //    }
    //}

    private bool IsBullet(Collision2D col)
    {
        return (col.gameObject.name == "PrimaryBullet(Clone)" || col.gameObject.name == "SecondaryBullet(Clone)");
    }
}
