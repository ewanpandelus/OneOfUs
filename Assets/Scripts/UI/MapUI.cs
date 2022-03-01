using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    bool showing = false;
    [SerializeField] GameObject map;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) || Input.GetMouseButton(1))
        {
            showing = !showing;
            map.SetActive(showing);
        }
    }
}
