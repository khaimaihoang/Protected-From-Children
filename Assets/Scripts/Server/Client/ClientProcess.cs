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
    bool _hasPlayerView = true;
    public int playerUserId;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        playerPositionFromServer = new Dictionary<int, Vector3>();
        players = new Dictionary<int, GameObject>();
        GenNewPlayeruserId();
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
        if (_hasPlayerView)
        {
            foreach (var item in playerPositionFromServer)
            {
                int userId = item.Key;
                Vector3 pos = item.Value;
                if (!players.ContainsKey(userId))
                {
                    CreateNewPlayer(userId);
                }
                GameObject player = players[userId];
                player.GetComponent<PlayerMovement>().HandleMovementToPosition(pos);
            }
        }

    }

    public void WinnerReceived(int userId)
    {
        Debug.Log("We got a winner: " + userId);
    }

    public void BattleRequestReceived(int requestuserId, int targetuserId)
    {
        if (targetuserId == playerUserId)
        {
            Debug.Log(targetuserId + ", you received a battle request from: " + requestuserId);
        }
    }

    public void QuestionsReceived(int[] questions)
    {
        BattleRoomManager.Instance.RequestOnStartBattle(questions); //FindObjectOfType vs Instance ???
    }

    public void ScoresReceived(int[] userIds, int[] scores)
    {
        BattleRoomManager.Instance.RequestOnEndBattle();
        BattleRoomManager.Instance.RequestOnAnnounceWinner(userIds, scores);
    }

    public void ReadyStateReceived(bool isReady)
    {
        if (!isReady)
        {
            Debug.Log("Someone is not ready!");
        }
        else if (isReady)
        {
            Debug.Log("All ready!");
        }
    }

    public void JoinGeneralRoom()
    {
        PlayerPrefs.SetString("roomName", InputManager.Instance.GeneralRoom);
        _joinAnotherRoom.Leave("Photon_Demo");
    }

    public void CreateNewPlayer(int newUserId){
        CreatePlayerWithUserId(newUserId);
        if (playerUserId == newUserId){ // ClientProcess of new player
            _isAuthentizated = true;
            ClientsViewController.Instance.RequestOnInitPlayerViews();
            ClientsViewController.Instance.RequestOnStopToPull(false);
            InitCamera();
            SendRequest.Instance.Init();
        }
    }

    private void InitCamera(){
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>().followTarget = players[playerUserId].transform;
    }

    private void CreatePlayerWithUserId(int userId){
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        newPlayer.GetComponent<PlayerManager>().userId = userId;
        players[userId] = newPlayer;
    }

    public void ChangeUserId(int newUserId)
    {
        if (playerUserId == newUserId && !_isAuthentizated)
        {
            GenNewPlayeruserId();
        }
    }

    public void GenNewPlayeruserId(){
        int newUserId = UnityEngine.Random.Range(0, 10);
        playerUserId = newUserId;
        // PlayerPrefs.SetInt("userId", newUserId);
        SendRequest.Instance.SendRequestCheckNewUserId(newUserId);
    }

    public void LoadMinigameScene(int userId, int minigame)
    {
        if (playerUserId == userId)
        {
            SceneManager.LoadScene((Minigame)minigame + "Minigame");
        }
    }
}
