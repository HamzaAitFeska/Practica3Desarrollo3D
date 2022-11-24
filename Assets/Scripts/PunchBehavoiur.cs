using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBehavoiur : StateMachineBehaviour
{
    MarioPlayerController m_MarioPlayerController;
    public float m_StartPctTime = 0.3f;
    public float m_EndPctTime = 0.3f;
    public MarioPlayerController.TPunchType m_PunchType;
    bool m_PunchActive = false;

    // OnStateEnter is called before OnStateEnter is called on any state inside this state machine
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MarioPlayerController = animator.GetComponent<MarioPlayerController>();
        m_MarioPlayerController.SetPunchActive(m_PunchType, false);
        m_PunchActive = false;
    }

    // OnStateUpdate is called before OnStateUpdate is called on any state inside this state machine
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if(!m_PunchActive && stateInfo.normalizedTime>=m_StartPctTime && stateInfo.normalizedTime <= m_EndPctTime)
       {
           m_MarioPlayerController.SetPunchActive(m_PunchType, true);
           m_PunchActive = true;
            
       }
       else if(m_PunchActive && stateInfo.normalizedTime > m_EndPctTime)
       {
            m_MarioPlayerController.SetPunchActive(m_PunchType, false);
            m_PunchActive = false;
        }
    }

    // OnStateExit is called before OnStateExit is called on any state inside this state machine
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_MarioPlayerController.SetPunchActive(m_PunchType, false);
        m_MarioPlayerController.SetIsPunchEnable(false);
    }

    // OnStateMove is called before OnStateMove is called on any state inside this state machine
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateIK is called before OnStateIK is called on any state inside this state machine
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMachineEnter is called when entering a state machine via its Entry Node
    //override public void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}

    // OnStateMachineExit is called when exiting a state machine via its Exit Node
    //override public void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    //{
    //    
    //}
}
