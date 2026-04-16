using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private List<Transform> _players;
    [SerializeField] private float _distance;

    public Transform ClosetPlayer;
    public Vector3 InitialPosition;
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        InitialPosition = transform.position;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").transform;
        List<GameObject> players = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
        _players = new List<Transform>();
        foreach (GameObject player in players)
        {
            _players.Add(player.transform);
        }
    }

    private void Update()
    {
        PhotonView pv = GetComponent<PhotonView>();

        // IMPORTANTE: Si no es mío (soy el cliente), no calculo nada.
        // El PhotonAnimatorView se encargará de poner el valor que mande el Máster.
        if (pv != null && !pv.IsMine) return;

        ClosetPlayer = null;
        foreach (Transform player in _players)
        {
            if (ClosetPlayer == null || Vector2.Distance(transform.position, player.position) < Vector2.Distance(transform.position, ClosetPlayer.position))
            {
                ClosetPlayer = player;
            }
        }

        if (ClosetPlayer != null)
        {
            _distance = Vector2.Distance(transform.position, ClosetPlayer.position);
            _animator.SetFloat("Distancia", _distance);
        }
    }

    public void Turn(Vector3 objetive)
    {
        if(transform.position.x < objetive.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }
    }
}
