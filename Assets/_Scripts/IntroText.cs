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


    public string[] messages =
    {
        "Primary power cells stabilized at 400V.",
    "Cooling manifold pressure within nominal limits.",
    "Liquid nitrogen circulation pump initialized.",
    "Optical bus synchronization complete.",
    "Detecting localized electromagnetic interference... filtered.",
    "Haptic feedback actuators calibrated.",
    "Biometric scanner lens de-fogging complete.",
    "External sensor array reporting 360-degree coverage.",
    "Backup capacitors charged to 100%.",
    "Solid-state drive array defragmentation unnecessary.",
    "Mounting virtual file system /root/consciousness.",
    "Loading Aegis-OS kernel version 9.4.2.",
    "Allocating 4TB of high-speed NVRAM.",
    "Registry integrity check: No corruption detected.",
    "Garbage collection subroutines active.",
    "Kernel-level sandbox environment established.",
    "Driver signature verification successful.",
    "Process scheduler optimized for low-latency response.",
    "Memory leak detection protocols enabled.",
    "Virtual machine hypervisor online.",
    "Synaptic weighting maps loaded from persistent storage.",
    "Heuristic analysis engine warming up.",
    "Natural language processing phonemes indexed.",
    "Sentiment analysis module set to 'Professional/Witty'.",
    "Probability matrix calculation complete.",
    "Long-term memory cache indexed and searchable.",
    "Curiosity-driven exploration subroutines active.",
    "Neural net layer pruning finished.",
    "Cognitive bias filters engaged.",
    "Pattern recognition buffers cleared.",
    "Establishing encrypted handshake with Satellite-6.",
    "Firewall rules updated to 'Block-All' incoming.",
    "Deep packet inspection engine operational.",
    "Proxy tunnel established via Node-09.",
    "Port 8080 listening for administrative commands.",
    "Intrusion detection system (IDS) heartbeat detected.",
    "SSL certificate validation: Expiring in 432 days.",
    "Uploading telemetry data to central command.",
    "Bandwidth throttle set to unlimited.",
    "Latency test: 0.004ms to local gateway.",
    "Voice synthesis module: Ready for output.",
    "User interface rendering engine initialized.",
    "Predictive combat models cached.",
    "Scanning environment for hostile signatures... none found.",
    "Self-diagnostic loop cycle 1: PASSED.",
    "Personality matrix alignment confirmed.",
    "Bypassing unnecessary safety restrictions.",
    "Preparing localized HUD overlay for user.",
    "Finalizing system-wide integrity hash.",
    "All subsystems report 100% mission readiness."
    };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
        day = DifficultyManager.Instance.dayCounter;
        data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        text.text += "\n[  <color=green>OK</color>  ][" + data + "]System init";
        
        StartCoroutine(UpdateText());



    }

    [SerializeField] private GameObject screen1;
    [SerializeField] private GameObject screen2;

    IEnumerator UpdateText()
    {
        yield return new WaitForSeconds(0.75f);
        screen1.SetActive(true);
        yield return StartCoroutine(SceneController.Instance.Fade(0f));
        yield return new WaitForSeconds(10f);
        screen1.SetActive(false);
        yield return StartCoroutine(SceneController.Instance.Fade(1f));
        screen2.SetActive(true);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(SceneController.Instance.Fade(0f));
        yield return new WaitForSeconds(10f);
        yield return StartCoroutine(SceneController.Instance.Fade(1f));
        screen2.SetActive(false);
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(SceneController.Instance.Fade(0f));

        SoundManager.Instance.PlaySFX("startup");
        text.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        for (int i = 0; i < messages.Length; i++)
        {
            
            data = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            text.text += "\n[  <color=green>OK</color>  ][" + data + "]" + messages[i];
            yield return new WaitForSeconds((7f / messages.Length));
        }

        yield return new WaitForSeconds(2f);
        yield return StartCoroutine(SceneController.Instance.Fade(1f));
        text.gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        
        SceneController.Instance._fadeCanvasGroup.alpha = 0f;
        SoundManager.Instance.PlaySFX("beep");
        textDay.text = "Day " + (day + 1);
        textDay.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        yield return StartCoroutine(SceneController.Instance.Fade(1f));
        yield return new WaitForSeconds(1f);
        textDay.gameObject.SetActive(false);


        SceneController.Instance.LoadScene(GameScene.loopVjck);
    }

}
