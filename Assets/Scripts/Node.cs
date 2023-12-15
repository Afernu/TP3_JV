using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NodeState { Resting, Running, Success, Failure }
public abstract class Node
{
    public string Tag { get; private set; }
    public bool IsLoggingTag { get; private set; } = false;
    public Node Parent { get; private set; }
    protected List<Node> Children { get; private set; } = new();
    public NodeState State { get; protected set; } = NodeState.Resting;


    private Dictionary<string, object> _data = new Dictionary<string, object>();
    public Node(string tag) => Tag = tag;
    public Node(string tag, IEnumerable<Node> children)
        : this(tag)
    {
        foreach (var child in children)
            Attach(child);
    }
    public Node(List<Node> children)
    {
        foreach (var child in children)
            Attach(child);
    }
    public Node()
    {
        Parent = null;
    }
    private void Attach(Node child)
    {
        Children.Add(child);
        child.Parent = this;
    }

    public NodeState Evaluate()
    {
        if (IsLoggingTag)
            Debug.Log(Tag);
        return InnerEvaluate();
    }
    protected abstract NodeState InnerEvaluate();
}

public enum NodeStateU
{
    RUNNING,
    SUCCESS,
    FAILURE
}
public class NodeU
{
    protected NodeStateU state;

    public NodeU parent;
    protected List<NodeU> children = new List<NodeU>();

    private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

    public NodeU()
    {
        parent = null;
    }
    public NodeU(List<NodeU> children)
    {
        foreach (NodeU child in children)
            _Attach(child);
    }

    private void _Attach(NodeU node)
    {
        node.parent = this;
        children.Add(node);
    }

    public virtual NodeStateU Evaluate() => NodeStateU.FAILURE;

    public void SetData(string key, object value)
    {
        _dataContext[key] = value;
    }

    public object GetData(string key)
    {
        object value = null;
        if (_dataContext.TryGetValue(key, out value))
            return value;

        NodeU node = parent;
        while (node != null)
        {
            value = node.GetData(key);
            if (value != null)
                return value;
            node = node.parent;
        }
        return null;
    }

    public bool ClearData(string key)
    {
        if (_dataContext.ContainsKey(key))
        {
            _dataContext.Remove(key);
            return true;
        }

        NodeU node = parent;
        while (node != null)
        {
            bool cleared = node.ClearData(key);
            if (cleared)
                return true;
            node = node.parent;
        }
        return false;
    }
}
