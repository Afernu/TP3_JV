using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TaskPatrolU : NodeU
{
    private Transform entity;
    private Animator animator;
    private Transform[] patrolPoints;

    private int _currentWaypointIndex = 0;

    private float _waitTime = 1f; // in seconds
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public TaskPatrolU(Transform transform, Transform[] pp)
    {
        entity = transform;
        animator = transform.GetComponent<Animator>();
        patrolPoints = pp;
    }

    public override NodeStateU Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            Transform wp = patrolPoints[_currentWaypointIndex];
            if (Vector3.Distance(entity.position, wp.position) < 0.01f)
            {
                entity.position = wp.position;
                _waitCounter = 0f;
                _waiting = true;

                _currentWaypointIndex = (_currentWaypointIndex + 1) % patrolPoints.Length;
                animator.SetBool("Walking", false);
            }
            else
            {
                entity.position = Vector3.MoveTowards(entity.position, wp.position, 5f * Time.deltaTime);
                entity.LookAt(wp.position);
            }
        }


        state = NodeStateU.RUNNING;
        return state;
    }

}
