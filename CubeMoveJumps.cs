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

    [Header("Límites de Movimiento")]
    public float limiteIzquierdo = -4f;  // Límite en X negativo
    public float limiteDerecho = 4f;     // Límite en X positivo
    void Start()
    {
        // Cambia el color al inicio (ejemplo: rojo)
        GetComponent<Renderer>().material.color = colorInicial;
    }

    private void FixedUpdate()
    {
        Vector3 forwardMove = transform.forward * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = transform.right * horizontalInput * speed * Time.fixedDeltaTime * horizontalMultiplier;
        // Calcula la nueva posición
        Vector3 newPosition = rb.position + forwardMove + horizontalMove;

        // Aplica los límites en el eje X
        newPosition.x = Mathf.Clamp(newPosition.x, limiteIzquierdo, limiteDerecho);
        // Mueve el Rigidbody
        rb.MovePosition(newPosition);
    }
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");

        // Versión simplificada para testear
        enSuelo = Physics.Raycast(transform.position, Vector3.down, 0.6f, capaSuelo);

        if (Input.GetKeyDown(KeyCode.Space) && enSuelo)
        {
            rb.AddForce(Vector3.up * fuerzaSalto, ForceMode.Impulse);
            // Cambio de color con Espacio
            if (Input.GetKeyDown(KeyCode.Space))
            {
                GetComponent<Renderer>().material.color = new Color(
                    Random.value,
                    Random.value,
                    Random.value,
                    1f
                );
            }


        }
    }
 }
