using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Photon.Pun;

//[RequireComponent(typeof(Tilemap))]
public class PullTileObject : MonoBehaviour
{
    private List<Tilemap> _tilemaps;
    private Camera _mainCamera;
    private Transform _playerTransform;
    private TileData _tileData;
    // List<Vector3Int> _standingTiles;
    // List<Vector3Int> _nearbyTiles;
    private float _rotateSpeed = 10;
    private bool _stopToPull = true;

    public int radius, outside;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Init()
    {
        _tilemaps = new List<Tilemap>();
        _mainCamera = GameObject.FindObjectOfType<Camera>();
        // _playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        _playerTransform = ClientProcess.Instance.players[ClientProcess.Instance.playerUserId].transform;

        GameObject _mainGrid = GameObject.Find("MainGrid");
        if (_mainGrid != null)
        {
            foreach (Tilemap tilemap in _mainGrid.GetComponentsInChildren<Tilemap>())
            {
                _tilemaps.Add(tilemap);
            }
        }
        this.ResetAllTilemaps();
    }

    private void ResetAllTilemaps()
    {
        foreach (var tilemap in _tilemaps)
        {
            if (tilemap.name == "Ground") continue;
            foreach (var pos in tilemap.cellBounds.allPositionsWithin)
            {
                if (tilemap.GetSprite(pos) != null)
                {
                    SetTransparency(pos, 0, tilemap, false);
                    tilemap.SetTransformMatrix(pos, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(90, 0, 0), Vector3.one));
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_stopToPull)
        {
            if (_playerTransform.GetComponent<PlayerManager>().IsMine())
            {
                GetCellsByRadiusAndOutside(radius, outside);
            }
        }
    }

    private void OnChangeTile(Vector3Int location, Tilemap _tilemap, bool flag)
    {
        if (flag)
        {
            this.PullUp(location, _tilemap);
        }
        else
        {
            this.FoldDown(location, _tilemap);
        }
    }

    private void GetCellsByRadiusAndOutside(int radius, int outside)
    {

        foreach(Tilemap _tilemap in _tilemaps)
        {
            if (_tilemap.name == "Ground") continue;
            Vector3Int baseLocation = _tilemap.WorldToCell(_playerTransform.position);
            for (int x = -radius - outside; x <= radius + outside; x++)
            {
                for (int y = -radius - outside; y <= radius + outside; y++)
                {
                    
         
                    if ((Mathf.Abs(x) <= radius) && (Mathf.Abs(y) <= radius))
                    {
                        TilemapController.Instance.RequestOnChangeTile(new Vector3Int(baseLocation.x + x, baseLocation.y + y, baseLocation.z), _tilemap, true);
                    }
                    else
                    {
                        TilemapController.Instance.RequestOnChangeTile(new Vector3Int(baseLocation.x + x, baseLocation.y + y, baseLocation.z), _tilemap, false);
                    }
                }
            }
        }
        
    }

    private void FoldDown(Vector3Int location, Tilemap _tilemap)
    {
        //Vector3Int location = new Vector3Int(baseLocation.x + x, baseLocation.y + y, baseLocation.z);
        if (_tilemap.GetTile(location) && _tilemap.GetSprite(location) != null)
        {
            if (_tilemap.GetTransformMatrix(location).rotation.eulerAngles.x != 90)
            {
                //Quaternion rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(_mainCamera.transform.eulerAngles.x * Time.deltaTime , 0, 0), 1);
                float newAngle = _tilemap.GetTransformMatrix(location).rotation.eulerAngles.x + (-_tilemap.GetTransformMatrix(location).rotation.eulerAngles.x + 90)* Time.deltaTime * _rotateSpeed;
                // if (rotateAngle < 0)
                // {
                //     rotateAngle = 0;
                // }
                _tilemap.SetTransformMatrix(location, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(newAngle, 0, 0), Vector3.one));
                float ratio = (90 - newAngle) / _mainCamera.transform.eulerAngles.x;
                SetTransparency(location, ratio, _tilemap, false);
            }
            //Debug.Log("Fall " + _tilemap.GetTransformMatrix(location).rotation.eulerAngles.x);
            //Debug.Log(_tilemap.GetTile(location));
        }
    }

    private void PullUp(Vector3Int location, Tilemap _tilemap)
    {
        //Vector3Int location = new Vector3Int(baseLocation.x + x, baseLocation.y + y, baseLocation.z);
        if (_tilemap.GetTile(location) && _tilemap.GetSprite(location) != null)
        {
            if (_tilemap.GetTransformMatrix(location).rotation.eulerAngles.x != _mainCamera.transform.eulerAngles.x)
            {

                //Quaternion rotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(_mainCamera.transform.eulerAngles.x * Time.deltaTime , 0, 0), 1);
                float newAngle = _tilemap.GetTransformMatrix(location).rotation.eulerAngles.x + (-_tilemap.GetTransformMatrix(location).rotation.eulerAngles.x + _mainCamera.transform.eulerAngles.x)* Time.deltaTime * _rotateSpeed;
                // Debug.Log(newAngle);
                // if (rotateAngle < 0)
                // {
                //     rotateAngle = 0;
                // }
                _tilemap.SetTransformMatrix(location, Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(newAngle, 0, 0), Vector3.one));
                float ratio = (90 - newAngle) / _mainCamera.transform.eulerAngles.x;
                SetTransparency(location, ratio, _tilemap, true);
            }
            //Debug.Log("Pull " + _tilemap.GetTransformMatrix(location).rotation.eulerAngles.x);
            //Debug.Log(_tilemap.GetTile(location));
        }
    }

    private void SetTransparency(Vector3Int location, float ratio, Tilemap _tilemap, bool flag)
    {
        //Player behind Tile Objects
        //if(flag == true && _tilemap.name == "Standing Objects" && ratio > 0.5f)
        //{
        //    Sprite sprite = _tilemap.GetSprite(location);
        //    float value = sprite.bounds.size.y / Mathf.Cos(_tilemap.GetTransformMatrix(location).rotation.eulerAngles.x);
        //    Vector3 spritePos = _tilemap.CellToWorld(location);
            
        //    if (_playerTransform.position.z - spritePos.z > 0 && _playerTransform.position.z - spritePos.z < value * 0.75f && Mathf.Abs(_playerTransform.position.x - spritePos.x) < sprite.bounds.size.x * 0.75f)
        //    {
        //        ratio = Mathf.Abs((_playerTransform.position - spritePos).sqrMagnitude) * 0.05f / ratio;
        //    }
        //}
        //Set Transparency
        Color color = _tilemap.GetColor(location);
        color.a = ratio;

        _tilemap.SetTileFlags(location, TileFlags.None);
        _tilemap.SetColor(location, color);
    }

    void OnStopToPull(bool stopToPull)
    {
        _stopToPull = stopToPull;
    }

    private void OnDisable()
    {
        ClientsViewController.Instance.OnInitPlayerViews -= Init;
        TilemapController.Instance.OnChangeTile -= OnChangeTile;
        ClientsViewController.Instance.OnStopToPull -= OnStopToPull;
    }

    public void OnEnable()
    {
        ClientsViewController.Instance.OnInitPlayerViews += Init;
        TilemapController.Instance.OnChangeTile += OnChangeTile;
        ClientsViewController.Instance.OnStopToPull += OnStopToPull;
    }
}
