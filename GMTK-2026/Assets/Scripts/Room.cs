using UnityEngine;

public class Room : MonoBehaviour
{
    bool cleared = false;
    [SerializeField] int roomID;
    static Gamemode.room layout;

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
            if (layout.enemies.Length > 0)
            {
                for (int i = 0; i < layout.enemies.Length; i++)
                {
                    Instantiate(layout.enemies[i].go, layout.enemies[i].pos, Quaternion.Euler(layout.enemies[i].rot));
                    cleared = true;
                }
            }
        }
    }
}
