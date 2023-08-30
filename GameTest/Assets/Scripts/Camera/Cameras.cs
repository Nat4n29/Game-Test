using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Cameras : MonoBehaviour
{
    [SerializeField]
    private Camera cam;
    [SerializeField]
    private float zoomStep;
    private float minSize = 3f, maxSize = 20f, mapMinX, mapMaxX, mapMinY, mapMaxY;
    private Vector3 originDrag, atualPoint;
    [SerializeField]
    private Renderer mapRenderer;
 
    void Awake()
    {
        mapMinX = mapRenderer.transform.position.x - mapRenderer.bounds.size.x / 2f;
        mapMaxX = mapRenderer.transform.position.x + mapRenderer.bounds.size.x / 2f;

        mapMinY = mapRenderer.transform.position.y - mapRenderer.bounds.size.y / 2f;
        mapMaxY = mapRenderer.transform.position.y + mapRenderer.bounds.size.y / 2f;
    }

    void Update()
    {
        Mover();
        Zoom();
    }

    private void Mover()
    {
        if(Input.GetMouseButtonDown(0))
        {
            originDrag = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if(Input.GetMouseButton(0))
        {
            Vector3 difference = originDrag - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position = ClampCamera(cam.transform.position + difference);
        }
    }
    private void Zoom()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            ZoomIn();
        }
        if(Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            ZoomOut();
        }
    }

    private void ZoomIn()
    {
        atualPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        float newSize = cam.orthographicSize - zoomStep;
        cam.orthographicSize = Math.Clamp(newSize, minSize, maxSize);
        cam.transform.position = ClampCamera(cam.transform.position + atualPoint - cam.ScreenToWorldPoint(Input.mousePosition));
    }
    private void ZoomOut()
    {
        atualPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        float newSize = cam.orthographicSize + zoomStep;
        cam.orthographicSize = Math.Clamp(newSize, minSize, maxSize);
        cam.transform.position = ClampCamera(cam.transform.position + atualPoint - cam.ScreenToWorldPoint(Input.mousePosition));
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float camHeight = cam.orthographicSize;
        float camWidth = cam.orthographicSize * cam.aspect;

        float minX = mapMinX + camWidth;
        float maxX = mapMaxX - camWidth;
        float minY = mapMinY + camHeight;
        float maxY = mapMaxY - camHeight;

        float newX = Mathf.Clamp(targetPosition.x, minX, maxX);
        float newY = Mathf.Clamp(targetPosition.y, minY, maxY);

        return new Vector3(newX, newY, targetPosition.z);
    }
}
