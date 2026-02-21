using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public Transform goal;
    private NavMeshAgent agent;
    private bool reachedGoal = false;

    public float detectionRange = 1.2f;
    public float startOffset = 0.6f;

    private Vector2 lastLookDir = Vector2.right;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (reachedGoal) return;
       
        if (agent.velocity.sqrMagnitude > 0.01f)
        {
            lastLookDir = agent.velocity.normalized;
        }
        else if (!agent.isStopped)
        {
            
            lastLookDir = (goal.position - transform.position).normalized;
        }

        Vector2 rayStart = (Vector2)transform.position + (lastLookDir * startOffset);

        RaycastHit2D hit = Physics2D.Raycast(rayStart, lastLookDir, detectionRange);

        Debug.DrawRay(rayStart, lastLookDir * detectionRange, Color.cyan);

        if (hit.collider != null && hit.collider.gameObject != this.gameObject && hit.collider.CompareTag("agent"))
        {
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
        }
        else
        {
            agent.isStopped = false;
            agent.SetDestination(goal.position);
        }
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