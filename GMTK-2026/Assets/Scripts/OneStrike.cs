using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OneStrike : MonoBehaviour
{

    public GameObject strike;
    public GameObject indicator;
    public WizardController target;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(timeStrike());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator timeStrike() {
        yield return new WaitForSeconds(1);
        indicator.SetActive(false);
        strike.SetActive(true);

        RaycastHit[] raycastHits = new RaycastHit[40];
        for (int i = 0; i < 40; i++) {
            float angle = (i/((float)40))*Mathf.PI*2.0f;
            Ray ray = new Ray(transform.position, new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)));
            Physics.SphereCast(ray, 0.3f, out raycastHits[i], 1.0f, LayerMask.GetMask("Player"));
        }
        foreach (RaycastHit hit in raycastHits) {
            if (hit.collider != null) {
            
                WizardController wizard = hit.collider.gameObject.GetComponent<WizardController>();
                if (wizard != null) {
                    target.hurt(40);
                    break;
                }
            }
        }

        yield return new WaitForSeconds(0.2f);
        Destroy(gameObject);
    }
}
