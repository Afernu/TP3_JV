using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AlertU : BTree
{
    public Transform[] patrolPoints;
    public static float range = 5f; 
    public static float speed = 5f;
    public static float chaseRange = 2f;
    protected override NodeU SetupT()
    {

        NodeU root = new Selector(new List<NodeU>
        {
            new SequenceU(new List<NodeU>() {
                new CheckChaseRange(transform)
            }),
            new SequenceU(new List<NodeU>
            {
                new CheckPlayerRange(transform),
                new GoToTarget(transform)
            }),
            new TaskPatrolU(transform,patrolPoints),
        }) ;

        return root;
    }
}
