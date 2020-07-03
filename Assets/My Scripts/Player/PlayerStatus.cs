using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



// This class makes modifications to and helps out with operations related to changing the status of the player
public class PlayerStatus : MonoBehaviour
{
    // controls where zoom is forced to upon Player death
    private int ForcedZoom;

    // variables to control how DeathHole shrinks and when
    private GameObject Hole;
    private bool HoleShrinking;
    private bool HoleShrinkComplete;
    private float FinalHoleScale;

    private float DeathHoleYOffset = 1.0f;

    private GameObject PlayerObj;

    // Start is called before the first frame update
    void Start()
    {
        ForcedZoom = 7;

        Hole = GameObject.Find("DeathHole");
        HoleShrinking = false;
        FinalHoleScale = 2; // the final resting size of the whole on player before fading to black

        PlayerObj = GameObject.Find("Player");
    }

    void FixedUpdate()
    {
        if (HoleShrinking)
        {
            if (Hole.transform.localScale.x > FinalHoleScale)
            {
                Hole.transform.localScale = Vector3.Scale(Hole.transform.localScale, new Vector3(0.97f, 0.97f, 0.97f));
                HoleShrinkComplete = true;
            }
            else if (HoleShrinkComplete)
            {
                HoleShrinking = false;

                Transform image = GameObject.Find("UI Canvas").transform.Find("FadeImage");
                LevelFading fadeScript = image.gameObject.GetComponent<LevelFading>();
                fadeScript.FadeToLevel(SceneManager.GetActiveScene().buildIndex);
            }
            Vector3 playerPos = PlayerObj.transform.position;
            Hole.transform.position = new Vector3(playerPos.x, playerPos.y + DeathHoleYOffset, 0);
        }
    }

    public void KillPlayer(int deathType)
    {
        // force zoom target to be certain number
        CameraZoom zoomScript = GameObject.Find("Zoom").GetComponent<CameraZoom>();
        zoomScript.ForceZoom(ForcedZoom);

        // generate effect of circular spotlight focusing on where player died
        BeginDeathHoleEffect(gameObject.transform.position, DeathHoleYOffset);

        // stop player from moving if death type is 0
        if(deathType == 0)
        {
            Rigidbody2D playerRB = gameObject.GetComponent<Rigidbody2D>();
            playerRB.bodyType = RigidbodyType2D.Static;
        }

        // disable movement inputs script to disable any input driven animation changes
        CharacterController2D controlScript = gameObject.GetComponent<CharacterController2D>();
        controlScript.DisableMovement();

        // disable mouse aiming from affecting the direction player is looking / flipping
        PlayerFaceCorrectDirection faceScript = gameObject.GetComponent<PlayerFaceCorrectDirection>();
        faceScript.enabled = false;

        // remove the gun
        Transform gunTransform = gameObject.transform.Find("Gun");
        Destroy(gunTransform.gameObject);



        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // MAKE PLAYER DYING ANIMATION / EFFECTS HERE (different animation / effects based on 'deathType' parameter) //
        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////



        //Destroy(collisionInfo.gameObject);
    }

    private void BeginDeathHoleEffect(Vector3 targetPos, float yOffset)
    {
        Hole.transform.localScale = new Vector3(35, 35, 1);
        Hole.transform.position = new Vector3(targetPos.x, targetPos.y + yOffset, 0);
        HoleShrinking = true;
    }
}
