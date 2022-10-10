using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomState: int
{
    Waiting,
        Playing
}
public class RoomInfo : MonoBehaviour
{
    public int roomId { get; private set; }
    public int roomState { get; private set; }
    public List<int> currentPlayers { get; private set; }
    public int maxPlayer { get; private set; }
    public Minigame minigame { get; private set; }
    public BattleProcess battleProcess { get; private set; }

    private List<int> readyingPlayers;

    public void SetRoomInfo(int roomId, int creatingPlayer, int maxPlayer, Minigame minigame)
    {
        this.roomId = roomId;
        this.currentPlayers.Add(creatingPlayer);
        this.maxPlayer = maxPlayer;
        this.minigame = minigame;
        this.roomState = (int)RoomState.Waiting;
    }

    public void SetRoomInfo(int roomId, List<int> players, int maxPlayer, Minigame minigame)
    {
        this.roomId = roomId;
        this.currentPlayers = players;
        this.maxPlayer = maxPlayer;
        this.minigame = minigame;
        this.roomState = (int)RoomState.Waiting;
    }

    public bool AddPlayer(int player)
    {
        if (this.roomState == (int)RoomState.Playing)
        {
            return false;
        }
        else if (currentPlayers.Count < maxPlayer)
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
            this.roomState = (int)RoomState.Playing;
        }
    }
}
