using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
public class LevelLoader : MonoBehaviour
{

    [SerializeField] GameObject fadeObj;
    [SerializeField] Image image;
    public void LoadMainLevel()
    {
        fadeObj.SetActive(true);
        StartCoroutine(LerpAlpha());
 

    }
    public IEnumerator LerpAlpha()
    {

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            image.color = new Color(0, 0, 0, t);
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
