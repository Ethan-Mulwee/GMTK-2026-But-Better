using System.Collections;
using UnityEngine;

public class OneStrike : MonoBehaviour
{

    public GameObject strike;
    public GameObject indicator;
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
    }
}
