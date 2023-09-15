using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GeneratorSelectionUI : MonoBehaviour
{
    [SerializeField] private GameObject detailsGameObject;
    [SerializeField] private Transform arrowsTransform;
    [SerializeField] private TextMeshProUGUI algorithmNameText;
    [SerializeField] private TextMeshProUGUI widthText;
    [SerializeField] private TextMeshProUGUI heightText;
    [SerializeField] private TextMeshProUGUI cullChanceText;
    [SerializeField] private TextMeshProUGUI numStepsText;

    [SerializeField] private GameObject cullHolder;
    [SerializeField] private GameObject stepsHolder;

    private bool detailsAreVisible;

    private void Awake()
    {
        detailsAreVisible = true;
    }

    public void SetAlgorithm(GeneratorAlgorithm algorithm)
    {
        switch (algorithm)
        {
            case GeneratorAlgorithm.UniformRandom:
                algorithmNameText.text = "Uniform Random";
                cullHolder.SetActive(true);
                stepsHolder.SetActive(false);
                break;
            case GeneratorAlgorithm.DiffusionLimitedAggregation:
                algorithmNameText.text = "Diffusion Limited Aggregation";
                cullHolder.SetActive(false);
                stepsHolder.SetActive(true);
                break;
            case GeneratorAlgorithm.DrunkardsWalk:
                algorithmNameText.text = "Drunkards Walk";
                cullHolder.SetActive(true);
                stepsHolder.SetActive(true);
                break;
            default:
                algorithmNameText.text = "UNHANDLED ALGO!";
                cullHolder.SetActive(true);
                stepsHolder.SetActive(true);
                break;
        }

    }

    public void ToggleDetails()
    {
        if (detailsAreVisible)
        {
            detailsGameObject.SetActive(false);
            arrowsTransform.localScale = Vector3.one;
            detailsAreVisible = false;
        }
        else
        {
            detailsGameObject.SetActive(true);
            arrowsTransform.localScale = new Vector3(1, -1, 1);
            detailsAreVisible = true;
        }
    }

    public void UpdateWidthText(float value)
    {
        widthText.text = $"Map Width: <color=blue>{value}";
    }

    public void UpdateHeightText(float value)
    {
        heightText.text = $"Map Height: <color=blue>{value}";
    }

    public void UpdateCullChanceText(float value)
    {
        cullChanceText.text = $"Cull Chance: <color=blue>{value}%";
    }

    public void UpdateNumStepsText(float value)
    {
        numStepsText.text = $"Number of Steps: <color=blue>{value}";
    }
}
