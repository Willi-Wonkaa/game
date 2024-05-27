using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class tradecardscript : MonoBehaviour
{
    public InventoryManager inventory_manager;
    public Transform player;

    private TMP_Text name_for_card;
    public Image sprite_for_card;
    public ItemScriptableObject purchased_item;
    public int purchased_count;

    public List<ItemScriptableObject> conditions_items;
    public List<int> conditions_items_count;

    public List<Image> purchased_icons;
    public List<TMP_Text> purchased_count_shower;

    private int conditions_items_len;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        inventory_manager = GameObject.FindGameObjectWithTag("test_UI").GetComponent<InventoryManager>();

        name_for_card = transform.GetChild(0).GetComponent<TMP_Text>();
        conditions_items_len = conditions_items.Count;
    }

    void Update()
    {
        name_for_card.text = purchased_item.item_name.ToString();
        sprite_for_card.sprite = purchased_item.icon;

        for (int i = 0; i < conditions_items_len; i++)
        {
            purchased_icons[i].sprite = conditions_items[i].icon;
            purchased_count_shower[i].text = conditions_items_count[i].ToString();
        }

        for (int i = conditions_items_len; i < purchased_icons.Count; i++)
        {
            purchased_icons[i].sprite = null;
            purchased_count_shower[i].text = "";
        }

    }

    public void ItemPurchase()
    {
        if (IsCanBuy())
        {
            ConsumeItems();
            AwardIssuance();
        }
    }


    public bool IsCanBuy()
    {
        for (int i = 0; i < conditions_items_len; ++i)
        {
            if (!inventory_manager.InventoryItemChecer(conditions_items[i], conditions_items_count[i]))
                return false;
        }
        return true;
    }


    public void ConsumeItems()
    {
        for (int i = 0; i < conditions_items_len; ++i)
        {
            inventory_manager.DeleteItemFromPlayerInventory(conditions_items[i], conditions_items_count[i]);
        }
    }

    public void AwardIssuance()
    {
        inventory_manager.AddItem(purchased_item, purchased_count);
    }
}
