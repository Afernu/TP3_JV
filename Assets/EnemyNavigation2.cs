using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class EnemyNavigation2 : MonoBehaviour
{
    private Node behaviorTree;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform playerTransform;
    [SerializeField] Animator animator;

    private void Start()
    {
        GenerateRandomPoints generatedPoints = FindObjectOfType<GenerateRandomPoints>();

        if (generatedPoints != null)
        {
            List<Vector3> patrolPoints = new List<Vector3>();

            foreach (Transform child in generatedPoints.transform)
            {
                patrolPoints.Add(child.position);
            }

            TaskBT[] patrolTask = new TaskBT[]
            {
                new Patrol(patrolPoints.ToArray(), agent,playerTransform),
                new DummyTask("Patrol Completed", TaskState.Success)
            };

            TaskBT[] chaseTask = new TaskBT[]
            {
                new ChasePlayer(playerTransform, agent, animator),
                new DummyTask("Player Chased", TaskState.Success)
            };

            TaskNode patrolNode = new TaskNode("Patrol", patrolTask);
            TaskNode chaseNode = new TaskNode("Chase", chaseTask);
            animator.SetBool("IsWalking", true);

            Node sequence0 = new Sequence("Sequence0", new[] { patrolNode, chaseNode });

            behaviorTree = sequence0;

            StartCoroutine(RunBehaviorTree());
        }
        else
        {
            Debug.LogError("GenerateRandomPoints component not found.");
        }
    }

    private IEnumerator RunBehaviorTree()
    {
        while (true)
        {
            behaviorTree.Evaluate();
            yield return null;
        }
    }
    private void Update()
    {
        HandleOffMeshLinkTraversal();
    }
    private void HandleOffMeshLinkTraversal()
    {
        if (agent.isOnOffMeshLink)
        {
            animator.SetBool("IsJumping", true);
            StartCoroutine(CompleteOffMeshLink());
        }
    }

    private IEnumerator CompleteOffMeshLink()
    {
        OffMeshLinkData linkData = agent.currentOffMeshLinkData;
        Vector3 startPos = agent.transform.position;
        Vector3 endPos = linkData.endPos + Vector3.up * agent.baseOffset;
        float jumpDuration = 1f;
        float normalizedTime = 0.0f;

        while (normalizedTime < 1.0f)
        {
            float yOffset = Mathf.Sin(Mathf.PI * normalizedTime) * 1f;
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / jumpDuration;
            yield return null;
        }

        agent.CompleteOffMeshLink();
        animator.SetBool("IsJumping", false);
    }
}

public class ChasePlayer : TaskBT
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator animator;
    private float patrolStoppingDistance = 5f;

    public ChasePlayer(Transform playerTransform, NavMeshAgent agent, Animator animator)
    {
        this.playerTransform = playerTransform;
        this.agent = agent;
        this.animator = animator;
    }

    public override TaskState Execute()
    {
        Debug.Log("EXECUTED CHASE");
        agent.destination = playerTransform.position;
        animator.SetBool("IsWalking", false);
        animator.SetBool("IsRunning", true);

        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        // Set stopping distance for chasing
        agent.stoppingDistance = 1f;

        if (distanceToPlayer <= agent.stoppingDistance)
        {
            Debug.Log("Game Over - Player Caught!");
            return TaskState.Success;
        }

        // If player is out of range, return to patrolling mode
        if (distanceToPlayer > patrolStoppingDistance)
        {
            Debug.Log("Player out of range - Returning to patrol");
            // Reset stopping distance for patrolling
            agent.stoppingDistance = patrolStoppingDistance;
            //animator.SetBool("IsWalking", false); 
            return TaskState.Success; 
        }

        return TaskState.Running;
    }
}


