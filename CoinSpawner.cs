using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    [Header("Configuración de Carriles")]
    public float[] xPositions = { -2f, 0f, 2f }; // Posiciones X fijas para los 3 carriles
    public float spawnDistance = 20f;             // Distancia inicial en Z
    public float coinSpacing = 3f;                // Espacio entre monedas en Z
    public float yPosition = 1f;                  // Altura de las monedas
    public int coinsPerRow = 3;                   // Monedas por fila (1 por carril)

    private float _nextZPosition;
    private Transform _player;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform; // Asegúrate de que tu cubo tenga tag "Player"
        _nextZPosition = spawnDistance;
        GenerateInitialCoins();
    }

    void Update()
    {
        if (_player.position.z > _nextZPosition - (coinsPerRow * coinSpacing))
        {
            SpawnCoinRow();
            _nextZPosition += coinSpacing;
        }
    }

    void GenerateInitialCoins()
    {
        for (int i = 0; i < coinsPerRow * 2; i++) // Genera dos filas iniciales
        {
            SpawnCoinRow();
            _nextZPosition += coinSpacing;
        }
    }

    void SpawnCoinRow()
    {
        foreach (float x in xPositions) // Genera 1 moneda por carril
        {
            GameObject coin = CoinPool.Instance.GetCoin();
            coin.transform.position = new Vector3(x, yPosition, _nextZPosition);
        }
    }
}

