using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    bool cleared = false;
    [SerializeField] int roomID;
    [SerializeField] List<GameObject> enemyList;
    [SerializeField] GameObject reward;
    public DoorTrigger door;
    static Gamemode.room layout;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        layout = Gamemode.rooms[roomID];
        if (reward != null)
            reward.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < enemyList.Count; i++) {
            if (enemyList[i] == null)
                enemyList.Remove(enemyList[i]);
        }
        if (enemyList.Count == 0 && !cleared) {
            // Debug.Log("room cleared");
            if (reward != null)
                reward.SetActive(true);
            cleared = true;
            if (door)
                door.unlock();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !cleared) {
            foreach (GameObject enemy in enemyList) {
                Enemy3 a = enemy.GetComponent<Enemy3>();
                if (a != null) {
                    a.activated = true;
                }
                else {
                    Enemy2 b = enemy.GetComponent<Enemy2>();
                    if (b != null) {
                        b.activated = true;
                    }
                }
            }
        }
        // {
        //     if (layout.enemies.Length > 0)
        //     {
        //         for (int i = 0; i < layout.enemies.Length; i++)
        //         {
        //             Instantiate(layout.enemies[i].go, layout.enemies[i].pos, Quaternion.Euler(layout.enemies[i].rot));
        //             cleared = true;
        //         }
        //     }
        // }
    }
}
