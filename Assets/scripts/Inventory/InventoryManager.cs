using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    private PlayerController player_controller;

    public GameObject UI_panel_inventory;
    public GameObject UI_panel_equipment;
    public Transform inventory_panel;
    public Transform equipment;
    public Transform hot_bar;
    public List<InventorySlot> slots = new List<InventorySlot>();
    // Armor Charm, Shield
    public List<InventorySlot> equipment_slots = new List<InventorySlot>();

    public float influence_hp_by_number_equipment = 0;


    public bool is_inventory_open = false;

    private void Awake()
    {
        UI_panel_inventory.SetActive(true);
        UI_panel_equipment.SetActive(true);
    }
    void Start()
    {

        UI_panel_inventory.SetActive(false); 
        UI_panel_equipment.SetActive(false);

        for (int i = 0; i < hot_bar.childCount; ++i)
        {
            slots.Add(hot_bar.GetChild(i).GetComponent<InventorySlot>());
        }

        for (int i = 0; i < inventory_panel.childCount; ++i)
        {
            slots.Add(inventory_panel.GetChild(i).GetComponent<InventorySlot>());
        }
        for (int i = 0; i < equipment.childCount - 1; ++i)
        {
            equipment_slots.Add(equipment.GetChild(i).GetComponent<InventorySlot>());
        }

        player_controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            is_inventory_open = !is_inventory_open;

            if (is_inventory_open)
            {
                UI_panel_inventory.SetActive(true);
                UI_panel_equipment.SetActive(true);
            } 
            else
            {
                UI_panel_inventory.SetActive(false);
                UI_panel_equipment.SetActive(false);
            }
        }
        if (is_inventory_open)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UI_panel_inventory.SetActive(false);
                UI_panel_equipment.SetActive(false);
                is_inventory_open = !is_inventory_open;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            InventoryCheckAllItems();
        }
        CheckEquipment();
    }

    public void AddItem(ItemScriptableObject _item, int _amount)
    {
        foreach(InventorySlot slot in slots)
        {
            if (slot.item == _item)
            {
                slot.amount += _amount;
                slot.count.text = slot.amount.ToString();
                Debug.Log("Игрок подобрал " + _item.item_name + ", в количестве " + _amount);
                return;
            }
        }

        foreach(InventorySlot slot in slots)
        {
            if (slot.is_empty == true)
            {
                slot.item = _item;
                slot.amount = _amount;
                slot.is_empty = false;
                slot.SetIcon(_item.icon);
                slot.count.text = _amount.ToString();
                Debug.Log("Игрок подобрал " + _item.item_name + ", в количестве " + _amount);
                return;
            }
        }
    }

    
    public void InventoryCheckAllItems()
    {
        Debug.Log("У игрока в инвентаре содержится: \n");
        foreach(InventorySlot slot in slots)
        {
            if (slot.is_empty == false)
            {

                Debug.Log(slot.item.item_name + " в количестве: " + slot.amount);
            }
        }
    }



    public bool InventoryItemChecer(ItemScriptableObject desired_item, int needed_count)
    {
        foreach (InventorySlot slot in slots)
        {
            if (slot.is_empty == false)
            {
                if (slot.item == desired_item && slot.amount >= needed_count)
                {
                    return true;
                } 
            }
        }
        return false;
    }

    public void DeleteItemFromPlayerInventory(ItemScriptableObject del_item_name, int del_count)
    {
        Debug.Log("У игрока в инвентаре содержится: \n");
        
        foreach (InventorySlot slot in slots)
        {
            if (slot.is_empty == false)
            {
                if (slot.item == del_item_name)
                {
                    slot.amount -= del_count;
                    if (slot.amount == 0)
                    {
                        NullifySlotData(slot);
                    } 

                }
            }
        }
        

    }

    public void CheckEquipment()
    {
        player_controller.influence_stats_by_times = 1;
        influence_hp_by_number_equipment = 0;
        foreach (InventorySlot equip in equipment_slots)
        {
            if (equip.is_empty == false)
            {
                if (equip.item.equipment_type == EquipmentType.Armor) {
                    influence_hp_by_number_equipment += equip.item.increases_hp_by_number;
                } else if (equip.item.equipment_type == EquipmentType.Charm) {
                    player_controller.influence_stats_by_times = equip.item.increases_stats_by_times;
                } else if (equip.item.equipment_type == EquipmentType.Shield) {
                    influence_hp_by_number_equipment += equip.item.increases_hp_by_number;
                }
            }
        }
        player_controller.influence_hp_by_number = influence_hp_by_number_equipment;
    }

    public void NullifySlotData(InventorySlot slot)
    {
        // убираем значения InventorySlot
        slot.item = null;
        slot.amount = 0;
        slot.is_empty = true;
        //oldSlot.icon_go.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        slot.icon_go.GetComponent<Image>().sprite = null;
        slot.count.text = "";
    }

}
