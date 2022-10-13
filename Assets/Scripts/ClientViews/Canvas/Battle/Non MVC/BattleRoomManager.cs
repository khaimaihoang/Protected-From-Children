using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleRoomManager : MonoSingleton<BattleRoomManager>
{
    private WaitingLounge _waitingLounge;
    private BattleLounge _battleLounge;
    private ResultLounge _resultLounge;

    public event UnityAction<int[]> OnStartBattle;
    public event UnityAction OnEndBattle;
    public event UnityAction<int[], int[]> OnAnnounceWinner;
    private void Awake()
    {
        _waitingLounge = GetComponent<WaitingLounge>();
        _battleLounge = GetComponent<BattleLounge>();
        _resultLounge = GetComponent<ResultLounge>();
    }
    private void Start()
    {
        this.RequestOnInitBattleRoom();
    }

    public void RequestOnInitBattleRoom()
    {
        _waitingLounge.Init();
        //GameObject.Find("Battle Lounge").SetActive(false);
        //GameObject.Find("Result Lounge").SetActive(false);
    }

    public void RequestOnStartBattle(int[] questions)
    {
        OnStartBattle?.Invoke(questions);
    }

    public void RequestOnEndBattle()
    {
        OnEndBattle?.Invoke();
    }

    public void RequestOnAnnounceWinner(int[] viewIds, int[] scores)
    {
        OnAnnounceWinner?.Invoke(viewIds, scores);
    }

    public void RequestOnSendReadyState(bool isReady)
    {
        if (isReady)
        {
            SendRequest.Instance.SendPlayerReadyRequest();
        }
    }

    public void RequestOnSendAnswers(string[] playerAnswers)
    {
        //Debug.Log("RequestOnSendAnswers: AnswerLength = " + playerAnswers.Length);
        SendRequest.Instance.SendAnswers(playerAnswers);
    }
}
