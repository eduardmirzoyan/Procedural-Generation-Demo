using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NumberTile : MonoBehaviour
{
    [SerializeField] private TextMeshPro text;

    private void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void SetValue(int value)
    {
        text.text = $"{value}";
    }
}
