using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Player Follow")]
    public Transform Player;
    public Vector3 RotationOffset = new Vector3(0, 0, -120);

    [Header("Projectile")]
    public Transform FireLocation;
    public Transform Projectile;
    public float ProjectileForce = 10;
    public float MinPlayerDistanceFromSpider = 6f;
    public float MaxTimeSinceLastProjectile = 3f;

    private Vector3 objectDirection;
    private Vector3 fireDirection;
    private bool fireProjectile;
    private Transform projectile;
    private float timeSinceLastProjectile = 3f;

    private void Update()
    {
        timeSinceLastProjectile += Time.deltaTime;

        objectDirection = Player.position - transform.position;
        fireDirection = Player.position - FireLocation.position;
        float angle = Mathf.Atan2(objectDirection.x, objectDirection.y) * Mathf.Rad2Deg;

        if (objectDirection.magnitude < MinPlayerDistanceFromSpider)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, RotationOffset.z - angle);

            if (timeSinceLastProjectile > MaxTimeSinceLastProjectile)
                fireProjectile = true;
        }
    }

    private void FixedUpdate()
    {
        if (fireProjectile)
        {
            projectile = Instantiate(Projectile, FireLocation.position, Quaternion.identity);
            var prb = projectile.GetComponent<Rigidbody2D>();
            prb.velocity = Vector2.zero;
            prb.AddForce(fireDirection.normalized * ProjectileForce * Time.deltaTime, ForceMode2D.Impulse);

            timeSinceLastProjectile = 0f;
            fireProjectile = false;
        }
    }
}
