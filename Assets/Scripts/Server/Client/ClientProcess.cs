using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ClientProcess : MonoSingleton<ClientProcess>
{
    public UnityAction< Dictionary<int, Vector3>> onUpdatePlayerPosition;
    public UnityAction onGenNewPlayerUserId;
    public UnityAction<int> onChangeUserId;
    public UnityAction<int> onCreateNewPlayer;
    public Func<int, GameObject> onGetPlayerGameObjectWithId;
    public UnityAction<int> onWinnerReceived;
    public UnityAction<int[]> onQuestionsReceived;
    public UnityAction<int[], int[]> onScoresReceived;

    public int playerUserId;
    public bool _isAuthentizated = false;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        // playerPositionFromServer = new Dictionary<int, Vector3>();
    }

    void Start(){
        GenNewPlayerUserId();
    }

    public void UpdatePlayerPosition(int []userIds, Vector3[] poss)
    {
        Dictionary<int, Vector3> playerPositionFromServer = new Dictionary<int, Vector3>();
        for(int i = 0; i < userIds.Length; i++){
            playerPositionFromServer[userIds[i]] = poss[i];
        }
        onUpdatePlayerPosition?.Invoke(playerPositionFromServer);
    }

    public void GenNewPlayerUserId(){
        onGenNewPlayerUserId?.Invoke();
    }

    public void ChangeUserId(int newUserId){
        onChangeUserId?.Invoke(newUserId);
    }

    public void CreateNewPlayer(int newUserId){
        onCreateNewPlayer?.Invoke(newUserId);
    }

    public GameObject GetThisPlayerGameObject(){
        return GetPlayerGameObjectWithId(playerUserId);
    }

    public GameObject GetPlayerGameObjectWithId(int userId){
        return onGetPlayerGameObjectWithId?.Invoke(userId);
    }

    public void WinnerReceived(int userId){
        onWinnerReceived?.Invoke(userId);
    }

    public void QuestionsReceived(int[] questions){
        onQuestionsReceived?.Invoke(questions);
    }

    public void ScoresReceived(int[] userIds, int[] scores){
        onScoresReceived?.Invoke(userIds, scores);
    }

    public void LoadMinigameScene(int userId, int minigame)
    {
        if (ClientProcess.Instance.playerUserId == userId)
        {
            SceneManager.LoadScene((Minigame)minigame + "Minigame");
        }
    }
}
