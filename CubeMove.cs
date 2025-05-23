using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody rb;
    public Color colorInicial;     // Color al iniciar

    float horizontalInput;
    public float horizontalMultiplier =2f;

    void Start()
    {
        // Cambia el color al inicio (ejemplo: rojo)
        GetComponent<Renderer>().material.color = colorInicial;
    }

    private void FixedUpdate()
    {
        Vector3 fowardMove = transform.forward * speed*Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput*speed*Time.fixedDeltaTime*horizontalMultiplier;
        rb.MovePosition(rb.position + fowardMove + horizontalMove);
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        
        // Cambia de color con la barra espaciadora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Color colorAleatorio = new Color(
                Random.value,  // R (rojo)
                Random.value,  // G (verde)
                Random.value,  // B (azul)
                1f            // Alpha (transparencia)
            );
            GetComponent<Renderer>().material.color = colorAleatorio;
        }
    }
}
