using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWidthManager : MonoBehaviour
{
    #region "Singleton"

    private static ScreenWidthManager _instance;

    public static ScreenWidthManager Instance => _instance;

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

    public SpriteRenderer FirstSceneBackground;
    public CinemachineVirtualCamera _vCam;

    private void Start()
    {
        _vCam.Follow = FirstSceneBackground.transform;
        _vCam.m_Lens.OrthographicSize = (FirstSceneBackground.bounds.size.x * Screen.height) / Screen.width * 0.5f;
    }
}
