using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PostProcessingManager : MonoBehaviour
{
    private Volume _volume;
    private bool deathPostProcessingSet = false;

    private void Start()
    {
        _volume = GetComponent<Volume>();
    }

    private void Update()
    {
        if (PlayerHealth.Instance.IsDead && !deathPostProcessingSet)
        {
            _volume.enabled = true;

            deathPostProcessingSet = true;
        }
    }
}
