using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;

[RequireComponent(typeof(Light2D))]
public class AlarmLight : MonoBehaviour
{
    [Header("Flicker Settings")]
    [Tooltip("How many times the light flashes on and off")]
    public int flashCount = 3;
    [Tooltip("How fast the strobe effect is (lower is faster)")]
    public float flashSpeed = 0.1f;
    [Tooltip("How bright the light gets when it flashes")]
    public float flashIntensity = 2.5f;

    private Light2D _light;

    private void Awake()
    {
        _light = GetComponent<Light2D>();
        _light.intensity = 0f;
    }

    public void TriggerAlarm()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            _light.intensity = flashIntensity; 
            yield return new WaitForSeconds(flashSpeed);
            
            _light.intensity = 0f; 
            yield return new WaitForSeconds(flashSpeed);
        }
        
        _light.intensity = 0f;
    }
}