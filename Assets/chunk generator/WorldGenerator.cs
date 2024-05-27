using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WorldGenerator : MonoBehaviour
{
    public GameObject city;
    public GameObject boss_chunk;
    private bool is_boos_spawned = false;



    public List<GameObject> first_circle_parts;


    public List<GameObject> relics;
    private List<bool> is_relic_spawned;

    private int X = 24;
    private int Y = 24;

    private int world_x_size;

    private int world_y_size;

    private int current_x;
    private int current_y;

    private int x = -600;
    private int y = -600;
    private int c = 0;
    private int boss_room_pos;

    void Start()
    {
        /*
        for (int i = 0; i < relics.Count; ++i)
        {
            is_relic_spawned.Add(false);
        } */

        boss_room_pos = UnityEngine.Random.Range(0, 83);
        //MapFilling(X, Y);
        for (int i = 0; i < X; i++)
        {
            for (int j = 0; j < Y; j++)
            {
                if (IsNotOnCity(x, y)) {
                    if (PosRadius(i, j) == 11 && is_boos_spawned == false)
                    {
                        c++;
                        if (c == boss_room_pos) {
                            BossRoomMaker(x, y);
                        } else
                        {
                            ChunkMaker(x, y);
                        }
                    } else
                    {
                        ChunkMaker(x, y);
                    }                   
                } else
                {
                    Debug.Log(PosRadius(i, j));
                }
                y += 50;

            }
            x += 50;
            y = -600;
        }

        CityBuilder();
    }

    public void ChunkMaker(int x, int y)
    {
        Instantiate(first_circle_parts[UnityEngine.Random.Range(0, first_circle_parts.Count)], new Vector3(x, y, transform.position.z), Quaternion.identity);
        
    }

    public void BossRoomMaker(int x, int y)
    {
        Instantiate(boss_chunk, new Vector3(x, y, transform.position.z), Quaternion.identity);

        //Instantiate(first_circle_parts[UnityEngine.Random.Range(0, first_circle_parts.Count)], new Vector3(x, y, transform.position.z), Quaternion.identity);
    }

    public void CityBuilder()
    {
        Instantiate(city, new Vector3(0, 0, transform.position.z), Quaternion.identity);
    }

    public bool IsNotOnCity(int x, int y) 
    {
        if ((x >= -75 && x <= 75) && (y >= -75 && y <= 75))
        {
            return false;
        }
        return true;
    }

    private int PosRadius(int x, int y)
    {
        if ((x == 23 || x == 0) || (y == 23 || y == 0))
            return 12;

        if (((x == 22 || x == 1) && (y != 0 && y != 23)) || ((y == 22 || y == 1) && (x != 0 && x != 23)))
            return 11;

        if (((x == 21 || x == 2) && (y > 1 && y < 22)) || ((y == 21 || y == 2) && (x > 1 && x < 22)))
            return 10;

        if (((x == 20 || x == 3) && (y > 2 && y < 21)) || ((y == 20 || y == 3) && (x > 2 && x < 21)))
            return 9;

        if (((x == 19 || x == 4) && (y > 3 && y < 20)) || ((y == 19 || y == 4) && (x > 3 && x < 20)))
            return 8;

        if (((x == 18 || x == 5) && (y > 4 && y < 19)) || ((y == 18 || y == 5) && (x > 4 && x < 19)))
            return 7;

        if (((x == 17 || x == 6) && (y > 5 && y < 18)) || ((y == 17 || y == 6) && (x > 5 && x < 18)))
            return 6;

        if (((x == 16 || x == 7) && (y > 6 && y < 17)) || ((y == 16 || y == 7) && (x > 6 && x < 17)))
            return 5;

        if (((x == 15 || x == 8) && (y > 7 && y < 16)) || ((y == 15 || y == 8) && (x > 7 && x < 16)))
            return 4;

        if (((x == 14 || x == 9) && (y > 8 && y < 15)) || ((y == 14 || y == 9) && (x > 8 && x < 15)))
            return 3;

        if (((x == 13 || x == 8) && (y > 9 && y < 16)) || ((y == 13 || y == 10) && (x > 9 && x < 15)))
            return 2;

        return 0;

    }


}


