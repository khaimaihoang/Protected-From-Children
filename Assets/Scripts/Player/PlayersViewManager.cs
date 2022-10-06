using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersViewManager : MonoSingleton<PlayersViewManager>
{
    private Dictionary<int, GameObject> players;

    private void Awake()
    {
        
    }
    private void Start()
    {
        Init();
    }
    private void Init()
    {
        players = new Dictionary<int, GameObject>();
        //players.Add(1, GameObject.Find("Player 1"));
        //players.Add(2, GameObject.Find("Player 2"));
    }

    void OnReceivePlayerPositions(int id, Vector3 newPosition)
    {
        GameObject player;
        if(players.TryGetValue(id, out player))
        {
            // player.GetComponent<PlayerMovement>().HandleSpecificMovement(newPosition); 
            //
        }

    }

    void FixedUpdate()
    {
        //ClientsViewController.Instance.RequestOnReceivePlayerPositions(2, ((GameObject)players[1]).transform.position);
    }

    private void OnEnable()
    {
        ClientsViewController.Instance.OnInitPlayerViews += Init;
        ClientsViewController.Instance.OnReceivePlayerPositions += OnReceivePlayerPositions;
    }

    private void OnDisable()
    {
        ClientsViewController.Instance.OnInitPlayerViews -= Init;
        ClientsViewController.Instance.OnReceivePlayerPositions -= OnReceivePlayerPositions;
    }
}
