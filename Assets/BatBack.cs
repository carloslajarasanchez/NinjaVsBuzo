using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatBack : StateMachineBehaviour
{
    [SerializeField] private float _baseSpeed = 1f;
    private Vector3 _initialPositon;
    private BatController _batController;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _batController = animator.gameObject.GetComponent<BatController>();
        _initialPositon = _batController.InitialPosition;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    [PunRPC]
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PhotonView pv = animator.GetComponent<PhotonView>();

        // Solo el dueño (Master) calcula el movimiento y lo sincroniza
        if (pv != null && !pv.IsMine) return;

        animator.transform.position = Vector2.MoveTowards(animator.transform.position, _initialPositon, _baseSpeed * Time.deltaTime);
        _batController.Turn(_initialPositon);
        if(Vector2.Distance(animator.transform.position, _initialPositon) < 0.01f)
        {
            animator.transform.position = _initialPositon;
            animator.SetTrigger("Llego");
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
