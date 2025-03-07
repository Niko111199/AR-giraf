using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class PlaneSpawner : MonoBehaviour
{
    public ARPlaneManager planeManager;
    public List<GameObject> planePrefabs; // Liste af plane prefabs

    private Dictionary<ARPlane, GameObject> spawnedPlanes = new Dictionary<ARPlane, GameObject>();
    private List<Vector3> occupiedPositions = new List<Vector3>(); // Liste af optagede positioner
    public float minDistanceBetweenPrefabs = 1.0f; // Minimum afstand

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
                occupiedPositions.Remove(spawnedPlanes[plane].transform.position);
                Destroy(spawnedPlanes[plane]);
                spawnedPlanes.Remove(plane);
            }
        }
    }

    void SpawnPlane(ARPlane plane)
    {
        if (planePrefabs.Count == 0) return;

        // Tjek om planet er vandret (f.eks. gulv)
        Vector3 planeNormal = plane.transform.up;
        if (Vector3.Dot(planeNormal, Vector3.up) < 0.98f)
        {
            return; // Afvis planer, der ikke er næsten lodrette (f.eks. vægge eller loft)
        }

        // Tjek om placeringen er for tæt på andre prefabs
        Vector3 planePosition = plane.transform.position;
        foreach (var position in occupiedPositions)
        {
            if (Vector3.Distance(planePosition, position) < minDistanceBetweenPrefabs)
            {
                return; // Undgå spawn hvis for tæt
            }
        }

        // Vælg et tilfældigt plane prefab
        GameObject selectedPrefab = planePrefabs[Random.Range(0, planePrefabs.Count)];

        // Spawn prefab over AR-plane
        GameObject spawnedPlane = Instantiate(selectedPrefab, planePosition, plane.transform.rotation);
        spawnedPlane.transform.parent = plane.transform;

        // Gem referencen og positionen
        spawnedPlanes[plane] = spawnedPlane;
        occupiedPositions.Add(planePosition);
    }
}
