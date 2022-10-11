using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Collider2DGenerator : MonoBehaviour
{
    private List<Tilemap> _tilemaps;
    private GameObject _objectCollidersContainer, _triggerZoneContainer, _mainCanvas;
    private Dictionary<string, GameObject[]> _envObjList;
    private Quaternion _tilemapRotation;
    private Vector3 _cellSize;

    private void Init()
    {
        _tilemaps = new List<Tilemap>();

        _objectCollidersContainer = new GameObject("Env Object Container");
        _triggerZoneContainer = new GameObject("Trigger Zone Container");
        _envObjList = new Dictionary<string, GameObject[]>();
        GameObject mainGrid = GameObject.Find("MainGrid");
        if (mainGrid != null)
        {
            foreach (Transform child in mainGrid.transform)
            {
                if (child.name == "Collider Tile")
                {
                    foreach (Tilemap childTile in child.GetComponentsInChildren<Tilemap>())
                    {
                        _tilemaps.Add(childTile);
                    }
                    break;
                }
            }
        }
        _mainCanvas = FindObjectOfType<Canvas>().gameObject;
        foreach (Tilemap _tilemap in _tilemaps)
        {
            _tilemapRotation = _tilemap.orientationMatrix.rotation;
            _cellSize = _tilemap.transform.parent.GetComponentInParent<Grid>().cellSize;
            GenEnvObjContainer(_tilemap);
        }

        _mainCanvas.SetActive(false);
    }

    private void GenEnvObjContainer(Tilemap _tilemap){
        bool isInteractable = false;
        
        if(_tilemap.name.Contains("Interactable") && !_tilemap.name.Contains("NonInteractable"))
        {
            isInteractable = true;
        }
        foreach (var pos in _tilemap.cellBounds.allPositionsWithin){
            Sprite tileSprite = _tilemap.GetSprite(pos);
            if (tileSprite){
                //Debug.Log(pos + " " + tileSprite.bounds.size);
                GameObject envObj = new GameObject("Object " + pos.ToString());
                envObj.transform.SetParent(_objectCollidersContainer.transform);
                Vector3 spritePositionInt = _tilemap.CellToWorld(pos);
                Vector3 spriterPosition = new Vector3(spritePositionInt.x + _cellSize.x / 2, 0, spritePositionInt.z + _cellSize.y * 1.5f);
                envObj.transform.position = spriterPosition;
                envObj.transform.rotation = _tilemapRotation;

                // Generate Collider
                BoxCollider collider = envObj.AddComponent<BoxCollider>();
                collider.center = tileSprite.bounds.center;
                collider.size = new Vector3 (tileSprite.bounds.size.x * 0.95f, tileSprite.bounds.size.y, tileSprite.bounds.size.z);

                //Generate Interactable Zone
                GameObject triggerZone = null;
                if (isInteractable)
                {
                    triggerZone = new GameObject("Trigger " + pos.ToString());
                    triggerZone.transform.SetParent(_triggerZoneContainer.transform);

                    //Generate Collider
                    SphereCollider sphereCollider = triggerZone.AddComponent<SphereCollider>();
                    TriggerZoneController triggerScript = triggerZone.AddComponent<TriggerZoneController>();
                    triggerScript.Init(_mainCanvas);
                    sphereCollider.radius = 1.5f;
                    //sphereCollider.center = tileSprite.bounds.min;
                    sphereCollider.transform.position = new Vector3(spriterPosition.x, 0, spriterPosition.z - sphereCollider.radius * 1.5f);

                    sphereCollider.isTrigger = true;
                    triggerZone.SetActive(false);
                }
                //Add to List
                _envObjList.Add(pos.ToString(), new GameObject[2] {envObj, triggerZone});

                //envObj.layer = LayerMask.NameToLayer("Standing Objects");
                envObj.SetActive(false);
            }
        }
    }

    public void GenerateColliderInRadius(Vector3Int location, Tilemap _tilemap)
    {
        if (!_tilemaps.Contains(_tilemap))
        {
            return;
        }
        Sprite tileSprite = _tilemap.GetSprite(location);
        GameObject[] childObjects = null;
        if (tileSprite && _envObjList.TryGetValue(location.ToString(), out childObjects))
        {
            foreach (GameObject child in childObjects)
            {
                if(child != null)
                {
                    child.SetActive(true);
                }
            }
        }

    }

    public void DestroyColliderOutRadius(Vector3Int location, Tilemap _tilemap)
    {
        if (!_tilemaps.Contains(_tilemap))
        {
            return;
        }
        Sprite tileSprite = _tilemap.GetSprite(location);
        GameObject[] childObjects = null;
        if (tileSprite && _envObjList.TryGetValue(location.ToString(), out childObjects))
        {
            foreach(GameObject child in childObjects)
            {
                if (child != null)
                {
                    child.SetActive(false);
                }
            }
        }

    }

    private void OnDisable()
    {
        ClientsViewController.Instance.OnInitPlayerViews -= Init;
        //TilemapController.Instance.OnChangeTile -= OnChangeTile;
    }

    private void OnEnable()
    {
        ClientsViewController.Instance.OnInitPlayerViews += Init;
        //TilemapController.Instance.OnChangeTile += OnChangeTile;
    }

}
