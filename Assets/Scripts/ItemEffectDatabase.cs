using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemEffect
{
    public string itemName;
    [Tooltip("You can apply. only HP, HUNGRY, THIRSTY")]
    public string[] part;
    public int[] num;
}
public class ItemEffectDatabase : MonoBehaviour
{
    [SerializeField]
    private ItemEffect[] itemEffects;

    [SerializeField]
    private Controller thePlayerStatus;

    [SerializeField]
    private SlotToolTip theSlotToolTip;

    private const string HP = "HP", HUNGRY = "HUNGRY", THIRSTY = "THIRSTY";

    public void ShowToolTip(Item _item, Vector3 _pos)
    {
        theSlotToolTip.ShowToolTip(_item, _pos);
    }
    public void HideToolTip()
    {
        theSlotToolTip.HideToolTip();
    }

    public void UseItem(Item _item)
    {
        if(_item.itemType == Item.ItemType.Used)
        {
            for(int x = 0; x < itemEffects.Length; x++)
            {
                if(itemEffects[x].itemName == _item.itemName)
                {
                    for(int y = 0; y < itemEffects[x].part.Length; y++)
                    {
                        switch(itemEffects[x].part[y])
                        {
                            case HP:
                                thePlayerStatus.IncreaseHP(itemEffects[x].num[y]);
                                break;
                            case HUNGRY:
                                thePlayerStatus.IncreaseHungry(itemEffects[x].num[y]);
                                break;
                            case THIRSTY:
                                thePlayerStatus.IncreaseThirsty(itemEffects[x].num[y]);
                                break;
                            default:
                                Debug.Log("Wrong part. You can path only HP, HUNGRY, THIRSTY");
                                break;
                        }
                        Debug.Log("use" + _item.itemName);
                    }
                    return;
                }
                

            }
            Debug.Log("No matching item name found.");
        }
    }
}
