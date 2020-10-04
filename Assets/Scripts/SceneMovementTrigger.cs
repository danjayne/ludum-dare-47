using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMovementTrigger : MonoBehaviour
{
    public Transform LeftScene;
    public Transform RightScene;
    public CinemachineVirtualCamera _vCam;
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        var bc = GetComponent<BoxCollider2D>();

        if (collision.transform.position.x > bc.bounds.max.x)
        {
            _vCam.Follow = RightScene;
        }
        else if (collision.transform.position.x < bc.bounds.min.x)
        {
            _vCam.Follow = LeftScene;
        }
    }
}
