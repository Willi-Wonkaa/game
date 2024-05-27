using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class EnemyScript : MonoBehaviour
{

    private PlayerController player_controller;

    public int radius_of_vision = 5;
    public int radius_of_attack = 3;
    public float movement_speed = 3;

    public int gain_exp = 10;
    public float max_hp = 100;
    public float middle_damage = 10;

    public float current_hp = 100;


    public float area_for_relocation;
    public float attack_speed = 1;


    public bool is_angry = false;
    public bool is_alive = true;
    public bool is_player_in_attack_range = false;
    public bool is_player_in_vision_range = false;
    public bool is_object_in_player_action_range = false;
    public bool is_can_attack = false;
    public bool is_must_follow = false;

    public bool face_direction_right;
    public bool face_direction_up;
    public float distance;

    private float last_attack_time;

    Vector3 range_for_player;

    public Image indicator_hp;

    [SerializeField] private Transform player;


    public List<GameObject> loot_items;

    // дополнительные способности 
    public bool is_vampire;
    public float vampirism_power = 0.1f;

    public bool is_necromancer;
    public GameObject spawned_object;
    // -----------------------

    public float current_damage;

    private float uncertainty = 1;
    private float position_noize = 1;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

    }

    public void Update()
    {
        if (IsEnemyActive())
        {

            uncertainty = UnityEngine.Random.Range(0.8f, 1.2f);
            current_damage = middle_damage * uncertainty;

            face_direction_right = IsFaceDirectionRight();
            face_direction_up = IsFaceDirectionUp();

            is_angry = IsAngry();
            is_alive = IsAlive();
            is_player_in_attack_range = IsPlayerInAttackRange();
            is_player_in_vision_range = IsPlayerInVisionRange();
            is_must_follow = IsMustFollow();
            is_object_in_player_action_range = IsObjectInPlayerActionRange();



            range_for_player = RangeForPlayer();

            if (is_object_in_player_action_range)
            {
                GetDamage();
            }

            if (is_alive)
            {
                if (is_angry)
                {
                    Fight();
                }

                if (is_must_follow)
                {
                    Follow();
                }
            }
            else
            {
                Death();
            }
        }

    }

    public void FixedUpdate()
    {


    }



    void Follow()
    {
        if (face_direction_up)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + movement_speed * Time.deltaTime, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - movement_speed * Time.deltaTime, transform.position.z);
        }

        if (face_direction_right)
        {
            transform.position = new Vector3(transform.position.x + movement_speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x - movement_speed * Time.deltaTime, transform.position.y, transform.position.z);
        }
    }

    void Fight()
    {
        if (is_player_in_attack_range && ((Time.time - last_attack_time) >= attack_speed))
        {
            Attack();
            last_attack_time = Time.time;
            player_controller.last_get_damage = Time.time;
        }
    }

    void GetDamage()
    {
        if (player_controller.is_hitting)
        {
            current_hp -= player_controller.player_current_damage;
            Debug.Log("игрок ударил и нанес следующий урон: " + player_controller.player_current_damage);
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
        player_controller.player_exp += gain_exp;
        player_controller.LevelUp();
        foreach (var loot_item in loot_items)
        {
            position_noize = UnityEngine.Random.Range(-2.0f, 2.0f);
            Instantiate(loot_item, new Vector3(transform.position.x + position_noize, transform.position.y + position_noize, transform.position.z), Quaternion.identity);
        }

        
    }

    void Attack()
    {
        player_controller.player_current_hp -= current_damage;
        Debug.Log("враг нанес урон равный: " + current_damage);
        if (player_controller.player_current_hp <= 0)
        {
            player_controller.Death();
        }

        if (is_vampire)
        {
            Vampirims();
        }
        if (is_necromancer)
        {
            Necromancy();
        }
    }

    void Vampirims()
    {
        if (current_hp < max_hp)
        {
            current_hp += 1 + (int)(vampirism_power * current_damage);
            if (current_hp > max_hp)
            {
                current_hp = max_hp;
            }
        }
    }

    void Necromancy()
    {
        Instantiate(spawned_object, new Vector3(transform.position.x + 1, transform.position.y + 1, transform.position.z), Quaternion.identity);
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
        if(current_hp > 0)
        {
            is_alive = true;
        }
        else
        {
            is_alive = false;
        }
        return is_alive;
    }

    bool IsAngry()
    {
        if(Vector3.Distance(transform.position, player.position) <= radius_of_vision)
        {
            is_angry = true;
        } 
        else
        {
            is_angry = false;
        }
        return is_angry;
    }

    bool IsPlayerInAttackRange()
    {
        if (Vector3.Distance(transform.position, player.position) <= radius_of_attack)
        {
            is_player_in_attack_range = true;
        }
        else
        {
            is_player_in_attack_range = false;
        }
        return is_player_in_attack_range;
    }

    bool IsEnemyActive()
    {
        if (Vector3.Distance(transform.position, player.position) <= 50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool IsPlayerInVisionRange()
    {
        if (Vector3.Distance(transform.position, player.position) <= radius_of_vision)
        {
            is_player_in_vision_range = true;
        }
        else
        {
            is_player_in_vision_range = false;
        }
        return is_player_in_vision_range;
    }

    bool IsFaceDirectionRight()
    {
        if (transform.position.x <= player.position.x)
        {
            face_direction_right = true;
        } else
        {
        face_direction_right = false;
        }
        return face_direction_right;
    }

    bool IsFaceDirectionUp()
    {
        if (transform.position.y <= player.position.y)
        {
            face_direction_up = true;
        } else
        {
            face_direction_up = false;
        }
        return face_direction_up;
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
    /*
    bool IsCanAttack()
    {
        if ()
        {
            is_can_attack = true;
        } 
        else
        {
            is_can_attack = false;
        }
        return is_can_attack;
    }*/

    bool IsMustFollow()
    {
        if (Vector3.Distance(transform.position, player.position) < radius_of_vision)
        {
            is_must_follow = true;
        }
        if (Vector3.Distance(transform.position, player.position) < radius_of_attack)
        {
            is_must_follow = false;
        }

        return is_must_follow;
    }
}
