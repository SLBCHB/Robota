using UnityEngine;
using Unity.Cinemachine; 

public class CameraMoveController : MonoBehaviour
{
    [Header("Camera Target")]
    public Transform cameraTarget;
    public CinemachineCamera vcam; 
    public float panSpeed = 8f;

    [Header("Grid Positions")]
    public Transform topLeftPos;
    public Transform topRightPos;
    public Transform bottomLeftPos;
    public Transform bottomRightPos;

    [Header("Room Zoom Sizes (Orthographic Size)")]
    public float topLeftSize = 5f;
    public float topRightSize = 5f;
    public float bottomLeftSize = 5f;
    public float bottomRightSize = 5f;

    [Header("UI Buttons")]
    public GameObject upButton;
    public GameObject downButton;
    public GameObject leftButton;
    public GameObject rightButton;

    private Transform _currentPos;
    private float _targetOrthoSize;

    private void Start()
    {
        if (bottomLeftPos != null)
        {
            _currentPos = bottomLeftPos;
            
            if (cameraTarget != null) cameraTarget.position = _currentPos.position;
            
            UpdateTargetSize();
            if (vcam != null) vcam.Lens.OrthographicSize = _targetOrthoSize;
            
            UpdateButtonVisibility(); 
        }
        else
        {
            Debug.LogError("<color=red>ERROR: Bottom Left Pos is missing in the Inspector!</color>");
        }
    }

    private void Update()
    {
        if (cameraTarget != null && _currentPos != null)
        {
            cameraTarget.position = Vector3.Lerp(cameraTarget.position, _currentPos.position, Time.deltaTime * panSpeed);
        }

        if (vcam != null)
        {
            vcam.Lens.OrthographicSize = Mathf.Lerp(vcam.Lens.OrthographicSize, _targetOrthoSize, Time.deltaTime * panSpeed);
        }
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
        if (upButton != null) 
            upButton.SetActive(_currentPos == bottomLeftPos || _currentPos == bottomRightPos);
            
        if (downButton != null) 
            downButton.SetActive(_currentPos == topLeftPos || _currentPos == topRightPos);
            
        if (leftButton != null) 
            leftButton.SetActive(_currentPos == topRightPos || _currentPos == bottomRightPos);
            
        if (rightButton != null) 
            rightButton.SetActive(_currentPos == topLeftPos || _currentPos == bottomLeftPos);
    }
}