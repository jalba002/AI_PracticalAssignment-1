using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera Following attributes")]
    public float camSmoothFollowingTiome = 3.0f;

    [Header("Camera Zoom attributes")]
    public float maxZoomIn = 5.0f;
    public float maxZoomOut = 15.0f;


    private Camera cam;
    private float targetZoom;
    private float zoomFactor = 3.0f;
    private float zoomLerpSpeed = 10.0f;


    void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
    }

    void Update()
    {
        float scrollData;
        scrollData = Input.GetAxis("Mouse ScrollWheel");

        targetZoom -= scrollData * zoomFactor;
        targetZoom = Mathf.Clamp(targetZoom, maxZoomIn, maxZoomOut);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomLerpSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        Vector3 smoothposition = Vector3.Lerp(transform.position, new Vector3(GameController.Instance.playerController.transform.position.x, GameController.Instance.playerController.transform.position.y, -10), camSmoothFollowingTiome * Time.fixedDeltaTime);
        transform.position = smoothposition;
    }
}
