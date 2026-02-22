using System.Collections.Generic;
using UnityEngine;

public class FidgetSpinnerBlade : MonoBehaviour
{
    [Header("Spin Settings")]
    public float spinFriction = 0.5f; 
    public float maxSpinSpeed = 2000f; 
    
    [Header("Drag Settings")]
    [Tooltip("If you click this close to the center, it drags instead of spins!")]
    public float centerDragRadius = 0.6f;

    private bool _isBeingSpun = false;
    private float _angularVelocity = 0f;
    private Vector2 _lastMouseDir;
    private Camera _mainCam;

    private void Start()
    {
        _mainCam = Camera.main;
        
        if (CameraController.Instance != null)
        {
            CameraController.Instance.OnCameraClickEvent += HandleClick;
        }
    }

    private void OnDestroy()
    {
        if (CameraController.Instance != null)
        {
            CameraController.Instance.OnCameraClickEvent -= HandleClick;
        }
    }

    private void HandleClick(List<GameObject> clickedObj)
    {

        foreach (GameObject clk in clickedObj)
        {

            if (clk == gameObject)
            {
                Vector2 worldPos = GetMouseWorldPos();
                float distToCenter = Vector2.Distance(worldPos, transform.parent.position);

                // Check if we actually clicked near the center dot
                if (distToCenter <= centerDragRadius)
                {
                    // Pass the interaction to the parent so we drag the toy!
                    CameraObject parentDrag = transform.parent.GetComponent<CameraObject>();
                    if (parentDrag != null)
                    {
                        parentDrag.OnClick();
                    }
                }
                else
                {
                    // We clicked the outer arms, start spinning!
                    _isBeingSpun = true;
                    _lastMouseDir = GetMouseDir(worldPos);
                    _angularVelocity = 0f;
                }
            }
        }
    }

    private void Update()
    {
        if (_isBeingSpun)
        {
            // Stop spinning if we let go of the mouse
            if (!InputManager.Instance.IsLeftClickHeld)
            {
                _isBeingSpun = false;
                return;
            }

            Vector2 worldPos = GetMouseWorldPos();
            Vector2 currentDir = GetMouseDir(worldPos);
            
            float angleDelta = Vector2.SignedAngle(_lastMouseDir, currentDir);
            transform.Rotate(Vector3.forward, angleDelta);
            
            float instantVelocity = angleDelta / Time.deltaTime;
            _angularVelocity = Mathf.Lerp(_angularVelocity, instantVelocity, Time.deltaTime * 15f);
            _angularVelocity = Mathf.Clamp(_angularVelocity, -maxSpinSpeed, maxSpinSpeed);
            
            _lastMouseDir = currentDir;
        }
        else
        {
            // Freewheeling
            if (Mathf.Abs(_angularVelocity) > 0.1f)
            {
                transform.Rotate(Vector3.forward, _angularVelocity * Time.deltaTime);
                _angularVelocity = Mathf.Lerp(_angularVelocity, 0f, spinFriction * Time.deltaTime);
            }
            else
            {
                _angularVelocity = 0f;
            }
        }
    }

    private Vector2 GetMouseWorldPos()
    {
        Vector2 screenPos = InputManager.Instance.MousePosition;
        float dist = Mathf.Abs(_mainCam.transform.position.z);
        Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));
        return new Vector2(worldPos.x, worldPos.y);
    }

    private Vector2 GetMouseDir(Vector2 worldPos)
    {
        return (worldPos - (Vector2)transform.parent.position).normalized;
    }
}