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

    public Transform desk;
    public Transform goal;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;
    public bool reachedGoal = false;
    public bool agentisstoped;

    public float detectionRange = 0.1f;

    // Add a 'flip' variable to control sprite flipping. 
    // You may want to set this based on your logic, here it's set to false by default.
    private bool flip = false;


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
                gameObject.GetComponent<Robotnik>().dir = RobotnikDirection.Side;
                gameObject.GetComponent<Robotnik>().setVizual();
                Transform spriteRoot = gameObject.GetComponentInChildren<RobotnikSpriteController>().transform;

                

               
                Debug.Log("Stopping");
            }
        }
        else if (state == State.Stopped)
        {
            if (Vector2.Distance(transform.position, goal.position) >= detectionRange)
            {
                gameObject.GetComponent<Robotnik>().dir = RobotnikDirection.Front;
                gameObject.GetComponent<Robotnik>().setVizual();
                Transform spriteRoot = gameObject.GetComponentInChildren<RobotnikSpriteController>().transform;

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