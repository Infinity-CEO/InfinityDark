using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SlotToolTip : MonoBehaviour
{
    [SerializeField]
    private GameObject go_Base;

    [SerializeField]
    private TextMeshProUGUI text_ItemName;
    [SerializeField]
    private TextMeshProUGUI text_ItemDesc;
    [SerializeField]
    private TextMeshProUGUI text_ItemHowToUsed;
    
    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        go_Base.SetActive(true);
        _pos += new Vector3(go_Base.GetComponent<RectTransform>().rect.width * 0.5f,
            -go_Base.GetComponent<RectTransform>().rect.height * 0.5f, 0f);
        go_Base.transform.position = _pos;

        text_ItemName.text = _item.itemName;
        text_ItemDesc.text = _item.itemDesc;
        
        if(_item.itemType == Item.ItemType.Equipment)
        {
            text_ItemHowToUsed.text = "Equip-Touch";
        }
        else if(_item.itemType == Item.ItemType.Equipment)
        {
            text_ItemHowToUsed.text = "Use-Touch";
        }
        else
        {
            text_ItemHowToUsed.text = "";
        }
    }
    public void HideToolTip()
    {
        go_Base.SetActive(false);
    }
}
