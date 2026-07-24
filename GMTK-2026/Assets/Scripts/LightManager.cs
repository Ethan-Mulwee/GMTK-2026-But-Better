using UnityEngine;

public class LightManager : MonoBehaviour
{
    Light[] lights;
    [SerializeField] float activeLightDistance = 10.0f;
    void Start()
    {
        lights = Object.FindObjectsByType<Light>(FindObjectsSortMode.None);

        foreach (Light light in lights) {
            light.enabled = false;
        }
    }

    void Update() {
        foreach (Light light in lights) {
            if (Vector3.Distance(light.gameObject.transform.position, gameObject.transform.position) < activeLightDistance)
                light.enabled = true;
            else
                light.enabled = false;
        }
    }
}
