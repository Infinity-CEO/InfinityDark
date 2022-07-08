using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text amo, money;

    public void setAmo(string i)
    {
        amo.text = i;
    }
    public void setMoney(string i)
    {
        money.text = i;
    }
}
