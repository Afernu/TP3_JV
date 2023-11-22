using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BehaviourEnemy : MonoBehaviour
{
    [SerializeField]
    Transform[] patrolPoints;
    [SerializeField]
    public NavMeshAgent agent;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform playerTransform;
    private Node rootBt;
    private bool playerInRange;

    void Awake()
    {
        Vector3[] destinations = patrolPoints.Select(t => t.position).ToArray();
        TaskBT[] tasks0 = new TaskBT[]
        {
            new Patrol(destinations, agent, animator)
        };

        TaskBT[] tasks1 = new TaskBT[]
        {
            new Sprint(agent, animator)
        };

        TaskNode patrolNode = new TaskNode("PatrolNode", tasks0);
        TaskNode sprintNode = new TaskNode("SprintNode", tasks1);

        rootBt = new Sequence("RootSelector", new Node[] { patrolNode, sprintNode });

        StartCoroutine(CheckPlayerRange());
    }

    private void Start()
    {
        animator.SetBool("IsWalking", true);
    }

    void Update()
    {
        NodeState result = rootBt.Evaluate();
        UpdateAnimations(result);
        HandleOffMeshLinkTraversal();
    }

    private void UpdateAnimations(NodeState state)
    {
        // Update animations based on the current state of the behavior tree.

        // Example: Animation of walking
        animator.SetBool("IsWalking", state == NodeState.Running);
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
            float yOffset = Mathf.Sin(Mathf.PI * normalizedTime) * 1.5f;
            agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
            normalizedTime += Time.deltaTime / jumpDuration;
            yield return null;
        }

        agent.CompleteOffMeshLink();
        animator.SetBool("IsJumping", false);
    }

    private IEnumerator CheckPlayerRange()
    {
        while (true)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer < 5f && !playerInRange)
            {
                playerInRange = true;
                ChangeToSprint();
            }
            else if (distanceToPlayer >= 5f && playerInRange)
            {
                playerInRange = false;
                ChangeToPatrol();
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void ChangeToSprint()
    {
        animator.SetBool("IsRunning", true);
        Debug.Log("Changed to Sprint!");
    }

    private void ChangeToPatrol()
    {
        animator.SetBool("IsRunning", false);
        Debug.Log("Changed to Patrol!");
    }

    public class Sprint : TaskBT
    {
        private NavMeshAgent agent;
        private Animator animator;

        public Sprint(NavMeshAgent agent, Animator animator)
        {
            this.agent = agent;
            this.animator = animator;
        }

        public override TaskState Execute()
        {
            agent.speed = 10f;
            return TaskState.Success;
        }
    }

    public class Patrol : TaskBT
    {
        private NavMeshAgent agent;
        private Animator animator;
        private Vector3[] destinations;
        private int currentDestinationIndex;

        public Patrol(Vector3[] destinations, NavMeshAgent agent, Animator animator)
        {
            this.destinations = destinations;
            this.agent = agent;
            this.animator = animator;
            currentDestinationIndex = 0;
        }

        public override TaskState Execute()
        {
            agent.speed = 3.5f; // Adjust the speed back to your desired patrol speed

            if (destinations.Length == 0)
            {
                Debug.LogError("No patrol destinations set!");
                return TaskState.Failure;
            }

            agent.SetDestination(destinations[currentDestinationIndex]);

            if (agent.remainingDistance < 0.5f)
            {
                // Arrived at the current destination, move to the next one
                currentDestinationIndex = (currentDestinationIndex + 1) % destinations.Length;
            }

            return TaskState.Running;
        }
    }
}
