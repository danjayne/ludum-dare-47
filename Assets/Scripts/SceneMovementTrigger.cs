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
    
    private Transform _player;

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //var bc = GetComponent<BoxCollider2D>();

        Debug.Log($"Entered Trigger at x position {collision.transform.position.x}");

        //if (collision.transform.position.x < bc.bounds.center.x)
        //{
        //    //_vCam.Follow = RightScene;
        //    //_player.position.Set(bc.bounds.max.x + 1, _player.position.y, _player.position.z);
        //}
        //else if (collision.transform.position.x > bc.bounds.center.x)
        //{
        //    //_vCam.Follow = LeftScene;
        //    //_player.position.Set(bc.bounds.min.x - 1, _player.position.y, _player.position.z);
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var bc = GetComponent<BoxCollider2D>();

        Debug.Log($"Entered Trigger at x position {collision.transform.position.x}");

        if (collision.transform.position.x > bc.bounds.max.x)
        {
            _vCam.Follow = RightScene;
            //_player.position.Set(bc.bounds.max.x + 1, _player.position.y, _player.position.z);
        }
        else if (collision.transform.position.x < bc.bounds.min.x)
        {
            _vCam.Follow = LeftScene;
            //_player.position.Set(bc.bounds.min.x - 1, _player.position.y, _player.position.z);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {

    }
}
