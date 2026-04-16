using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance;

    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public override void OnDisable()
    {
        base.OnDisable();
        SceneManager.sceneLoaded -= OnSceneLoaded; 
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log($"Escena cargada: {scene.name}, buildIndex: {scene.buildIndex}");

        if (scene.buildIndex == 1)
        {
            StartCoroutine(SpawnPlayerWhenReady());
        }
    }

    private IEnumerator SpawnPlayerWhenReady()
    {
        // Espera hasta que el SpawnManager exista en la escena
        float timeout = 5f; // segundos máximo de espera
        float elapsed = 0f;

        while (SpawnManager.Instance == null)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= timeout)
            {
                Debug.LogError("SpawnManager no encontrado tras esperar 5 segundos!");
                yield break;
            }
            yield return null;
        }

        int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(playerIndex);
        string prefabName = PhotonNetwork.IsMasterClient ? "Frog-2" : "Frog-3";
        PhotonNetwork.Instantiate(prefabName, spawnPoint.position, spawnPoint.rotation);
    }
}
