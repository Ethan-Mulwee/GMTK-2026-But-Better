using UnityEngine;
using UnityEngine.SceneManagement;

public class fallkill : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("collidered");
      if (other.gameObject.tag == "Player") {
        Debug.Log("asdsdf");
        SceneManager.LoadScene("Dungeon");
      }  
    }
}
