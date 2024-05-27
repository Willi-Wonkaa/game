using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ChunkGenerator : MonoBehaviour
{
    public GameObject chunk;

    public List<GameObject> spawn_objects;
    public List<int> spawn_count;
    private int spawn_list_len;

    private float X;
    private float Y;

    void Start()
    {
        spawn_list_len = spawn_objects.Count;
        GenerateFullChunk();
    }

    public void SpawnObjectInChunk(GameObject spawned_object)
    {
        X = transform.position.x + UnityEngine.Random.Range(-24.5f, 24.5f);
        Y = transform.position.y + UnityEngine.Random.Range(-24.5f, 24.5f);
        Instantiate(spawned_object, new Vector3(X, Y, transform.position.z), Quaternion.identity);
    }

    public void GenerateFullChunk()
    {
        for (int i = 0; i < spawn_list_len; i++)
        {
            for (int j = 0; j < spawn_count[i]; j++)
            {
                SpawnObjectInChunk(spawn_objects[i]);
            }
        }
    }

}
