using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Header("Points de passage")]
    [Tooltip("Liste des Transforms représentant la trajectoire de la plateforme")]
    public Transform[] waypoints;

    [Header("Réglages du mouvement")]
    [Tooltip("Vitesse de déplacement")]
    public float speed = 3.0f;
    [Tooltip("Temps de pause à chaque waypoint (en secondes)")]
    public float waitTime = 1.0f;

    private int currentWaypointIndex = 0;
    private float waitTimer = 0f;
    private bool isWaiting = false;

    private void Update()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
                currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            }
            return;
        }

        MovePlatform();
    }

    private void MovePlatform()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetWaypoint.position,
            speed * Time.deltaTime
        );

        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.05f)
        {
            transform.position = targetWaypoint.position;
            isWaiting = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length < 2) return;

        Gizmos.color = Color.cyan;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] != null)
            {
                Gizmos.DrawWireSphere(waypoints[i].position, 0.3f);

                int nextIndex = (i + 1) % waypoints.Length;
                if (waypoints[nextIndex] != null)
                {
                    Gizmos.DrawLine(waypoints[i].position, waypoints[nextIndex].position);
                }
            }
        }
    }
}