using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPan : MonoBehaviour
{
    private float mouseX, mouseY;

    [SerializeField] private MapUI mapUI;
    [SerializeField] private float panSensitivity = 0.2f;
    [SerializeField] private float scrollSensitivity = 10;
    private float t = 0;
    [SerializeField] private float leftPanBound, rightPanBound, upperPanBound, lowerPanBound;
    [SerializeField] private float lowerScrollBound, upperScrollBound;
    [SerializeField] private float recenteringSensitivity;
    [SerializeField] private float scrollLeniancy = 1;
    private Vector3 cameraPos, smoothPos;
    private Camera cam;
   
    private void Start()
    {
        cameraPos = transform.position;
        cam = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!mapUI.GetMapOpen()) return;

   
        if (Input.GetMouseButton(0))
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            CalcNewCameraPanPosition();

           
        }
        if (Input.mouseScrollDelta.magnitude > 0)
        {
            CalcNewCameraScrollPosition();
        }
        if (cameraPos != transform.position)
        {
            smoothPos = Vector3.Lerp(transform.position, cameraPos, t);
            t += Time.deltaTime * 4;
            transform.position = smoothPos;
            return;
        }
        t = 0;

    }
    private void CalcNewCameraScrollPosition()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (scrollDelta > 0 && cam.orthographicSize > lowerScrollBound)
        {
            cam.orthographicSize -= Time.deltaTime * scrollSensitivity;
        }
        if (scrollDelta < 0 && cam.orthographicSize < upperScrollBound)
        {
            cam.orthographicSize += Time.deltaTime * scrollSensitivity;
            CheckScrollInsidePanPounds();
        }

    }

    private void CheckScrollInsidePanPounds()
    {
        Vector3 scrolledPos = cameraPos * Mathf.Clamp(cam.orthographicSize / (lowerScrollBound * scrollLeniancy), 1, 1.3f);
        Vector3 correctedPos = AccountForPanBoundsScrolling(scrolledPos);

        if (scrolledPos != correctedPos)
        {
            cameraPos = correctedPos;
            cameraPos.z = -10;
        }
    }   
    private void CalcNewCameraPanPosition()
    {
        cameraPos += transform.right * (mouseX * -1) * panSensitivity * cam.orthographicSize;
        cameraPos += transform.up * (mouseY * -1) * panSensitivity * cam.orthographicSize;
        cameraPos = AccountForPanBoundsPanning(cameraPos);
    }
    private Vector3 AccountForPanBoundsPanning(Vector3 _cameraPos)
    {
        if (_cameraPos.x > rightPanBound*(1/(cam.orthographicSize/rightPanBound)))
        {
            _cameraPos.x = rightPanBound * (1 / (cam.orthographicSize / rightPanBound));
        }
        if (_cameraPos.x < -leftPanBound * (1 / (cam.orthographicSize / leftPanBound)))
        {
            _cameraPos.x = -leftPanBound*(1 / (cam.orthographicSize / leftPanBound));
        }
        if (_cameraPos.y > upperPanBound * (1 / (cam.orthographicSize / upperPanBound)))
        {
            _cameraPos.y = upperPanBound * (1 / (cam.orthographicSize / upperPanBound));
        }
        if (_cameraPos.y < -lowerPanBound * (1 / (cam.orthographicSize / lowerPanBound)))
        {
            _cameraPos.y = -lowerPanBound * (1 / (cam.orthographicSize / lowerPanBound));
        }
        return _cameraPos;
    }
    private Vector3 AccountForPanBoundsScrolling(Vector3 _cameraPos)
    {
       
        if (_cameraPos.x > rightPanBound * (1 / (cam.orthographicSize / rightPanBound)))
        {
            _cameraPos.x -= recenteringSensitivity * cam.orthographicSize;
        }
        if (_cameraPos.x < -leftPanBound * (1 / (cam.orthographicSize / leftPanBound)))
        {
            _cameraPos.x  += recenteringSensitivity * cam.orthographicSize;
        }
        if (_cameraPos.y > upperPanBound * (1 / (cam.orthographicSize / upperPanBound)))
        {
            _cameraPos.y -= recenteringSensitivity * cam.orthographicSize;
        }
        if (_cameraPos.y < -lowerPanBound * (1 / (cam.orthographicSize / lowerPanBound)))
        {
            _cameraPos.y += recenteringSensitivity * cam.orthographicSize;
        }
        return _cameraPos;
    }

    public void SetPosition(Vector3 _pos, float _orthoSize)
    {
        transform.position = _pos;
        cameraPos = _pos;
        cam.orthographicSize = _orthoSize;
    }
}