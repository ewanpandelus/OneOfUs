using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleEffects 
{
    private PlayerAnimator playerAnimator;
    private PlayerController playerController;
    private GameObject fireEffectPrefab;
    private float rotateCount = 128f;
    private float waitTime = 0.5f;
    public MiracleEffects(PlayerAnimator _playerAnimator, GameObject _fireEffectPrefab,PlayerController _playerController)
    {
        playerAnimator = _playerAnimator;
        fireEffectPrefab = _fireEffectPrefab;
        playerController = _playerController;

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
        yield return new WaitForSeconds(5f);
        GameObject.Destroy(fireObj);
    }
}
