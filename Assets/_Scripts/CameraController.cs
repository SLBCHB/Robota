using UnityEngine;
using Unity.Cinemachine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraController : MonoBehaviour
{   
    public static CameraController Instance; 

    [Header("Camera Settings")] 
    public Vector2 shiftStrength = new Vector2(2f, 2f);
    public float smoothSpeed = 5f;

    [Header("Cursor Settings")]
    public Canvas cursorCanvas; 
    public GameObject defaultCursorPrefab;
    public GameObject pointerCursorPrefab;
    public string interactableTag = "interactable";

    public event Action<GameObject> OnCameraClickEvent;

    private CinemachineCamera _vcam;
    private CinemachineFollow _followComponent;
    private Vector3 _initialOffset;

    private RectTransform _cursorDefaultRT;
    private RectTransform _cursorPointerRT;
    private Camera _mainCam;
    
    private GraphicRaycaster _uiRaycaster;
    private EventSystem _eventSystem;
    private PointerEventData _pointerEventData;
    private List<RaycastResult> _raycastResults = new List<RaycastResult>(); 

    private Vector2 _currentMousePos;
    private bool _isOverUI;
    private GameObject _hoveredWorldObject;

    private void Awake() => Instance = this;

    private void Start()
    {
        _vcam = GetComponent<CinemachineCamera>();
        _followComponent = _vcam.GetComponent<CinemachineFollow>();
        _mainCam = Camera.main;
        _uiRaycaster = cursorCanvas.GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
        
        _pointerEventData = new PointerEventData(_eventSystem);
        
        if (_followComponent != null)
            _initialOffset = _followComponent.FollowOffset;

        Cursor.visible = false;
        
        GameObject defCursorObj = Instantiate(defaultCursorPrefab, cursorCanvas.transform);
        GameObject ptrCursorObj = Instantiate(pointerCursorPrefab, cursorCanvas.transform);
        
        _cursorDefaultRT = defCursorObj.GetComponent<RectTransform>();
        _cursorPointerRT = ptrCursorObj.GetComponent<RectTransform>();

        _cursorDefaultRT.pivot = new Vector2(0f, 1f);
        _cursorPointerRT.pivot = new Vector2(0f, 1f);

        if (defCursorObj.TryGetComponent(out Image defImg)) defImg.raycastTarget = false;
        if (ptrCursorObj.TryGetComponent(out Image ptrImg)) ptrImg.raycastTarget = false;

        _cursorPointerRT.gameObject.SetActive(false);

        InputManager.Instance.CameraClickEvent += HandleInputClick;
    }

    private void OnDestroy()
    {
        if (InputManager.Instance != null)
            InputManager.Instance.CameraClickEvent -= HandleInputClick;
    }

    private void Update()
    {
        if (_followComponent == null || InputManager.Instance == null) return;

        _currentMousePos = GetClampedMousePos();
        _isOverUI = CheckUI(_currentMousePos);
        _hoveredWorldObject = GetWorldObjectUnderMouse(_currentMousePos);

        HandleCameraShift(_currentMousePos);
        UpdateCursorVisuals(_currentMousePos);
    }

    private void HandleInputClick()
    {
        if (_isOverUI)
        {
            if (_raycastResults.Count > 0)
            {
                GameObject clickedUI = _raycastResults[0].gameObject;
                ExecuteEvents.Execute(clickedUI, _pointerEventData, ExecuteEvents.pointerClickHandler);
                Debug.Log($"Manager: Clicked UI element {clickedUI.name}");
            }
            return; 
        }

        OnCameraClickEvent?.Invoke(_hoveredWorldObject);
        
        if (_hoveredWorldObject != null)
            Debug.Log($"Manager: Clicked on {_hoveredWorldObject.name}");
    }

    private void UpdateCursorVisuals(Vector2 mousePos)
    {
        _cursorDefaultRT.position = mousePos;
        _cursorPointerRT.position = mousePos;

        bool isOverInteractable = _hoveredWorldObject != null && _hoveredWorldObject.CompareTag(interactableTag);
        bool shouldShowPointer = _isOverUI || isOverInteractable;

        _cursorDefaultRT.gameObject.SetActive(!shouldShowPointer);
        _cursorPointerRT.gameObject.SetActive(shouldShowPointer);
    }
    
    private Vector2 GetClampedMousePos()
    {
        Vector2 raw = InputManager.Instance.MousePosition;
        return new Vector2(Mathf.Clamp(raw.x, 0, Screen.width), Mathf.Clamp(raw.y, 0, Screen.height));
    }

    private void HandleCameraShift(Vector2 mousePos)
    {
        float mouseX = (mousePos.x / Screen.width) * 2f - 1f;
        float mouseY = (mousePos.y / Screen.height) * 2f - 1f;
        Vector3 targetShift = new Vector3(mouseX * shiftStrength.x, mouseY * shiftStrength.y, 0f);
        _followComponent.FollowOffset = Vector3.Lerp(_followComponent.FollowOffset, _initialOffset + targetShift, Time.deltaTime * smoothSpeed);
    }

    private bool CheckUI(Vector2 mousePos)
    {
        if (_eventSystem == null) return false;
        
        _pointerEventData.position = mousePos;
        _raycastResults.Clear();
        _uiRaycaster.Raycast(_pointerEventData, _raycastResults);
        
        return _raycastResults.Count > 0;
    }

    private GameObject GetWorldObjectUnderMouse(Vector2 mousePos)
    {
        float dist = Mathf.Abs(_mainCam.transform.position.z);
        Vector3 worldPos = _mainCam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, dist));
        worldPos.z = 0;
        Collider2D hit = Physics2D.OverlapPoint(worldPos);
        return hit != null ? hit.gameObject : null;
    }
}