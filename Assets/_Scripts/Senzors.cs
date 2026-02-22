using UnityEngine;
using UnityEngine.UI;

public class Senzors : MonoBehaviour
{
    public Button[] stoly;
    public Button[] zachody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Button sensor in stoly)
        {
            sensor.GetComponent<Image>().color = Color.red;
        }

        foreach (Button sensor in zachody)
        {
            sensor.GetComponent<Image>().color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
