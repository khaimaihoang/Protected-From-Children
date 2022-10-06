using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileLookAt : MonoBehaviour
{
    private Camera _mainCamera;
    private List<Tilemap> _tilemaps;
    private Quaternion _camRotation;
    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        GameObject gridMap = GameObject.Find("MainGrid");
        _tilemaps = new List<Tilemap>();
        if(gridMap != null)
        {
            // Debug.Log("Grid not null!");
            foreach(Tilemap tilemap in gridMap.GetComponentsInChildren<Tilemap>())
            {
                _tilemaps.Add(tilemap);
            }
        }
        _camRotation = Quaternion.Euler(Vector3.zero);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!_camRotation.Equals(_mainCamera.transform.rotation))
        {
            FollowCameraAngle();
        }
        _camRotation = _mainCamera.transform.rotation;
    }

    private void FollowCameraAngle()
    {
        foreach(var tilemap in _tilemaps)
        {
            if (tilemap.name == "Ground") continue;
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                tilemap.SetTransformMatrix(pos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(_mainCamera.transform.eulerAngles.x, 0, 0), Vector3.one));
            }
        }
    }
}
