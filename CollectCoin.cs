using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectCoin : MonoBehaviour
{
    [Header("Configuraci�n")]
    public AudioClip collectSound;  // Arrastra tu sonido aqu� en el Inspector
    public int coinValue = 1;      // Puntos por moneda
    [Range(0, 1)] public float volume = 1f; // Volumen del sonido

    private AudioSource _audioSource;
    private Collider _collider;
    private MeshRenderer _renderer;

    private void Awake()
    {
        // Cachear componentes para mejor rendimiento
        _collider = GetComponent<Collider>();
        _renderer = GetComponent<MeshRenderer>();

        // Configurar AudioSource
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.volume = volume;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verificar si lo toca el jugador (aseg�rate de que tiene el tag "Player")
        if (other.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        // 1. Desactivar componentes visuales/f�sicos
        _collider.enabled = false;
        _renderer.enabled = false;

        // 2. Reproducir sonido (si est� asignado)
        if (collectSound != null)
        {
            _audioSource.PlayOneShot(collectSound);
        }
        else
        {
            Debug.LogWarning("�No hay sonido asignado!", gameObject);
        }

        // 3. Sumar puntos (requiere GameManager con m�todo AddCoins)
        if (GameController.Instance != null)
        {
            GameController.Instance.AddCoins(coinValue);
        }

        // 4. Destruir o reciclar despu�s del sonido
        float destroyDelay = collectSound != null ? collectSound.length : 0.5f;
        Destroy(gameObject, destroyDelay);

        // Opci�n con Pooling (recomendado para muchas monedas):
        // Invoke(nameof(ReturnToPool), destroyDelay);
    }

    // private void ReturnToPool() {
    //     CoinPool.Instance.ReturnToPool(gameObject);
    // }
}