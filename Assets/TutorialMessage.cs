using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialMessage : MonoBehaviour
{
    public GameManager _gameManager;
    public TMP_Text _text;

    public void SetText(string text)
    {
        gameObject.SetActive(true);
        _gameManager._StartScreen.gameObject.SetActive(false);
        _text.text = text;
    }

    public void Update()
    {
        if (Input.GetKeyUp(KeyCode.Return))
        {
            gameObject.SetActive(false);
            _gameManager._GameScreen.SetActive(true);

            _gameManager.BeginMatch(_gameManager._player, _gameManager._enemy);
        }

    }

}
