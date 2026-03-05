using Photon.Pun;
using Photon.Realtime;
using System.Linq;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviourPunCallbacks
{
    public static GameUI Instance;

    [SerializeField] private TMP_Text player1NameText;
    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player2NameText;
    [SerializeField] private TMP_Text player2ScoreText;
    [SerializeField] private GameObject winPanel;
    [SerializeField] private TMP_Text winText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // Inicializa los nombres de los jugadores
        Photon.Realtime.Player[] players = PhotonNetwork.PlayerList;
        if (players.Length > 0) player1NameText.text = players[0].NickName;
        if (players.Length > 1) player2NameText.text = players[1].NickName;

        winPanel.SetActive(false);
    }

    public void UpdateScore(int actorNumber, int score)
    {
        // ActorNumber empieza en 1
        if (actorNumber == PhotonNetwork.PlayerList[0].ActorNumber)
            player1ScoreText.text = score.ToString();
        else
            player2ScoreText.text = score.ToString();
    }

    public void ShowWinner(string winnerName)
    {
        winPanel.SetActive(true);
        winText.text = winnerName == PhotonNetwork.NickName
            ? "?? ¡Has ganado!"
            : $"Ha ganado {winnerName}";
    }
}