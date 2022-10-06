using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLoungeView : MonoBehaviour
{
    private Button _readyButton, _leaveButton;
    private Text _numberText, _readyText;

    // Start is called before the first frame update
    private void Init()
    {
        _readyButton = GameObject.Find("Ready Button").GetComponent<Button>();
        _readyText = _readyButton.GetComponentInChildren<Text>();
        _leaveButton = GameObject.Find("Leave Button").GetComponent<Button>();
        _numberText = GameObject.Find("Number Text").GetComponent<Text>();
    }

    private void OnReadyButtonClicked()
    {
        if(_readyText.text == "Ready") //not ready
        {
            _readyText.text = "Cancel";
            _numberText.text = (_numberText.text[0] - '0' + 1) + "/2";
        }
        else
        {
            _readyText.text = "Ready";
            _numberText.text = (_numberText.text[0] - '0' - 1) + "/2";
        }

    }

    private void OnEnable()
    {
        BattleRoomController.Instance.OnInit += Init;
        BattleRoomController.Instance.OnReady += OnReadyButtonClicked;
    }

    private void OnDisable()
    {
        BattleRoomController.Instance.OnInit -= Init;
        BattleRoomController.Instance.OnReady -= OnReadyButtonClicked;
    }
}
