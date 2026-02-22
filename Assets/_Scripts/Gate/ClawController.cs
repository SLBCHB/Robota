using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ClawController : MonoBehaviour
{
    public static ClawController Instance;

    [Header("Claw Sprites")]
    public Sprite releasedSprite;
    public Sprite grabbedSprite;
    
    [Header("Sorting Layers (Z-Index)")]
    public int releasedSortingOrder = -5; 
    public int grabbedSortingOrder = 100;

    [Header("Positioning & Movement")]
    public Vector2 handleOffset;
    [Tooltip("An Empty GameObject placed above the screen where the claw rests.")]
    public Transform idlePosition; 
    [Tooltip("How fast the claw retracts when you look down at the table.")]
    public float retractSpeed = 25f;

    private SpriteRenderer _spriteRenderer;
    private Camera _mainCam;
    private bool _isActive = false;

    private void Awake() => Instance = this;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _mainCam = Camera.main;
        
        SetGrabState(false);

        if (idlePosition != null)
        {
            transform.position = idlePosition.position;
        }
    }

    private void Update()
    {
        if (_isActive)
        {
            Vector2 screenPos = InputManager.Instance.MousePosition;
            float dist = Mathf.Abs(_mainCam.transform.position.z);
            Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));
            
            transform.position = new Vector3(worldPos.x + handleOffset.x, worldPos.y + handleOffset.y, transform.position.z);
        }
        else
        {
            if (idlePosition != null)
            {
                transform.position = Vector3.MoveTowards(transform.position, idlePosition.position, retractSpeed * Time.deltaTime);
            }
        }
    }

    public void SetGrabState(bool isGrabbing)
    {
        _spriteRenderer.sprite = isGrabbing ? grabbedSprite : releasedSprite;
       // _spriteRenderer.sortingOrder = isGrabbing ? grabbedSortingOrder : releasedSortingOrder;
    }

    public void SetActiveState(bool active)
    {
        _isActive = active;
    }
}