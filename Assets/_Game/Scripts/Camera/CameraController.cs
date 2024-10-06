using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] public GameObject target;
    [SerializeField] private float smoothTime;
    private float defaultSmoothTime = 0;
    private Vector3 current = Vector3.zero;

    private bool isMoveCam = false;
    private Transform tf;

    public Transform Tf
    {
        get
        {
            if (tf == null)
            {
                tf = transform;
            }
            return tf;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.transform.position + offset;
            Tf.position = Vector3.SmoothDamp(transform.position, targetPosition, ref current, smoothTime);
            Tf.LookAt(target.transform);

        }
        if (Vector3.Distance(Tf.position, offset) < 1.0f)
        {
            isMoveCam = false;
            if (isMoveCam == false)
            {
                smoothTime = defaultSmoothTime;
            }
        }
    }

    public void SetUpCamera(float distanceFormTager, float heightAboveTager)
    {
        if (target != null)
        {

            Vector3 newPosition = target.transform.position - target.transform.forward * distanceFormTager;
            newPosition.y = target.transform.position.y + heightAboveTager;
            Tf.position = newPosition;
            offset = Tf.position - target.transform.position;
        }
    }

    public void MoveCamera(Vector3 newPosition, float smooth)
    {
        isMoveCam = true;
        if (target != null)
        {
            offset = newPosition;
            smoothTime = smooth;
        }
    }
}
