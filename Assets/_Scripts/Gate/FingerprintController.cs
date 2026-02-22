using UnityEngine;

public class FingerprintController : MonoBehaviour
{
    public static FingerprintController Instance;

    [Header("Slide Positions")]
    [Tooltip("Empty GameObject where the scanner sits when you are using it.")]
    public Transform inPosition;
    [Tooltip("Empty GameObject off-screen to the right where it hides.")]
    public Transform outPosition;

    [Header("Movement Settings")]
    [Tooltip("How fast it slides in and out. Higher is faster.")]
    public float slideSpeed = 8f;

    private bool _isActive = false;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (outPosition != null)
        {
            transform.position = outPosition.position;
        }
    }

    private void Update()
    {
        if (inPosition == null || outPosition == null) return;

        Vector3 targetPos = _isActive ? inPosition.position : outPosition.position;

        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * slideSpeed);
    }

    public void SetActiveState(bool active)
    {
        _isActive = active;
    }
}