using UnityEngine;
using UnityEngine.AI;

public class SquishWalk : MonoBehaviour
{
    public NavMeshAgent agent;

    public float walkSpeed = 10f;
    public float bounceHeight = 0.3f;

    public float squishAmount = 0.2f;

    public float tiltAngle = 10f; // sidetoside

    private Vector3 startScale;
    private float timer;

    void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        startScale = transform.localScale;
    }

    void Update()
    {
        walkSpeed = agent.velocity.magnitude;

        if (walkSpeed > 0.1f)
        {
            // --- MOVING STATE ---
            timer += Time.deltaTime * walkSpeed;

            float yOffset = Mathf.Abs(Mathf.Sin(timer)) * bounceHeight;
            float squish = Mathf.Sin(timer * 2f) * squishAmount;
            float tilt = Mathf.Sin(timer * 0.5f) * tiltAngle;

            // Apply movement
            transform.localPosition = new Vector3(transform.localPosition.x, yOffset, transform.localPosition.z);

            transform.localScale = new Vector3(
                startScale.x - squish,
                startScale.y + squish,
                startScale.z
            );

            transform.localRotation = Quaternion.Euler(0, 0, tilt);
        }
        else
        {
            // --- STOPPING STATE ---
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, Time.deltaTime * 5f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);

        }
    }
}