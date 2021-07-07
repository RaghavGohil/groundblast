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

    //ParticleSystem:
    [SerializeField]
    ParticleSystem _particleSystem;

    //bool:
    bool _canShoot; //Can shoot or not in the current time.
    public bool _hasShot; //Has clicked or not.

    //Raycasts:
    public RaycastHit2D _hit;

    #region private void

    void Start()
    {

        _hasShot = false;
        _canShoot = true;
        _point = new Vector3();
        _theta = new float();
        _ground = "ground";
        _rotationDelay = 10f;
        _forceAmount = 100f; //DirectSet
        _nextShootTime = 1f;
        _dist = 1f;
        _recoil = 30f;
        _rb = GetComponent<Rigidbody2D>();

    }

    void FixedUpdate()
    {
        
        Shoot();
        RotateCannon();

    }

    void Update() => ShootCheck();

    void RotateCannon()
    {

        _point = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x , Input.mousePosition.y , -_camera.transform.position.z)) - _cannon.transform.position;

        _theta = Mathf.Atan2(_point.x , _point.y) * Mathf.Rad2Deg;

        Quaternion rotation = Quaternion.AngleAxis(-_theta , Vector3.forward);

        _cannon.transform.rotation = Quaternion.Lerp(_cannon.transform.rotation , rotation , _rotationDelay*Time.deltaTime);

    }

    void ShootCheck()
    {

        if(Input.GetMouseButtonDown(0) && _canShoot)
        {
            _hasShot = true; //Has Pulled Trigger
            StartCoroutine(WaitForNextShoot(_nextShootTime));
        }
    }

    void Shoot()
    {
        
        _hit = Physics2D.Raycast(_cannon.transform.position, _point , _dist, _layerMask);

        if(_hasShot)
        {

            //Apply some force.
            _rb.AddForce(-_point * _forceAmount, ForceMode2D.Force);
            _particleSystem.Play();
            ApplyRecoil();

        }

        _hasShot = false;

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
