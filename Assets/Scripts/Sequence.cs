using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.VisualScripting.Metadata;

public class Sequence : Node
{
    private int currentChildID = 0;

    public Sequence(string tag) : base(tag) { }
    public Sequence(string tag, IEnumerable<Node> children)
        : base(tag, children) { }
    protected override NodeState InnerEvaluate()
    {
        var currentChild = Children[currentChildID];

        NodeState childState = currentChild.Evaluate();
        switch (childState)
        {
            case NodeState.Failure:
                State = NodeState.Failure;
                break;
            case NodeState.Running:
                State = NodeState.Running;
                break;
            case NodeState.Success:
                if (currentChildID == Children.Count - 1)
                    State = NodeState.Success;
                else
                    State = NodeState.Running;

                currentChildID = (currentChildID + 1) % Children.Count;
                break;
        }
        return State;
    }
}
public class Selector : NodeU
{
    public Selector() : base() { }
    public Selector(List<NodeU> children) : base(children) { }

    public override NodeStateU Evaluate()
    {
        foreach (NodeU node in children)
        {
            switch (node.Evaluate())
            {
                case NodeStateU.FAILURE:
                    continue;
                case NodeStateU.SUCCESS:
                    state = NodeStateU.SUCCESS;
                    return state;
                case NodeStateU.RUNNING:
                    state = NodeStateU.RUNNING;
                    return state;
                default:
                    continue;
            }
        }

        state = NodeStateU.FAILURE;
        return state;
    }

}
public class SequenceU : NodeU
{
    public SequenceU() : base() { }
    public SequenceU(List<NodeU> children) : base(children) { }

    public override NodeStateU Evaluate()
    {
        bool anyChildIsRunning = false;

        foreach (NodeU node in children)
        {
            switch (node.Evaluate())
            {
                case NodeStateU.FAILURE:
                    state = NodeStateU.FAILURE;
                    return state;
                case NodeStateU.SUCCESS:
                    continue;
                case NodeStateU.RUNNING:
                    anyChildIsRunning = true;
                    continue;
                default:
                    state = NodeStateU.SUCCESS;
                    return state;
            }
        }

        state = anyChildIsRunning ? NodeStateU.RUNNING : NodeStateU.SUCCESS;
        return state;
    }

}