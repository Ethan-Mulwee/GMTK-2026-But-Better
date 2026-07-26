using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject target;
    

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, target.transform.position, 0.06f);
    }

    public void StartShake(float duration, float strength) {
        StartCoroutine(Shake(duration, strength));
    }

    IEnumerator Shake(float duration, float strength) {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            transform.position += Random.insideUnitSphere * strength;
            yield return null;
        }


    }
}
