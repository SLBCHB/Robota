using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class CameraObject : MonoBehaviour
{
    [Header("Physics & Dragging")]
    public float dragResponsiveness = 15f;
    public float maxDragVelocity = 50f;
    public float tableFriction = 5f;

    [Header("Rotation & Swinging")]
    public float swingSensitivity = 2f;
    public float maxSwingAngle = 45f;
    public float selfRightingSpeed = 5f;

    [Header("Visuals")]
    public int dragSortingOrder = 999;

    protected Rigidbody2D rb;
    protected Camera mainCam;
    
    public bool IsBeingDragged { get; protected set; }
    
    protected SpriteRenderer spriteRenderer;
    private int defaultSortingOrder;
    private float defaultRotation;
    
    

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>(); 
        mainCam = Camera.main;

        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        
        rb.gravityScale = 0f; 
        rb.linearDamping = tableFriction; 

        defaultRotation = rb.rotation;

        if (spriteRenderer != null)
        {
            defaultSortingOrder = spriteRenderer.sortingOrder; 
        }

        if (CameraController.Instance != null)
        {
            CameraController.Instance.OnCameraClickEvent += HandleGlobalClick;
        }
    }

    protected virtual void OnDestroy()
    {
        if (CameraController.Instance != null)
            CameraController.Instance.OnCameraClickEvent -= HandleGlobalClick;
    }

    private void HandleGlobalClick(GameObject clickedObject)
    {
        if (ClawController.Instance != null) ClawController.Instance.SetGrabState(true);

        if (clickedObject == gameObject)
        {
            IsBeingDragged = true;
            rb.linearDamping = 10f; 
        
            if (spriteRenderer != null) spriteRenderer.sortingOrder = dragSortingOrder;
            
            OnClick(); 
        }
    }

    protected virtual void Update()
    {
        if (!IsBeingDragged) return;

        if (!InputManager.Instance.IsLeftClickHeld) 
        {
            IsBeingDragged = false;
            rb.linearDamping = tableFriction; 
        
            if (spriteRenderer != null) spriteRenderer.sortingOrder = defaultSortingOrder;
        
            if (ClawController.Instance != null) ClawController.Instance.SetGrabState(false);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (IsBeingDragged)
        {
            MoveWithPhysics();
            HandleSwinging();
        }
        else
        {
            HandleSelfRighting();
        }
    }

    protected virtual void MoveWithPhysics()
    {
        Vector2 screenPos = InputManager.Instance.MousePosition;
        float dist = Mathf.Abs(mainCam.transform.position.z);
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));
        
        Vector2 targetPosition = new Vector2(worldPos.x, worldPos.y);
        Vector2 difference = targetPosition - rb.position;
        Vector2 newVelocity = difference * dragResponsiveness;

        rb.linearVelocity = Vector2.ClampMagnitude(newVelocity, maxDragVelocity); 
    }

    protected virtual void HandleSwinging()
    {
        float targetAngle = -rb.linearVelocity.x * swingSensitivity;
        targetAngle = Mathf.Clamp(targetAngle, -maxSwingAngle, maxSwingAngle);

        float smoothedAngle = Mathf.LerpAngle(rb.rotation, targetAngle, Time.fixedDeltaTime * 10f);
        rb.MoveRotation(smoothedAngle);
        rb.angularVelocity = 0f;
    }

    private void HandleSelfRighting()
    {
        float uprightAngle = Mathf.LerpAngle(rb.rotation, defaultRotation, Time.fixedDeltaTime * selfRightingSpeed);
        rb.MoveRotation(uprightAngle);
        rb.angularVelocity = Mathf.Lerp(rb.angularVelocity, 0f, Time.fixedDeltaTime * selfRightingSpeed);
    }
    
    public abstract void OnClick();
}