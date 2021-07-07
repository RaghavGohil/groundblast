using UnityEngine;
using System.Collections;

public class Gas : MonoBehaviour
{
    
    //float:
    float _destroyTime;
    float _highestVelocityLimit;
    float _fieldOfImpact;
    float _forceOfImpact;
    float _shakeDuration;
    float _shakeMagnitude;

    //LayerMask:
    [SerializeField]
    LayerMask _layerMask;

    //ParticleSystems:
    [SerializeField]
    ParticleSystem[] _particleSystems;
    //Player:
    [SerializeField]
    GameObject _player;
    [SerializeField]
    GameObject _camera;

    #region private void

    void Start()
    {

        _destroyTime = 1f;
        _highestVelocityLimit = 10f;
        _shakeDuration = 0.1f;
        _shakeMagnitude = 1f;
        _fieldOfImpact = 3f;
        _forceOfImpact = 10f;

    }

    void Update() => CheckForFire();
    
    void PlayParticleSystems()
    {

        for(int i = 0; i < _particleSystems.Length ; i++)
        {

            _particleSystems[i].Play(); // Play all the particle systems..

        }

    }

    void ApplyImpactForce()
    {

        Collider2D[] objects = Physics2D.OverlapCircleAll(transform.position,_fieldOfImpact,_layerMask);

        foreach(Collider2D obj in objects)
        {

            Vector2 direction = obj.transform.position - transform.position;

            if(obj.GetComponent<Rigidbody2D>() != null)
                obj.GetComponent<Rigidbody2D>().AddForce(direction * _forceOfImpact , ForceMode2D.Impulse);

        }

    }

    void Disable()
    {

        GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,0f);
        GetComponent<BoxCollider2D>().enabled = false;

    }

    void AddCameraShake()
    {

        StartCoroutine(_camera.GetComponent<CameraShake>().ShakeCamera(_shakeDuration , _shakeMagnitude));

    }

    void CheckForFire()
    {
        if(_player.GetComponent<PlayerMovement>()._hit.transform != null)
        {
            if(_player.GetComponent<PlayerMovement>()._hit.transform.GetComponent<Gas>() != null && _player.GetComponent<PlayerMovement>()._hasShot)
            {
                StartCoroutine(_player.GetComponent<PlayerMovement>()._hit.transform.GetComponent<Gas>().Explode(_destroyTime));
                _player.GetComponent<PlayerMovement>().ApplyRecoil();
                _player.GetComponent<PlayerMovement>()._hasShot = false;
            }

        }

    }

    void OnCollisionEnter2D(Collision2D col) //ExplosionDueToVelocity:
    {

        if(col.transform.CompareTag(_player.transform.tag) && _player.GetComponent<Rigidbody2D>().velocity.magnitude > _highestVelocityLimit)
            StartCoroutine(Explode(_destroyTime));

    }

    #endregion

    #region private IEnumerator
    IEnumerator Explode(float t)
    {

        PlayParticleSystems(); //Before Explosion
        ApplyImpactForce(); //Apply Force to the other objects(including the player)
        AddCameraShake();
        Disable();
        yield return new WaitForSeconds(t);
        Destroy(gameObject); //AfterExplosion

    }

    #endregion

    #region private GIZMO

    void OnDrawGizmosSelected() //Unity Editor Draw Shoot Point Gizmo.
    {

        Gizmos.DrawWireSphere(transform.position,_fieldOfImpact);

    }

    #endregion

}
