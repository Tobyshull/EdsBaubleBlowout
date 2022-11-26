using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothing : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothing;

    private Vector3 currentVel;

    void Update()
    {
        Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref currentVel, smoothing);
    }
}
