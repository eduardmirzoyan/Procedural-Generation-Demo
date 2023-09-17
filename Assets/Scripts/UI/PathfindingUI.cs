using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUI : MonoBehaviour
{
    [SerializeField] private GameObject dijstraObject;
    [SerializeField] private GameObject aStarObject;

    public void SetDijstra(bool state)
    {
        dijstraObject.SetActive(state);
    }

    public void SetAStar(bool state)
    {
        aStarObject.SetActive(state);
    }
}
