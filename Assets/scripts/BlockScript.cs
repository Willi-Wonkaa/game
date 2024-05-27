using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Blockscript : MonoBehaviour
{
    private PlayerController player_controller;

    private bool is_alive = true;
    private bool is_object_in_player_action_range = false;

    public float current_hp = 10;
    private float max_hp;

    private float position_noize = 1;

    public List<GameObject> loot_items;

    public Transform player;

    public Image indicator_hp;

    private void Start()
    {
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        max_hp = current_hp;
    }

    public void Update()
    {
        is_object_in_player_action_range = IsObjectInPlayerActionRange();
        is_alive = IsAlive();

        if (is_alive)
        {
            if (is_object_in_player_action_range)
            {
                GetDamage();

            }
        } else
        {
            Death();
        }
    }

    public void FixedUpdate()
    {

    }

    
    void GetDamage()
    {
        if (player_controller.is_digging)
        {
            current_hp -= player_controller.player_current_digging;
            Debug.Log("игрок нанес следующий удар по блоку: " + player_controller.player_current_digging);
            indicator_hp.fillAmount = current_hp / max_hp;
        }
    }

    void Death()
    {
        DropLoot();
        Destroy(gameObject);
    }

    void DropLoot()
    {
        //GetComponent<LootBag>().InstantiateLoot(transform.position);
        foreach (var loot_item in loot_items)
        {
            position_noize = UnityEngine.Random.Range(-2.0f, 2.0f);
            Instantiate(loot_item, new Vector3(transform.position.x + position_noize, transform.position.y + position_noize, transform.position.z), Quaternion.identity);
        }
    }

    Vector3 RangeForPlayer()
    {
        Vector3 range_for_player = new Vector3(transform.position.x - player.position.x, transform.position.y - player.position.y, transform.position.z - player.position.z);
        if (range_for_player.x < 0)
        {
            range_for_player.x *= -1;
        }
        if (range_for_player.y < 0)
        {
            range_for_player.y *= -1;
        }
        if (range_for_player.z < 0)
        {
            range_for_player.z *= -1;
        }
        return range_for_player;
    }

    bool IsAlive()
    {
        if (current_hp > 0)
        {
            is_alive = true;
        }
        else
        {
            is_alive = false;
        }
        return is_alive;
    }

    bool IsObjectInPlayerActionRange()
    {
        if (Vector3.Distance(transform.position, player.position) <= player_controller.action_radius)
        {
            is_object_in_player_action_range = true;
        }
        else
        {
            is_object_in_player_action_range = false;
        }
        return is_object_in_player_action_range;
    }

}
/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private PlayerController player_controller;
    private PlayerStatistics player_statistics;

    public int basic_hp = 10;
    public int current_hp = 10;

    public Transform target;



    void Start()
    {
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        RangeForPlayer();
    }

    public void FixedUpdate()
    {
        Digging();
        if (current_hp <= 0)
        {
            Death();
        }
    }

    private void Digging()
    {
        if ((player_controller.is_digging) && (range_for_player <= player_controller.action_radius))
        {
            current_hp -= player_statistics.digging_power;
        }
    }

    Vector3 RangeForPlayer()
    {
        Vector3 range_for_player = new Vector3(transform.position.x - target.position.x, transform.position.y - target.position.y, transform.position.z - target.position.z);
        if (range_for_player.x < 0)
        {
            range_for_player.x *= -1;
        }
        if (range_for_player.y < 0)
        {
            range_for_player.y *= -1;
        }
        if (range_for_player.z < 0)
        {
            range_for_player.z *= -1;
        }
        return range_for_player;
    }


    void Death()
    {
        DropLoot();
        Destroy(gameObject);
    }

    void DropLoot()
    {

    }

    bool IsPlayerInAtionRange()
    {
        if (Vector3.Distance(transform.position, target.position) <= radius_of_vision)
        {
            is_player_in_attack_range = true;
        }
        else
        {
            is_player_in_attack_range = false;
        }
        return is_player_in_attack_range;
    }

}
*/
