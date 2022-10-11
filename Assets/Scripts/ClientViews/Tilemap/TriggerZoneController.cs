using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerZoneController : MonoBehaviour
{
    public GameObject canvasObject;

    public void Init(GameObject canvas)
    {
        canvasObject = canvas;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player" && other.transform.GetComponent<PlayerManager>().IsMine())
        {
            Debug.Log("Enter Trigger Zone!");
            canvasObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player" && other.transform.GetComponent<PlayerManager>().IsMine())
        {
            Debug.Log("Exit Trigger Zone!");
            canvasObject.SetActive(false);
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
