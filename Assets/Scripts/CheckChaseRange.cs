using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckChaseRange : NodeU
{

    private Transform transform;
    private Animator animator;

    public CheckChaseRange(Transform entity)
    {
        transform = entity;
        //animator = transform.getComponent<Animator>();
    }
    public override NodeStateU Evaluate()
    {
        object t = GetData("target");
        if (t == null)
        {
            state = NodeStateU.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (Vector3.Distance(transform.position, target.position) >= AlertU.chaseRange)
        {
            ClearData("target");
            state = NodeStateU.SUCCESS;
            return state;
        }

        state = NodeStateU.FAILURE;
        return state;
    }
}
