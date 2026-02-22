using System;
using TMPro;
using UnityEngine;

public class FitnessDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text fitnessText;
    private void FixedUpdate()
    {
        fitnessText.text = $"{GameManager.Instance.getFitness()} / {DifficultyManager.Instance.getCurrentDifficulty().requiredFitness}";
    }
}
