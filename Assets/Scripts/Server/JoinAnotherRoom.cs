using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class JoinAnotherRoom : MonoBehaviourPunCallbacks
{
    string level;

    private void Start()
    {
        InitMasterClient();
    }

    void InitMasterClient()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            GameObject master = Resources.Load<GameObject>("Master");
            Instantiate(master);
            if (PhotonNetwork.CurrentRoom.Name != InputManager.Instance.GeneralRoom)
            {
                GameObject battle = Resources.Load<GameObject>("BattleProcess");
                Instantiate(battle);
            }
        }
        GameObject client = Resources.Load<GameObject>("Client");
        Instantiate(client);
    }

    public void Leave(string level)
    {
        this.level = level;
        ClientsViewController.Instance.RequestOnStopToPull(true);
        if (PlayerPrefs.GetString("roomName") != "") PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("left room");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.JoinOrCreateRoom(PlayerPrefs.GetString("roomName"), new RoomOptions { MaxPlayers = 2 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room" + PhotonNetwork.CurrentRoom.Name);
        PhotonNetwork.LoadLevel(this.level);
    }

}
