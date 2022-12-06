// GENERATED AUTOMATICALLY FROM 'Assets/PlayerControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @PlayerControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControls"",
    ""maps"": [
        {
            ""name"": ""Gamplay"",
            ""id"": ""383b8479-b1dd-473f-bf97-192297eba67f"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""bc6787a0-cf5c-4248-a2b6-2288c89fd823"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""LookAround"",
                    ""type"": ""Value"",
                    ""id"": ""63fa9832-d55f-4dd8-9db6-66624c8d2a7a"",
                    ""expectedControlType"": ""Stick"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""a35968ca-b716-449f-829a-181839cca8c2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""bf22cfa7-9c81-48d5-9c1a-3a22e64c8327"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""72918cb8-6718-473f-a72f-db386327dcd8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""cca8d591-4867-428b-a721-405e638e57fa"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f0507fa2-99ae-4654-926a-3bd759a4bb10"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e3e42228-928a-465a-b392-95870c76067f"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d783c554-f556-49b0-810a-5b34309e2ed4"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""LookAround"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8198f6a8-d90a-484d-961d-5b12fccdedde"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Gamplay
        m_Gamplay = asset.FindActionMap("Gamplay", throwIfNotFound: true);
        m_Gamplay_Move = m_Gamplay.FindAction("Move", throwIfNotFound: true);
        m_Gamplay_LookAround = m_Gamplay.FindAction("LookAround", throwIfNotFound: true);
        m_Gamplay_Jump = m_Gamplay.FindAction("Jump", throwIfNotFound: true);
        m_Gamplay_Run = m_Gamplay.FindAction("Run", throwIfNotFound: true);
        m_Gamplay_Attack = m_Gamplay.FindAction("Attack", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Gamplay
    private readonly InputActionMap m_Gamplay;
    private IGamplayActions m_GamplayActionsCallbackInterface;
    private readonly InputAction m_Gamplay_Move;
    private readonly InputAction m_Gamplay_LookAround;
    private readonly InputAction m_Gamplay_Jump;
    private readonly InputAction m_Gamplay_Run;
    private readonly InputAction m_Gamplay_Attack;
    public struct GamplayActions
    {
        private @PlayerControls m_Wrapper;
        public GamplayActions(@PlayerControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Gamplay_Move;
        public InputAction @LookAround => m_Wrapper.m_Gamplay_LookAround;
        public InputAction @Jump => m_Wrapper.m_Gamplay_Jump;
        public InputAction @Run => m_Wrapper.m_Gamplay_Run;
        public InputAction @Attack => m_Wrapper.m_Gamplay_Attack;
        public InputActionMap Get() { return m_Wrapper.m_Gamplay; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(GamplayActions set) { return set.Get(); }
        public void SetCallbacks(IGamplayActions instance)
        {
            if (m_Wrapper.m_GamplayActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_GamplayActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_GamplayActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_GamplayActionsCallbackInterface.OnMove;
                @LookAround.started -= m_Wrapper.m_GamplayActionsCallbackInterface.OnLookAround;
                @LookAround.performed -= m_Wrapper.m_GamplayActionsCallbackInterface.OnLookAround;
                @LookAround.canceled -= m_Wrapper.m_GamplayActionsCallbackInterface.OnLookAround;
                @Jump.started -= m_Wrapper.m_GamplayActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_GamplayActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_GamplayActionsCallbackInterface.OnJump;
                @Run.started -= m_Wrapper.m_GamplayActionsCallbackInterface.OnRun;
                @Run.performed -= m_Wrapper.m_GamplayActionsCallbackInterface.OnRun;
                @Run.canceled -= m_Wrapper.m_GamplayActionsCallbackInterface.OnRun;
                @Attack.started -= m_Wrapper.m_GamplayActionsCallbackInterface.OnAttack;
                @Attack.performed -= m_Wrapper.m_GamplayActionsCallbackInterface.OnAttack;
                @Attack.canceled -= m_Wrapper.m_GamplayActionsCallbackInterface.OnAttack;
            }
            m_Wrapper.m_GamplayActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @LookAround.started += instance.OnLookAround;
                @LookAround.performed += instance.OnLookAround;
                @LookAround.canceled += instance.OnLookAround;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @Run.started += instance.OnRun;
                @Run.performed += instance.OnRun;
                @Run.canceled += instance.OnRun;
                @Attack.started += instance.OnAttack;
                @Attack.performed += instance.OnAttack;
                @Attack.canceled += instance.OnAttack;
            }
        }
    }
    public GamplayActions @Gamplay => new GamplayActions(this);
    public interface IGamplayActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnLookAround(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
    }
}
