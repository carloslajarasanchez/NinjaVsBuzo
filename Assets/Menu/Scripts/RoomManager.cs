using Photon.Pun;
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
        if (scene.buildIndex == 1)
        {
            // Obtenemos el índice de este jugador en la sala (0, 1, 2...)
            int playerIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1;

            Transform spawnPoint = SpawnManager.Instance.GetSpawnPoint(playerIndex);

            string prefabName = PhotonNetwork.IsMasterClient ? "Frog-2" : "VirtualGuy";
            PhotonNetwork.Instantiate(prefabName, spawnPoint.position, spawnPoint.rotation);
        }
    }
}
