using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region "Singleton"

    private static CameraManager _instance;

    public static CameraManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    #endregion

    [Header("Virtual Camera")]
    public CinemachineVirtualCamera _vCam;

    [Header("Camera Sizes")]
    public float CameraSize_DeathScene = 1f;
    public float CameraSize = 3f;

    private void Start()
    {
        ResetCamera();
    }

    public void PlayDeathCameraScene()
    {
        _vCam.m_Lens.OrthographicSize = CameraSize_DeathScene;
    }

    public void ResetCamera()
    {
        _vCam.m_Lens.OrthographicSize = CameraSize;
    }
}
