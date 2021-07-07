using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    #region public IEnumerator

    public IEnumerator ShakeCamera(float duration , float magnitude)
    {

        Vector3 orgPos = transform.position;

        float elapsed = 0.0f;

        while(elapsed < duration)
        {

            float x = Random.Range(-1f,1f)*magnitude;
            float y = Random.Range(-1f,1f)*magnitude;

            transform.position = new Vector3(x,y,orgPos.z);

            elapsed += Time.deltaTime;

            yield return null;

        }

    }

    #endregion
}
