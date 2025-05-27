using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMove : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody rb;
    public Color colorInicial;     // Color al iniciar

    public float fuerzaSalto = 7f; // Fuerza del salto
    public LayerMask capaSuelo; // Capa que identifica el suelo
    public Transform checkSuelo; // Objeto que verifica contacto con el suelo
    public float radioCheck = 0.4f; // Radio de detección del suelo

    float horizontalInput;
    public float horizontalMultiplier = 2f; 

    bool enSuelo; // ¿Está el cubo tocando el suelo?
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

         // Versión simplificada para testear
            enSuelo = Physics.Raycast(transform.position, Vector3.down, 0.6f, capaSuelo);

            if (Input.GetKeyDown(KeyCode.Space)) {
                if (enSuelo)
                {
                    rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
                    Debug.Log("Saltando!"); // Para verificar en consola
                }
                else
                {
                    Debug.Log("No está en suelo"); // Mensaje de depuración
                }
             }
        

    }
}
