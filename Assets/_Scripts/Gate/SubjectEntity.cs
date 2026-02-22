using UnityEngine;
using System.Collections;

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
    
    [Header("ID Card Setup")]
    public GameObject idCardPrefab;
    public Transform itemThrowPos;
    public Vector2 throwForce = new Vector2(0f, -5f);
    
    [Header("Return Mechanics")]
    [Tooltip("How close the card needs to be dropped to the throw pos to be caught")]
    public float receiveRadius = 1.5f;
    [Tooltip("The tag we use to identify the ID Card")]
    public string idCardTag = "IDCard";

    public bool IsProcessed { get; set; }
    public bool IsSliding => _isReturning;
    public bool HasReturnedID { get; private set; } = false;
    
    private bool _isReturning = false; 
    private Vector2 _returnVelocity; 
    private float _ignoreItemTimer = 0f; 

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
        if (_ignoreItemTimer > 0)
        {
            _ignoreItemTimer -= Time.fixedDeltaTime; 
        }
        else if (!HasReturnedID && !IsSliding)
        {
            CheckForReturnedItem();
        }
    }
    
    public void SetSortingOrder(int order)
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sortingOrder = order;
        }
        else
        {
            SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
            if (sr != null) sr.sortingOrder = order;
        }
    }

    private void HandleReturnToBase()
    {
        if (basePosition == null) return;

        if (!_isReturning && rb.linearVelocity.magnitude < 2f) _isReturning = true;

        if (_isReturning)
        {
            Vector2 nextPos = Vector2.SmoothDamp(rb.position, basePosition.position, ref _returnVelocity, returnSmoothTime, maxReturnSpeed, Time.fixedDeltaTime);
            rb.MovePosition(nextPos);

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
    
    public void MoveToNewSpot(Transform newSpot)
    {
        basePosition = newSpot;
        _isReturning = true; 
        
        if (rb != null) rb.WakeUp(); 
    }

    public void SetInteractable(bool isInteractable)
    {
        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = isInteractable;

        gameObject.tag = isInteractable ? "interactable" : "Untagged";
    }
    
    public GameObject TossIDCard()
    {
        if (idCardPrefab == null) return null;
        
        GameObject card = Instantiate(idCardPrefab, itemThrowPos.position, Quaternion.identity);

        if (card.TryGetComponent(out SpriteRenderer cardSprite))
        {
            cardSprite.sortingOrder = 50; 
        }

        if (card.TryGetComponent(out Rigidbody2D cardRb))
        {
            Vector2 massiveThrowForce = new Vector2(Random.Range(-5f, 5f), -25f);
            cardRb.linearVelocity = massiveThrowForce;
            cardRb.angularVelocity = Random.Range(-360f, 360f);
        }

        _ignoreItemTimer = 1.5f; 

        return card;
    }

    private void CheckForReturnedItem()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(itemThrowPos.position, receiveRadius);
        foreach (var hit in hits)
        {
            if (hit.CompareTag(idCardTag))
            {
                if (hit.TryGetComponent(out CameraObject camObj) && !camObj.IsBeingDragged)
                {
                    HasReturnedID = true;
                    StartCoroutine(AbsorbItemRoutine(camObj));
                    break;
                }
            }
        }
    }

    private IEnumerator AbsorbItemRoutine(CameraObject item)
    {
        item.enabled = false;
        
        if (item.TryGetComponent(out Rigidbody2D itemRb))
        {
            itemRb.isKinematic = true;
            itemRb.linearVelocity = Vector2.zero;
            itemRb.angularVelocity = 0f;
        }

        if (item.TryGetComponent(out Collider2D col)) col.enabled = false;

        Vector3 startPos = item.transform.position;
        Vector3 startScale = item.transform.localScale;
        float elapsed = 0f;
        float duration = 0.2f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            item.transform.position = Vector3.Lerp(startPos, itemThrowPos.position, t);
            item.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t); // Shrinks to 0
            
            yield return null;
        }

        Destroy(item.gameObject);
        Debug.Log($"<color=cyan>{gameObject.name} tucked their ID safely away!</color>");
    }
}