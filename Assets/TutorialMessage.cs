using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialMessage : MonoBehaviour
{
    public GameManager _gameManager;
    public TMP_Text _text;

    GameScreen _screen;
    Player _player;
    Player _enemy;
    List<CardAbility> _inventory;
    
    public void SetMessage(string text, GameScreen screen, Player player, Player enemy = null, List<CardAbility> inventory = null)
    {
        gameObject.SetActive(true);
        _gameManager._StartScreen.gameObject.SetActive(false);
        _text.text = text;

        _screen = screen;
        _player = player;
        _enemy = enemy;
        _inventory = inventory;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            gameObject.SetActive(false);

            if (_screen == GameScreen.Battle)
            {
                _gameManager._battle.gameObject.SetActive(true);
                _gameManager._battle.BeginMatch(_player, _enemy);
            }
            else if (_screen == GameScreen.DeckManage)
            {
                _gameManager._deckManager.gameObject.SetActive(true);
                _gameManager._deckManager.BeginManagement(_player, _inventory);
            }

        }

    }

}
