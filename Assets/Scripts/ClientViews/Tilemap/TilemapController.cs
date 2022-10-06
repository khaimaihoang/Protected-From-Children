using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class TilemapController: ApiSingleton<TilemapController>
{
    public event UnityAction<Vector3Int, Tilemap, bool> OnChangeTile;
    // Start is called before the first frame update

    public void RequestOnChangeTile(Vector3Int location, Tilemap _tilemap, bool flag)
    {
        OnChangeTile?.Invoke(location, _tilemap, flag);
    }
}
