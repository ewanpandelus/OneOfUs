using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPan : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    [SerializeField] private float panSensitivity = 0.2f;
    [SerializeField] private float scrollSensitivity = 10;
    [SerializeField] private float smoothSpeed;
    private Vector3 cameraPos;
    private Camera camera;
    private void Start()
    {
        cameraPos = transform.position;
        camera = GetComponent<Camera>();
    }
    // Update is called once per frame
    void Update()
    {
        float scrollDelta = Input.GetAxis("Mouse ScrollWheel");
        if (Input.GetMouseButton(0))
        {
            mouseX = Input.GetAxis("Mouse X");
            mouseY = Input.GetAxis("Mouse Y");
            cameraPos += transform.right * (mouseX * -1) * panSensitivity * camera.orthographicSize;
            cameraPos += transform.up * (mouseY * -1) * panSensitivity * camera.orthographicSize;
            Vector3 smoothPost = Vector3.Lerp(transform.position, cameraPos, smoothSpeed);
            transform.position = smoothPost;
        }
        if (scrollDelta > 0 && camera.orthographicSize > 8)
        {
            camera.orthographicSize -= Time.deltaTime * scrollSensitivity;
        }
        if (scrollDelta < 0 && camera.orthographicSize < 24)
        {
            camera.orthographicSize += Time.deltaTime * scrollSensitivity;
        }
    }
    public void SetPosition(Vector3 _pos, float _orthoSize)
    {
        transform.position = _pos;
        cameraPos = _pos;
        camera.orthographicSize = _orthoSize;
    }
}