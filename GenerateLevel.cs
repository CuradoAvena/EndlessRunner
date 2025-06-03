
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [Header("Configuraci�n")]
    public GameObject[] sectionPrefabs; // Prefabs de las secciones
    public int initialPoolSize = 10; // Cantidad inicial de cada secci�n
    public float spawnDistance = 50f; // Distancia entre secciones
    public float spawnDelay = 2f; // Tiempo entre generaciones
    public int maxActiveSections = 5; // M�ximo de secciones activas

    [Header("Debug")]
    public List<GameObject>[] pooledSections; // Pool por tipo de secci�n
    public List<GameObject> activeSections; // Secciones activas actualmente

    private float nextZPos = 0;
    private bool isGenerating = false;

    private void Start()
    {
        InitializePool();
        // Generar primera secci�n manualmente
        GenerateInitialSection();
    }

    private void Update()
    {
        if (!isGenerating && activeSections.Count < maxActiveSections)
        {
            StartCoroutine(GenerateSection());
        }
    }

    // Inicializa el pool de objetos
    private void InitializePool()
    {
        pooledSections = new List<GameObject>[sectionPrefabs.Length];
        activeSections = new List<GameObject>();

        for (int i = 0; i < sectionPrefabs.Length; i++)
        {
            pooledSections[i] = new List<GameObject>();

            for (int j = 0; j < initialPoolSize; j++)
            {
                GameObject obj = Instantiate(sectionPrefabs[i], Vector3.zero, Quaternion.identity);
                obj.SetActive(false);
                obj.transform.parent = transform; // Opcional: organizar en jerarqu�a
                pooledSections[i].Add(obj);
            }
        }
    }

    // Genera una secci�n inicial (opcional)
    private void GenerateInitialSection()
    {
        GameObject section = GetPooledSection(0); // Usa la primera secci�n
        section.transform.position = new Vector3(0, 0, nextZPos);
        section.SetActive(true);
        activeSections.Add(section);
        nextZPos += spawnDistance;
    }

    // Corrutina para generaci�n continua
    private IEnumerator GenerateSection()
    {
        isGenerating = true;

        // Espera antes de generar
        yield return new WaitForSeconds(spawnDelay);

        // Obtener secci�n aleatoria del pool
        int randomIndex = Random.Range(0, sectionPrefabs.Length);
        GameObject newSection = GetPooledSection(randomIndex);

        if (newSection != null)
        {
            // Posicionar y activar
            newSection.transform.position = new Vector3(0, 0, nextZPos);
            newSection.SetActive(true);
            activeSections.Add(newSection);
            nextZPos += spawnDistance;

            // Reciclar la secci�n m�s antigua si superamos el m�ximo
            if (activeSections.Count > maxActiveSections)
            {
                ReturnSectionToPool(activeSections[0]);
                activeSections.RemoveAt(0);
            }
        }

        isGenerating = false;
    }

    // Obtiene una secci�n del pool
    private GameObject GetPooledSection(int prefabIndex)
    {
        for (int i = 0; i < pooledSections[prefabIndex].Count; i++)
        {
            if (!pooledSections[prefabIndex][i].activeInHierarchy)
            {
                return pooledSections[prefabIndex][i];
            }
        }

        // Si no hay disponibles, crea una nueva (opcional)
        GameObject newObj = Instantiate(sectionPrefabs[prefabIndex], Vector3.zero, Quaternion.identity);
        pooledSections[prefabIndex].Add(newObj);
        newObj.SetActive(false);
        return newObj;
    }

    // Devuelve una secci�n al pool
    private void ReturnSectionToPool(GameObject section)
    {
        section.SetActive(false);
        section.transform.position = Vector3.zero;
    }
}
