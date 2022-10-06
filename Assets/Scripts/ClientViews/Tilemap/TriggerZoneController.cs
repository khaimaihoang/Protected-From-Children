using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerZoneController : MonoBehaviour
{
    public string sceneName;
    public GameObject canvasObject;
    private List<int> _playerList;

    private ButtonZoneController _gameManager; 

    // Start is called before the first frame update
    public void Init()
    {
        sceneName = "";
        _playerList = new List<int>();
        _gameManager = FindObjectOfType<ButtonZoneController>();
    }

    public void Init(ButtonZoneController buttonZone)
    {
        sceneName = "";
        _playerList = new List<int>();
        _gameManager = buttonZone;
    }

    public void Init(GameObject canvas, ButtonZoneController buttonZone)
    {
        sceneName = "";
        canvasObject = canvas;
        _playerList = new List<int>();
        _gameManager = buttonZone;
    }

    private void OnEnterTriggerZone(int playerID)
    {
        _playerList.Add(playerID);
        canvasObject.SetActive(true);

        _gameManager.OnGetSceneName(this);
    }

    private void OnExitTriggerZone(int playerID)
    {
        _playerList.Remove(playerID);
        canvasObject.SetActive(false);

        _gameManager.OnRemoveSceneName();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Debug.Log("Enter Trigger Zone!");
            this.OnEnterTriggerZone(other.GetComponent<PlayerManager>().viewId);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Debug.Log("Exit Trigger Zone!");
            this.OnExitTriggerZone(other.GetComponent<PlayerManager>().viewId);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.5f, 0.1f, 0.3f, 0.5f);
        SphereCollider sphere = GetComponent<SphereCollider>();
        if(sphere != null)
        {
            Gizmos.DrawSphere(transform.position, sphere.radius);
        }
        
    }
}
