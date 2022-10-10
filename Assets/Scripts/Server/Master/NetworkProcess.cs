using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NetworkProcess : MonoSingleton<NetworkProcess>
{
    public List<int> playerList = new List<int>();
    public Dictionary<int, Vector3> clientPosition;
    public Dictionary<int, int> playerInputs = new Dictionary<int, int>();
    [SerializeField] public Dictionary<int, Vector3> playerPositions = new Dictionary<int, Vector3>();
    [SerializeField] private float _moveSpeed = 0.11f;

    [SerializeField] private bool _checkWinner = false;
    [SerializeField] private Vector3 _goalCenter= Vector3.zero;
    [SerializeField] private Vector3 _goalSize = Vector3.one;
    [SerializeField] private int minUid = 1, maxUid = 9999;
    private Bounds _goalBound;

    void Awake(){
        clientPosition = new Dictionary<int, Vector3>();
        _goalBound = new Bounds(_goalCenter, _goalSize);
    }

    private void FixedUpdate()
    {
        PlayerPositionCalculate();
        if (_checkWinner)
        {
            CheckWinner();
        }
    }

    public void CheckNewUid(int newUid){
        if (playerList.Contains(newUid))
        {
            SendReply.Instance.SendReplyChangeNewUid(newUid);
        }
        else
        {
            AddNewPlayer(newUid);
            SendReply.Instance.SendReplyNewUidAccepted(newUid);
        }
    }

    public int AddNewPlayer(int viewId = -1)
    {
        if (!playerList.Contains(viewId))
        {
            playerList.Add(viewId);
            playerPositions.Add(viewId, Vector3.zero);
        }
        return viewId;
    }

    public void PlayerInputProcess(int viewId, int playerInput)
    {

        if ((PlayerInput)playerInput == PlayerInput.STOP || playerInputs == null)
        {
            if (playerInputs.ContainsKey(viewId))
            {
                playerInputs.Remove(viewId);
            }
            return;
        }
        else
        {
            if (playerInputs.ContainsKey(viewId))
            {
                playerInputs[viewId] = playerInput;
            }
            else
            {
                playerInputs.Add(viewId, playerInput);
            }
        }
    }

    public void BattleRequest(int requestViewId, int targetViewId)
    {
        SendReply.Instance.SendBattleNotification(requestViewId, targetViewId);
    }

    private void PlayerPositionCalculate()
    {
        if (playerInputs.Count == 0)
        {
            return;
        }
        foreach (var state in playerInputs)
        {
            
            if ((PlayerInput)state.Value == PlayerInput.LEFT)
            {
                playerPositions[state.Key] = playerPositions[state.Key] + new Vector3(-1 * _moveSpeed, 0, 0);
            }
            else if ((PlayerInput)state.Value == PlayerInput.UP)
            {
                playerPositions[state.Key] = playerPositions[state.Key] + new Vector3(0, 0, 1 * _moveSpeed);
            }
            else if ((PlayerInput)state.Value == PlayerInput.RIGHT)
            {
                playerPositions[state.Key] = playerPositions[state.Key] + new Vector3(1 * _moveSpeed, 0, 0);
            }
            else if ((PlayerInput)state.Value == PlayerInput.DOWN)
            {
                playerPositions[state.Key] = playerPositions[state.Key] + new Vector3(0, 0, -1 * _moveSpeed);
            }

            // Debug.Log(state.Key +" - "+ playerPositions[state.Key]);
        }
    }

    private void CheckWinner()
    {
        foreach (int viewId in playerList)
        {
            if (_goalBound.Contains(playerPositions[viewId]))
            {
                SendReply.Instance.SendWinnerReply(viewId);
            }
        }
    }
}
