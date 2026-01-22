using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]private float _speed;
    [SerializeField]private float _jumpForce = 200f;
    [SerializeField]private float _raycastSize = .1f;
    private bool _isGrounded = false;
    private Rigidbody2D _rigidbody2D;
    private Animator _animator;

    public string NickName;

    public float Speed { get { return _speed; } private set { _speed = value; } }
    public Rigidbody2D Rigidbody2D { get { return _rigidbody2D; } private set { _rigidbody2D = value; } }

    // Start is called before the first frame update
    void Awake()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();

            Camera.main.transform.SetParent(transform);
            Camera.main.transform.position = transform.position + transform.forward * -10;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<PhotonView>().IsMine)
        {

            Move();
            Jump();
            UpdateAnimations();
        }
        
    }

    [PunRPC]
    public void RotateSprite(bool rotate)
    {
        Debug.Log("Rotation: " + rotate);
        GetComponent<SpriteRenderer>().flipX = rotate;
    }

    private void Move()
    {
        _rigidbody2D.velocity = (transform.right * Speed * Input.GetAxis("Horizontal")) + (transform.up * _rigidbody2D.velocity.y);

        if (_rigidbody2D.velocity.x > 0.1f && GetComponent<SpriteRenderer>().flipX)
        {
            GetComponent<PhotonView>().RPC("RotateSprite", RpcTarget.All, false);
        }
        else if (_rigidbody2D.velocity.x < -0.1f && !GetComponent<SpriteRenderer>().flipX)
        {
            GetComponent<PhotonView>().RPC("RotateSprite", RpcTarget.All, true);
        }
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            if (Input.GetButtonDown("Jump"))
            {
                _rigidbody2D.AddForce(transform.up * _jumpForce);
                _isGrounded = false;
            }
        }
        
    }

    private void UpdateAnimations()
    {
        _animator.SetFloat("velocityX", Mathf.Abs(_rigidbody2D.velocity.x));
        _animator.SetFloat("velocityY", _rigidbody2D.velocity.y);
    }

    private bool IsGrounded()
    {
        Debug.DrawRay(transform.position, Vector2.down * _raycastSize, Color.green);
        Debug.DrawRay(transform.position + new Vector3(.05f, 0, 0), Vector2.down * _raycastSize, Color.green);
        Debug.DrawRay(transform.position + new Vector3(-.05f, 0, 0), Vector2.down * _raycastSize, Color.green);

        return Physics2D.Raycast(transform.position, Vector2.down, _raycastSize, LayerMask.GetMask("Ground")) ||
            Physics2D.Raycast(transform.position + new Vector3(.05f, 0, 0), Vector2.down, _raycastSize, LayerMask.GetMask("Ground")) ||
            Physics2D.Raycast(transform.position + new Vector3(-.05f, 0, 0), Vector2.down, _raycastSize, LayerMask.GetMask("Ground"));
    }

}

