using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Connection : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();// Nos conectamos con los parametros definidos
        PhotonNetwork.AutomaticallySyncScene = true;// Activamos la sincronizacion de escena [Necesario para el intercambio entre escenas]
    }
    // Metodo que controla si pasamos a la siguiente escena si somos mas de una persona
    private void Update()
    {
        if(PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount > 1)
        {
            // Cargamos el siguiente nivel
            PhotonNetwork.LoadLevel(1);
            Destroy(this);
        }
    }

    // Metodo para conectarse al master
    public override void OnConnectedToMaster()
    {
        print("Conectado al master");
    }

    // Metodo de conexion con el master con el boton
    public void ButtonConnect()
    {
        RoomOptions options = new RoomOptions() { MaxPlayers = 4 };
        PhotonNetwork.JoinOrCreateRoom("room1", options, TypedLobby.Default);
    }

    // Metodo de conexion con una sala
    public override void OnJoinedRoom()
    {
        Debug.Log($"Conectado a la sala {PhotonNetwork.CurrentRoom.Name}");
        Debug.Log($"Hay {PhotonNetwork.CurrentRoom.PlayerCount} jugadores");
    }


     
}
