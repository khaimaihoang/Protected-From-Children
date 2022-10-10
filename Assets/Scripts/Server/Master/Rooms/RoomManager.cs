using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public Dictionary<int, RoomInfo> roomInfos;
    //public Dictionary<int, int> playersInRoom;

    public void CreateMinigameRoom(int roomId, int creatingPlayer, int maxPlayer, Minigame minigame)
    {
        RoomInfo newRoomInfo = Instantiate(new RoomInfo(), gameObject.transform);
        newRoomInfo.SetRoomInfo(roomId, creatingPlayer, maxPlayer, minigame);
        roomInfos.Add(roomId, newRoomInfo);
    }

    public bool JoinMinigameRoom(int roomId, int viewId)
    {
        return roomInfos[roomId].AddPlayer(viewId);
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
