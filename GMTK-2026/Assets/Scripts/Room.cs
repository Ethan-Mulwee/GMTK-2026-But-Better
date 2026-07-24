using UnityEngine;

public class Room : MonoBehaviour
{
    bool cleared = false;
    [SerializeField] private int roomID;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !cleared)
        {
            Debug.Log("ID: " + roomID);
            Debug.Log("Gamemode.rooms[roomID].enemies.Length: " + Gamemode.rooms[roomID].enemies.Length);

            for (int i = 0; i < Gamemode.rooms[roomID].enemies.Length; i++)
            {
                Instantiate(Gamemode.rooms[roomID].enemies[i].go, Gamemode.rooms[roomID].enemies[i].pos, Quaternion.Euler(Gamemode.rooms[roomID].enemies[i].rot));
                cleared = true;
            }
        }
    }
}
