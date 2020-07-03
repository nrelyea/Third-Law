using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool Engaged = false;
    
    public float TimeBetweenShots;
    private float TimeSinceFired;

    public float TimeToChargeShot;
    private float TimeSincePlayerDetected;

    private bool BeamsVisible = false;

    public Material LineMaterial;
    public Color BeamColor;
    private LineRenderer Line;
    private List<LineRenderer> lines;
    private float BeamWidth = 0.15f;
    public float BeamFadeTime;
    public float FadeBeamWidening;

    public int NumLines;

    private float BeamAngle;
    
    // Start is called before the first frame update
    void Start()
    {
        // remove range indicator
        Transform rangeIndicatorTransform = gameObject.transform.Find("RangeIndicator");
        Destroy(rangeIndicatorTransform.gameObject);

        TimeSinceFired = TimeBetweenShots;
        lines = new List<LineRenderer> { };

        // Calculate the angle between each beam that will be projected from enemy
        BeamAngle = 360.0f / NumLines;
    }

    void FixedUpdate()
    {
        if (BeamsVisible)
        {
            // If beams are visible, fade them out based on given fade time
            float currentAlpha = lines[0].startColor.a;
            if(currentAlpha > 0)
            {
                float alphaToDecrement = Time.fixedDeltaTime / BeamFadeTime;
                Color newColor = new Color(BeamColor.r, BeamColor.g, BeamColor.b, lines[0].startColor.a - alphaToDecrement);
                foreach (LineRenderer line in lines)
                {
                    // apply new color with altered alpha
                    line.SetColors(newColor, newColor);
                    
                    // adjust width slightly to conceal some visual artifacts that result from above step
                    line.startWidth += alphaToDecrement / FadeBeamWidening;
                    line.endWidth += alphaToDecrement / FadeBeamWidening;
                }
            }
            else
            {
                // if beams have been fully faded, indicate as such and remove them from the scene
                BeamsVisible = false;
                ClearLines();
            }           
        }
    }

    void Update()
    {
        if (Engaged)
        {
            if (TimeSincePlayerDetected < TimeToChargeShot)
            {
                TimeSincePlayerDetected += Time.deltaTime;
            }
            else
            {
                Engaged = false;
                TimeSinceFired = 0;
                Debug.Log("Firing!");
                Shoot();               
            }
        }

        if (TimeSinceFired < TimeBetweenShots)
        {
            TimeSinceFired += Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collider)
    {
        
        if (collider.gameObject.name == "Player")
        {                       
            // determine direction that raycast should fire
            float deltaX = collider.gameObject.transform.position.x - gameObject.transform.position.x;
            float deltaY = collider.gameObject.transform.position.y - gameObject.transform.position.y;
            float deltaY_Bottom = deltaY;

            // temporarily disable collider / trigger of enemy so raycasts do not hit it
            CircleCollider2D enemyCC = gameObject.GetComponent<CircleCollider2D>();
            enemyCC.enabled = false;

            // Make three Raycast towards player
            //   - one towards Feet / Wheel
            //   - one towards Middle of body
            //   - one towards Top of Head

            RaycastHit2D hitInfo;
            bool playerDetected = false;

            while(deltaY <= deltaY_Bottom + GlobalVars.PlayerHeight)
            {
                hitInfo = Physics2D.Raycast(gameObject.transform.position, new Vector2(deltaX, deltaY));
                if(hitInfo.transform.name == "Player")
                {
                    playerDetected = true;
                    break;
                }
                deltaY += GlobalVars.PlayerHeight / 2;
            }

            // re-enable enemy's collider
            enemyCC.enabled = true;


            // if the player has been detected, proceed with logic on how to handle enemy's behavior
            if (playerDetected)
            {

                if(!Engaged && TimeSinceFired >= TimeBetweenShots)
                {
                    Debug.Log("Charging!");
                    Engaged = true;
                    TimeSincePlayerDetected = 0;

                    //TimeSinceFired = 0;
                    //Debug.Log("Firing!");
                    //Shoot();
                }

            }         
        }
    }

    private void Shoot()
    {

        // ensure that all previous lines drawn by this enemy have been cleared
        ClearLines();

        // temporarily disable collider / trigger of enemy so raycasts do not hit it
        CircleCollider2D enemyCC = gameObject.GetComponent<CircleCollider2D>();
        enemyCC.enabled = false;

        // bolean that will keep track if the player was hit
        bool playerHit = false;

        // send out a bunch of beams in a circle around the enemy given the angle between each beam
        for(float ang = 0; ang < 360; ang += BeamAngle)
        {
            RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, DegreeToVector2(ang));

            // if the current RayCast hit the player, indicate as such using the playerHit var
            if (hit.transform.name == "Player")
            {
                playerHit = true;
            }

            // draw a line corresponding to the current RayCast
            Line = new GameObject("Line " + ang).AddComponent<LineRenderer>();
            Line.material = LineMaterial;
            Line.SetColors(BeamColor, BeamColor);
            Line.startWidth = BeamWidth;
            Line.endWidth = BeamWidth;

            Line.SetPosition(0, gameObject.transform.position);            
            Line.SetPosition(1, hit.point);            

            // add the line to the list containing all lines made by this shot by the enemy
            lines.Add(Line);
        }

        // Indicate that beams are visible, so they can begin being faded away
        BeamsVisible = true;


        // re-enable enemy's collider
        enemyCC.enabled = true;

        if (playerHit)
        {

            Debug.Log("Hit the player!");

            // Call for player to be killed with animation type '1'\
            GameObject playerObj = GameObject.Find("Player");
            playerObj.GetComponent<PlayerStatus>().KillPlayer(1);

        }
    }

    // clears all lines from scene
    private void ClearLines()
    {
        if(lines.Count > 0)
        {
            foreach (LineRenderer line in lines)
            {
                Destroy(line.gameObject);
            }
            lines.Clear();
        }
    }

    private static Vector2 DegreeToVector2(float degree)
    {
        return new Vector2(Mathf.Cos(degree * Mathf.Deg2Rad), Mathf.Sin(degree * Mathf.Deg2Rad));
    }


}
