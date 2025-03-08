using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputsManager : MonoBehaviour
{
    private InputSystem_Actions m_inputActions { get; set; }
    [SerializeField] PlayerInput m_playerInput;
    private static UserInputsManager m_instance { get; set; }
    public static UserInputsManager Instance => m_instance;

    public bool ShootInput { get; private set; }
    public bool PauseInput { get; private set; }
    
    public bool UINavigate { get; private set; }
    
    private void Awake()
    {
        if (m_instance == null)
        {
            m_instance = this;
        }

        m_inputActions = new InputSystem_Actions();
        m_inputActions.Enable();
    }

    private void Start()
    {
        ScreenManager.Instance.OnPause += OnPause;
        ScreenManager.Instance.OnResume += OnResume;
    }

    private void Update()
    {
        ShootInput = m_inputActions.Player.Attack.WasPressedThisFrame();
        PauseInput = m_inputActions.Player.Pause.WasPressedThisFrame();
        UINavigate = m_inputActions.UI.Navigate.WasPerformedThisFrame();
    }

    void OnPause()
    {
        m_inputActions.Player.Disable();
        m_playerInput.SwitchCurrentActionMap("UI");
    }

    void OnResume()
    {
        m_inputActions.Player.Enable();
        m_playerInput.SwitchCurrentActionMap("Player");
    }
    
    
    public Vector2 GetMovementInput()
    {
        return m_inputActions.Player.Move.ReadValue<Vector2>();
    }
    
    private void OnDisable()
    {
        ScreenManager.Instance.OnPause -= OnPause;
        ScreenManager.Instance.OnResume -= OnResume;
        
        m_inputActions.Disable();
    }
}