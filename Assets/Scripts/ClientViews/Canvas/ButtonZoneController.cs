using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ButtonZoneController : MonoBehaviour
{
    [SerializeField] private GameObject _canvas;
    [SerializeField] Text _roomCode;
    [SerializeField] Button _createRoomBtn;
    [SerializeField] Button _joinRoomBtn;

    private void Awake()
    {
        _canvas = FindObjectOfType<Canvas>().gameObject;
    }

    void Init()
    {
        _roomCode = _canvas.GetComponentInChildren<Text>();
        Button[] btns = _canvas.GetComponentsInChildren<Button>();
        foreach (Button btn in btns)
        {
            if (btn.gameObject.name == "Create")
            {
                _createRoomBtn = btn;
                _createRoomBtn.onClick.AddListener(OnCreateRoomBtn);
            }
            else if (btn.gameObject.name == "Join")
            {
                _joinRoomBtn = btn;
                _joinRoomBtn.onClick.AddListener(OnJoinRoomBtn);
            }
        }
    }


    void OnCreateRoomBtn()
    {
        SendRequest.Instance.SendCreateNewRoomRequest(int.Parse(_roomCode.text), ClientProcess.Instance.playerUserId);
    }

    void OnJoinRoomBtn()
    {
        SendRequest.Instance.SendJoinRoomRequest(int.Parse(_roomCode.text), ClientProcess.Instance.playerUserId);
    }

    private void OnEnable()
    {
        ClientsViewController.Instance.OnInitPlayerViews += Init;
    }

    private void OnDisable()
    {
        ClientsViewController.Instance.OnInitPlayerViews -= Init;
    }
}
