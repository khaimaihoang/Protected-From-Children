using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update
    // private InputManager inputManager;
    private PlayerMovement playerMovement;
    private CameraManager cameraManager;
    private Camera _mainCamera;
    public int userId;

    private bool isHasServer;

    // public Transform targetTransform;

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cameraManager = FindObjectOfType<CameraManager>();
        _mainCamera = FindObjectOfType<Camera>();
        userId = 0;
    }

    void Start(){
        isHasServer = FindObjectOfType<NetworkProcess>() ? true : false;
    }

    public bool IsMine(){
        return userId == ClientProcess.Instance.playerUserId;
    }

    // Update is called once per frame
    void Update(){
        if (IsMine())
        {
            InputManager.Instance.HandleAllInput();
        }
    }
    void FixedUpdate()
    {
        if (IsMine()){
            // if (!isHasServer)
            // {
            //     playerMovement.HandleMovementFromInput();
            // }
            //Camera
            this.CameraProjection();
        }
    }

    private void CameraProjection()
    {
        if (InputManager.Instance.cameraProjection > 0.5f)
        {
            if (!_mainCamera.orthographic)
            {
                _mainCamera.orthographic = true;
                _mainCamera.orthographicSize = 5f;
            }

            //Scroll zoom
            _mainCamera.orthographicSize += InputManager.Instance.mouseScroll * Time.deltaTime * 100;

            if(_mainCamera.orthographicSize > 25)
            {
                _mainCamera.orthographicSize = 25;
            }
            if(_mainCamera.orthographicSize < 5)
            {
                _mainCamera.orthographicSize = 5;
            }

        }
        else
        {
            if (_mainCamera.orthographic)
            {
                _mainCamera.orthographicSize = 5;
                _mainCamera.orthographic = false;
            }
        }
    }

    // void LateUpdate(){
    //     cameraManager.HandleAllCameraControl();
    // }
}
