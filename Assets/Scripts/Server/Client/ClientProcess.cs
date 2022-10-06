using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ClientProcess : MonoSingleton<ClientProcess>
{
    public Dictionary<int, Vector3> playerPositionFromServer;
    private GameObject[] players;
    private PlayerManager playerManager;
    JoinAnotherRoom _joinAnotherRoom;

    // private PlayerManager playerManager;
    void Awake()
    {
        playerPositionFromServer = new Dictionary<int, Vector3>();
    }

    void Start(){
        playerManager = FindObjectOfType<PlayerManager>();
        _joinAnotherRoom = FindObjectOfType<JoinAnotherRoom>();
    }

    public void UpdatePlayerPosition()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            int viewId = playerManager.viewId;
            if (!playerPositionFromServer.ContainsKey(viewId)) continue;
            player.GetComponent<PlayerMovement>().HandleMovementToPosition(playerPositionFromServer[viewId]);
        }
    }

    public void WinnerReceived(int viewId)
    {
        Debug.Log("We got a winner: " + viewId);
    }

    public void BattleRequestReceived(int requestViewId, int targetViewId)
    {
        if (targetViewId == playerManager.viewId)
        {
            Debug.Log(targetViewId + ", you received a battle request from: " + requestViewId);
        }
    }

    public void QuestionsReceived(int[] questions)
    {
        //foreach (int question in questions)
        //{
        //    Debug.Log("Question: " + question);
        //}
        FindObjectOfType<BattleRoomManager>().RequestOnStartBattle(questions); //FindObjectOfType vs Instance ???
    }

    public void ScoresReceived(int[] viewIds, int[] scores)
    {
        BattleRoomManager.Instance.RequestOnEndBattle();
        BattleRoomManager.Instance.RequestOnAnnounceWinner(viewIds, scores);
        //for (int i = 0; i < viewIds.Length; i++)
        //{
        //    //if (PlayerManager.IsMine(viewIds[i]))
        //    //{
        //    //    Debug.Log("My scored: " + scores[i]);
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log(viewIds[i] + " scored: " + scores[i]);
        //    //}
        //=======================================================================
        //    //if (PhotonNetwork.GetPhotonView(viewIds[i]).IsMine)
        //    //{
        //    //    Debug.Log("My scored: " + scores[i]);
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log(viewIds[i] + " scored: " + scores[i]);
        //    //}

        //}
    }

    public void ReadyStateReceived(bool allReady)
    {
        if (!allReady)
        {
            Debug.Log("Someone is not ready!");
            PlayerPrefs.SetString("roomName", InputManager.Instance.GeneralRoom);
            PhotonNetwork.LoadLevel("Photon_Demo");
        }
    }

    public void JoinGeneralRoom()
    {
        PlayerPrefs.SetString("roomName", InputManager.Instance.GeneralRoom);
        _joinAnotherRoom.Leave("Photon_Demo");
    }

    public void AddNewPlayerWithId(int newUid){
        players = GameObject.FindGameObjectsWithTag("Player");
        foreach (var player in players)
        {
            PlayerManager playerManager = player.GetComponent<PlayerManager>();
            if (playerManager.viewId == 0){
                playerManager.viewId = newUid;
                PlayerPrefs.SetInt("viewId", newUid);
                return;
            }
        }
    }
}
