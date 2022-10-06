using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform followTarget;
    Vector3 basePosition;
    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (followTarget != null)
            transform.position = followTarget.position + basePosition;
    }

    void GetFollowTarget(GameObject playerObject)
    {
        followTarget = playerObject.transform;
    }
}
