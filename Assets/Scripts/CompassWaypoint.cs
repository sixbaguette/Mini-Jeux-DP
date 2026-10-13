using UnityEngine;
using UnityEngine.UI;

public class CompassWaypoint : MonoBehaviour
{
    [Header("Références UI")]
    [Tooltip("La caméra principale du joueur")]
    public Camera mainCamera;
    [Tooltip("Le Canvas (Render Mode: Screen Space - Overlay)")]
    public Canvas parentCanvas;
    [Tooltip("Le prefab du waypoint à instancier (doit contenir une Image et un Text)")]
    public GameObject waypointPrefab;

    [Header("Configuration Boussole")]
    [Tooltip("Rayon du cercle de la boussole sur l'écran (en pixels)")]
    public float compassRadius = 200f;

    [Header("Cibles")]
    [Tooltip("Liste des objets à suivre")]
    public Transform[] targetObjects;

    private void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // Création des waypoints pour chaque objet
        foreach (Transform target in targetObjects)
        {
            if (target != null)
            {
                CreateWaypointForTarget(target);
            }
        }
    }

    void CreateWaypointForTarget(Transform target)
    {
        GameObject wpInstance = Instantiate(waypointPrefab, parentCanvas.transform);
        WaypointUI wpScript = wpInstance.AddComponent<WaypointUI>();
        wpScript.Setup(target, mainCamera, compassRadius);
    }
}

public class WaypointUI : MonoBehaviour
{
    private Transform target;
    private Camera cam;
    private float radius;
    private RectTransform rectTransform;
    private Text distanceText;

    public void Setup(Transform targetTransform, Camera cameraRef, float compassRadius)
    {
        target = targetTransform;
        cam = cameraRef;
        radius = compassRadius;
        rectTransform = GetComponent<RectTransform>();
        distanceText = GetComponentInChildren<Text>();
    }

    private void Update()
    {
        if (target == null || cam == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 targetDir = target.position - cam.transform.position;

        Vector3 localDir = cam.transform.InverseTransformDirection(targetDir.normalized);

        float angle = Mathf.Atan2(localDir.x, localDir.z);

        float x = Mathf.Sin(angle) * radius;
        float y = Mathf.Cos(angle) * radius;

        rectTransform.anchoredPosition = new Vector2(x, y);

        if (distanceText != null)
        {
            float distance = Vector3.Distance(cam.transform.position, target.position);
            distanceText.text = $"{Mathf.RoundToInt(distance)}m";
        }
    }
}