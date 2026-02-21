using UnityEngine;

public class Keycard : CameraObject
{
    [Header("Card Properties")]
    public bool isValidCard = true;

    private CardScanner _activeScanner;
    private bool _isSnapped = false;

    public override void OnClick()
    {
        Debug.Log("Grabbed the Keycard!");
    }

    protected override void Update()
    {
        base.Update();
        
        // FIXED: Capitalized IsBeingDragged
        if (!IsBeingDragged && _isSnapped)
        {
            BreakSnap();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // FIXED: Capitalized IsBeingDragged
        if (IsBeingDragged && !_isSnapped)
        {
            if (other.TryGetComponent(out CardScanner scanner))
            {
                _activeScanner = scanner;
                _isSnapped = true;
                _activeScanner.ResetScanner();
                
                rb.angularVelocity = 0f; 
                
                if (spriteRenderer != null && _activeScanner.TryGetComponent(out SpriteRenderer scannerSprite))
                {
                    spriteRenderer.sortingOrder = scannerSprite.sortingOrder - 1;
                }
            }
        }
    }

    protected override void MoveWithPhysics()
    {
        if (_isSnapped && _activeScanner != null)
        {
            HandleSwipingPhysics();
        }
        else
        {
            base.MoveWithPhysics();
        }
    }

    protected override void HandleSwinging()
    {
        if (_isSnapped) return; 
        
        base.HandleSwinging();
    }

    private void HandleSwipingPhysics()
    {
        Vector2 screenPos = InputManager.Instance.MousePosition;
        float dist = Mathf.Abs(mainCam.transform.position.z);
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));
        Vector2 mousePos = new Vector2(worldPos.x, worldPos.y);

        Vector2 start = _activeScanner.slotStart.position;
        Vector2 end = _activeScanner.slotEnd.position;
        Vector2 slotDir = (end - start).normalized;
        float slotLength = Vector2.Distance(start, end);

        Vector2 startToMouse = mousePos - start;
        float dotProduct = Vector2.Dot(startToMouse, slotDir);

        Vector2 projectedTargetPos = start + slotDir * Mathf.Clamp(dotProduct, 0, slotLength);

        if (Vector2.Distance(mousePos, projectedTargetPos) > _activeScanner.breakDistance)
        {
            BreakSnap();
            base.MoveWithPhysics(); 
            return;
        }

        Vector2 difference = projectedTargetPos - rb.position;
        rb.linearVelocity = difference * dragResponsiveness;

        rb.MoveRotation(_activeScanner.transform.rotation.eulerAngles.z);

        float progress = dotProduct / slotLength;
        _activeScanner.CheckSwipeProgress(progress, isValidCard);
    }

    private void BreakSnap()
    {
        _isSnapped = false;
        _activeScanner = null;

        // FIXED: Capitalized IsBeingDragged
        if (spriteRenderer != null && IsBeingDragged)
        {
            spriteRenderer.sortingOrder = dragSortingOrder;
        }
    }
}