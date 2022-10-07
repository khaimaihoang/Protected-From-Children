using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;

public class ClientProcess : MonoSingleton<ClientProcess>
{
    public Dictionary<int, Vector3> playerPositionFromServer;
    private Dictionary<int, GameObject> players;
    JoinAnotherRoom _joinAnotherRoom;
    bool _isAuthentizated = false;
    public int thisPlayerUid;
    void Awake()
    {
        playerPositionFromServer = new Dictionary<int, Vector3>();
        players = new Dictionary<int, GameObject>();
        GenNewPlayerUid();
    }

    void Start(){
        // playerManager = FindObjectOfType<PlayerManager>();
        _joinAnotherRoom = FindObjectOfType<JoinAnotherRoom>();
    }

    public void UpdatePlayerPosition()
    {
        // players = GameObject.FindGameObjectsWithTag("Player");
        // foreach (var player in players)
        // {
        //     int viewId = player.GetComponent<PlayerManager>().viewId;
        //     if (!playerPositionFromServer.ContainsKey(viewId)) continue;
        //     player.GetComponent<PlayerMovement>().HandleMovementToPosition(playerPositionFromServer[viewId]);
        // }
        foreach(var item in playerPositionFromServer){
            int viewId = item.Key;
            Vector3 pos = item.Value;
            if (!players.ContainsKey(viewId)){
                CreateNewPlayer(viewId);
            }
            GameObject player = players[viewId];
            player.GetComponent<PlayerMovement>().HandleMovementToPosition(pos);
        }
    }

    public void WinnerReceived(int viewId)
    {
        Debug.Log("We got a winner: " + viewId);
    }

    public void BattleRequestReceived(int requestViewId, int targetViewId)
    {
        if (targetViewId == thisPlayerUid)
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

    public void CreateNewPlayer(int newUid){
        if (thisPlayerUid == newUid){
            _isAuthentizated = true;
            SendRequest.Instance.Init();
        }
        CreatePlayerWithUid(newUid);
    }

    private void CreatePlayerWithUid(int uid){
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        newPlayer.GetComponent<PlayerManager>().viewId = uid;
        players[uid] = newPlayer;
    }

    public void ChangeUid(int newUid)
    {
        if (thisPlayerUid == newUid && !_isAuthentizated)
        {
            GenNewPlayerUid();
        }
    }

    public void GenNewPlayerUid(){
        int newUid = UnityEngine.Random.Range(0, 10);
        thisPlayerUid = newUid;
        // PlayerPrefs.SetInt("viewId", newUid);
        SendRequest.Instance.SendRequestCheckNewUid(newUid);
    }
}
