using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DeckInfo : MonoBehaviour
{
    public TMP_Text _text;

    Player _player;

    public void SetPlayer(Player player)
    {
        _player = player;
    }

    public void UpdateText()
    {
        string txt = $@"Deck = {_player._deck._deck.Count} <br> Library = {_player._deck._library.Count} <br> Hand = {_player._deck._hand.Count} <br> Play = {_player._deck._play.Count} <br> Discard = {_player._deck._discard.Count} <br> Wounded = {_player._deck._wounded.Count}";
        _text.text = txt;
    }
    
}
