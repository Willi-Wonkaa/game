using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public InventoryManager inventory_manager;

    public ItemScriptableObject item;
    public int amount;

    public bool is_picked = false;
    public LayerMask what_is_player;

    void Start()
    {
        inventory_manager = GameObject.FindGameObjectWithTag("test_UI").GetComponent<InventoryManager>();
    }

    void Update()
    {
        is_picked = Physics2D.OverlapCircle(gameObject.transform.position, 2, what_is_player);
        if (is_picked)
        {
            inventory_manager.AddItem(item, amount);
            Destroy(gameObject);
        }
    }
}
