using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoSingleton<RoomManager>
{
    public Dictionary<int, RoomInfo> roomInfos = new Dictionary<int, RoomInfo>();
    //public Dictionary<int, int> playersInRoom;

    public void CreateMinigameRoom(int roomId, int creatingPlayer, int maxPlayer, Minigame minigame)
    {
        GameObject g = new GameObject("RoomInfo");
        g.transform.SetParent(gameObject.transform);
        RoomInfo newRoomInfo = g.AddComponent<RoomInfo>();
        newRoomInfo.SetRoomInfo(roomId, creatingPlayer, maxPlayer, minigame);
        roomInfos.Add(roomId, newRoomInfo);
        SendReply.Instance.SendCreateNewRoomReply(creatingPlayer, (int)minigame);
    }

    public bool JoinMinigameRoom(int roomId, int userId)
    {
        return roomInfos[roomId].AddPlayer(userId);
    }

    public void QuitMinigameRoom(int roomId, int viewId)
    {
        if (roomInfos.ContainsKey(roomId))
        {
            if (roomInfos[roomId].currentPlayers.Contains(viewId))
            {
                roomInfos[roomId].RemovePlayer(viewId);
            }
        }
    }
}
