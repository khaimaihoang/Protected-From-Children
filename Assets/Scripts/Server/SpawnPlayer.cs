using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayer : MonoBehaviour
{
    FollowPlayer _cam;

    public float minX = -25;
    public float maxX = -15;
    public float minZ = 25;
    public float maxZ = 40;

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
        //Vector3 pos = new Vector3(Random.Range(minX, maxX), 0, Random.Range(minZ, maxZ));
        if (_cam.followTarget == null)
            _cam.followTarget = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity).transform;
        else PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
        ClientsViewController.Instance.RequestOnInitPlayerViews();
        SendRequest.Instance.Init();
    }
}
