using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLounge : MonoBehaviour
{
    private Button _readyButton, _leaveButton;
    private Text _numberText, _readyText;

    private List<GameObject> _playerList;
    private GameObject _battleLounge, _waitingLounge, _resultLounge;
    private bool _isReady;
    public void Init()
    {
        _readyButton = GameObject.Find("Ready Button").GetComponent<Button>();
        _leaveButton = GameObject.Find("Leave Button").GetComponent<Button>();

        _readyText = _readyButton.GetComponentInChildren<Text>();
        _numberText = GameObject.Find("Number Text").GetComponent<Text>();

        _playerList = new List<GameObject>(2);
        foreach (var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            _playerList.Add(player);
        }

        _readyButton.onClick.AddListener(OnControllerReadyBattleClicked);
        _leaveButton.onClick.AddListener(OnControllerLeaveRoomClicked);

        _battleLounge = GameObject.Find("Battle Lounge");
        _waitingLounge = GameObject.Find("Waiting Lounge");
        _resultLounge = GameObject.Find("Result Lounge");

        _battleLounge.SetActive(false);
        _resultLounge.SetActive(false);

        _isReady = false;
    }

    #region Controller
    private void OnControllerReadyBattleClicked()
    {
        Debug.Log("Player is ready");
        //Send request ready
        _isReady = !_isReady;
        BattleRoomManager.Instance.RequestOnSendReadyState(_isReady);
        this.OnViewReadyBattleClicked();
    }

    private void OnControllerLeaveRoomClicked()
    {
        Debug.Log("Player left room");
        //Send request left room
    }

    private void OnControllerStartBattle(int[] questions)
    {
        Debug.Log("Start battle now");
        _battleLounge.SetActive(true);
        _waitingLounge.SetActive(false);

        this.GetComponent<BattleLounge>().Init(_waitingLounge, _battleLounge, _resultLounge, questions);
    }

    #endregion

    #region View
    private void OnViewReadyBattleClicked()
    {
        if (_isReady)
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
    #endregion

    #region Event Registers
    private void OnEnable()
    {
        BattleRoomManager.Instance.OnStartBattle += OnControllerStartBattle;
    }

    private void OnDisable()
    {
        BattleRoomManager.Instance.OnStartBattle -= OnControllerStartBattle;
    }

    #endregion
}
