using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndocrinateDestroy : MonoBehaviour
{
    private float destroyTime = 0;

    private void Update()
    {
        destroyTime += Time.deltaTime;
        if (destroyTime > 5)
        {
            Destroy(gameObject);
        }
    }
}
