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

    public void DecideWinner()
    {
        PlayerScore[] scores = FindObjectsByType<PlayerScore>(FindObjectsSortMode.None);
        PlayerScore winner = scores.OrderByDescending(p => p.GetScore()).First();
        string winnerName = winner.photonView.Owner.NickName;
        photonView.RPC("AnnounceWinner", RpcTarget.All, winnerName);
    }



    [PunRPC]
    private void AnnounceWinner(string winnerName)
    {
        GameUI.Instance.ShowWinner(winnerName);
    }
}