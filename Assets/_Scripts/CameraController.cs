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

    private GameObject _cursorDefault;
    private GameObject _cursorPointer;
    private Camera _mainCam;
    
    private GraphicRaycaster _uiRaycaster;
    private EventSystem _eventSystem;

    private void Awake() => Instance = this;

    private void Start()
    {
        _vcam = GetComponent<CinemachineCamera>();
        _followComponent = _vcam.GetComponent<CinemachineFollow>();
        _mainCam = Camera.main;
        _uiRaycaster = cursorCanvas.GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
        
        if (_followComponent != null)
            _initialOffset = _followComponent.FollowOffset;

        Cursor.visible = false;
        
        _cursorDefault = Instantiate(defaultCursorPrefab, cursorCanvas.transform);
        _cursorPointer = Instantiate(pointerCursorPrefab, cursorCanvas.transform);
        _cursorPointer.SetActive(false);

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

        Vector2 mousePos = GetClampedMousePos();
        HandleCameraShift(mousePos);
        UpdateCursorVisuals(mousePos);
    }

    private void HandleInputClick()
    {
        Vector2 mousePos = GetClampedMousePos();
        
        if (CheckUI(mousePos)) return;
        GameObject worldObject = GetWorldObjectUnderMouse(mousePos);
        OnCameraClickEvent?.Invoke(worldObject);
        
        if (worldObject != null)
            Debug.Log($"Manager: Clicked on {worldObject.name}");
    }

    private void UpdateCursorVisuals(Vector2 mousePos)
    {
        _cursorDefault.transform.position = mousePos;
        _cursorPointer.transform.position = mousePos;

        bool isOverUI = CheckUI(mousePos);
        GameObject worldObject = GetWorldObjectUnderMouse(mousePos);
        bool isOverInteractable = worldObject != null && worldObject.CompareTag(interactableTag);

        bool shouldShowPointer = isOverUI || isOverInteractable;
        _cursorDefault.SetActive(!shouldShowPointer);
        _cursorPointer.SetActive(shouldShowPointer);
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
        PointerEventData eventData = new PointerEventData(_eventSystem) { position = mousePos };
        List<RaycastResult> results = new List<RaycastResult>();
        _uiRaycaster.Raycast(eventData, results);
        
        foreach (var result in results)
        {
            if (result.gameObject != _cursorDefault && result.gameObject != _cursorPointer)
                return true;
        }
        return false;
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