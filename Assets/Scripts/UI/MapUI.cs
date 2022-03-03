using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    bool showing = false;
    [SerializeField] GameObject map;
    [SerializeField] MapPan mapCam;
    private Vector3 mapCentredPos;
    private float mapOriginalOrthoSize;

    private void Start()
    {
        mapCentredPos = mapCam.transform.position;
        mapOriginalOrthoSize = mapCam.GetComponent<Camera>().orthographicSize;

        map.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            showing = !showing;
            map.SetActive(showing);
            mapCam.SetPosition(mapCentredPos, mapOriginalOrthoSize);
        }
    }
}