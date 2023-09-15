using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera cam;

    [Header("Settings")]
    [SerializeField] private float zoom;
    [SerializeField] private float zoomMultiplier = 4f;
    [SerializeField] private float minZoom = 2f;
    [SerializeField] private float maxZoom = 8f;
    [SerializeField] private float velocity = 0f;
    [SerializeField] private float smoothTime = 0.25f;

    private Vector3 origin;

    private void Awake()
    {
        cam = Camera.main;
        zoom = cam.orthographicSize;
    }

    private void Update()
    {
        Zoom();
        Pan();
    }

    private void Pan()
    {
        // First instance of right click
        if (Input.GetMouseButtonDown(1))
        {
            origin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        // While right click held down
        if (Input.GetMouseButton(1))
        {
            Vector3 difference = origin - cam.ScreenToWorldPoint(Input.mousePosition);
            cam.transform.position += difference;
        }
    }

    private void Zoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0)
        {
            zoom -= scroll * zoomMultiplier;
            zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        }

        cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, zoom, ref velocity, smoothTime);
    }

    public void CenterCamera(int width, int height)
    {
        // Get center of map
        Vector3 position = new Vector3(width / 2, height / 2, -10);
        cam.transform.position = position;

        // Zoom out camera
        int largestAxis = Mathf.Max(width, height);
        cam.orthographicSize = largestAxis / 2;

        // Update max zoom
        maxZoom = largestAxis / 2;
        zoom = maxZoom;
    }
}
