using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class JoinGeneralRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] string _roomName = "GeneralRoom";
    [SerializeField] string scene = "Lobby";
    [SerializeField] Button _joinGeneralRoom;

    private void Start()
    {
        Init();
    }

    void Init()
    {
        _joinGeneralRoom = FindObjectOfType<Button>();
        _joinGeneralRoom.onClick.AddListener(JoinRoom);
    }

    void JoinRoom()
    {
        PhotonNetwork.JoinRoom(_roomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel(scene);
    }
}
