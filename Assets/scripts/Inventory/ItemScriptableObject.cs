using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ItemType {Materials, Sword, Tool, Armour};
public enum EquipmentType {None, Armor, Charm, Shield};

public class ItemScriptableObject : ScriptableObject
{
    public ItemType item_type;
    public EquipmentType equipment_type;
    public int id;

    public float increases_stats_by_times = 1;

    public float increases_attack_on_number = 0;
    public float increases_hp_by_number = 0;
    public float increases_digging_by_number = 0;

    public string item_name;
    public GameObject item_prefab;
    public Sprite icon;
}
