using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{

    //floats:
    float _theta;
    float _forceAmount;
    float _rotationDelay;
    float _dist;
    float _recoil;
    float _nextShootTime;
    public float _reloadTime {get;private set;}

    //Int:
    int _maxAmmo;
    public int _clipsRemainingCount{get; private set;}
    public int _currentAmmoCount{get; private set;}
    
    //Rigidbody2D:
    Rigidbody2D _rb;

    //Vectors:
    Vector3 _point;
    
    //Camera:
    [SerializeField]
    Camera _camera;

    //LayerMasks:
    [SerializeField]
    LayerMask _layerMask;

    //strings:
    string _ground;

    //GameObjects:
    [SerializeField]
    GameObject _cannon; // Attach cannon in the ins
    [SerializeField]
    GameObject _ammoCounter;

    //ParticleSystem:
    [SerializeField]
    ParticleSystem _particleSystem;

    //bool:
    bool _canShoot; //Can shoot or not in the current time.
    public bool _isReloading{get;private set;}

    //Raycasts:
    public RaycastHit2D _hit;

    #region private void

    void Start()
    {

        _canShoot = true;
        _isReloading = false;
        _point = new Vector3();
        _theta = new float();
        _ground = "ground";
        _rotationDelay = 10f;
        _forceAmount = 100f; //DirectSet
        _currentAmmoCount = 5;
        _maxAmmo = 5;
        _clipsRemainingCount = 5;
        _nextShootTime = 1f;
        _reloadTime = 2f;
        _dist = 1f;
        _recoil = 30f;
        _rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    { 
        RotateCannon(); 
        ReloadCheck();
    }

    void Update() => CheckAndShoot();

    void RotateCannon()
    {

        _point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y , -_camera.transform.position.z)) - _cannon.transform.position;

        _theta = Mathf.Atan2(_point.x , _point.y) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(-_theta , Vector3.forward);

        _cannon.transform.rotation = Quaternion.Lerp(_cannon.transform.rotation , rotation , _rotationDelay*Time.deltaTime);

    }

    void CheckAndShoot()
    {
        
        if(Input.GetMouseButtonDown(0) && _canShoot)
        {

            _hit = Physics2D.Raycast(_cannon.transform.position, _point , _dist, _layerMask);

            StartCoroutine(WaitForNextShoot(_nextShootTime));

            //Apply some force.
            _rb.AddForce(-_point * _forceAmount, ForceMode2D.Force);
            _particleSystem.Play();
            _currentAmmoCount --;
            ApplyRecoil();

        }

    }

    void ReloadCheck()
    {

        if((_currentAmmoCount <= 0 && !_isReloading) && _clipsRemainingCount>=1)
        {

            StartCoroutine(ReloadCannon(_reloadTime));

        }else if(_clipsRemainingCount <=0 && _currentAmmoCount <=0){_canShoot = false;}

    }

    #endregion

    #region public void

    public void ApplyRecoil()
    {

        _cannon.transform.rotation = Quaternion.AngleAxis(-_theta+_recoil , Vector3.forward);

    }

    #endregion

    #region private IEnumerator
        
    IEnumerator WaitForNextShoot(float t)
    {

        _canShoot = false;

        yield return new WaitForSeconds(t);
        
        if(_currentAmmoCount > 0)
            _canShoot = true;

    }

    IEnumerator ReloadCannon(float t)
    {

        _canShoot = false;
        _isReloading = true;
        yield return new WaitForSeconds(t);
        //Reload
        _isReloading = false;
        _currentAmmoCount = _maxAmmo;
        _clipsRemainingCount--; //DecreaseClipCount.
        _canShoot = true;
        
    }
    
    #endregion

    #region private GIZMO

    void OnDrawGizmoSelected() //Unity Editor Draw Shoot Point Gizmo When Selected
    {

        Debug.DrawRay(_cannon.transform.position, _point.normalized * _dist ,Color.red); // normalized into unit vector.

    }

    #endregion

}
