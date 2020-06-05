using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IO_Collision : MonoBehaviour
{
    //public Sprite NormalSprite;
    public Material NormalMaterial;
    //public Sprite FrozenSprite;
    public Material FrozenMaterial;
    private SpriteRenderer spriteRenderer;

    public Rigidbody2D rb;
    public bool Frozen;
    private Vector2 StoredLinearVelocity;

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
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void ToggleFrozen()
    {
        if(rb.bodyType == RigidbodyType2D.Dynamic)  // Freeze the object
        {
            StoredLinearVelocity = rb.velocity;

            rb.bodyType = RigidbodyType2D.Static;

            //spriteRenderer.sprite = FrozenSprite;
            spriteRenderer.material = FrozenMaterial;

        }
        else                                        // Un-Freeze the object
        {
            rb.bodyType = RigidbodyType2D.Dynamic;

            rb.velocity = new Vector2(StoredLinearVelocity.x, StoredLinearVelocity.y);

            //spriteRenderer.sprite = NormalSprite;
            spriteRenderer.material = NormalMaterial;

        }
    }
}
