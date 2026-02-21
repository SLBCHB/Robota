using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    public enum State
    {
        Moving,
        Stopped
    }
    public State state;

    public Transform goal;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    public bool reachedGoal = false;
    public bool agentisstoped;

    public float detectionRange = 0.1f;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        obstacle = GetComponent<NavMeshObstacle>();
        state = State.Moving;
    }

    void Update()
    {
        if (state == State.Moving)
        {
            agent.SetDestination(goal.position);
            if (Vector2.Distance(transform.position, goal.position) < detectionRange)
            {
                state = State.Stopped;
                agent.enabled = false;
                obstacle.enabled = true;
                Debug.Log("Stopping");
            }
        }
        else if (state == State.Stopped)
        {
            if (Vector2.Distance(transform.position, goal.position) >= detectionRange)
            {
                state = State.Moving;
                obstacle.enabled = false;
                agent.enabled = true;
                Debug.Log("Resuming movement.");
            }
        }
    }

   /* private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FinishLine"))
        {
            reachedGoal = true;
            agent.isStopped = true;
            Debug.Log("Goal Reached!");
        }
    }*/
}