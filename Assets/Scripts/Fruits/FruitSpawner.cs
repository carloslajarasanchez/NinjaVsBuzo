using Photon.Pun;
using UnityEngine;

public class FruitSpawner : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject fruitPrefab; // Tu prefab de fruta
    [SerializeField] private int fruitCount = 10;    // Cuántas frutas spawnear
    [SerializeField] private Vector2 areaMin;        // Esquina inferior izquierda del área
    [SerializeField] private Vector2 areaMax;        // Esquina superior derecha del área

    private void Start()
    {
        Debug.Log($"FruitSpawner: IsMasterClient = {PhotonNetwork.IsMasterClient}");
        Debug.Log($"FruitSpawner: IsConnected = {PhotonNetwork.IsConnected}");

        if (PhotonNetwork.IsMasterClient)
        {
            FruitManager.Instance.RegisterFruits(fruitCount);
            SpawnFruits();
        }
    }

    private void SpawnFruits()
    {
        Debug.Log($"Intentando spawnear {fruitCount} frutas");
        for (int i = 0; i < fruitCount; i++)
        {
            Vector3 randomPos = new Vector3(
                Random.Range(areaMin.x, areaMax.x),
                Random.Range(areaMin.y, areaMax.y),
                0f
            );
            Debug.Log($"Spawneando fruta en {randomPos}");
            PhotonNetwork.Instantiate(fruitPrefab.name, randomPos, Quaternion.identity);
        }
    }

    // Visualiza el área en el editor para colocarla fácilmente
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 center = new Vector3((areaMin.x + areaMax.x) / 2, (areaMin.y + areaMax.y) / 2, 0);
        Vector3 size = new Vector3(areaMax.x - areaMin.x, areaMax.y - areaMin.y, 0);
        Gizmos.DrawWireCube(center, size);
    }
}