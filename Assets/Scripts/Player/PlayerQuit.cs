using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerQuit : MonoBehaviour
{
    private void OnApplicationQuit()
    {
        SendRequest.Instance.SendPlayerQuitRequest(ClientProcess.Instance.playerUserId);
        PhotonNetwork.SendAllOutgoingCommands();
    }
}
