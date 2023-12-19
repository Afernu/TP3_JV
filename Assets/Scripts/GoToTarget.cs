using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToTarget : NodeU
{
    private Transform inner_transform;

    public GoToTarget(Transform transform)
    {
        inner_transform = transform;
    }
    public override NodeStateU Evaluate()
    {
        Transform target = (Transform)GetData("target");

        if(Vector3.Distance(inner_transform.position, target.position) > 0.01f)
        {
            inner_transform.position = Vector3.MoveTowards(inner_transform.position, target.position, AlertU.speed * Time.deltaTime);
            inner_transform.LookAt(target.position);
        }
        state = NodeStateU.RUNNING;
        return state;
    }
}
