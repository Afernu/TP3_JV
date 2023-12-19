using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayerRange : NodeU
{
    private static int layerMask = 1 << 3;
    private Transform transform;

    public CheckPlayerRange(Transform entity)
    {
        transform = entity;
    }
    public override NodeStateU Evaluate()
    {
        object t = GetData("target");
        if(t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, AlertU.range, layerMask);

            if (colliders.Length > 0)
            {
                parent.parent.SetData("target", colliders[0].transform);
                state = NodeStateU.SUCCESS;
                return state;
            }

            state = NodeStateU.FAILURE;
            return state;
        }
        state = NodeStateU.SUCCESS;
        return state;
    }
}
