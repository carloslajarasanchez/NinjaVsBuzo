using Photon.Pun;
using UnityEngine;

public class PlayerScore : MonoBehaviourPun
{
    private int _score = 0;

    public void AddPoints(int points)
    {
        _score += points;

        // Actualizamos la UI en todos los clientes via RPC
        photonView.RPC("SyncScore", RpcTarget.All, photonView.Owner.ActorNumber, _score);
    }

    public int GetScore() => _score;

    [PunRPC]
    private void SyncScore(int actorNumber, int score)
    {
        GameUI.Instance.UpdateScore(actorNumber, score);
    }
}