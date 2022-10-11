using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClientBattle : MonoBehaviour
{
    public void WinnerReceived(int userId)
    {
        Debug.Log("We got a winner: " + userId);
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

    public void SendReadyState()
    {

    }

    private void OnEnable()
    {
        ClientProcess.Instance.onQuestionsReceived += QuestionsReceived;
        ClientProcess.Instance.onScoresReceived += ScoresReceived;
    }

    private void OnDisable()
    {
        ClientProcess.Instance.onQuestionsReceived -= QuestionsReceived;
        ClientProcess.Instance.onScoresReceived -= ScoresReceived;
    }
}
