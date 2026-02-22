using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ScannerController : MonoBehaviour
{
    public static ScannerController Instance;

    [Header("Movement Settings")]
    public float rotationSpeed = 15f;
    [Tooltip("If the light points sideways, change this. Usually 0, 90, or -90.")]
    public float angleOffset = 90f; 
    [Tooltip("The angle it returns to when put away.")]
    public float idleAngle = -45f;

    [Header("Scan Mechanic Setup")]
    public LayerMask scannableLayer;

    private Camera _mainCam;
    private bool _isActive = false;
    
    private Light2D[] _scannerLights;

    private GameObject _currentlyHoveredPart;
    private float _hoverTimer = 0f;

    private void Awake() 
    {
        Instance = this;
        _scannerLights = GetComponentsInChildren<Light2D>(true);
    }

    private void Start()
    {
        _mainCam = Camera.main;
        SetActiveState(false);
    }

    private void Update()
    {
        if (_isActive)
        {
            AimAtMouse();
            CheckForScannables();
        }
        else
        {
            ReturnToIdle();
        }
    }

    private void AimAtMouse()
    {
        Vector2 screenPos = InputManager.Instance.MousePosition;
        float dist = Mathf.Abs(_mainCam.transform.position.z);
        Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));
        
        Vector2 direction = (Vector2)worldPos - (Vector2)transform.position;
        
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        targetAngle += angleOffset;

        Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    private void ReturnToIdle()
    {
        Quaternion idleRotation = Quaternion.Euler(0, 0, idleAngle);
        transform.rotation = Quaternion.Lerp(transform.rotation, idleRotation, Time.deltaTime * (rotationSpeed / 2f));
    }

    private void CheckForScannables()
    {
        Vector2 screenPos = InputManager.Instance.MousePosition;
        float dist = Mathf.Abs(_mainCam.transform.position.z);
        Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, dist));

        Collider2D hit = Physics2D.OverlapPoint(worldPos, scannableLayer);

        if (hit != null)
        {
            if (_currentlyHoveredPart != hit.gameObject)
            {
                _currentlyHoveredPart = hit.gameObject;
                _hoverTimer = 0f; 
                Debug.Log($"<color=cyan>Scanner beam hitting: {_currentlyHoveredPart.name}</color>");
            }
            else
            {
                _hoverTimer += Time.deltaTime;
                
                if (_hoverTimer >= 1f && _hoverTimer < 1.05f) 
                {
                    Debug.Log($"<color=yellow>Scanned {_currentlyHoveredPart.name} for 1 second!</color>");
                }
            }
        }
        else
        {
            if (_currentlyHoveredPart != null)
            {
                Debug.Log($"<color=grey>Scanner beam left {_currentlyHoveredPart.name}</color>");
                _currentlyHoveredPart = null;
                _hoverTimer = 0f;
            }
        }
    }

    public void SetActiveState(bool active)
    {
        _isActive = active;
        
        if (_scannerLights != null)
        {
            foreach (var light in _scannerLights)
            {
                if (light != null) light.enabled = active;
            }
        }
    }
}