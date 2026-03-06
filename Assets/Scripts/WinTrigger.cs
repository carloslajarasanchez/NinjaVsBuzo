using Photon.Pun;
using UnityEngine;

public class WinTrigger : MonoBehaviourPun
{
    [SerializeField] private int bonusPoints = 200;

    private bool _activated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Solo el primer jugador que llegue activa el trigger
        if (_activated) return;

        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            _activated = true;

            // Suma los 200 puntos al jugador que llegó primero
            other.GetComponent<PlayerScore>().AddPoints(bonusPoints);

            // Avisamos a todos que se active la condición de victoria
            photonView.RPC("TriggerWin", RpcTarget.MasterClient);
        }
    }

    [PunRPC]
    private void TriggerWin()
    {
        FruitManager.Instance.DecideWinner();
    }
}