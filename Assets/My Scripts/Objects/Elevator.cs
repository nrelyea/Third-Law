using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public GameObject StartPoint;
    public GameObject EndPoint;

    public double TimeToComplete;
    private int UpdatesPerLoop;
    private Vector3 DistanceChangePerUpdate;
    private bool StartIsBelowEnd;

    private Vector3 StartPosition;
    private Vector3 EndPosition;

    private bool MovingTowardsEnd;
    private bool isPaused;

    private HashSet<string> FrozenBlockers;

    private HashSet<string> ConnectedObjects;


    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer startSpriteRenderer = StartPoint.GetComponent<SpriteRenderer>();
        startSpriteRenderer.enabled = false;
        StartPosition = startSpriteRenderer.transform.position;


        SpriteRenderer endSpriteRenderer = EndPoint.GetComponent<SpriteRenderer>();
        endSpriteRenderer.enabled = false;
        EndPosition = endSpriteRenderer.transform.position;

        gameObject.transform.position = StartPosition;
        MovingTowardsEnd = true;
        isPaused = false;

        FrozenBlockers = new HashSet<string> { };
        ConnectedObjects = new HashSet<string> { };

        UpdatesPerLoop = (int)(50 * (TimeToComplete / 2));
        float deltaX = EndPosition.x - StartPosition.x;
        float deltaY = EndPosition.y - StartPosition.y;

        DistanceChangePerUpdate = new Vector3(deltaX / UpdatesPerLoop, deltaY / UpdatesPerLoop, 0);

        if (StartPosition.y < EndPosition.y) StartIsBelowEnd = true;
        else StartIsBelowEnd = false;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(StartPosition.ToString() + " => " + EndPosition.ToString());
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            Vector3 currentPos = gameObject.transform.position;

            if (MovingTowardsEnd && (StartIsBelowEnd && currentPos.y > EndPosition.y) || (!StartIsBelowEnd && currentPos.y < EndPosition.y))
            {
                MovingTowardsEnd = false;
            }
            else if (!MovingTowardsEnd && (StartIsBelowEnd && currentPos.y < StartPosition.y) || (!StartIsBelowEnd && currentPos.y > StartPosition.y))
            {
                MovingTowardsEnd = true;
            }

            if (MovingTowardsEnd)
            {
                gameObject.transform.position = new Vector3(currentPos.x + DistanceChangePerUpdate.x, currentPos.y + DistanceChangePerUpdate.y, 0);
            }
            else
            {
                gameObject.transform.position = new Vector3(currentPos.x - DistanceChangePerUpdate.x, currentPos.y - DistanceChangePerUpdate.y, 0);
            }
        }

        PrintConnectedObjects();
    }

    private void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        //Debug.Log("Collision");

        if(!IsBullet(collisionInfo))
        {
            IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();
            if (io == null || (io != null && !io.Frozen))                               // if collider is player or unfrozen IO, mount it on the elevator
            {
                var middleManObj = new GameObject();
                middleManObj.name = collisionInfo.gameObject.name + " Helper";
                collisionInfo.collider.transform.SetParent(middleManObj.transform);
                middleManObj.transform.SetParent(transform);
            }
            else                                                                        // otherwise, freeze the elevator and ad IO to the list of blockers
            {
                isPaused = true;
                FrozenBlockers.Add(collisionInfo.gameObject.name);
            }

            ConnectedObjects.Add(collisionInfo.gameObject.name);
        }
    }

    private void OnCollisionExit2D(Collision2D collisionInfo)
    {
        if (!IsBullet(collisionInfo) && collisionInfo.collider.transform.parent != null)      // if collider exits, remove from mounted objects on elevator
        {
            var middleManObj = collisionInfo.collider.transform.parent.gameObject;
            middleManObj.transform.SetParent(null);
            collisionInfo.collider.transform.SetParent(null);
            Destroy(middleManObj);

            ConnectedObjects.Remove(collisionInfo.gameObject.name);
        }
    }

    private void OnCollisionStay2D(Collision2D collisionInfo)
    {
        IO_Collision io = collisionInfo.gameObject.GetComponent<IO_Collision>();
        if(io != null)
        {
            if (isPaused)
            {
                // if object that was previously frozen was blocking elevator now isn't...
                if (!io.Frozen && FrozenBlockers.Contains(collisionInfo.gameObject.name))   
                { 
                    // remove it from blockers
                    FrozenBlockers.Remove(collisionInfo.gameObject.name);

                    // and connect / reconnect it to elevator if it isn't already
                    if (collisionInfo.collider.transform.parent == null)
                    {
                        var middleManObj = new GameObject();
                        middleManObj.name = collisionInfo.gameObject.name + " Helper";
                        collisionInfo.collider.transform.SetParent(middleManObj.transform);
                        middleManObj.transform.SetParent(transform);
                    }

                    // restart elevator's motion if it is no longer blocked by anything
                    if (FrozenBlockers.Count < 1)
                    {
                        isPaused = false;
                    }
                }
            }
            else
            {
                if (io.Frozen)
                {
                    if (MovingTowards(collisionInfo.gameObject.transform.position.y))
                    {
                        isPaused = true;
                        FrozenBlockers.Add(collisionInfo.gameObject.name);
                    }
                    else    // if object was frozen and elevator is moving away, separate object from elevator
                    {
                        var middleManObj = collisionInfo.collider.transform.parent.gameObject;

                        middleManObj.transform.SetParent(null);

                        collisionInfo.collider.transform.SetParent(null);

                        Destroy(middleManObj);
                    }
                }
            }
        }
        
    }

    private void PrintConnectedObjects()
    {
        if(ConnectedObjects.Count > 0)
        {
            string lst = "";
            foreach (string name in ConnectedObjects)
            {
                lst += name + ", ";
            }
            Debug.Log("Connected Objects: " + lst + "   # of Frozen blockers: " + FrozenBlockers.Count);
        }
    }

    // helper function that returns true if the elevator is currently moving closer towards a given position
    private bool MovingTowards(float objPositionY)
    {
        if (MovingTowardsEnd)
        {
            if (StartIsBelowEnd)
            {
                if(objPositionY > gameObject.transform.position.y)
                {
                    return true;
                }                
            }
            else
            {
                if (objPositionY < gameObject.transform.position.y)
                {
                    return true;
                }
            }
        }
        else
        {
            if (StartIsBelowEnd)
            {
                if (objPositionY < gameObject.transform.position.y)
                {
                    return true;
                }
            }
            else
            {
                if (objPositionY > gameObject.transform.position.y)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool IsBullet(Collision2D col)
    {
        return (col.gameObject.name == "PrimaryBullet(Clone)" || col.gameObject.name == "SecondaryBullet(Clone)");
    }
}
