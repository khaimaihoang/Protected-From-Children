using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using ExitGames.Client.Photon;

public class SendRequest : MonoSingleton<SendRequest>
{
    //public const byte (byte)NetworkEvent.PositionEventCode = 1;
    private PlayerManager playerManager;
    // Start is called before the first frame update
    public void Init()
    {
        playerManager = FindObjectOfType<PlayerManager>();
        if (PhotonNetwork.CurrentRoom.Name == InputManager.Instance.GeneralRoom) SendPlayerInputRequest((int)PlayerInput.STOP);
        // StartCoroutine(SendPlayerPositionRequestCoroutine(0.01f));
    }

    IEnumerator SendPlayerPositionRequestCoroutine(float time){
        while(true){
            SendPlayerPositionRequest();
            yield return new WaitForSeconds(time);
        }
    }

    public void SendPlayerPositionRequest(){
        object[] content = new object[] {playerManager.viewId, playerManager.transform.position};
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.PositionEventCode, content, raiseEventOptions, SendOptions.SendUnreliable);
    }

    public void SendPlayerInputRequest(int playerInput)
    {
        object[] content = new object[] { playerManager.viewId, playerInput };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.InputEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendBattleRequest(int targetViewId)
    {
        object[] content = new object[] { playerManager.viewId, targetViewId };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.BattleRequestEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }
    
    public void SendAnswers(string[] answers)
    {
        object[] content = new object[] { playerManager.viewId, answers };
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient };
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.AnswerEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    public void SendStateChangeRequest(int state)
    {
        object[] content = new object[] { playerManager.viewId, state };
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
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.ExitEventCode, "", new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }

    public void SendRequestNewUid(){
        PhotonNetwork.RaiseEvent((byte)NetworkEvent.RequestNewUidEventCode, "", new RaiseEventOptions { Receivers = ReceiverGroup.MasterClient }, SendOptions.SendReliable);
    }
}
