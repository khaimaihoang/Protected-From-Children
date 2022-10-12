using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;

public class SendReply : MonoSingleton<SendReply>
{
    //public const byte (byte)NetworkEvent.GetClientPositionEventCode = 2;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SendClientPositionReplyCoroutine(0.01f));
    }

    IEnumerator SendClientPositionReplyCoroutine(float time)
    {
        while (true)
        {
            SendClientPositionReply();
            // Debug.Log("Send Reply");
            yield return new WaitForSeconds(time);
        }
    }

    private void SendClientPositionReply()
    {
        List<int> userIds = new List<int>();
        List<Vector3> poss = new List<Vector3>();
        foreach (var item in NetworkProcess.Instance.playerPositions)
        {
            userIds.Add(item.Key);
            poss.Add(item.Value);
        }
        if (userIds.Count == 0) return;
        object[] content = new object[] { userIds.ToArray(), poss.ToArray() };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetClientPositionEventCode, content, raiseEventOptions, SendOptions.SendUnreliable);
    }

    public void SendReplyChangeNewUserId(int newUserId)
    {
        object content = newUserId;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.ChangeNewUserIdEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPlayerLeaveReply(object userId)
    {
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetPlayerLeaveEventCode, userId,
            new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void SendWinnerReply(int userId)
    {
        object content = userId;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetWinnerEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendBattleNotification(int requestuserId, int targetuserId)
    {
        object[] content = new object[] { requestuserId, targetuserId };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetBattleRequestEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendQuestions(int[] userIds, int[] questions)
    {
        object[] content = new object[] { userIds, questions };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetQuestionsEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendScores(int[] userIds, int[] scores)
    {
        object[] content = new object[] { userIds, scores };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetScoresEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendReplyNewUserIdAccepted(int newUserId){
        object content = newUserId;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.NewUserIdAcceptedEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendCreateNewRoomReply(int userId, int minigame)
    {
        object[] content = new object[] { userId, minigame };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetCreatNewRoomEventCode, content,
            new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void SendJoinRoomReply(int userId, int minigame)
    {
        object[] content = new object[] { userId, minigame };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetJoinRoomEventCode, content,
            new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

}
