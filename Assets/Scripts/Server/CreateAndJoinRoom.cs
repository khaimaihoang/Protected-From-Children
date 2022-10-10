using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class CreateAndJoinRoom : MonoBehaviourPunCallbacks
{
    [SerializeField] string roomName = "GeneralRoom";
    [SerializeField] string lobbyName;
    [SerializeField] private Button _createBtn;
    [SerializeField] private Button _joinBtn;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        foreach(Button btn in FindObjectsOfType<Button>())
        {
            if (btn.gameObject.name == "CreateRoom")
            {
                _createBtn = btn;
                _createBtn.onClick.AddListener(Create);
            } else if (btn.gameObject.name == "JoinRoom")
            {
                _joinBtn = btn;
                _joinBtn.onClick.AddListener(Join);
            }
        }
        if (roomName == "GeneralRoom"){
            lobbyName = "Photon_Demo";
        }
    }

    public void Create(){
        PhotonNetwork.CreateRoom(roomName);
    }

    public void Join(){
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        // Debug.Log("Success");
        PhotonNetwork.LoadLevel(lobbyName);
    }
}
