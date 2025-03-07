using System;
using UnityEngine;

public class UserInputsManager : MonoBehaviour
{
    InputSystem_Actions m_inputActions;
    private static UserInputsManager m_instance;
    public static UserInputsManager Instance => m_instance;
    
    public bool ShootInput { get; private set; }

    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }

        m_inputActions = new InputSystem_Actions();
        m_inputActions.Enable();
    }

    private void Update()
    {
        ShootInput = m_inputActions.Player.Attack.WasPressedThisFrame();
    }

    public Vector2 GetMovementInput()
    {
        return m_inputActions.Player.Move.ReadValue<Vector2>();
    }
}