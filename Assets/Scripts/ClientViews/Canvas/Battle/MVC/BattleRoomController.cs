using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleRoomController : ApiSingleton<BattleRoomController>
{
    public event UnityAction OnJoinedRoom;
    public event UnityAction OnLeftRoom;
    public event UnityAction OnReady;
    public event UnityAction OnInit;
    public event UnityAction OnStartBattle;
    public event UnityAction<string, string> OnChooseAnswer;
    public event UnityAction<QuestionForm> OnRenewQuestion;

    public void RequestOnJoinedRoom()
    {
        OnJoinedRoom?.Invoke();
    }

    public void RequestOnLeftRoom()
    {
        OnLeftRoom?.Invoke();
    }

    public void RequestOnReady()
    {
        OnReady?.Invoke();
    }

    public void RequestOnInit()
    {
        OnInit?.Invoke();
    }

    public void RequestOnStartBattle()
    {
        OnStartBattle?.Invoke();
    }

    public void RequestOnChooseAnswer(string choseAnswer, string rightAnswer)
    {
        OnChooseAnswer?.Invoke(choseAnswer, rightAnswer);
    }

    public void RequestOnRenewQuestion(QuestionForm questionForm)
    {
        OnRenewQuestion?.Invoke(questionForm);
    }
}
