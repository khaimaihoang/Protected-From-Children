using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun;
public class ReplyReceiver : MonoBehaviour
{
    //public const byte (byte)NetworkEvent.GetClientPositionEventCode = 2;

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
        if (eventCode == (byte)NetworkEvent.GetClientPositionEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int[] userIds = (int[]) data[0];
            Vector3[] poss = (Vector3[]) data[1];
            
            ClientProcess.Instance.UpdatePlayerPosition(userIds, poss);
        }
        else if (eventCode == (byte)NetworkEvent.GetWinnerEventCode)
        {
            object data = (object)photonEvent.CustomData;
            int userId = (int)data;
            ClientProcess.Instance.WinnerReceived(userId);
        }
        else if (eventCode == (byte)NetworkEvent.GetQuestionsEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int[] questions = (int[])data[1];
            ClientProcess.Instance.QuestionsReceived(questions);
        }
        else if (eventCode == (byte)NetworkEvent.GetScoresEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int[] userIds = (int[])data[0];
            int[] scores = (int[])data[1];
            ClientProcess.Instance.ScoresReceived(userIds, scores);
        }
        else if (eventCode == (byte)NetworkEvent.NewUserIdAcceptedEventCode){
            object data = (object)photonEvent.CustomData;
            int newUserId = (int)data;
            ClientProcess.Instance.CreateNewPlayer(newUserId);
        }
        else if (eventCode == (byte)NetworkEvent.ChangeNewUserIdEventCode)
        {
            object data = (object)photonEvent.CustomData;
            int newUserId = (int)data;
            ClientProcess.Instance.ChangeUserId(newUserId);
        }
        else if (eventCode == (byte)NetworkEvent.GetCreatNewRoomEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int userId = (int)data[0];
            int minigame = (int)data[1];
            ClientProcess.Instance.LoadScene(userId, (Minigame)minigame + "Minigame");
        }
        else if (eventCode == (byte)NetworkEvent.GetJoinRoomEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int userId = (int)data[0];
            int minigame = (int)data[1];
            if ((Minigame)minigame != Minigame.None)
            {
                ClientProcess.Instance.LoadScene(userId, (Minigame)minigame + "Minigame");
            } else
            {
                Debug.Log("Can't join room");
            }
        }
        else if (eventCode == (byte)NetworkEvent.GetPlayerLeaveEventCode)
        {
            int userId = (int)photonEvent.CustomData;
            ClientProcess.Instance.LoadScene(userId, "Lobby");
        }
    }
}
