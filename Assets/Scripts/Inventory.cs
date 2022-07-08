using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Inventory : MonoBehaviour{

    public static bool inventoryActivated = false;


    // 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField]
    private GameObject go_SlotsParent;

    // 슬롯들.
    private Slot[] slots;

    public Slot[] GetSlots()
    {
        return slots;
    }
    [SerializeField]
    private Item[] items;
    public void LoadToInven(int _arrayNum, string _itemName, int _itemNum)
    {
        for(int i = 0; i < items.Length; i++)
        {
            if(items[i].itemName == _itemName)
            {
                slots[_arrayNum].AddItem(items[i], _itemNum);
            }
        }
    }


    // Use this for initialization
    void Start()
    {
        slots = go_SlotsParent.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gm.gState != GameManager.GameState.Start)
        {
            return;
        }
        TryOpenInventory();
    }

    private void TryOpenInventory()
    {
        if (CrossPlatformInputManager.GetButtonDown("Inventory"))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        Debug.Log("열림");
        go_InventoryBase.SetActive(true);
    }

    public void CloseInventory()
    {
        Debug.Log("닫힘");
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(Item.ItemType.Equipment != _item.itemType)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if(slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }
    }
}