using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
using System.Linq;

public class RequestReceiver : MonoBehaviour
{
    //public const byte (byte)NetworkEvent.PositionEventCode = 1;

    private void OnEnable()
    {
        PhotonNetwork.NetworkingClient.EventReceived += OnEvent;
    }

    private void OnDisable()
    {
        PhotonNetwork.NetworkingClient.EventReceived -= OnEvent;
    }

    public void OnEvent(EventData photonEvent)
    {
        byte eventCode = photonEvent.Code;
        if (eventCode == (byte)NetworkEvent.PositionEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int viewId = (int)data[0];
            Vector3 pos = (Vector3)data[1];
            NetworkProcess.Instance.clientPosition[viewId] = pos;
        }
        else if (eventCode == (byte)NetworkEvent.InputEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int viewId = (int)data[0];
            int input = (int)data[1];
            NetworkProcess.Instance.AddNewPlayer(viewId);
            NetworkProcess.Instance.playerInputs[viewId] = input;
        }
        else if (eventCode == (byte)NetworkEvent.BattleRequestEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int requestViewId = (int)data[0];
            int targetViewId = (int)data[1];
            if (PhotonNetwork.GetPhotonView(targetViewId) == null)
            {
                return;
            }
            NetworkProcess.Instance.BattleRequest(requestViewId, targetViewId);
        }
        else if (eventCode == (byte)NetworkEvent.AnswerEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int viewId = (int)data[0];
            string[] answers = (string[])data[1];
            if (PhotonNetwork.GetPhotonView(viewId) == null)
            {
                return;
            }
            // BattleProcess.Instance.AnswerReceived(viewId, answers);
        }
        else if (eventCode == (byte)NetworkEvent.StateChangeEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int viewId = (int)data[0];
            int state = (int)data[1];
            if (PhotonNetwork.GetPhotonView(viewId) == null)
            {
                return;
            }
            // BattleProcess.Instance.PlayerStateChange(viewId, state);
        } else if (eventCode == (byte)NetworkEvent.AllInRoomEventCode)
        {
            Debug.Log("Request Received");
            // BattleProcess.Instance.StartWaitForReady();
        } else if (eventCode == (byte)NetworkEvent.ExitEventCode)
        {
            Debug.Log("Request exit received");
            // BattleProcess.Instance.ExitGame();
        }
        else if (eventCode == (byte)NetworkEvent.RequestCheckNewUserIdEventCode)
        {
            object data = (object)photonEvent.CustomData;
            int newUserId = (int)data;
            NetworkProcess.Instance.CheckNewUserId(newUserId);
        }
        else if (eventCode == (byte)NetworkEvent.CreateNewRoomEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int roomCode = (int)data[0];
            int userId = (int)data[1];
            int minigame = (int)data[2];
            RoomManager.Instance.CreateMinigameRoom(roomCode, userId, 2, (Minigame)minigame);
        }
        else if (eventCode == (byte)NetworkEvent.JoinRoomEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int roomCode = (int)data[0];
            int userId = (int)data[1];
            if (RoomManager.Instance.JoinMinigameRoom(roomCode, userId))
            {
                SendReply.Instance.SendJoinRoomReply(userId, (int)RoomManager.Instance.roomInfos[roomCode].minigame);
            } else
            {
                SendReply.Instance.SendJoinRoomReply(userId, (int)Minigame.None);
            }
        }
    }
}
