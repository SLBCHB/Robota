using TMPro;
using UnityEngine;
using System;
using System.Collections;

public class IntroText : MonoBehaviour
{
    public TMP_Text text;
    public TMP_Text textDay;
    private string data;
    private int day;


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
        "Finished",
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        day = DifficultyManager.Instance.dayCounter;
        data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        text.text += "\n[  <color=green>OK</color>  ][" + data + "]System init";

        StartCoroutine(UpdateText());



    }

    IEnumerator UpdateText()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < messages.Length; i++)
        {
            int waitTime = UnityEngine.Random.Range(3, 15);
            data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            text.text += "\n[  <color=green>OK</color>  ][" + data + "]" + messages[i];
            yield return new WaitForSeconds(waitTime/10);
        }

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(SceneController.Instance.Fade(1f));
        text.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        
        SceneController.Instance._fadeCanvasGroup.alpha = 0f;
        textDay.text = "Day " + (day + 1);
        textDay.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(SceneController.Instance.Fade(1f));
        yield return new WaitForSeconds(1f);
        textDay.gameObject.SetActive(false);


        SceneController.Instance.LoadScene(GameScene.Map);
    }

}
