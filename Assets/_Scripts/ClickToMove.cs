using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    public bool reachedGoal = false;
    public bool agentisstoped;

    public float detectionRange = 1.2f;
    public float startOffset = 0.6f;

    private Vector2 lastLookDir = Vector2.right;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;

    }

    void Update()
    {
        agent.isStopped = agentisstoped;

        if (reachedGoal) return;
        if (Vector2.Distance(transform.position, goal.position) < 0.1f)
        {
            reachedGoal = true;
            agent.enabled = false;
            obstacle.enabled = true;
            agent.isStopped = true;
            Debug.Log("Goal Reached!");
            return;
        }
        agent.SetDestination(goal.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            reachedGoal = true;
            agent.isStopped = true;
            Debug.Log("Goal Reached!");
        }
    }
}