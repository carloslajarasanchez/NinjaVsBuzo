using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatFollow : StateMachineBehaviour
{
    [SerializeField] private float baseTime = 5f;
    [SerializeField] private float baseSpeed = 3f;
    
    private Transform _player;
    private BatController _batController;
    private float followTime;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        followTime = baseTime;
        _batController = animator.gameObject.GetComponent<BatController>();
        _player = _batController.ClosetPlayer;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PhotonView pv = animator.GetComponent<PhotonView>();

        // Solo el dueño (Master) calcula el movimiento y lo sincroniza
        if (pv != null && !pv.IsMine) return;

        animator.transform.position = Vector2.MoveTowards(animator.transform.position, _player.position, baseSpeed * Time.deltaTime);
        _batController.Turn(_player.position);
        followTime -= Time.deltaTime;
        if (followTime <= 0)
        {
            animator.SetTrigger("Volver");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
