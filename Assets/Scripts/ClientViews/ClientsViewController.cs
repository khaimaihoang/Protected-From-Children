using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ClientsViewController : ApiSingleton<ClientsViewController>
{
    public event UnityAction<int, Vector3> OnReceivePlayerPositions;
    public event UnityAction OnInitPlayerViews;
    public event UnityAction<bool> OnStopToPull;

    public void RequestOnReceivePlayerPositions(int id, Vector3 newPosition)
    {
        OnReceivePlayerPositions?.Invoke(id, newPosition);
    }

    public void RequestOnInitPlayerViews()
    {
        OnInitPlayerViews?.Invoke();
    }

    public void RequestOnStopToPull(bool stopToPull)
    {
        OnStopToPull?.Invoke(stopToPull);
    }

}
