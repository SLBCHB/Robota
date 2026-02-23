using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour
{
    public GameObject quotaSreen;
    public GameObject workersSreen;
    private int dayCounter;
    public TMP_Text dayCounterText;
    public Slider quta;

    public GameObject prefab;
    public GameObject parent;

    void Start()
    {
        dayCounter = DifficultyManager.Instance.dayCounter;
        dayCounterText.text = "Day: " + (dayCounter +1);

        quta.maxValue = DifficultyManager.Instance.getCurrentDifficulty().requiredFitness;

        Instantiate(prefab, parent.transform);

    }

    private void Update()
    {
        quta.value = GameManager.Instance.getFitness();
    }

    public void workers()
    {
        quotaSreen.SetActive(false);
        workersSreen.SetActive(true);
    }


    void robotak()
    {
        Instantiate(prefab, parent.transform);
    }

   public void quota()
    {
        workersSreen.SetActive(false);
        quotaSreen.SetActive(true);
    }
}
