using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class CardScanner : CameraObject
{
    [Header("Scanner Settings")]
    public Transform slotStart; 
    public Transform slotEnd;   

    public float breakDistance = 1.5f;
    public float swipeThreshold = 0.85f;

    [Header("LED Feedback")] public GameObject grantedLed;
    public GameObject deniedLed;
    public int blinkCount = 3;
    public float blinkInterval = 0.15f;

    [Header("Events")]
    public UnityEvent OnScanSuccess;
    public UnityEvent OnScanFailed;

    private bool _hasScannedThisSwipe = false;
    private Coroutine _activeBlinkRoutine;
    
    protected override void Start()
    {
        base.Start();
        rb.bodyType = RigidbodyType2D.Static;

        if (grantedLed != null) grantedLed.SetActive(false);
        if (deniedLed != null) deniedLed.SetActive(false);
    }
    
    public override void OnClick() { }

    public void CheckSwipeProgress(float progressPercentage, bool isValidCard)
    {
        if (!_hasScannedThisSwipe && progressPercentage >= swipeThreshold)
        {
            _hasScannedThisSwipe = true;

            if (_activeBlinkRoutine != null) StopCoroutine(_activeBlinkRoutine);

            if (isValidCard)
            {
                Debug.Log("<color=green>ACCESS GRANTED!</color>");
                _activeBlinkRoutine = StartCoroutine(BlinkLedRoutine(grantedLed));
                OnScanSuccess?.Invoke();
            }
            else
            {
                Debug.Log("<color=red>ACCESS DENIED!</color>");
                _activeBlinkRoutine = StartCoroutine(BlinkLedRoutine(deniedLed));
                OnScanFailed?.Invoke();
            }
        }
    }

    public void ResetScanner()
    {
        _hasScannedThisSwipe = false;
    }

    private IEnumerator BlinkLedRoutine(GameObject targetLed)
    {
        if (targetLed == null) yield break;
        
        if (grantedLed != null) grantedLed.SetActive(false);
        if (deniedLed != null) deniedLed.SetActive(false);

        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < blinkCount; i++)
        {
            targetLed.SetActive(true);
            yield return new WaitForSeconds(blinkInterval);
            
            targetLed.SetActive(false);
            yield return new WaitForSeconds(blinkInterval);
        }
    }
}