using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPatternSpawner : MonoBehaviour
{
    public enum SpawnPattern { Lineal, Arco, Zigzag }

    [Header("Configuraci�n General")]
    public float spawnDistance = 20f;
    public float yPosition = 1f;
    public float zSpacing = 3f;
    public int coinsPerPattern = 5;

    [Header("Cambio de Patr�n")]
    public float patternChangeInterval = 10f; // Cambia cada 10 segundos
    private float _nextChangeTime;

    [Header("Patr�n: Arco")]
    public float arcHeight = 2f;
    public float arcWidth = 4f;

    [Header("Patr�n: Zigzag")]
    public float zigzagIntensity = 3f;
    public float zigzagFrequency = 0.5f;

    private float _nextZPosition;
    private Transform _player;
    private SpawnPattern _currentPattern;

    [Header("Detecci�n de Obst�culos")]
    public LayerMask obstacleMask; // Capa de obst�culos (as�gnala en el Inspector)
    public float raycastDownDistance = 5f; // Distancia m�xima del Raycast
    public float verticalOffset = 0.5f; // Ajuste de altura al spawnear

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        _nextZPosition = spawnDistance;
        _nextChangeTime = Time.time + patternChangeInterval;
        PickRandomPattern(); // Primer patr�n aleatorio
        GenerateInitialPatterns();
    }

    void Update()
    {
        // Cambia el patr�n cada X segundos
        if (Time.time >= _nextChangeTime)
        {
            PickRandomPattern();
            _nextChangeTime = Time.time + patternChangeInterval;
        }

        // Genera monedas mientras el jugador avanza
        if (_player.position.z > _nextZPosition - (coinsPerPattern * zSpacing))
        {
            SpawnPatternCoins();
            _nextZPosition += zSpacing * coinsPerPattern;
        }
    }

    void PickRandomPattern()
    {
        // Elige un patr�n aleatorio (excluyendo el actual para mayor variedad)
        SpawnPattern newPattern;
        do
        {
            newPattern = (SpawnPattern)Random.Range(0, 3);
        } while (newPattern == _currentPattern && System.Enum.GetValues(typeof(SpawnPattern)).Length > 1);

        _currentPattern = newPattern;
        Debug.Log($"Patr�n cambiado a: {_currentPattern}");
    }

    void GenerateInitialPatterns()
    {
        for (int i = 0; i < 2; i++)
        {
            SpawnPatternCoins();
            _nextZPosition += zSpacing * coinsPerPattern;
        }
    }

    void SpawnPatternCoins()
    {
        for (int i = 0; i < coinsPerPattern; i++)
        {
            float z = _nextZPosition + (i * zSpacing);
            Vector3 spawnPos = CalculatePatternPosition(z);

            // Verifica si la posici�n est� libre de obst�culos
            if (IsPositionValid(spawnPos))
            {
                GameObject coin = CoinPool.Instance.GetCoin();
                coin.transform.position = spawnPos + Vector3.up * verticalOffset;
            }
        }
    }

    bool IsPositionValid(Vector3 position)
    {
        // Raycast hacia abajo para detectar obst�culos
        if (Physics.Raycast(position, Vector3.down, out RaycastHit hit, raycastDownDistance, obstacleMask))
        {
            // Si golpea un obst�culo, la posici�n NO es v�lida
            return false;
        }
        return true; // Posici�n v�lida
    }

    Vector3 CalculatePatternPosition(float z)
    {
        switch (_currentPattern)
        {
            case SpawnPattern.Arco:
                float t = Mathf.Clamp01((z - _nextZPosition) / (zSpacing * (coinsPerPattern - 1)));
                float x = Mathf.Sin(t * Mathf.PI) * arcWidth;
                float y = yPosition + Mathf.Sin(t * Mathf.PI) * arcHeight;
                return new Vector3(x, y, z);

            case SpawnPattern.Zigzag:
                float xZig = Mathf.PingPong(z * zigzagFrequency, zigzagIntensity * 2) - zigzagIntensity;
                return new Vector3(xZig, yPosition, z);

            default: // Lineal
                return new Vector3(0, yPosition, z);
        }
    }


}
