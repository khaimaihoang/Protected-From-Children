using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class SendRequest : MonoSingleton<SendRequest>
{
    //public const byte (byte)NetworkEvent.PositionEventCode = 1;
    private int thisPlayerUid;
    // Start is called before the first frame update
    public void Init()
    {
        thisPlayerUid = ClientProcess.Instance.thisPlayerUid;
        if (PhotonNetwork.CurrentRoom.Name == InputManager.Instance.GeneralRoom) SendPlayerInputRequest((int)PlayerInput.STOP);
    }

    public void SendPlayerInputRequest(int playerInput)
    {
        object[] content = new object[] { thisPlayerUid, playerInput };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.InputEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendBattleRequest(int targetViewId)
    {
        object[] content = new object[] { thisPlayerUid, targetViewId };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.BattleRequestEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    
    public void SendAnswers(string[] answers)
    {
        object[] content = new object[] { thisPlayerUid, answers };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.AnswerEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendStateChangeRequest(int state)
    {
        object[] content = new object[] { thisPlayerUid, state };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.StateChangeEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendAllInRoomSignal()
    {
        Debug.Log("Send Request");
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.AllInRoomEventCode, "", new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    public void SendExitRequest()
    {
        Debug.Log("Send Exit Request");
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.ExitEventCode, null, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    //public void SendRequestNewUid()
    //{
    //    int tempUid = UnityEngine.Random.RandomRange(0, 10000);
    //    thisPlayerUid = tempUid;
    //    PlayerPrefs.SetInt("viewId", tempUid);
    //    object content = thisPlayerUid;
    //    PhotonNetwork.RaiseEvent((byte)NetworkEvent.RequestNewUidEventCode, content, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    //}

    public void SendRequestCheckNewUid(int newUid){
        object content = newUid ;
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.RequestCheckNewUidEventCode, content, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }
}
