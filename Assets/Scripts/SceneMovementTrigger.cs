using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMovementTrigger : MonoBehaviour
{
    public Transform LeftScene;
    public Transform RightScene;

    private CinemachineVirtualCamera _vCam;
    
    void Start()
    {
        _vCam = Camera.main.GetComponent<CinemachineVirtualCamera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _vCam.Follow = collision.transform.position.x < GetComponent<SpriteRenderer>().bounds.center.x ?
            _vCam.Follow = RightScene :
            _vCam.Follow = LeftScene;
    }
}
