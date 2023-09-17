using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LegendUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI legendText;

    private void Start()
    {
        // Start Hidden
        Hide();
    }

    public void Show(string entranceText, string exitText)
    {
        canvasGroup.alpha = 1f;
        legendText.text = $"{entranceText}\n{exitText}";
    }

    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }
}
