using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (targetTransform != null)
        {
            navMeshAgent.destination = targetTransform.position;
            animator.SetBool("IsWalking", true);
        }
    }

    private void Update()
    {
        if (targetTransform == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, targetTransform.position);

        if (distanceToTarget <= navMeshAgent.stoppingDistance)
        {
            Debug.Log("gotcha");
            animator.SetBool("IsWalking", false);
        }
        else
        {
            navMeshAgent.destination = targetTransform.position;
            animator.SetBool("IsWalking", true);
        }

        HandleOffMeshLinkTraversal();
    }

    private void HandleOffMeshLinkTraversal()
    {
        if (navMeshAgent.isOnOffMeshLink)
        {
            animator.SetBool("IsJumping", true);
            StartCoroutine(CompleteOffMeshLink());
        }
    }

    private IEnumerator CompleteOffMeshLink()
    {
        OffMeshLinkData linkData = navMeshAgent.currentOffMeshLinkData;
        Vector3 startPos = navMeshAgent.transform.position;
        Vector3 endPos = linkData.endPos + Vector3.up * navMeshAgent.baseOffset;
        float jumpDuration = 1f;
        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f)
        {
            float yOffset = Mathf.Sin(Mathf.PI * normalizedTime) * 1.5f;
            navMeshAgent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / jumpDuration;
            yield return null;
        }

        navMeshAgent.CompleteOffMeshLink();
        animator.SetBool("IsJumping", false);
    }

}
