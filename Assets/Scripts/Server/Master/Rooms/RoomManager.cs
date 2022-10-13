using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoSingleton<RoomManager>
{
    public Dictionary<int, RoomInfo> roomInfos = new Dictionary<int, RoomInfo>();
    public Dictionary<int, int> roomOfPlayer = new Dictionary<int, int>();

    public void CreateMinigameRoom(int roomId, int creatingPlayer, int maxPlayer, Minigame minigame)
    {
        GameObject g = new GameObject("RoomInfo");
        g.transform.SetParent(gameObject.transform);
        RoomInfo newRoomInfo = g.AddComponent<RoomInfo>();
        newRoomInfo.SetRoomInfo(roomId, creatingPlayer, maxPlayer, minigame);
        SendReply.Instance.SendCreateNewRoomReply(creatingPlayer, (int)minigame);
        DictionaryUpdate(roomId, newRoomInfo, creatingPlayer);
    }

    public bool IsPlayerInRoom(int userId)
    {
        if (roomOfPlayer.ContainsKey(userId)) return true; else return false;
    }

    public bool JoinMinigameRoom(int roomId, int userId)
    {
        if (roomInfos[roomId].AddPlayer(userId))
        {
            DictionaryUpdate(roomId, userId);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void QuitMinigameRoom(int roomId, int userId)
    {
        if (roomInfos.ContainsKey(roomId))
        {
            if (roomInfos[roomId].currentPlayers.Contains(userId))
            {
                roomInfos[roomId].RemovePlayer(userId);
                if (roomInfos[roomId].currentPlayers.Count == 0)
                {
                    DictionaryRemove(roomId);
                }
            }
            SendReply.Instance.SendPlayerLeaveReply(userId);
        }
    }

    private void DictionaryUpdate(int roomId, RoomInfo roomInfo, int viewId)
    {
        roomInfos.Add(roomId, roomInfo);
        roomOfPlayer.Add(viewId, roomId);
    }

    private void DictionaryUpdate(int roomId, RoomInfo roomInfo)
    {
        roomInfos.Add(roomId, roomInfo);
    }

    private void DictionaryUpdate(int roomId, int viewId)
    {
        roomOfPlayer.Add(viewId, roomId);
    }

    private void DictionaryRemove(int roomId)
    {
        foreach (int viewId in roomInfos[roomId].currentPlayers)
        {
            roomOfPlayer.Remove(viewId);
        }
        Destroy(roomInfos[roomId].gameObject);
        roomInfos.Remove(roomId);
    }
}
