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
            int[] viewIds = (int[]) data[0];
            Vector3[] poss = (Vector3[]) data[1];
            for(int i = 0; i < viewIds.Length; i++){
                ClientProcess.Instance.playerPositionFromServer[viewIds[i]] = poss[i];
                // Debug.Log(viewIds[i] + " - " + poss[i]);
            }
            ClientProcess.Instance.UpdatePlayerPosition();
        }
        else if (eventCode == (byte)NetworkEvent.GetWinnerEventCode)
        {
            object data = (object)photonEvent.CustomData;
            int viewId = (int)data;
            ClientProcess.Instance.WinnerReceived(viewId);
        }
        else if(eventCode == (byte)NetworkEvent.GetBattleRequestEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int requestViewId = (int)data[0];
            int targetViewId = (int)data[1];
            ClientProcess.Instance.BattleRequestReceived(requestViewId, targetViewId);
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
            int[] viewIds = (int[])data[0];
            int[] scores = (int[])data[1];
            ClientProcess.Instance.ScoresReceived(viewIds, scores);
        } else if (eventCode == (byte)NetworkEvent.GetReadyEventCode)
        {
            Debug.Log("Reply Received");
            bool data = (bool)photonEvent.CustomData;
            ClientProcess.Instance.ReadyStateReceived(data);
        } else if (eventCode == (byte)NetworkEvent.ExitEventCode)
        {
            Debug.Log("Reply Exit Room");
            ClientProcess.Instance.JoinGeneralRoom();
        }
        else if (eventCode == (byte)NetworkEvent.NewUidAcceptedEventCode){
            object data = (object)photonEvent.CustomData;
            int newUid = (int)data;
            ClientProcess.Instance.CreateNewPlayer(newUid);
        }
        else if (eventCode == (byte)NetworkEvent.ChangeNewUidEventCode)
        {
            object data = (object)photonEvent.CustomData;
            int newUid = (int)data;
            ClientProcess.Instance.ChangeUid(newUid);
        }
        else if (eventCode == (byte)NetworkEvent.GetCreatNewRoomEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int userId = (int)data[0];
            int minigame = (int)data[1];
            ClientProcess.Instance.LoadMinigameScene(userId, minigame);
        }
        else if (eventCode == (byte)NetworkEvent.GetJoinRoomEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;
            int userId = (int)data[0];
            int minigame = (int)data[1];
            if ((Minigame)minigame != Minigame.None)
            {
                ClientProcess.Instance.LoadMinigameScene(userId, minigame);
            } else
            {
                Debug.Log("Can't join room");
            }
        }
    }
}
