using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float Speed = 5f;
    public float Distance = 2f;
    public Transform Ground;
    public LayerMask EnvironmentMask;

    private bool _movingRight = false;

    private void Update()
    {
        transform.Translate(Vector2.left * Speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(Ground.position, Vector2.down, Distance, EnvironmentMask);

        if (!groundInfo.collider)
        {
            if (!_movingRight)
            {
                transform.eulerAngles = new Vector3(0, -180, 0);
                _movingRight = true;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 0, 0);
                _movingRight = false;
            }
        }
    }
}
