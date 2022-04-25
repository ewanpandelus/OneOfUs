using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
public class MiracleEffects  :MonoBehaviour
{
    [SerializeField] private GameObject playerSheep;
    [SerializeField] private GameObject player;
    [SerializeField] private CameraFollow camFollow;
    private PlayerAnimator playerAnimator;
    private PlayerController playerController;
    private SheepAnimator sheepAnimator;
    private GameObject fireEffectPrefab;
    private GameObject chiefLightEffectPrefab;
    private GameObject chiefLightEffectObj;
    private Light2D globalLight;
    private Light2D beamOfLight;
    private float savedBeamOfLightIntensity;

    private float rotateCount = 100f;
    private float waitTime = 0.5f;
    public  void SetupMiracleEffects(PlayerAnimator _playerAnimator, GameObject _fireEffectPrefab, PlayerController _playerController, 
        GameObject _chiefLightEffect, SheepAnimator _sheepAnimator)
    {
        playerAnimator = _playerAnimator;
        fireEffectPrefab = _fireEffectPrefab;
        playerController = _playerController;
        chiefLightEffectPrefab = _chiefLightEffect;
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        sheepAnimator = _sheepAnimator;
    }
    public void TurnBackIntoHuman()
    {
        player.transform.position = playerSheep.transform.position;
        player.GetComponent<SpriteRenderer>().DOColor( new Color(1, 1, 1, 1),0.5f);
        player.GetComponent<Collider2D>().enabled = true;
        playerSheep.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0),0.65f);
        playerSheep.GetComponent<Collider2D>().enabled = false;
        sheepAnimator.SetActive(false);
        camFollow.SetTarget(player.transform);
        playerController.SetCanMove(true);
    }
    public IEnumerator SheepEffect()
    {
        playerSheep.transform.position = player.transform.position;
        yield return new WaitForSeconds(3.8f);
        player.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.4f);
        player.GetComponent<Collider2D>().enabled = false;
        playerSheep.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 1), 0.65f);
        playerSheep.GetComponent<Collider2D>().enabled = true;
        sheepAnimator.SetActive(true);
        camFollow.SetTarget(playerSheep.transform);
        MiracleManager.instance.SetMiracleOccuring(false);
    }
 
    public IEnumerator FireEffect()
    {
        GameObject fireObj = null;
        playerController.SetCanMove(false);
        playerAnimator.SetShouldAutoAnimate(false);
        bool fire = false;
        for (int i = 0; i <= rotateCount; i++)
        {
            yield return new WaitForSeconds(waitTime);
            if (waitTime > 0.04f) { waitTime -= 0.018f; }
            else if (!fire)
            {
                fireObj = GameObject.Instantiate(fireEffectPrefab, playerController.transform);
                fireObj.transform.position += new Vector3(0, -0.7f, 0);
                fire = true;
            }
            playerAnimator.ManuallySetAnimator(i % 4);
        }
        playerAnimator.SetShouldAutoAnimate(true);
        StartCoroutine(SlowDownSpin());
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(fireObj);
    }
    public IEnumerator SlowDownSpin()
    {
        playerAnimator.SetShouldAutoAnimate(false);
        for (int i = 0; i <= 20; i++)
        {
            yield return new WaitForSeconds(waitTime);
            if (waitTime < 0.2f) { waitTime += 0.008f; }
            playerAnimator.ManuallySetAnimator(i % 4);
        }
        playerAnimator.SetShouldAutoAnimate(true);
        playerController.SetCanMove(true);
        MiracleManager.instance.SetMiracleOccuring(false);
    }
    public void BeamLightEffect()
    {
        chiefLightEffectObj = Instantiate(chiefLightEffectPrefab);
        chiefLightEffectObj.transform.position = playerController.transform.position + new Vector3(0.55f, 3.05f, 0);
        beamOfLight = chiefLightEffectObj.transform.GetChild(0).GetComponent<Light2D>();
        beamOfLight.intensity = 0f;
        StartCoroutine(FadeInLight(beamOfLight));
    }

    private IEnumerator FadeInLight(Light2D beamOfLight)
    {
        float x = 0f;
        float t = 0f;
        float startIntensity = globalLight.intensity;
        while (t < 1 )
        {
            t += Time.deltaTime/3;
            x = Mathf.Lerp(0, 0.9f, t);
            globalLight.intensity = startIntensity - x/1.2f;
            beamOfLight.intensity = 1 - globalLight.intensity-(1-startIntensity);
            yield return null;
        }
    }
    
    public void SetBeamOfLightIntensity(float _intensity)
    {
        if (beamOfLight)
        {
            savedBeamOfLightIntensity = beamOfLight.intensity;
            beamOfLight.intensity = _intensity;
        }
       
    }
    public IEnumerator FlashOfLight()
    {
        Destroy(chiefLightEffectObj);
        float x = 0f;
        float t = 0f;
        float slowMult = 1f;
        for(int i = 0; i < 3; i++)
        {
            slowMult -= 0.3f;
            yield return new WaitForSeconds(0.8f);
            
            SoundManager.instance.PlayOneShot("Thunder");
            while (t < 1)
            {
                t += Time.deltaTime*4*slowMult;
                x = Mathf.Lerp(globalLight.intensity, 2f, t);
                globalLight.intensity = x;
                yield return null;
            }
            t = 0;
            while (t < 1)
            {
                t += Time.deltaTime*4*slowMult;
                x = Mathf.Lerp(2, 1f, t);
                globalLight.intensity = x;
                yield return null;
            }
        }
    }
    public float GetSavedBeamOfLightIntensity() => savedBeamOfLightIntensity;
    public void SetGlobalLightIntensity(float _intensity) => globalLight.intensity = _intensity;
    public void DestroyBeam()
    {
        if (chiefLightEffectObj) Destroy(chiefLightEffectObj);
    }
}
