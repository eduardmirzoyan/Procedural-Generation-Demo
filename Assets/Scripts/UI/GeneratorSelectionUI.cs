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
    [SerializeField] private GameObject roomWidthHolder;
    [SerializeField] private GameObject roomHeightHolder;
    [SerializeField] private GameObject thresholdHolder;
    [SerializeField] private GameObject scaleHolder;

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
                roomHeightHolder.SetActive(false);
                roomWidthHolder.SetActive(false);
                thresholdHolder.SetActive(false);
                scaleHolder.SetActive(false);
                break;
            case GeneratorAlgorithm.DiffusionLimitedAggregation:
                algorithmNameText.text = "Diffusion Limited Aggregation";
                cullHolder.SetActive(false);
                stepsHolder.SetActive(true);
                roomHeightHolder.SetActive(false);
                roomWidthHolder.SetActive(false);
                thresholdHolder.SetActive(false);
                scaleHolder.SetActive(false);
                break;
            case GeneratorAlgorithm.DrunkardsWalk:
                algorithmNameText.text = "Drunkards Walk";
                cullHolder.SetActive(true);
                stepsHolder.SetActive(true);
                roomHeightHolder.SetActive(false);
                roomWidthHolder.SetActive(false);
                thresholdHolder.SetActive(false);
                scaleHolder.SetActive(false);
                break;
            case GeneratorAlgorithm.IssacRoomGeneration:
                algorithmNameText.text = "Binding Of Issac";
                cullHolder.SetActive(false);
                stepsHolder.SetActive(false);
                roomHeightHolder.SetActive(false);
                roomWidthHolder.SetActive(true);
                thresholdHolder.SetActive(false);
                scaleHolder.SetActive(false);
                break;
            case GeneratorAlgorithm.PerlinNoise:
                algorithmNameText.text = "Perlin Noise";
                cullHolder.SetActive(false);
                stepsHolder.SetActive(false);
                roomHeightHolder.SetActive(false);
                roomWidthHolder.SetActive(false);
                thresholdHolder.SetActive(true);
                scaleHolder.SetActive(true);
                break;
            case GeneratorAlgorithm.VoronoiDiagram:
                algorithmNameText.text = "Voronoi Diagram";
                cullHolder.SetActive(false);
                stepsHolder.SetActive(false);
                roomHeightHolder.SetActive(false);
                roomWidthHolder.SetActive(true);
                thresholdHolder.SetActive(true);
                scaleHolder.SetActive(true);
                break;
            default:
                algorithmNameText.text = "UNHANDLED ALGO!";
                cullHolder.SetActive(false);
                stepsHolder.SetActive(false);
                roomHeightHolder.SetActive(false);
                roomWidthHolder.SetActive(false);
                thresholdHolder.SetActive(false);
                scaleHolder.SetActive(false);
                break;
        }

        // Update Canvas
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
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
        widthText.text = $"Map Width: <color=black>{value}";
    }

    public void UpdateHeightText(float value)
    {
        heightText.text = $"Map Height: <color=black>{value}";
    }

    public void UpdateCullChanceText(float value)
    {
        cullChanceText.text = $"Cull Chance: <color=black>{value}%";
    }

    public void UpdateNumStepsText(float value)
    {
        numStepsText.text = $"Number of Steps: <color=black>{value}";
    }

    public void UpdateRoomWidth(float value)
    {
        roomWidthHolder.GetComponentInChildren<TextMeshProUGUI>().text = $"Room Width: <color=black>{value}";
    }

    public void UpdateRoomHeight(float value)
    {
        roomHeightHolder.GetComponentInChildren<TextMeshProUGUI>().text = $"Room Height: <color=black>{value}";
    }

    public void UpdateThreshold(float value)
    {
        thresholdHolder.GetComponentInChildren<TextMeshProUGUI>().text = $"Threshold: <color=black>{value:F2}";
    }

    public void UpdateScale(float value)
    {
        scaleHolder.GetComponentInChildren<TextMeshProUGUI>().text = $"Scale: <color=black>{value}";
    }
}
