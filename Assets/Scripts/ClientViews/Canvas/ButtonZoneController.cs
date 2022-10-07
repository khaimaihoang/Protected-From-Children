using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;

public class ButtonZoneController : MonoBehaviour
{
    //private GameObject _canvas;
    [SerializeField] Text roomNameText;
    public static string roomName;
    JoinAnotherRoom _joinAnotherRoom;

    private string _sceneName;
    private TriggerZoneController _triggerZone;

    private void Init()
    {
        //_canvas = GameObject.Find("Canvas");
        _joinAnotherRoom = GetComponent<JoinAnotherRoom>();

        _sceneName = "Loading 1";
        _triggerZone = null;
    }

    public void OnCreateRoom()
    {
        roomName = roomNameText.text;
        PlayerPrefs.SetString("roomName", roomName);
        _joinAnotherRoom.Leave(_sceneName);
    }

    public void OnJoinRoom()
    {
        roomName = roomNameText.text;
        PlayerPrefs.SetString("roomName", roomName);
        _joinAnotherRoom.Leave(_sceneName);
    }

    public void OnGetSceneName(TriggerZoneController currentTrigger)
    {
        _triggerZone = currentTrigger;
        _sceneName = _triggerZone.sceneName.Length == 0 ? "Loading 1" : _triggerZone.sceneName;
    }

    public void OnRemoveSceneName()
    {
        _triggerZone = null;
        _sceneName = "Loading 1";
    }

    private void OnEnable()
    {
        ClientsViewController.Instance.OnInitPlayerViews += Init;
    }

    private void OnDisable()
    {
        ClientsViewController.Instance.OnInitPlayerViews -= Init;
    }
}
