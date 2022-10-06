using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class SpawnPlayer : MonoBehaviour
{
    FollowPlayer _cam;

    private void Start()
    {
        _cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FollowPlayer>();
        Spawn();
        ClientsViewController.Instance.RequestOnStopToPull(false);
        if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers && PlayerPrefs.GetString("roomName") != InputManager.Instance.GeneralRoom)
        {
            SendRequest.Instance.SendAllInRoomSignal();
        }
    }

    public void Spawn()
    {
        GameObject playerPrefab = Resources.Load<GameObject>("Player");
        if (_cam.followTarget == null)
            _cam.followTarget = GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity).transform;
        else 
            GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        ClientsViewController.Instance.RequestOnInitPlayerViews();
        SendRequest.Instance.Init();
        SendRequest.Instance.SendRequestCheckNewUid();
    }
}
