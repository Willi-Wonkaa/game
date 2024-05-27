using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoadManager : MonoBehaviour
{
    private string file_path;

    public List<GameObject> enemy_saves = new List<GameObject>();

    // Start is called before the first frame update
    private void Start()
    {
        file_path = Application.persistentDataPath + "/save.gamesave";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(file_path, FileMode.Create);

        Save save = new Save();

        save.SaveEnemies(enemy_saves);

        bf.Serialize(fs, save);

        fs.Close();
    }

    public void LoadGame()
    {
        if (!File.Exists(file_path)) {
            return;
        }
            

        BinaryFormatter bf = new BinaryFormatter();
        FileStream fs = new FileStream(file_path, FileMode.Open); 

        Save save = (Save)bf.Deserialize(fs);
        fs.Close();

    }
}


[System.Serializable]
public class Save
{
    [System.Serializable]
    public struct Vec3
    {
        public float x, y, z;

        public Vec3(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }
    [System.Serializable]
    public struct EnemySaveData
    {
        public Vec3 position;

        public EnemySaveData(Vec3 pos)
        {
            position = pos;
            
        }
    }

    public List<EnemySaveData> enemies_data = new List<EnemySaveData>();

    public void SaveEnemies(List<GameObject> enemies)
    {
        foreach (var enemy in enemies)
        {
            var em = enemy.GetComponent<EnemyScript>();

            Vec3 pos = new Vec3(enemy.transform.position.x, enemy.transform.position.y, enemy.transform.position.z);

            enemies_data.Add(new EnemySaveData(pos));
        }
    }
}

