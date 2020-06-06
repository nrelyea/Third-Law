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

        velocityIndicator = this.gameObject.transform.GetChild(0);
    }

    // Update is called once per frame
    void Update()
    {
        

    }

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

    public void UpdateIndicator()
    {
        SpriteRenderer indicatorSR = velocityIndicator.GetComponent(typeof(SpriteRenderer)) as SpriteRenderer;

        if (Frozen)
        {
            float angle = IndicatorAngle(StoredLinearVelocity);
            if (angle != 420.0f)    // if frozen object has movement stored, show the arror with appropriate angle and scale
            {
                velocityIndicator.eulerAngles = new Vector3(0.0f, 0.0f, angle);

                float magnitude = Magnitude(StoredLinearVelocity);
                velocityIndicator.localScale = new Vector3(0.5f, magnitude / 10, 1.0f);

                indicatorSR.enabled = true;
            }

            
        }
        else
        {
            indicatorSR.enabled = false;
        }
    }

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

        //Debug.Log("velocity:  x: " + x + "  y: " + y);

        double angle = Math.Atan2(y, x) * (180 / Math.PI) - 90;

        //Debug.Log("angle: " + angle);

        return (float)angle;
    }

    public void ApplyForce(Vector2 incomingVelocity)
    {
        float xModifier = incomingVelocity.x / rb.mass / MassVelocityModifier;
        float yModifier = incomingVelocity.y / rb.mass / MassVelocityModifier;

        if (Frozen)                                 // If object is frozen, modify its stored velocity
        {
            StoredLinearVelocity = new Vector2(StoredLinearVelocity.x + xModifier, StoredLinearVelocity.y + yModifier);

            float magnitude = Magnitude(StoredLinearVelocity);

            if (magnitude > MaximumVelocity)
            {
                Debug.Log("Max velocity exceeded!");

                float StoredToMaxRatio = magnitude / MaximumVelocity;
                Vector2 adjustedVelocity = new Vector2(StoredLinearVelocity.x / StoredToMaxRatio, StoredLinearVelocity.y / StoredToMaxRatio);
                StoredLinearVelocity = adjustedVelocity;

                Debug.Log("New magnitude: " + Magnitude(StoredLinearVelocity));

            }

            UpdateIndicator();
        }
        else                                        // If object isn't frozen, modify its velocity
        {
            rb.velocity = new Vector2(rb.velocity.x + xModifier, rb.velocity.y + yModifier);
        }
    }

    public float Magnitude(Vector2 v)
    {
        double velX = Convert.ToDouble(v.x);
        double velY = Convert.ToDouble(v.y);

        return (float)Math.Sqrt(Math.Pow(velX, 2) + Math.Pow(velY, 2));
    }
}
