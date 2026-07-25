using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] private int roomID;

    [HideInInspector] public int enemyCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        enemyCount = Gamemode.roomList[roomID].enemyList.Length;
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

            for (int i = 0; i < enemyCount; i++)
            {
                Instantiate(Gamemode.roomList[roomID].enemyList[i].go, Gamemode.roomList[roomID].enemyList[i].pos, Quaternion.Euler(Gamemode.roomList[roomID].enemyList[i].rot));
            }
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
