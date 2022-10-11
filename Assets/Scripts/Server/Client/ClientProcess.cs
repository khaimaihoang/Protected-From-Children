using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;

public class ClientProcess : MonoSingleton<ClientProcess>
{
    public Dictionary<int, Vector3> playerPositionFromServer;
    public Dictionary<int, GameObject> players;
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
        //     int userId = player.GetComponent<PlayerManager>().userId;
        //     if (!playerPositionFromServer.ContainsKey(userId)) continue;
        //     player.GetComponent<PlayerMovement>().HandleMovementToPosition(playerPositionFromServer[userId]);
        // }
        foreach(var item in playerPositionFromServer){
            int userId = item.Key;
            Vector3 pos = item.Value;
            if (!players.ContainsKey(userId)){
                CreateNewPlayer(userId);
            }
            GameObject player = players[userId];
            player.GetComponent<PlayerMovement>().HandleMovementToPosition(pos);
        }
    }

    public void WinnerReceived(int userId)
    {
        Debug.Log("We got a winner: " + userId);
    }

    public void BattleRequestReceived(int requestuserId, int targetuserId)
    {
        if (targetuserId == thisPlayerUid)
        {
            Debug.Log(targetuserId + ", you received a battle request from: " + requestuserId);
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

    public void ScoresReceived(int[] userIds, int[] scores)
    {
        BattleRoomManager.Instance.RequestOnEndBattle();
        BattleRoomManager.Instance.RequestOnAnnounceWinner(userIds, scores);
        //for (int i = 0; i < userIds.Length; i++)
        //{
        //    //if (PlayerManager.IsMine(userIds[i]))
        //    //{
        //    //    Debug.Log("My scored: " + scores[i]);
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log(userIds[i] + " scored: " + scores[i]);
        //    //}
        //=======================================================================
        //    //if (PhotonNetwork.GetPhotonView(userIds[i]).IsMine)
        //    //{
        //    //    Debug.Log("My scored: " + scores[i]);
        //    //}
        //    //else
        //    //{
        //    //    Debug.Log(userIds[i] + " scored: " + scores[i]);
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
        CreatePlayerWithUid(newUid);
        if (thisPlayerUid == newUid){ // ClientProcess of new player
            _isAuthentizated = true;
            ClientsViewController.Instance.RequestOnInitPlayerViews();
            ClientsViewController.Instance.RequestOnStopToPull(false);
            InitCamera();
            SendRequest.Instance.Init();
        }
    }

    private void InitCamera(){
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>().followTarget = players[thisPlayerUid].transform;
    }

    private void CreatePlayerWithUid(int uid){
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        newPlayer.GetComponent<PlayerManager>().userId = uid;
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
        // PlayerPrefs.SetInt("userId", newUid);
        SendRequest.Instance.SendRequestCheckNewUserId(newUid);
    }

    public void LoadMinigameScene(int userId, int minigame)
    {
        if (thisPlayerUid == userId)
        {
            SceneManager.LoadScene((Minigame)minigame + "Minigame");
        }
    }
}
