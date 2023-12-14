using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private bool startMoving = false;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        StartCoroutine(DelayedStart());
    }

    private void Update()
    {
        if (targetTransform == null || !startMoving) return;

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
    private IEnumerator DelayedStart()
    {
        float delayTime = Random.Range(0f, 2f);
        yield return new WaitForSeconds(delayTime);

        if (targetTransform != null)
        {
            navMeshAgent.speed = Random.Range(1f, 3.5f);
            navMeshAgent.destination = targetTransform.position;
            animator.SetBool("IsWalking", true);
            startMoving = true;
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
            float yOffset = Mathf.Sin(Mathf.PI * normalizedTime) * 1f;
            navMeshAgent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / jumpDuration;
            yield return null;
        }

        navMeshAgent.CompleteOffMeshLink();
        animator.SetBool("IsJumping", false);
    }

}
