using UnityEngine;

public class Gamemode : MonoBehaviour
{
    public struct enemy
    {
        public GameObject go;
        public Vector3 pos;
        public Vector3 rot;
    }

    enemy newEnemy (GameObject go, Vector3 pos, Vector3 rot)
    {
        enemy output;
        output.go = go;
        output.pos = pos;
        output.rot = rot;

        return output;
    }
    
    public struct room
    {
        public enemy[] enemies;
    }

    public static room[] rooms = new room[15];

    [Header("Enemies")]
    [SerializeField] GameObject goblinPF;

    private void Awake()
    {
        rooms[0].enemies = new enemy[0];

        rooms[1].enemies = new enemy[1];
        rooms[1].enemies[0] = newEnemy(goblinPF, new Vector3(16, 1, -3), new Vector3(0, 0, 0));

        rooms[2].enemies = new enemy[2];
        rooms[2].enemies[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        rooms[2].enemies[1] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        rooms[3].enemies = new enemy[2];
        rooms[3].enemies[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        rooms[3].enemies[1] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        rooms[4].enemies = new enemy[0];

        rooms[5].enemies = new enemy[0];

        rooms[6].enemies = new enemy[0];

        rooms[7].enemies = new enemy[3];
        rooms[7].enemies[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        rooms[7].enemies[1] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        rooms[7].enemies[2] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        rooms[8].enemies = new enemy[0];

        rooms[9].enemies = new enemy[0];

        rooms[10].enemies = new enemy[0];

        rooms[11].enemies = new enemy[0];

        rooms[12].enemies = new enemy[0];

        rooms[13].enemies = new enemy[0];

        rooms[14].enemies = new enemy[0];

        for (int i = 0; i < rooms.Length; i++)
        {
            Debug.Log(rooms[i].enemies.Length);
        }

        Debug.Log("init complete");
    }
}
