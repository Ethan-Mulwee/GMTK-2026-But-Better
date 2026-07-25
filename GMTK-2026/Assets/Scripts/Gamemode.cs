using UnityEngine;

public class Gamemode : MonoBehaviour
{
    public enum enemyType
    {
        goblin,
        skeleton
    }

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
        public enemy[] enemyList;
    }

    public static room[] roomList = new room[15];

    [Header("Enemies")]
    [SerializeField] public GameObject goblinPF;
    [SerializeField] public GameObject skeletonPF;
    [SerializeField] public GameObject bossPF;

    [HideInInspector] public static GameObject gm;

    private void Awake()
    {
        roomList[0].enemyList = new enemy[0];

        roomList[1].enemyList = new enemy[1];
        roomList[1].enemyList[0] = newEnemy(goblinPF, new Vector3(16, 1, 0), new Vector3(0, 0, 0));

        roomList[2].enemyList = new enemy[2];
        roomList[2].enemyList[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[2].enemyList[1] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        roomList[3].enemyList = new enemy[2];
        roomList[3].enemyList[0] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[3].enemyList[1] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        roomList[4].enemyList = new enemy[0];

        roomList[5].enemyList = new enemy[0];

        roomList[6].enemyList = new enemy[0];

        roomList[7].enemyList = new enemy[3];
        roomList[7].enemyList[0] = newEnemy(skeletonPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[7].enemyList[1] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[7].enemyList[2] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        roomList[8].enemyList = new enemy[0];

        roomList[9].enemyList = new enemy[0];

        roomList[10].enemyList = new enemy[4];
        roomList[10].enemyList[0] = newEnemy(skeletonPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[10].enemyList[1] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[10].enemyList[2] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[10].enemyList[3] = newEnemy(goblinPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        roomList[11].enemyList = new enemy[0];

        roomList[12].enemyList = new enemy[0];

        roomList[13].enemyList = new enemy[2];
        roomList[13].enemyList[0] = newEnemy(skeletonPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
        roomList[13].enemyList[1] = newEnemy(skeletonPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));

        roomList[14].enemyList = new enemy[1];
        roomList[14].enemyList[0] = newEnemy(bossPF, new Vector3(0, 0, 0), new Vector3(0, 0, 0));
    }

    private void Start()
    {
        gm = gameObject;
    }

    public static enemyType checkType(enemy enemy)
    {
        if (enemy.go == gm.GetComponent<Gamemode>().skeletonPF)
        {
            return enemyType.skeleton;
        } else
        {
            return enemyType.goblin;
        }
    }

    public static void removeEnemy(int roomID, enemyType type)
    {
        int newLength = roomList[roomID].enemyList.Length - 1;
        enemy[] newEnemyList = new enemy[newLength];

        bool wasRemoved = false;

        if (newLength >= 0)
        {
            for (int i = 0; i < roomList[roomID].enemyList.Length; i++)
            {
                if (wasRemoved)
                {
                    newEnemyList[i] = roomList[roomID].enemyList[i];
                }
                else
                {
                    if (checkType(roomList[roomID].enemyList[i]) == type)
                    {
                    }
                    else
                    {
                        newEnemyList[i] = roomList[roomID].enemyList[i];
                    }
                }
            }

            roomList[roomID].enemyList = newEnemyList;
        }
    }
}
