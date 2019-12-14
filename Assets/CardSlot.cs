using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSlot : MonoBehaviour
{
    public CardTray _tray;
    public CardObject _card = null;
    public string _name;

    public void SetTray(CardTray tray, string name)
    {
        _tray = tray;
        _name = name;
    }

}
