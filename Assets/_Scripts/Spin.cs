using UnityEngine;
using UnityEngine.UI;

public class Spin : MonoBehaviour
{
    public Image image;
    public float loopDuration = 1.5f;

    private float time = 0f;

    void Update()
    {
        time += Time.deltaTime;
        float t = (time % loopDuration) / loopDuration;
        float smoothProgress = (1f - Mathf.Cos(Mathf.PI * t)) / 2f;
        float angle = smoothProgress * 360f;
        image.rectTransform.localEulerAngles = new Vector3(0, 0, -angle);
    }
}