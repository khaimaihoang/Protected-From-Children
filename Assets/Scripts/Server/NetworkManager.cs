using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class NetworkManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject client = Resources.Load<GameObject>("ClientManager");
        Instantiate(client);
        client = Resources.Load<GameObject>("ClientNetwork");
        Instantiate(client);
    }

}
