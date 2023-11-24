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
                new Patrol(patrolPoints.ToArray(), agent,playerTransform,animator),
                new DummyTask("Patrol Completed", TaskState.Success)
            };

            TaskBT[] chaseTask = new TaskBT[]
            {
                new ChasePlayer(playerTransform, agent, animator),
                new DummyTask("Player Chased", TaskState.Success)
            };

            TaskNode patrolNode = new TaskNode("Patrol", patrolTask);
            TaskNode chaseNode = new TaskNode("Chase", chaseTask);

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
            yield return new WaitForSeconds(1f);
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
public class Patrol : TaskBT
{
    private Vector3[] Destinations { get; set; }
    private NavMeshAgent Agent { get; set; }
    private Transform PlayerTransform { get; set; }
    private int CurrentDestinationID { get; set; }
    private Animator Animator { get; set; }

    public Patrol(Vector3[] destinations, NavMeshAgent agent, Transform playerTransform, Animator animator)
    {
        Destinations = destinations;
        Agent = agent;
        PlayerTransform = playerTransform;
        Animator = animator;
    }

    public override TaskState Execute()
    {
        float distanceToPlayer = Vector3.Distance(Agent.transform.position, PlayerTransform.position);
        Agent.stoppingDistance = 10f;

        Animator.SetBool("IsWalking", true);

        if (distanceToPlayer <= Agent.stoppingDistance)
        {
            return TaskState.Success;
        }

        Vector3 currentDestination = Destinations[CurrentDestinationID];
        Agent.destination = currentDestination;

        if (Vector3.Distance(currentDestination, Agent.transform.position) < Agent.stoppingDistance)
        {
            CurrentDestinationID = (CurrentDestinationID + 1) % Destinations.Length;
            Animator.SetBool("IsWalking", false);
            Debug.Log("Patrol Point: " + CurrentDestinationID);
        }

        return TaskState.Running;
    }
}

public class ChasePlayer : TaskBT
{
    private Transform playerTransform;
    private NavMeshAgent agent;
    private Animator animator;
    private float patrolStoppingDistance = 10f;

    public ChasePlayer(Transform playerTransform, NavMeshAgent agent, Animator animator)
    {
        this.playerTransform = playerTransform;
        this.agent = agent;
        this.animator = animator;
    }

    public override TaskState Execute()
    {
        Debug.Log("EXECUTING CHASE");

        animator.SetBool("IsWalking", true);
        animator.SetBool("IsRunning", true);

        agent.destination = playerTransform.position;
        agent.stoppingDistance = 0.5f;
        agent.speed = 7f;
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        if (distanceToPlayer <= agent.stoppingDistance)
        {
            Debug.Log("Game Over - Player Caught!");
        }

        if (distanceToPlayer > patrolStoppingDistance)
        {
            agent.speed = 3f; //reset la vitesse
            Debug.Log("Player out of range - Returning to patrol");
            animator.SetBool("IsWalking", false);
            animator.SetBool("IsRunning", false);
            return TaskState.Success;
        }

        return TaskState.Running;
    }
}




