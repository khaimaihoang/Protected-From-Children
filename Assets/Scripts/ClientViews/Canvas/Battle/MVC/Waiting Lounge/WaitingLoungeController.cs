using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingLoungeController : MonoBehaviour
{
    private Button _readyButton, _leaveButton;
    private List<GameObject> _playerList;
    private GameObject _battleLounge, _waitingLounge;

    // Start is called before the first frame update
    private void Start()
    {
        BattleRoomController.Instance.RequestOnInit();
        _battleLounge.SetActive(false);
        //Debug.Log(GameObject.Find("Battle Lounge") == null);

    }
    private void Init()
    {
        _readyButton = GameObject.Find("Ready Button").GetComponent<Button>();
        _leaveButton = GameObject.Find("Leave Button").GetComponent<Button>();
        _playerList = new List<GameObject>(2);
        foreach(var player in GameObject.FindGameObjectsWithTag("Player"))
        {
            _playerList.Add(player);
        }

        _readyButton.onClick.AddListener(OnReadyBattleClicked);
        _leaveButton.onClick.AddListener(OnLeaveRoomClicked);

        _battleLounge = GameObject.Find("Battle Lounge");
        _waitingLounge = GameObject.Find("Waiting Lounge");
    }

    private void OnReadyBattleClicked()
    {
        Debug.Log("Player is ready");
        //Send request ready
        
        BattleRoomController.Instance.RequestOnReady();        
    }

    private void OnLeaveRoomClicked()
    {
        Debug.Log("Player left room");
        //Send request left room
        //BattleRoomController.Instance.RequestOnLeftRoom();
        _battleLounge.SetActive(true);
        BattleRoomController.Instance.RequestOnStartBattle();
    }

    private void OnStartBattle()
    {
        //Recheck
        Debug.Log("Start battle now");
        _waitingLounge.SetActive(false);        
    }

    private void OnEnable()
    {
        BattleRoomController.Instance.OnInit += Init;
        BattleRoomController.Instance.OnStartBattle += OnStartBattle;
    }

    private void OnDisable()
    {
        BattleRoomController.Instance.OnInit -= Init;
        BattleRoomController.Instance.OnStartBattle -= OnStartBattle;

    }
}
