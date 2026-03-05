using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager Instance;

    [SerializeField] private Transform[] spawnPoints;

    private void Awake()
    {
        Instance = this;
    }

    // Devuelve un spawn basado en el índice del jugador
    public Transform GetSpawnPoint(int playerIndex)
    {
        if (spawnPoints.Length == 0)
        {
            Debug.LogWarning("No hay spawn points asignados!");
            return transform;
        }
        // Si hay más jugadores que spawns, los reparte cíclicamente
        return spawnPoints[playerIndex % spawnPoints.Length];
    }
}
