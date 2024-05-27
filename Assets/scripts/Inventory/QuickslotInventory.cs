using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuickslotInventory : MonoBehaviour
{
    public PlayerController player_controller;
    public float item_current_damage = 0;
    public float item_current_digging = 0;

    // ������ � �������� ���� �������� �������
    public Transform quickslot_parent;
    public InventoryManager inventory_manager;
    public int current_quickslot_ID = 0;
    public Sprite selected_sprite;
    public Sprite not_nelected_sprite;
    //public Text healthText;

    // Update is called once per frame
    void Update()
    {
        float mw = Input.GetAxis("Mouse ScrollWheel");
        // ���������� �������� �����
        if (mw < 0.1)
        {
            // ����� ���������� ���� � ������ ��� �������� �� �������
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
            // ���� ������ ��������� ����� ������ � ���� ����� currentQuickslotID ����� ���������� �����, �� �������� ��� ������ ���� (������ ���� ��������� �������)
            if (current_quickslot_ID >= quickslot_parent.childCount - 1)
            {
                current_quickslot_ID = 0;
            }
            else
            {
                // ���������� � ����� currentQuickslotID ��������
                current_quickslot_ID++;
            }
            // ����� ���������� ���� � ������ ��� �������� �� "���������"
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
            // ��� �� ������ � ���������:
            HandItemCheck();
        }
        if (mw > -0.1)
        {
            // ����� ���������� ���� � ������ ��� �������� �� �������
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
            // ���� ������ ��������� ����� ����� � ���� ����� currentQuickslotID ����� 0, �� �������� ��� ��������� ����
            if (current_quickslot_ID <= 0)
            {
                current_quickslot_ID = quickslot_parent.childCount - 1;
            }
            else
            {
                // ��������� ����� currentQuickslotID �� 1
                current_quickslot_ID--;
            }
            // ����� ���������� ���� � ������ ��� �������� �� "���������"
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
            // ��� �� ������ � ���������:
            HandItemCheck();
        }
        // ���������� �����
        for (int i = 0; i < quickslot_parent.childCount; i++)
        {
            // ���� �� �������� �� ������� 1 �� 5 ��...
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                // ��������� ���� ��� ��������� ���� ����� ����� ������� � ��� ��� ������, ��
                if (current_quickslot_ID == i)
                {
                    // ������ �������� "selected" �� ���� ���� �� "not selected" ��� ��������
                    if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite == not_nelected_sprite)
                    {
                        quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
                    }
                    else
                    {
                        quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
                    }
                }
                // ����� �� ������� �������� � ����������� ����� � ������ ���� ������� �� ��������
                else
                {
                    quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
                    current_quickslot_ID = i;
                    quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
                }
            }
            HandItemCheck();
        }

        // ���������� ������� �� ������� �� ����� ������ ����
            /*
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item != null)
                {
                    if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.isConsumeable && !inventory_manager.isOpened && quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite == selected_sprite)
                    {
                        // ��������� ��������� � �������� (������� � ������ � �����) 
                        ChangeCharacteristics();

                        if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().amount <= 1)
                        {
                            quickslot_parent.GetChild(current_quickslot_ID).GetComponentInChildren<DragAndDropItem>().NullifySlotData();
                        }
                        else
                        {
                            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().amount--;
                            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().itemAmountText.text = quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().amount.ToString();
                        }
                    }
                }
            }
            */
    }

    void HandItemCheck() {
        if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item != null)
        {
            if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.item_type == ItemType.Sword)
            {
                item_current_damage = quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.increases_attack_on_number;
                item_current_digging = 0;
            }
            else if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.item_type == ItemType.Tool)
            {
                item_current_digging = quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.increases_digging_by_number;
                item_current_damage = 0;
            }
        }
        else
        {
            item_current_damage = 0;
            item_current_digging = 0;
        }
    }
            /*
        private void ChangeCharacteristics()
        {
            // ���� �������� + ����������� �������� �� �������� ������ ��� ����� 100, �� ������ ����������... 
            if (int.Parse(healthText.text) + quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.changeHealth <= 100)
            {
                float newHealth = int.Parse(healthText.text) + quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.changeHealth;
                healthText.text = newHealth.ToString();
            }
            // �����, ������ ������ �������� �� 100
            else
            {
                healthText.text = "100";
            }

        }
    */
        



}
