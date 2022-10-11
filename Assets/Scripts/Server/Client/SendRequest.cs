using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;

public class SendRequest : MonoSingleton<SendRequest>
{
    //public const byte (byte)NetworkEvent.PositionEventCode = 1;
    private int playerUserId;
    // Start is called before the first frame update
    public void Init()
    {
        playerUserId = ClientProcess.Instance.playerUserId;
        if (PhotonNetwork.CurrentRoom.Name == InputManager.Instance.GeneralRoom) SendPlayerInputRequest((int)PlayerInput.STOP);
    }

    public void SendPlayerInputRequest(int playerInput)
    {
        object[] content = new object[] { playerUserId, playerInput };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.InputEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    
    public void SendAnswers(string[] answers)
    {
        object[] content = new object[] { playerUserId, answers };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.AnswerEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendPlayerReadyRequest()
    {
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.PlayerReadyEventCode, playerUserId, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendRequestCheckNewUserId(int newUserId){
        object content = newUserId ;
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.RequestCheckNewUserIdEventCode, content, new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    public void SendCreateNewRoomRequest(int roomCode, int userId, Minigame minigame = Minigame.Quiz)
    {
        object[] content = new object[] { roomCode, userId, (int)minigame };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.CreateNewRoomEventCode, content,
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    public void SendJoinRoomRequest(int roomCode, int userId)
    {
        object[] content = new object[] { roomCode, userId };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.JoinRoomEventCode, content,
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    public void SendPlayerQuitRequest(int userId)
    {
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.PlayerQuitEventCode, userId,
            new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }
}
