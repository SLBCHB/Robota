using UnityEngine;

public class SubjectEntity : CameraObject
{
    [Header("Position Constraints")]
    public Transform basePosition;
    public float maxY = 3f;
    public float minY = -3f;

    [Header("Game Feel Settings")]
    public float returnSmoothTime = 0.1f; 
    public float maxReturnSpeed = 100f;

    [Header("Grab Settings")]
    public Transform grabPoint;

    public bool IsProcessed { get; set; }
    
    private bool _isReturning = false; 
    private Vector2 _returnVelocity; 

    protected override void Start()
    {
        base.Start();
        if (spriteRenderer != null) dragSortingOrder = -4; 
        
        rb.linearDamping = 12f; 
        tableFriction = 12f;
    }

    public override void OnClick()
    {
        if (IsProcessed) return;
        _isReturning = false; 
    }

    protected override void FixedUpdate()
    {
        if (IsProcessed)
        {
            ClampVerticalPosition();
            return;
        }

        base.FixedUpdate();

        if (!IsBeingDragged)
        {
            HandleReturnToBase();
        }

        ClampVerticalPosition();
    }

    private void HandleReturnToBase()
    {
        if (basePosition == null) return;

        if (!_isReturning && rb.linearVelocity.magnitude < 2f) _isReturning = true;

        if (_isReturning)
        {
            rb.position = Vector2.SmoothDamp(rb.position, basePosition.position, ref _returnVelocity, returnSmoothTime, maxReturnSpeed, Time.fixedDeltaTime);

            if (Vector2.Distance(rb.position, basePosition.position) < 0.05f)
            {
                rb.position = basePosition.position; 
                rb.linearVelocity = Vector2.zero;
                _isReturning = false;
            }
        }
    }

    private void ClampVerticalPosition()
    {
        Vector2 pos = rb.position;
        bool didHitWall = false;

        if (pos.y > maxY) { pos.y = maxY; didHitWall = true; }
        else if (pos.y < minY) { pos.y = minY; didHitWall = true; }

        if (didHitWall)
        {
            rb.position = pos;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        }
    }

    protected override void MoveWithPhysics()
    {
        if (grabPoint == null)
        {
            base.MoveWithPhysics();
            return;
        }

        Vector2 screenPos = InputManager.Instance.MousePosition;
        float dist = Mathf.Abs(mainCam.transform.position.z);
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));
        
        Vector2 grabOffset = (Vector2)grabPoint.position - rb.position;
        Vector2 targetPosition = (Vector2)worldPos - grabOffset;
        Vector2 newVelocity = (targetPosition - rb.position) * (dragResponsiveness * 1.5f);

        rb.linearVelocity = Vector2.ClampMagnitude(newVelocity, maxDragVelocity); 
    }
}