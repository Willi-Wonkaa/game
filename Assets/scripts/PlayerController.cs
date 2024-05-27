using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;
    private InventoryManager inventory_manager;
    private QuickslotInventory quickslot_inventory;

    public static PlayerController instance = null;

    public GameObject[] item_in_inventory;


    public int player_strength = 1;
    public int player_intellegance = 1;
    public int player_lvl = 1;

    public float player_max_hp = 650f;
    public float base_player_max_hp = 650f;
    public float movement_speed = 5;
    public int talking_radius = 5;
    public int action_radius = 5;
    public float masking = 0f;


    public int player_money = 0;
    public int player_exp = 0;

    public float player_attack_speed = 0.3f;
    public float player_digging_speed = 0.3f;

    public float player_current_hp = 650f;
    public float base_player_regen_count = 0.25f;
    public float player_regen_count = 0.25f;

    public float player_middle_damage = 29f;
    public float item_damage = 0;

    public float player_digging_power = 1f;
    public float item_digging = 0;


    public float influence_stats_by_times = 1;
    public float influence_attack_on_number = 0;
    public float influence_hp_by_number = 0;
    public float influence_digging_by_number = 0;



    public int[] count_in_inventory;

    public bool is_hitting = false;
    public bool is_digging = false;
    private bool is_alive = true;
    public bool is_can_move = true;
    public bool is_can_regerate;

    private float last_hitting_time;
    private float last_digging_time;
    public float last_get_damage;

    public float player_current_damage;
    public float player_current_digging;

    private float uncertainty = 1;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        player_lvl = 1;
    }

    private void Start()
    {
        inventory_manager = GameObject.FindGameObjectWithTag("test_UI").GetComponent<InventoryManager>();
        quickslot_inventory = GameObject.FindGameObjectWithTag("quick_slot_pannel_tag").GetComponent<QuickslotInventory>();

        if (instance == null)
        {
            instance = this;
        } 
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        is_digging = IsDigging();
        is_hitting = IsHitting();
        is_can_regerate = IsCanRegenerate();
        is_can_move = !(inventory_manager.is_inventory_open);

        uncertainty = UnityEngine.Random.Range(0.8f, 1.2f);
        item_damage = quickslot_inventory.item_current_damage;
        item_digging = quickslot_inventory.item_current_digging ;
        player_current_damage = (player_middle_damage + item_damage) * influence_stats_by_times * uncertainty;
        player_current_digging = (player_digging_power + item_digging) * influence_stats_by_times;
        player_max_hp = (base_player_max_hp + influence_hp_by_number) * influence_stats_by_times;
        player_regen_count = base_player_regen_count * influence_stats_by_times;




        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log("урон: " + player_current_damage + ", добыча: " + player_current_digging);
        }
    }

    private void FixedUpdate()
    {
        if (is_alive)
        {
            if (is_can_move)
            {
                Move();
            }
            if (is_can_regerate && player_current_hp < player_max_hp)
            {
                Regeneration();
            }
        }
        
    }



    void Move()
    {
        Vector2 inputVector = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            inputVector.y = 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector.x = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector.y = -1f;
        }
        if (Input.GetKey(KeyCode.A)) 
        {
            inputVector.x = -1f;
        }
        inputVector = inputVector.normalized;

        rb.MovePosition(rb.position + inputVector * movement_speed * Time.fixedDeltaTime);
    }

    public void Death()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void Regeneration()
    {
        player_current_hp += player_regen_count * Time.fixedDeltaTime;
        if (player_current_hp > player_max_hp)
        {
            player_current_hp = player_max_hp;
        }
    }

    public void LevelUp()
    {
        while ((int)(10 + Math.Round(player_lvl * Math.Log(Math.Pow(6.9, player_lvl)))) <=  player_exp)
        {
            player_exp -= (int)(10 + Math.Round(player_lvl * Math.Log(Math.Pow(6.9, player_lvl))));
            ++player_lvl;
            ++player_strength;
            ++player_intellegance;
            player_middle_damage *= 1.02f;
            player_max_hp *= 1.02f;
            player_regen_count = player_max_hp / 150.0f;
        }
    }


    public bool IsHitting()
    {
        if (Input.GetMouseButtonDown(0) && (Time.time - last_hitting_time >= player_attack_speed))
        {
            is_hitting = true;
            last_hitting_time = Time.time;
        }
        else
        {
            is_hitting = false;
        }
        return is_hitting;
    }

    public bool IsDigging()
    {
        if (Input.GetMouseButton(0) && (Time.time - last_hitting_time >= player_attack_speed))
        {
            is_digging = true;
            last_digging_time = Time.time;
        }
        else
            {
                is_digging = false;
            }
        return is_digging;
    }

    bool IsCanRegenerate()
    {
        if (Time.time - last_get_damage >= 3)
        {
            is_can_regerate = true;
        } 
        else
        {
            is_can_regerate = false;
        }
        return is_can_regerate;
    }

    bool IsAlive()
    {
        if (player_current_hp > 0)
        {
            is_alive = true;
        }
        else
        {
            is_alive = false;
        }
        return is_alive;
    }

}
