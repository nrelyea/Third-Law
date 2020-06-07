using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{

    //public Vector3[] Target;

    public CinemachineVirtualCamera vcam;

    private float TargetZoom;

    public float DefaultZoom;
    public float MinZoom;
    public float MaxZoom;


    // Start is called before the first frame update
    void Start()
    {
        TargetZoom = DefaultZoom;
    }

    // Update is called once per frame
    void Update()
    {
        float size = vcam.m_Lens.OrthographicSize;

        var d = Input.GetAxis("Mouse ScrollWheel");
        if (d > 0f && TargetZoom > MinZoom)
        {
            TargetZoom--;            
            
            //Debug.Log("Zooming IN to " + TargetZoom);
        }
        else if (d < 0f && TargetZoom < MaxZoom)
        {
            TargetZoom++;
            //Debug.Log("Zooming OUT to " + TargetZoom);
        }

        if (Math.Abs(size - TargetZoom) > 0.05f)    // if actual zoom size and target zoom size are different
        {
            if(size < TargetZoom)
            {
                vcam.m_Lens.OrthographicSize += 0.05f;
            }
            else
            {
                vcam.m_Lens.OrthographicSize -= 0.05f;
            }

            var composer = vcam.GetCinemachineComponent<CinemachineFramingTransposer>();
            composer.m_DeadZoneWidth = 0.1f + ((TargetZoom - DefaultZoom) / 150);
            composer.m_DeadZoneHeight = 0.05f + ((TargetZoom - DefaultZoom) / 150);
        }
    }
}
