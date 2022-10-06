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
}
