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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rooms[0].enemies = new enemy[0];

        rooms[1].enemies = new enemy[1];
            rooms[1].enemies[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        rooms[2].enemies = new enemy[2];
            rooms[2].enemies[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
            rooms[2].enemies[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        // TODO: Add the enemies for all the rooms
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
