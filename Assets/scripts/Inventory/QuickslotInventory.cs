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

    // Объект у которого дети являются слотами
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
        // Используем колесико мышки
        if (mw < 0.1)
        {
            // Берем предыдущий слот и меняем его картинку на обычную
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
            // Если крутим колесиком мышки вперед и наше число currentQuickslotID равно последнему слоту, то выбираем наш первый слот (первый слот считается нулевым)
            if (current_quickslot_ID >= quickslot_parent.childCount - 1)
            {
                current_quickslot_ID = 0;
            }
            else
            {
                // Прибавляем к числу currentQuickslotID единичку
                current_quickslot_ID++;
            }
            // Берем предыдущий слот и меняем его картинку на "выбранную"
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
            // Что то делаем с предметом:
            HandItemCheck();
        }
        if (mw > -0.1)
        {
            // Берем предыдущий слот и меняем его картинку на обычную
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
            // Если крутим колесиком мышки назад и наше число currentQuickslotID равно 0, то выбираем наш последний слот
            if (current_quickslot_ID <= 0)
            {
                current_quickslot_ID = quickslot_parent.childCount - 1;
            }
            else
            {
                // Уменьшаем число currentQuickslotID на 1
                current_quickslot_ID--;
            }
            // Берем предыдущий слот и меняем его картинку на "выбранную"
            quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
            // Что то делаем с предметом:
            HandItemCheck();
        }
        // Используем цифры
        for (int i = 0; i < quickslot_parent.childCount; i++)
        {
            // если мы нажимаем на клавиши 1 по 5 то...
            if (Input.GetKeyDown((i + 1).ToString()))
            {
                // проверяем если наш выбранный слот равен слоту который у нас уже выбран, то
                if (current_quickslot_ID == i)
                {
                    // Ставим картинку "selected" на слот если он "not selected" или наоборот
                    if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite == not_nelected_sprite)
                    {
                        quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
                    }
                    else
                    {
                        quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
                    }
                }
                // Иначе мы убираем свечение с предыдущего слота и светим слот который мы выбираем
                else
                {
                    quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = not_nelected_sprite;
                    current_quickslot_ID = i;
                    quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite = selected_sprite;
                }
            }
            HandItemCheck();
        }

        // Используем предмет по нажатию на левую кнопку мыши
            /*
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item != null)
                {
                    if (quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.isConsumeable && !inventory_manager.isOpened && quickslot_parent.GetChild(current_quickslot_ID).GetComponent<Image>().sprite == selected_sprite)
                    {
                        // Применяем изменения к здоровью (будущем к голоду и жажде) 
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
            // Если здоровье + добавленное здоровье от предмета меньше или равно 100, то делаем вычисления... 
            if (int.Parse(healthText.text) + quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.changeHealth <= 100)
            {
                float newHealth = int.Parse(healthText.text) + quickslot_parent.GetChild(current_quickslot_ID).GetComponent<InventorySlot>().item.changeHealth;
                healthText.text = newHealth.ToString();
            }
            // Иначе, просто ставим здоровье на 100
            else
            {
                healthText.text = "100";
            }

        }
    */
        



}
