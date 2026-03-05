using Photon.Pun;
using UnityEngine;
using System.Linq;

public class FruitManager : MonoBehaviourPunCallbacks
{
    public static FruitManager Instance;

    private int _totalFruits = 0;
    private int _collectedFruits = 0;

    private void Awake()
    {
        Instance = this;
    }

    // Cada FruitSpawner registra cuántas frutas va a generar
    public void RegisterFruits(int count)
    {
        _totalFruits += count;
    }

    // Cada vez que se recoge una fruta, se llama este método
    public void OnFruitCollected()
    {
        if (!PhotonNetwork.IsMasterClient) return;

        _collectedFruits++;

        if (_collectedFruits >= _totalFruits)
        {
            // No quedan más frutas, calculamos el ganador
            DecideWinner();
        }
    }

    private void DecideWinner()
    {
        // Buscamos todos los PlayerScore en la escena
        PlayerScore[] scores = FindObjectsByType<PlayerScore>(FindObjectsSortMode.None);

        // Si no hay puntuaciones registradas (por ejemplo, no hay jugadores), anunciamos sin ganador
        if (scores == null || scores.Length == 0)
        {
            photonView.RPC("AnnounceWinner", RpcTarget.All, "Sin ganadores");
            return;
        }

        // Calculamos la puntuación máxima
        int maxScore = scores.Max(p => p.GetScore());

        // Tomamos todos los jugadores que tienen la puntuación máxima (podría haber empate)
        var topPlayers = scores.Where(p => p.GetScore() == maxScore).ToArray();

        string message;

        if (topPlayers.Length == 1)
        {
            // Un único ganador
            var owner = topPlayers[0].photonView.Owner;
            string winnerName = owner != null && !string.IsNullOrEmpty(owner.NickName) ? owner.NickName : "Jugador Desconocido";
            message = winnerName;
        }
        else
        {
            // Empate: construimos una lista de nombres únicos
            var names = topPlayers
                .Select(p => p.photonView.Owner)
                .Where(o => o != null)
                .Select(o => string.IsNullOrEmpty(o.NickName) ? "Jugador Desconocido" : o.NickName)
                .Distinct()
                .ToArray();

            if (names.Length == 0)
            {
                message = "Empate entre jugadores desconocidos";
            }
            else
            {
                message = "Empate: " + string.Join(", ", names);
            }
        }

        // Avisamos a todos los clientes
        photonView.RPC("AnnounceWinner", RpcTarget.All, message);
    }

    [PunRPC]
    private void AnnounceWinner(string winnerName)
    {
        GameUI.Instance.ShowWinner(winnerName);
    }
}