using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlertU : BTree
{
    public Transform[] patrolPoints;

    public static float speed = 5f;

    protected override NodeU SetupT()
    {
     
        NodeU root = new TaskPatrolU(transform, patrolPoints);

        return root;
    }
}
