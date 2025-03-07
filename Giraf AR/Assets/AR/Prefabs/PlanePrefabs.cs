using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultiPlaneSpawner : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public List<GameObject> planePrefabs; // Liste af plane prefabs

    private Dictionary<ARPlane, GameObject> spawnedPlanes = new Dictionary<ARPlane, GameObject>();

    void OnEnable()
    {
        planeManager.planesChanged += OnPlanesChanged;
    }

    void OnDisable()
    {
        planeManager.planesChanged -= OnPlanesChanged;
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        // Tilføj nye planer
        foreach (var plane in args.added)
        {
            SpawnPlane(plane);
        }

        // Fjern slettede planer
        foreach (var plane in args.removed)
        {
            if (spawnedPlanes.ContainsKey(plane))
            {
                Destroy(spawnedPlanes[plane]);
                spawnedPlanes.Remove(plane);
            }
        }
    }

    void SpawnPlane(ARPlane plane)
    {
        if (planePrefabs.Count == 0) return;

        // Vælg et tilfældigt plane prefab
        GameObject selectedPrefab = planePrefabs[Random.Range(0, planePrefabs.Count)];

        // Spawn prefab over AR-plane
        GameObject spawnedPlane = Instantiate(selectedPrefab, plane.transform.position, plane.transform.rotation);
        spawnedPlane.transform.parent = plane.transform;

        // Gem referencen
        spawnedPlanes[plane] = spawnedPlane;
    }
}
