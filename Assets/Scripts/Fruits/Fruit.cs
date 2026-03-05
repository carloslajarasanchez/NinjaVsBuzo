using Photon.Pun;
using UnityEngine;

public class Fruit : MonoBehaviourPun
{
    [SerializeField] private int pointValue = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Comprobamos que es un jugador y que es el nuestro (evita doble recolección)
        if (other.CompareTag("Player") && other.GetComponent<PhotonView>().IsMine)
        {
            // Sumamos puntos al jugador local
            other.GetComponent<PlayerScore>().AddPoints(pointValue);

            // Destruimos la fruta en todos los clientes
            photonView.RPC("DestroyFruit", RpcTarget.All);
        }
    }

    [PunRPC]
    private void DestroyFruit()
    {
        // Solo el MasterClient puede destruir objetos de red
        if (PhotonNetwork.IsMasterClient)
        {
            FruitManager.Instance.OnFruitCollected();
            PhotonNetwork.Destroy(gameObject);
        }
        else
        {
            // El resto la ocultan mientras el Master la destruye
            gameObject.SetActive(false);
        }
    }
}