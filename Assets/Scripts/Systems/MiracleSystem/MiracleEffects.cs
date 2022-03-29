using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
public class MiracleEffects  :MonoBehaviour
{
    private PlayerAnimator playerAnimator;
    private PlayerController playerController;
    private GameObject fireEffectPrefab;
    private GameObject chiefLightEffect;
    private Light2D globalLight;

    private float rotateCount = 100f;
    private float waitTime = 0.5f;
    public  void SetupMiracleEffects(PlayerAnimator _playerAnimator, GameObject _fireEffectPrefab, PlayerController _playerController, 
        GameObject _chiefLightEffect)
    {
        playerAnimator = _playerAnimator;
        fireEffectPrefab = _fireEffectPrefab;
        playerController = _playerController;
        chiefLightEffect = _chiefLightEffect;
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
    }

 
    public IEnumerator FireEffect()
    {
        GameObject fireObj = null;
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
        StartCoroutine(FireEffect2());
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(fireObj);
    }
    public IEnumerator FireEffect2()
    {
        playerAnimator.SetShouldAutoAnimate(false);

        for (int i = 0; i <= 20; i++)
        {
            yield return new WaitForSeconds(waitTime);
            if (waitTime < 0.2f) { waitTime += 0.008f; }
            playerAnimator.ManuallySetAnimator(i % 4);
        }
        playerAnimator.SetShouldAutoAnimate(true);
    }
    public void BeamLightEffect()
    {
        GameObject chiefEffect = Instantiate(chiefLightEffect);
        chiefEffect.transform.position = playerController.transform.position + new Vector3(0.44f, 3.05f, 0);
        Light2D light1 = chiefEffect.transform.GetChild(0).GetComponent<Light2D>();
        light1.intensity = 0f;
        StartCoroutine(FadeInLight(light1));

    }
    private IEnumerator FadeInLight(Light2D light1)
    {
        float x = 0f;
        var t = 0f;

        while (t < 1 )
        {
            t += Time.deltaTime/5;
            x = Mathf.Lerp(0, 0.9f, t);
            globalLight.intensity = 1 - x/1.5f;
            light1.intensity = 1 - globalLight.intensity;
            yield return null;
        }
    }
}
