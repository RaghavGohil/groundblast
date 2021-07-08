using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    //Vector3:
    Vector3 _orgPos;

    #region private void

    void Start()
    {

        _orgPos = new Vector3(0f,0f,0f);

    }

    void Update()
    {

        _orgPos = transform.position;
        
    }

    #endregion

    #region public IEnumerator
    public IEnumerator ShakeCamera(float duration , float magnitude)
    {

        float elapsed = 0.0f;

        while(elapsed < duration)
        {

            float x = Random.Range(-1f,1f)*magnitude;
            float y = Random.Range(-1f,1f)*magnitude;

            transform.position = new Vector3(_orgPos.x + x,_orgPos.y +y,_orgPos.z);

            elapsed += Time.deltaTime;

            yield return null;

        }

    }

    #endregion
}
