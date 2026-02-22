using UnityEngine;
using UnityEngine.AI;

public class SquishWalk : MonoBehaviour
{
    public NavMeshAgent agent;

    public float bounceHeight = 0.3f;
    public float squishAmount = 0.2f;
    public float tiltAngle = 10f;
    public float multiplier = 1f;

    private Vector3 startScale;
    private Vector3 startLocalPos; // New: Store the offset
    private float timer;
    private float walkSpeed;

    void Start()
    {
        
        // Get agent from parent if not assigned
        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        // Record initial states
        startScale = transform.localScale;
        startLocalPos = transform.localPosition;
    }

    

    private void kokot(bool isPaused)
    {
        if (isPaused)
        {
            // Reset to original state when paused
            transform.localPosition = startLocalPos;
            transform.localScale = startScale;
            transform.localRotation = Quaternion.identity;
            timer = 0f; // Reset timer to avoid weird jumps when resuming
        }
    }

    void Update()
    {
        walkSpeed = agent.velocity.magnitude;

        if (walkSpeed > 0.1f)
        {
            // --- MOVING STATE ---
            timer += Time.deltaTime * walkSpeed * multiplier;

            float yOffset = Mathf.Abs(Mathf.Sin(timer)) * bounceHeight;
            float squish = Mathf.Sin(timer * 2f) * squishAmount;
            float tilt = Mathf.Sin(timer * 0.5f) * tiltAngle;

            // Apply movement relative to startLocalPos instead of zero
            transform.localPosition = new Vector3(
                startLocalPos.x,
                startLocalPos.y + yOffset,
                startLocalPos.z
            );

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
            // Lerp back to startLocalPos instead of Vector3.zero
            transform.localPosition = Vector3.Lerp(transform.localPosition, startLocalPos, Time.deltaTime * 5f);
            transform.localScale = Vector3.Lerp(transform.localScale, startScale, Time.deltaTime * 5f);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 5f);

            // Reset timer slowly so the next walk starts fresh
            timer = Mathf.Lerp(timer, 0, Time.deltaTime * 5f);
        }
    }
}