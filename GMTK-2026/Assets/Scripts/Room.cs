using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int roomID;

    public int enemyCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyCount = Gamemode.rooms[roomID].enemies.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            // TODO: lock all doors leaving room

            //if (enemyCount > 0)
            //{
                for (int i = 0; i < enemyCount; i++)
                {
                    Instantiate(Gamemode.rooms[roomID].enemies[i].go, Gamemode.rooms[roomID].enemies[i].pos, Quaternion.Euler(Gamemode.rooms[roomID].enemies[i].rot));
                }
            //}
        }
    }

    public int getID()
    {
        return roomID;
    }

    public void checkEnemyCount()
    {
        if (enemyCount <= 0)
        {
            // TODO: open all doors leaving room
        }
    }
}
