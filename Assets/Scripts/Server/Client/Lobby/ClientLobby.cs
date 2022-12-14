using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientLobby : MonoBehaviour
{
    public Dictionary<int, GameObject> players;

    // Start is called before the first frame update
    void Awake(){
        players = new Dictionary<int, GameObject>();
    }
    
    void OnEnable()
    {
        ClientProcess.Instance.onUpdatePlayerPosition += UpdatePlayerPosition;
        ClientProcess.Instance.onChangeUserId += ChangeUserId;
        ClientProcess.Instance.onGenNewPlayerUserId += GenNewPlayeruserId;
        ClientProcess.Instance.onCreateNewPlayer += CreateNewPlayer;
        ClientProcess.Instance.onGetPlayerGameObjectWithId += GetPlayerGameObjectWithId;
    }

    // Update is called once per frame
    void OnDisable()
    {
        ClientProcess.Instance.onUpdatePlayerPosition -= UpdatePlayerPosition;
        ClientProcess.Instance.onChangeUserId -= ChangeUserId;
        ClientProcess.Instance.onGenNewPlayerUserId -= GenNewPlayeruserId;
        ClientProcess.Instance.onCreateNewPlayer -= CreateNewPlayer;
        ClientProcess.Instance.onGetPlayerGameObjectWithId -= GetPlayerGameObjectWithId;
    }

    private void CheckPlayerObject(int userId)
    {
        if (ClientProcess.Instance.playerUserId == userId)
        {
            Init();
        }
    }

    private GameObject GetPlayerGameObjectWithId(int userId){
        return players[userId];
    }

    void UpdatePlayerPosition(Dictionary<int, Vector3> playerPositionFromServer)
    {
        DestroyPlayerQuitGame(playerPositionFromServer);
        foreach (var item in playerPositionFromServer)
        {
            int userId = item.Key;
            Vector3 pos = item.Value;
            if (!players.ContainsKey(userId))
            {
                CreatePlayerWithUserId(userId);
                CheckPlayerObject(userId);
            }
            GameObject player = players[userId];
            player.GetComponent<PlayerMovement>().HandleMovementToPosition(pos);
        }
    }

    void DestroyPlayerQuitGame(Dictionary<int, Vector3> playerPositionFromServer)
    {
        foreach (var player in players)
        {
            if (!playerPositionFromServer.ContainsKey(player.Key))
            {
                Debug.Log(player.Key);
                Destroy(players[player.Key]);
                players.Remove(player.Key);
                break;
            }
        }
    }

    private void CreateNewPlayer(int newUserId){
        if (ClientProcess.Instance.playerUserId != newUserId) return;
        CreatePlayerWithUserId(newUserId);
        Init();
    }

    private void Init()
    {
        Debug.Log("Lobby 2");

        ClientProcess.Instance._isAuthentizated = true;
        ClientsViewController.Instance.RequestOnInitPlayerViews();
        ClientsViewController.Instance.RequestOnStopToPull(false);
        InitCamera();
        SendRequest.Instance.Init();
    }

    private void InitCamera(){
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>().followTarget = players[ClientProcess.Instance.playerUserId].transform;
    }

    private void CreatePlayerWithUserId(int userId){
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        GameObject newPlayer = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        newPlayer.GetComponent<PlayerManager>().userId = userId;
        players[userId] = newPlayer;
    }

    private void ChangeUserId(int newUserId)
    {
        if (ClientProcess.Instance.playerUserId == newUserId && !ClientProcess.Instance._isAuthentizated)
        {
            GenNewPlayeruserId();
        }
    }

    private void GenNewPlayeruserId(){
        int newUserId = UnityEngine.Random.Range(1, 10);
        ClientProcess.Instance.playerUserId = newUserId;
        // PlayerPrefs.SetInt("userId", newUserId);
        SendRequest.Instance.SendRequestCheckNewUserId(newUserId);
    }
}
