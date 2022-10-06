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
        List<int> viewIds = new List<int>();
        List<Vector3> poss = new List<Vector3>();
        foreach (var item in NetworkProcess.Instance.playerPositions)
        {
            viewIds.Add(item.Key);
            poss.Add(item.Value);
        }
        if (viewIds.Count == 0) return;
        object[] content = new object[] { viewIds.ToArray(), poss.ToArray() };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetClientPositionEventCode, content, raiseEventOptions, SendOptions.SendUnreliable);
    }

    public void SendReplyChangeNewUid(int newUid)
    {
        object content = new object[1] { newUid };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetChangeNewUidEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendWinnerReply(int viewId)
    {
        object content = viewId;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetWinnerEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendBattleNotification(int requestViewId, int targetViewId)
    {
        object[] content = new object[] { requestViewId, targetViewId };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetBattleRequestEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendQuestions(int[] questions)
    {
        object[] content = new object[] { questions };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetQuestionsEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendScores(int[] viewIds, int[] scores)
    {
        object[] content = new object[] { viewIds, scores };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetScoresEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendReadyState(bool _allReady)
    {
        object content = _allReady;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetReadyEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendExitReply()
    {
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.ExitEventCode, "", new RaiseEventOptions { Receivers = ReceiverGroup.All }, SendOptions.SendReliable);
    }

    public void SendReplyNewUid(int newUid){
        object content = new object[1]{newUid};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.GetNewUidEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
}
