using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneMovementTrigger : MonoBehaviour
{
    private Transform _leftScene;
    private Transform _rightScene;
    public CinemachineVirtualCamera _vCam;

    private void Start()
    {
        var sceneNumStr = gameObject.transform.parent.name.Split(new string[] { "TransitionTrigger" }, StringSplitOptions.RemoveEmptyEntries)[0];
        int.TryParse(sceneNumStr, out int sceneNum);

        _leftScene = GetSceneByIndex(sceneNum);
        _rightScene = GetSceneByIndex(sceneNum + 1);
    }

    private Transform GetSceneByIndex(int sceneNum)
    {
        return GameObject.Find($"Scene{sceneNum}")?.transform;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        var bc = GetComponent<BoxCollider2D>();

        if (collision.transform.position.x > bc.bounds.max.x)
        {
            _vCam.Follow = _rightScene;
        }
        else if (collision.transform.position.x < bc.bounds.min.x)
        {
            _vCam.Follow = _leftScene;
        }

        SpriteRenderer sr;
        if (!_vCam.Follow.TryGetComponent<SpriteRenderer>(out sr))
        {
            _vCam.Follow.Find("Cloud-Background").TryGetComponent<SpriteRenderer>(out sr);
        }

        _vCam.m_Lens.OrthographicSize = (sr.bounds.size.x * Screen.height) / Screen.width * 0.5f;
    }
}
