using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class IntroText : MonoBehaviour
{
    public TMP_Text text;
    private string data;

    private string[] messages =
    {
        "System check complete.",
        "All systems are operational.",
        "No errors detected.",
        "System performance is optimal.",
        "All components are functioning within normal parameters.",
        "System diagnostics passed successfully.",
        "No issues found during system scan.",
        "System integrity verified.",
        "All subsystems are online and stable.",
        "System init completed.",
        "kokoko",
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        text.text += "\n[  <color=green>OK</color>  ][" + data + "]System init";

        StartCoroutine(UpdateText());

    }

    IEnumerator UpdateText()
    {
        for (int i = 0; i < messages.Length; i++)
        {
            int waitTime = UnityEngine.Random.Range(3, 15);
            data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            text.text += "\n[  <color=green>OK</color>  ][" + data + "]" + messages[i];
            yield return new WaitForSeconds(waitTime/10);
        }

        //here load next scene or something
    }
}
