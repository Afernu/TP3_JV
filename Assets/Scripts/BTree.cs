using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BTree : MonoBehaviour
{
    private NodeU root = null;

    private void Start()
    {
        root = SetupT();
    }
    private void Update()
    {
        if (root != null)
            root.Evaluate();
    }
    protected abstract NodeU SetupT();
}
