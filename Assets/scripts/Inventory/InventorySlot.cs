using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    public ItemScriptableObject item;
    public int amount;
    public bool is_empty = true;
    public GameObject icon_go;
    public TMP_Text count;
    public EquipmentType equipment_type = EquipmentType.None;

    private void Awake()
    {
        icon_go = transform.GetChild(0).GetChild(0).gameObject;
        count = transform.GetChild(0).GetChild(1).GetComponent<TMP_Text>();
    }
    
    public void SetIcon(Sprite icon)
    {
        icon_go.GetComponent<Image>().sprite = icon;
    }
    
}
