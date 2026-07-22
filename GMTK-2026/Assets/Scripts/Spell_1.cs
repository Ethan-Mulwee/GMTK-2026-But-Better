using UnityEngine;

public class Spell_1 : MonoBehaviour
{
    float destroyTimer = 2.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTimer > 0.0f)
        {
            destroyTimer -= Time.deltaTime;
        } else
        {
            Destroy(gameObject);
        }
    }
}
