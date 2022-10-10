using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInfo : MonoBehaviour
{
    public int roomId;
    public List<int> currentPlayers;
    public int maxPlayer;
    public Minigame minigame;
    public BattleProcess battleProcess;

    private List<int> readyingPlayers;

    public void SetRoomInfo(int roomId, int creatingPlayer, int maxPlayer, Minigame minigame)
    {
        this.roomId = roomId;
        this.currentPlayers.Add(creatingPlayer);
        this.maxPlayer = maxPlayer;
        this.minigame = minigame;
    }

    public void SetRoomInfo(int roomId, List<int> players, int maxPlayer, Minigame minigame)
    {
        this.roomId = roomId;
        this.currentPlayers = players;
        this.maxPlayer = maxPlayer;
        this.minigame = minigame;
    }

    public bool AddPlayer(int player)
    {
        if (currentPlayers.Count < maxPlayer)
        {
            currentPlayers.Add(player);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemovePlayer(int player)
    {
        if (currentPlayers.Contains(player))
        {
            currentPlayers.Remove(player);
        }
    }

    private void StartProcessing()
    {
        battleProcess = Instantiate(new BattleProcess(), gameObject.transform);
    }

    public void PlayerReady(int viewId)
    {
        if (currentPlayers.Contains(viewId) && !readyingPlayers.Contains(viewId))
        {
            readyingPlayers.Add(viewId);
        }

        if (readyingPlayers.Count == currentPlayers.Count)
        {
            StartProcessing();
        }
    }
}
