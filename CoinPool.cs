using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPool : MonoBehaviour
{
    public static CoinPool Instance; // Singleton para acceso fácil

    [Header("Configuración")]
    public GameObject coinPrefab; // Prefab de la moneda (arrástralo en el Inspector)
    public int initialPoolSize = 20; // Cantidad inicial de monedas

    private Queue<GameObject> _coinPool = new Queue<GameObject>();

    private void Awake()
    {
        Instance = this; // Configura el Singleton
        InitializePool();
    }

    private void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject coin = Instantiate(coinPrefab, transform);
            coin.SetActive(false);
            _coinPool.Enqueue(coin);
        }
    }

    public GameObject GetCoin()
    {
        if (_coinPool.Count > 0)
        {
            GameObject coin = _coinPool.Dequeue();
            coin.SetActive(true);
            return coin;
        }
        else
        {
            // Si no hay monedas disponibles, crea una nueva
            GameObject newCoin = Instantiate(coinPrefab, transform);
            return newCoin;
        }
    }

    public void ReturnCoin(GameObject coin)
    {
        coin.SetActive(false);
        _coinPool.Enqueue(coin);
    }
}
