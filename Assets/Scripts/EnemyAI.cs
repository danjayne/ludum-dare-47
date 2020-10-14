using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
  public Transform Target;
  public float Speed = 200f;
  public float NextWaypointDistance = 3f;

  Path _path;
  int _currentWayPoint;
  bool _reachedEndOfPath;

  Seeker _seeker;
  Rigidbody2D _rb;

  private void Start()
  {
    _seeker = GetComponent<Seeker>();
    _rb = GetComponent<Rigidbody2D>();

    InvokeRepeating("UpdateAIPath", 0f, .5f);
  }

  void UpdateAIPath()
  {
    if (_seeker.IsDone())
    {
      _seeker.StartPath(_rb.position, Target.position, OnPathComplete);
    }
  }

  void OnPathComplete(Path p)
  {
    if (!p.error)
    {
      _path = p;
      _currentWayPoint = 0;
    }
  }

  private void FixedUpdate()
  {
    if (_path == null)
      return;

    if (_currentWayPoint >= _path.vectorPath.Count)
    {
      _reachedEndOfPath = true;
      return;
    }
    else
    {
      _reachedEndOfPath = false;
    }

    Vector2 direction = ((Vector2)_path.vectorPath[_currentWayPoint] - _rb.position).normalized;
    Vector2 force = direction * Speed * Time.deltaTime;

    _rb.AddForce(force);

    float distance = Vector2.Distance(_rb.position, _path.vectorPath[_currentWayPoint]);

    if (distance < NextWaypointDistance)
    {
      _currentWayPoint++;
    }

    if (force.x >= 0.01f)
    {
      transform.localScale = new Vector3(-1f, 1f, 1f);
    }
    else if (force.x <= -0.01f)
    {
      transform.localScale = new Vector3(1f, 1f, 1f);
    }
  }

}
