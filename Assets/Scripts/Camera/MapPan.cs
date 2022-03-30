using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPan : MonoBehaviour
{
    private float mouseX, mouseY;

    [SerializeField] private MapUI mapUI;
    [SerializeField] private float panSensitivity = 0.2f;
    [SerializeField] private float scrollSensitivity = 10;
    [SerializeField] private float smoothSpeed;
    [SerializeField] private float leftPanBound, rightPanBound, upperPanBound, lowerPanBound;
    [SerializeField] private float lowerScrollBound, upperScrollBound;
    private Vector3 cameraPos, smoothPos;
    private Camera camera;
   
    private void Start()
    {
        cameraPos = transform.position;
        camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!mapUI.GetMapOpen()) return;

        CalcNewCameraScrollPosition();
        if (Input.GetMouseButton(0))
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            CalcNewCameraPanPosition();
          
            smoothPos =  Vector3.Lerp(transform.position, cameraPos, smoothSpeed);
            transform.position = smoothPos;
        }
      
    }
    private void CalcNewCameraScrollPosition()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta > 0 && camera.orthographicSize > lowerScrollBound)
        {
            camera.orthographicSize -= Time.deltaTime * scrollSensitivity;
        }
        if (scrollDelta < 0 && camera.orthographicSize < upperScrollBound)
        {
            camera.orthographicSize += Time.deltaTime * scrollSensitivity;
        }
    }
    private void CalcNewCameraPanPosition()
    {
        cameraPos += transform.right * (mouseX * -1) * panSensitivity * camera.orthographicSize;
        cameraPos += transform.up * (mouseY * -1) * panSensitivity * camera.orthographicSize;
        cameraPos = AccountForPanBounds(cameraPos);
    }
    private Vector3 AccountForPanBounds(Vector3 _cameraPos)
    {
        if (_cameraPos.x > rightPanBound)
        {
            _cameraPos.x = rightPanBound;
        }
        if (_cameraPos.x < -leftPanBound)
        {
            _cameraPos.x = -leftPanBound;
        }
        if (_cameraPos.y > upperPanBound)
        {
            _cameraPos.y = upperPanBound;
        }
        if (_cameraPos.y < -lowerPanBound)
        {
            _cameraPos.y = -lowerPanBound;
        }
        return _cameraPos;
    }

    public void SetPosition(Vector3 _pos, float _orthoSize)
    {
        transform.position = _pos;
        cameraPos = _pos;
        camera.orthographicSize = _orthoSize;
    }
}