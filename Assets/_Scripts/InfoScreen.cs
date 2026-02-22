using UnityEngine;

public class InfoScreen : MonoBehaviour
{
    public GameObject quotaSreen;
    public GameObject workersSreen;


    void workers()
    {
        quotaSreen.SetActive(false);
        workersSreen.SetActive(true);
    }

    void quota()
    {
        workersSreen.SetActive(false);
        quotaSreen.SetActive(true);
    }
}
