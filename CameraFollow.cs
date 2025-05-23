using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //
    public float smoothSpeed = 5f; //
    public Vector3 offset = new Vector3(0, 2, -5); //

    void LateUpdate()
    {
        if (target == null) return;

        // Calcula la posición deseada
        Vector3 desiredPosition = target.position + offset;

        // Suaviza el movimiento
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        // Opcional: hace que la cámara siempre mire al cubo
        transform.LookAt(target);
    }
}

