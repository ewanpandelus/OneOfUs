using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.EventSystems;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject creditsBackButton;
    [SerializeField] private GameObject controlsBackButton;
    [SerializeField] GameObject fadeObj;
    [SerializeField] Image image;
    private void Start()
    {
        eventSystem.SetSelectedGameObject(playButton);
    }
    public void LoadMainLevel()
    {
        fadeObj.SetActive(true);
        StartCoroutine(LerpAlpha());
 

    }
    public void PlayMenuSound()
    {
        SoundManager.instance.PlayOneShot("MenuPress");
    }
    public void GoToCredits()
    {
        eventSystem.SetSelectedGameObject(creditsBackButton);
    }
    public void GoToControls()
    {
        eventSystem.SetSelectedGameObject(controlsBackButton);
    }
    public void GoToMainMenu()
    {
        eventSystem.SetSelectedGameObject(playButton);
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
