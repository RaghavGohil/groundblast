using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    
    //Floats:
    float _delta;
    float _positionZ;
    float _cameraZPosition;

    //GameObjects:
    [SerializeField]
    GameObject _player;

    #region private void
    void Start()
    {

        _delta = 10f;

        _cameraZPosition = transform.position.z;

    }

    void FixedUpdate()
    {

        _positionZ = (-_player.GetComponent<Rigidbody2D>().velocity.magnitude)/2f + _cameraZPosition;

        transform.position = Vector3.Lerp(transform.position , new Vector3(_player.transform.position.x,_player.transform.position.y,_positionZ) , _delta*Time.deltaTime);

    }

    #endregion

}
