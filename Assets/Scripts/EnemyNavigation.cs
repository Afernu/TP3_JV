using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private NavMeshAgent navMeshAgent;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start() => StartCoroutine(ChangePostion());

    private IEnumerator ChangePostion()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            navMeshAgent.destination = targetTransform.position;
        }

    }
}
