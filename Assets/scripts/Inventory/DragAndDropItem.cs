using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// IPointerDownHandler - Следит за нажатиями мышки по объекту на котором висит этот скрипт
/// IPointerUpHandler - Следит за отпусканием мышки по объекту на котором висит этот скрипт
/// IDragHandler - Следит за тем не водим ли мы нажатую мышку по объекту
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    private Transform player;
    public Sprite slot_for_empty;

    private void Start()
    {
        //ПОСТАВЬТЕ ТЭГ "PLAYER" НА ОБЪЕКТЕ ПЕРСОНАЖА!
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // Находим скрипт InventorySlot в слоте в иерархии
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Если слот пустой, то мы не выполняем то что ниже return;
        if (oldSlot.is_empty)
            return;
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.is_empty)
            return;
        // Делаем так чтобы нажатия мышкой не игнорировали эту картинку
        GetComponentInChildren<Image>().raycastTarget = false;
        // Делаем наш DraggableObject ребенком InventoryPanel чтобы DraggableObject был над другими слотами инвенторя
        transform.SetParent(transform.parent.parent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.is_empty)
            return;
        // И чтобы мышка опять могла ее засечь
        GetComponentInChildren<Image>().raycastTarget = true;

        //Поставить DraggableObject обратно в свой старый слот
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        
        if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            InventorySlot inventory_slot = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>();
            //Перемещаем данные из одного слота в другой

            if (inventory_slot.equipment_type != EquipmentType.None )
            {
                if (inventory_slot.equipment_type == oldSlot.item.equipment_type)
                {
                    ExchangeSlotData(inventory_slot);
                } else
                {
                    return;
                }
            } else
            {
                ExchangeSlotData(inventory_slot);
            }
            
        }
        
    }
    void NullifySlotData()
    {
        // убираем значения InventorySlot
        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.is_empty = true;
        //oldSlot.icon_go.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.icon_go.GetComponent<Image>().sprite = null;
        oldSlot.count.text = "";
    }

    void ExchangeSlotData(InventorySlot newSlot)
    {
        // Временно храним данные newSlot в отдельных переменных
        ItemScriptableObject item = newSlot.item;
        int amount = newSlot.amount;
        bool is_empty = newSlot.is_empty;
        GameObject icon_go = newSlot.icon_go;
        TMP_Text count = newSlot.count;

        // Заменяем значения newSlot на значения oldSlot
        newSlot.item = oldSlot.item;
        newSlot.amount = oldSlot.amount;
        if (oldSlot.is_empty == false)
        {
            newSlot.SetIcon(oldSlot.icon_go.GetComponent<Image>().sprite);
            newSlot.count.text = oldSlot.amount.ToString();
        }
        else
        {
            //newSlot.icon_go.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            newSlot.icon_go.GetComponent<Image>().sprite = slot_for_empty;
            newSlot.count.text = "";
        }

        newSlot.is_empty = oldSlot.is_empty;

        // Заменяем значения oldSlot на значения newSlot сохраненные в переменных
        oldSlot.item = item;
        oldSlot.amount = amount;
        if (is_empty == false)
        {
            oldSlot.SetIcon(icon_go.GetComponent<Image>().sprite);
            oldSlot.count.text = amount.ToString();
        }
        else
        {
           // oldSlot.icon_go.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            oldSlot.icon_go.GetComponent<Image>().sprite = slot_for_empty;
            oldSlot.count.text = "";
        }

        oldSlot.is_empty = is_empty;
    }
}
