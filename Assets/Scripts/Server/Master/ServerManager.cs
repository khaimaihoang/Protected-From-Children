using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerManager : MonoBehaviourPunCallbacks
{
    [SerializeField] private string _roomName = "GeneralRoom";

    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Init()
    {
        CreateRoom(_roomName);
        
    }

    public void CreateRoom(string roomName)
    {
        PhotonNetwork.ConnectUsingSettings();
        
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        PhotonNetwork.CreateRoom(_roomName);
        AddServerComponent();
    }


    public void AddServerComponent()
    {
        //GameObject master = GameObject.Find("Master");
        //master.SetActive(true);
        GameObject master = Resources.Load<GameObject>("Master");
        Instantiate(master);
    }
}

