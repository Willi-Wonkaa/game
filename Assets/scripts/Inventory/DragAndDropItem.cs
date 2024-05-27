using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

/// IPointerDownHandler - ������ �� ��������� ����� �� ������� �� ������� ����� ���� ������
/// IPointerUpHandler - ������ �� ����������� ����� �� ������� �� ������� ����� ���� ������
/// IDragHandler - ������ �� ��� �� ����� �� �� ������� ����� �� �������
public class DragAndDropItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public InventorySlot oldSlot;
    private Transform player;
    public Sprite slot_for_empty;

    private void Start()
    {
        //��������� ��� "PLAYER" �� ������� ���������!
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // ������� ������ InventorySlot � ����� � ��������
        oldSlot = transform.GetComponentInParent<InventorySlot>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        // ���� ���� ������, �� �� �� ��������� �� ��� ���� return;
        if (oldSlot.is_empty)
            return;
        GetComponent<RectTransform>().position += new Vector3(eventData.delta.x, eventData.delta.y);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (oldSlot.is_empty)
            return;
        // ������ ��� ����� ������� ������ �� ������������ ��� ��������
        GetComponentInChildren<Image>().raycastTarget = false;
        // ������ ��� DraggableObject �������� InventoryPanel ����� DraggableObject ��� ��� ������� ������� ���������
        transform.SetParent(transform.parent.parent);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (oldSlot.is_empty)
            return;
        // � ����� ����� ����� ����� �� ������
        GetComponentInChildren<Image>().raycastTarget = true;

        //��������� DraggableObject ������� � ���� ������ ����
        transform.SetParent(oldSlot.transform);
        transform.position = oldSlot.transform.position;
        
        if (eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>() != null)
        {
            InventorySlot inventory_slot = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<InventorySlot>();
            //���������� ������ �� ������ ����� � ������

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
        // ������� �������� InventorySlot
        oldSlot.item = null;
        oldSlot.amount = 0;
        oldSlot.is_empty = true;
        //oldSlot.icon_go.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        oldSlot.icon_go.GetComponent<Image>().sprite = null;
        oldSlot.count.text = "";
    }

    void ExchangeSlotData(InventorySlot newSlot)
    {
        // �������� ������ ������ newSlot � ��������� ����������
        ItemScriptableObject item = newSlot.item;
        int amount = newSlot.amount;
        bool is_empty = newSlot.is_empty;
        GameObject icon_go = newSlot.icon_go;
        TMP_Text count = newSlot.count;

        // �������� �������� newSlot �� �������� oldSlot
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

        // �������� �������� oldSlot �� �������� newSlot ����������� � ����������
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
