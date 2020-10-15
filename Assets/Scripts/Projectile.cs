using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float KillAfter = 10f;

    private void Awake()
    {
        Destroy(gameObject, KillAfter);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //TODO: hitEffect
        if (collision.gameObject.tag == "Player")
            Destroy(gameObject, 0.01f);
    }
}
