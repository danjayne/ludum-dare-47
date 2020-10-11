using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : MonoBehaviour
{
    [Header("Player Follow")]
    public Transform Player;
    public Vector3 RotationOffset = new Vector3(0,0,-120);

    [Header("Projectile")]
    public Transform FireLocation;
    public Transform Projectile;
    public float ProjectileForce = 10;
    public float MinPlayerDistanceFromSpider = 6f;
    public float TimeSinceLastProjectile = 3f;

    private Rigidbody2D rb;
    private Vector3 direction;
    private bool fireProjectile;
    private Transform projectile;
    private float timeSinceLastProjectile;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        timeSinceLastProjectile += Time.deltaTime;

        direction = Player.position - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        rb.rotation = RotationOffset.z - angle;

        if (direction.magnitude < MinPlayerDistanceFromSpider && timeSinceLastProjectile > TimeSinceLastProjectile)
        {
            fireProjectile = true;
        }
    }

    private void FixedUpdate()
    {
        if (fireProjectile)
        {
            projectile = Instantiate(Projectile, FireLocation);
            var prb = projectile.GetComponent<Rigidbody2D>();
            prb.velocity = Vector2.zero;
            prb.AddForce(direction * ProjectileForce * Time.deltaTime, ForceMode2D.Impulse);

            timeSinceLastProjectile = 0f;
            fireProjectile = false;
        }
    }
}
