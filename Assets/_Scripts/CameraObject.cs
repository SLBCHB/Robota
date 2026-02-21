using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public abstract class CameraObject : MonoBehaviour
{
    [Header("Shared Parameters")]
    public string itemName;
    public float dragSpeed = 10f;
    public float dropGravityScale = 1f;

    protected Rigidbody2D rb;
    protected bool isDragging = false;
    protected Camera mainCam;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        mainCam = Camera.main;
        
        rb.gravityScale = dropGravityScale;

        if (CameraController.Instance != null)
        {
            CameraController.Instance.OnCameraClickEvent += HandleGlobalClick;
        }
    }

    protected virtual void OnDestroy()
    {
        if (CameraController.Instance != null)
        {
            CameraController.Instance.OnCameraClickEvent -= HandleGlobalClick;
        }
    }

    private void HandleGlobalClick(GameObject clickedObject)
    {
        if (clickedObject == gameObject)
        {
            OnClick();
        }
    }

    protected virtual void OnMouseDrag()
    {
        isDragging = true;
        rb.bodyType = RigidbodyType2D.Kinematic;
        
        Vector3 mousePos = InputManager.Instance.MousePosition;
        Vector3 worldPos = mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Mathf.Abs(mainCam.transform.position.z)));
        worldPos.z = 0;

        transform.position = Vector3.Lerp(transform.position, worldPos, Time.deltaTime * dragSpeed);
    }

    protected virtual void OnMouseUp()
    {
        isDragging = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = dropGravityScale;
    }

    public abstract void OnClick();
}