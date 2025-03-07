using JetBrains.Annotations;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Animator m_anim;
    [SerializeField] private PlayerController m_playerController;
    readonly int m_goingRight = Animator.StringToHash("GoingRight");
    readonly int m_goingLeft = Animator.StringToHash("GoingLeft");
    readonly int m_stopMovingSide = Animator.StringToHash("StopMovingSide");

    private void Awake()
    {
        m_playerController.OnGoingRightChanged += OnGoingRightChanged;
        m_playerController.OnIsMovingChanged += OnIsMovingChanged;
    }

    private void OnGoingRightChanged(bool goingRight)
    {
        m_anim.SetTrigger(!goingRight ? m_goingLeft : m_goingRight);
    }
    private void OnIsMovingChanged(bool isMoving)
    {
        m_anim.SetBool(m_stopMovingSide, !isMoving);
    }
}