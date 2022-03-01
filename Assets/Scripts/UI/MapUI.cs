using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapUI : MonoBehaviour
{
    bool showing = false;
    [SerializeField] GameObject map;

    private void Start()
    {
        map.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            showing = !showing;
            map.SetActive(showing);
        }
    }
}
