using UnityEngine;
using Unity.Cinemachine;

public class CameraMoveController : MonoBehaviour
{
    [Header("Cameras")]
    [Tooltip("The camera used for the main Gate minigame")]
    public CinemachineCamera gateCamera;
    [Tooltip("The camera that pans around the CCTV screens")]
    public CinemachineCamera cctvCamera;

    [Header("CCTV Target")]
    [Tooltip("The empty GameObject that the CCTV Camera follows")]
    public Transform cctvTarget;
    public float panSpeed = 8f;

    [Header("CCTV Grid Positions")]
    public Transform topLeftPos;
    public Transform topRightPos;
    public Transform bottomLeftPos;
    public Transform bottomRightPos;

    [Header("Room Zoom Sizes")]
    public float topLeftSize = 5f;
    public float topRightSize = 5f;
    public float bottomLeftSize = 5f;
    public float bottomRightSize = 5f;

    [Header("UI Buttons")]
    public GameObject upButton;
    public GameObject downButton;
    public GameObject leftButton;
    public GameObject rightButton;

    public bool isAtGate = true; 

    private Transform _currentPos;
    private float _targetOrthoSize;

    private void Start()
    {
        if (bottomLeftPos != null)
        {
            _currentPos = bottomLeftPos;
            if (cctvTarget != null) cctvTarget.position = _currentPos.position;
            UpdateTargetSize();
            if (cctvCamera != null) cctvCamera.Lens.OrthographicSize = _targetOrthoSize;
        }

        SetGateView(true);
    }

    private void Update()
    {
        if (cctvTarget != null && _currentPos != null)
        {
            cctvTarget.position = Vector3.Lerp(cctvTarget.position, _currentPos.position, Time.deltaTime * panSpeed);
        }

        if (cctvCamera != null)
        {
            cctvCamera.Lens.OrthographicSize = Mathf.Lerp(cctvCamera.Lens.OrthographicSize, _targetOrthoSize, Time.deltaTime * panSpeed);
        }
    }

    public void ToggleGateCCTV()
    {
        SetGateView(!isAtGate);
    }

    private void SetGateView(bool toGate)
    {
        isAtGate = toGate;
        
        if (gateCamera != null) gateCamera.Priority = toGate ? 10 : 0;
        if (cctvCamera != null) cctvCamera.Priority = toGate ? 0 : 10;
        
        UpdateButtonVisibility();
    }


    public void MoveUp()
    {
        if (_currentPos == bottomLeftPos) _currentPos = topLeftPos;
        else if (_currentPos == bottomRightPos) _currentPos = topRightPos;
        UpdateRoomState();
    }

    public void MoveDown()
    {
        if (_currentPos == topLeftPos) _currentPos = bottomLeftPos;
        else if (_currentPos == topRightPos) _currentPos = bottomRightPos;
        UpdateRoomState();
    }

    public void MoveLeft()
    {
        if (_currentPos == topRightPos) _currentPos = topLeftPos;
        else if (_currentPos == bottomRightPos) _currentPos = bottomLeftPos;
        UpdateRoomState();
    }

    public void MoveRight()
    {
        if (_currentPos == topLeftPos) _currentPos = topRightPos;
        else if (_currentPos == bottomLeftPos) _currentPos = bottomRightPos;
        UpdateRoomState();
    }

    private void UpdateRoomState()
    {
        UpdateButtonVisibility();
        UpdateTargetSize();
    }

    private void UpdateTargetSize()
    {
        if (_currentPos == topLeftPos) _targetOrthoSize = topLeftSize;
        else if (_currentPos == topRightPos) _targetOrthoSize = topRightSize;
        else if (_currentPos == bottomLeftPos) _targetOrthoSize = bottomLeftSize;
        else if (_currentPos == bottomRightPos) _targetOrthoSize = bottomRightSize;
    }

    private void UpdateButtonVisibility()
    {
        if (isAtGate)
        {
            if (upButton != null) upButton.SetActive(false);
            if (downButton != null) downButton.SetActive(false);
            if (leftButton != null) leftButton.SetActive(false);
            if (rightButton != null) rightButton.SetActive(false);
            return;
        }

        if (upButton != null) upButton.SetActive(_currentPos == bottomLeftPos || _currentPos == bottomRightPos);
        if (downButton != null) downButton.SetActive(_currentPos == topLeftPos || _currentPos == topRightPos);
        if (leftButton != null) leftButton.SetActive(_currentPos == topRightPos || _currentPos == bottomRightPos);
        if (rightButton != null) rightButton.SetActive(_currentPos == topLeftPos || _currentPos == bottomLeftPos);
    }
}