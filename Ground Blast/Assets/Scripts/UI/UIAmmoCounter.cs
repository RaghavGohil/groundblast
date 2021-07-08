using UnityEngine;
using UnityEngine.UI;

public class UIAmmoCounter : MonoBehaviour
{
    
    //float:
    float _maxFillValue;
    float _alpha;
    float _maxAlpha;

    //GameObject:
    [SerializeField]
    GameObject _player;

    //Texts:
    [SerializeField]
    Text _clipsRemaining;
    [SerializeField]
    Text _currentAmmo;

    //Image:
    [SerializeField]
    Image _reloadFillRing;

    #region private void

    void Start()
    {

        _clipsRemaining.text = "";
        _currentAmmo.text = "";
        _alpha = 0;
        _maxAlpha = 0.2f;
        _maxFillValue = 1f;

    }

    void FixedUpdate()
    {

        _currentAmmo.text = _player.GetComponent<PlayerMovement>()._currentAmmoCount.ToString();
        _clipsRemaining.text = _player.GetComponent<PlayerMovement>()._clipsRemainingCount.ToString();
        FillRing(_player.GetComponent<PlayerMovement>()._reloadTime);

    }

    void FillRing(float t)
    {

        if(_player.GetComponent<PlayerMovement>()._isReloading)
        {

            _reloadFillRing.fillAmount += Time.fixedDeltaTime/t;// Change Amount.
            
            if(_alpha<=_maxAlpha)
            {
                _alpha += Time.fixedDeltaTime/t;
                print(_alpha);
            }
            _reloadFillRing.color = new Color(255f,255f,255f,_alpha);


        }else{ ResetRing(); }

    }

    void ResetRing()
    {

        _reloadFillRing.fillAmount = 0; 
        _alpha = 0f;
        _reloadFillRing.color = new Color(255f,255f,255f,0f);

    }

    #endregion

}
