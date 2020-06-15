using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorNew : MonoBehaviour
{
    // SIGNAL CONTROL
    
    public int ButtonsNeeded;           // number of buttons being pressed / signals needed to be recieved to open door
    private int SignalsRecieved = 0;    // current number of signals recieved

    private bool DoorOpen = false;

    // MOVEMENT CONTROL

    public GameObject OpenPoint;

    private Vector3 ClosedPosition;
    private Vector3 OpenPosition;

    public double TimeToOpen;
    private int UpdatesPerLoop;
    private Vector3 DistanceChangePerUpdate;

    // BLOCKING CONTROL

    public GameObject DoorBottom;

    private bool Blocked;
    private HashSet<string> FrozenBlockers;



    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer openSpriteRenderer = OpenPoint.GetComponent<SpriteRenderer>();
        openSpriteRenderer.enabled = false;

        OpenPosition = OpenPoint.transform.position;
        ClosedPosition = gameObject.transform.position;

        UpdatesPerLoop = (int)(50 * (TimeToOpen / 2));
        float deltaX = OpenPosition.x - ClosedPosition.x;
        float deltaY = OpenPosition.y - ClosedPosition.y;

        DistanceChangePerUpdate = new Vector3(deltaX / UpdatesPerLoop, deltaY / UpdatesPerLoop, 0);

        Blocked = false;
        FrozenBlockers = new HashSet<string> { };


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 currentPos = gameObject.transform.position;
        if (DoorOpen && NetDistance(currentPos, OpenPosition) > 0.01)
        {
            gameObject.transform.position = new Vector3(currentPos.x + DistanceChangePerUpdate.x, currentPos.y + DistanceChangePerUpdate.y, 0);
            Blocked = false;
            FrozenBlockers.Clear();
        }
        else if(!DoorOpen && NetDistance(currentPos, ClosedPosition) > 0.01 && !Blocked)
        {
            gameObject.transform.position = new Vector3(currentPos.x - DistanceChangePerUpdate.x, currentPos.y - DistanceChangePerUpdate.y, 0);
        }
    }

    public double NetDistance(Vector3 v1, Vector3 v2)
    {
        return (Math.Abs((double)v1.x - (double)v2.x) + Math.Abs((double)v1.y - (double)v2.y));
    }

    public void RecieveSignal(bool signal)
    {        
        if (signal)
        {
            SignalsRecieved++;
        }
        else if (!signal && SignalsRecieved > 0)
        {
            SignalsRecieved--;
        }

        DoorOpen = (SignalsRecieved >= ButtonsNeeded);
    }



    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        //Debug.Log("Collision");

        if (!IsBullet(collisionInfo))
        {
            IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();

            // TEMPORARY method of "ensuring" only objects UNDER door stop door from closing
            // checks if center of object hitting door is below the position of the Door Bottom by more than a unit of 0.1
            if (io != null && io.Frozen && DoorBottom.transform.position.y > collisionInfo.gameObject.transform.position.y + 0.1f)                               
            {
                Blocked = true;
                //Debug.Log(collisionInfo.gameObject.name);
                FrozenBlockers.Add(collisionInfo.gameObject.name);
            }                     
        }
    }

    private void OnCollisionStay2D(Collision2D collisionInfo)
    {
        IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();
        if (io != null)
        {
            if (Blocked)
            {
                // if object that was previously frozen was blocking elevator now isn't...
                if (!io.Frozen && FrozenBlockers.Contains(collisionInfo.gameObject.name))
                {
                    // remove it from blockers
                    FrozenBlockers.Remove(collisionInfo.gameObject.name);

                    // restart door's motion if it is no longer blocked by anything
                    if (FrozenBlockers.Count < 1)
                    {
                        Blocked = false;
                    }
                }
            }          
        }
    }







    private bool IsBullet(Collision2D col)
    {
        return (col.gameObject.name == "PrimaryBullet(Clone)" || col.gameObject.name == "SecondaryBullet(Clone)");
    }
}
