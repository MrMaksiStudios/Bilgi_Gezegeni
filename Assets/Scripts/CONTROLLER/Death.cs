using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Death : MonoBehaviour
{
    private const string SpawnPointXKey = "LastSpawnPointX";
    private const string SpawnPointYKey = "LastSpawnPointY";
    private const string SpawnPointZKey = "LastSpawnPointZ";
    private const string SpawnPointRotXKey = "LastSpawnPointRotX";
    private const string SpawnPointRotYKey = "LastSpawnPointRotY";
    private const string SpawnPointRotZKey = "LastSpawnPointRotZ";
    private const string SpawnPointRotWKey = "LastSpawnPointRotW";

    private static Vector3 lastSpawnPosition;
    private static Quaternion lastSpawnRotation;
    private static bool hasSavedSpawnPoint;

    private void Awake()
    {
        LoadSavedSpawnPoint();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Spawn"))
        {
            SetLastSpawnPoint(other.transform);
            return;
        }

        if (other.CompareTag("Enemy"))
        {
            TeleportToLastSpawnPoint();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            TeleportToLastSpawnPoint();
        }
    }

    private void SetLastSpawnPoint(Transform spawnPoint)
    {
        lastSpawnPosition = spawnPoint.position;
        lastSpawnRotation = spawnPoint.rotation;
        hasSavedSpawnPoint = true;

        SaveSpawnPointToPrefs();

        Debug.Log("Spawn point updated to: " + spawnPoint.name);
    }

    private void SaveSpawnPointToPrefs()
    {
        PlayerPrefs.SetFloat(SpawnPointXKey, lastSpawnPosition.x);
        PlayerPrefs.SetFloat(SpawnPointYKey, lastSpawnPosition.y);
        PlayerPrefs.SetFloat(SpawnPointZKey, lastSpawnPosition.z);
        PlayerPrefs.SetFloat(SpawnPointRotXKey, lastSpawnRotation.x);
        PlayerPrefs.SetFloat(SpawnPointRotYKey, lastSpawnRotation.y);
        PlayerPrefs.SetFloat(SpawnPointRotZKey, lastSpawnRotation.z);
        PlayerPrefs.SetFloat(SpawnPointRotWKey, lastSpawnRotation.w);
        PlayerPrefs.Save();
    }

    private void LoadSavedSpawnPoint()
    {
        if (!PlayerPrefs.HasKey(SpawnPointXKey))
        {
            hasSavedSpawnPoint = false;
            return;
        }

        lastSpawnPosition = new Vector3(
            PlayerPrefs.GetFloat(SpawnPointXKey),
            PlayerPrefs.GetFloat(SpawnPointYKey),
            PlayerPrefs.GetFloat(SpawnPointZKey)
        );

        lastSpawnRotation = new Quaternion(
            PlayerPrefs.GetFloat(SpawnPointRotXKey),
            PlayerPrefs.GetFloat(SpawnPointRotYKey),
            PlayerPrefs.GetFloat(SpawnPointRotZKey),
            PlayerPrefs.GetFloat(SpawnPointRotWKey)
        );

        hasSavedSpawnPoint = true;
    }

    private void TeleportToLastSpawnPoint()
    {
        if (!hasSavedSpawnPoint)
        {
            Debug.LogWarning("No spawn point has been set yet.");
            return;
        }

        transform.SetPositionAndRotation(lastSpawnPosition, lastSpawnRotation);
        Debug.Log("Teleported to last spawn point.");
    }
}
