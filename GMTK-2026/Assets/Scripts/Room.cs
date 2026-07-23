using UnityEngine;

public class Room : MonoBehaviour
{
    bool cleared = false;
    [SerializeField] int roomID;
    Gamemode.room layout;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layout = Gamemode.rooms[roomID];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !cleared)
        {
            foreach (Gamemode.enemy e in layout.enemies)
            {
                Instantiate(e.go, e.pos, Quaternion.Euler(e.rot));
                cleared = true;
            }
        }
    }
}
