//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.3.0
//     from Assets/PlayerControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerControl : IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerControl"",
    ""maps"": [
        {
            ""name"": ""Dreamworld"",
            ""id"": ""20da467f-a79f-4b8b-a2ca-292f0899c181"",
            ""actions"": [
                {
                    ""name"": ""Dash"",
                    ""type"": ""Button"",
                    ""id"": ""53a15f5d-e384-4b0b-a6df-fcff38485d9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Reload"",
                    ""type"": ""Button"",
                    ""id"": ""b408be3e-9016-4b53-9224-32537044ca74"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""f8b6ff4f-b4d0-4a27-a80c-a91c00c0315d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""8c3c4cf7-3033-4184-8ad8-8c10ac2e421d"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b48ea1e3-0a78-4739-9923-ecd7a46f1d54"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""bf51f114-8042-4b2b-9d74-77573c7086b4"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Dash"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""81e1b373-fb9f-4a19-a617-0709d9e3b870"",
                    ""path"": ""<Keyboard>/r"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Reload"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83947ae5-aaf9-40a3-8ced-d516de6ef9ae"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Texting"",
            ""id"": ""5d561337-b3e0-4029-9acc-e0694808030c"",
            ""actions"": [
                {
                    ""name"": ""Continue"",
                    ""type"": ""Button"",
                    ""id"": ""384b85f8-a65b-46d5-af12-4ba84b8a58f4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""815f1faa-4628-42b1-96d4-7f6ee5ad4fe0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""451bb8d0-54e3-4a75-a362-374bdacf1350"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""43f6809b-7c0c-4e98-964c-97e71b5e652f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""918e3a96-746b-477b-9cb4-7593455cb5bb"",
                    ""path"": ""<Touchscreen>/Press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Continue"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0fa30ce1-6e3b-4d94-b54a-be8ec1b79097"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Universal"",
            ""id"": ""d818afec-a6a4-47fe-8b98-3c420c8808a4"",
            ""actions"": [
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""c1737a55-a5ae-4ee3-90b0-fcdbee2cff29"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""201462d0-8697-4771-9b62-0bc3db1ed6ad"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Dreamworld
        m_Dreamworld = asset.FindActionMap("Dreamworld", throwIfNotFound: true);
        m_Dreamworld_Dash = m_Dreamworld.FindAction("Dash", throwIfNotFound: true);
        m_Dreamworld_Reload = m_Dreamworld.FindAction("Reload", throwIfNotFound: true);
        m_Dreamworld_Pause = m_Dreamworld.FindAction("Pause", throwIfNotFound: true);
        // Texting
        m_Texting = asset.FindActionMap("Texting", throwIfNotFound: true);
        m_Texting_Continue = m_Texting.FindAction("Continue", throwIfNotFound: true);
        m_Texting_Pause = m_Texting.FindAction("Pause", throwIfNotFound: true);
        // Universal
        m_Universal = asset.FindActionMap("Universal", throwIfNotFound: true);
        m_Universal_Pause = m_Universal.FindAction("Pause", throwIfNotFound: true);
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
    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }
    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Dreamworld
    private readonly InputActionMap m_Dreamworld;
    private IDreamworldActions m_DreamworldActionsCallbackInterface;
    private readonly InputAction m_Dreamworld_Dash;
    private readonly InputAction m_Dreamworld_Reload;
    private readonly InputAction m_Dreamworld_Pause;
    public struct DreamworldActions
    {
        private @PlayerControl m_Wrapper;
        public DreamworldActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Dash => m_Wrapper.m_Dreamworld_Dash;
        public InputAction @Reload => m_Wrapper.m_Dreamworld_Reload;
        public InputAction @Pause => m_Wrapper.m_Dreamworld_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Dreamworld; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DreamworldActions set) { return set.Get(); }
        public void SetCallbacks(IDreamworldActions instance)
        {
            if (m_Wrapper.m_DreamworldActionsCallbackInterface != null)
            {
                @Dash.started -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnDash;
                @Dash.performed -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnDash;
                @Dash.canceled -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnDash;
                @Reload.started -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnReload;
                @Reload.performed -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnReload;
                @Reload.canceled -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnReload;
                @Pause.started -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_DreamworldActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_DreamworldActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Dash.started += instance.OnDash;
                @Dash.performed += instance.OnDash;
                @Dash.canceled += instance.OnDash;
                @Reload.started += instance.OnReload;
                @Reload.performed += instance.OnReload;
                @Reload.canceled += instance.OnReload;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public DreamworldActions @Dreamworld => new DreamworldActions(this);

    // Texting
    private readonly InputActionMap m_Texting;
    private ITextingActions m_TextingActionsCallbackInterface;
    private readonly InputAction m_Texting_Continue;
    private readonly InputAction m_Texting_Pause;
    public struct TextingActions
    {
        private @PlayerControl m_Wrapper;
        public TextingActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Continue => m_Wrapper.m_Texting_Continue;
        public InputAction @Pause => m_Wrapper.m_Texting_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Texting; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(TextingActions set) { return set.Get(); }
        public void SetCallbacks(ITextingActions instance)
        {
            if (m_Wrapper.m_TextingActionsCallbackInterface != null)
            {
                @Continue.started -= m_Wrapper.m_TextingActionsCallbackInterface.OnContinue;
                @Continue.performed -= m_Wrapper.m_TextingActionsCallbackInterface.OnContinue;
                @Continue.canceled -= m_Wrapper.m_TextingActionsCallbackInterface.OnContinue;
                @Pause.started -= m_Wrapper.m_TextingActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_TextingActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_TextingActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_TextingActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Continue.started += instance.OnContinue;
                @Continue.performed += instance.OnContinue;
                @Continue.canceled += instance.OnContinue;
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public TextingActions @Texting => new TextingActions(this);

    // Universal
    private readonly InputActionMap m_Universal;
    private IUniversalActions m_UniversalActionsCallbackInterface;
    private readonly InputAction m_Universal_Pause;
    public struct UniversalActions
    {
        private @PlayerControl m_Wrapper;
        public UniversalActions(@PlayerControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Pause => m_Wrapper.m_Universal_Pause;
        public InputActionMap Get() { return m_Wrapper.m_Universal; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UniversalActions set) { return set.Get(); }
        public void SetCallbacks(IUniversalActions instance)
        {
            if (m_Wrapper.m_UniversalActionsCallbackInterface != null)
            {
                @Pause.started -= m_Wrapper.m_UniversalActionsCallbackInterface.OnPause;
                @Pause.performed -= m_Wrapper.m_UniversalActionsCallbackInterface.OnPause;
                @Pause.canceled -= m_Wrapper.m_UniversalActionsCallbackInterface.OnPause;
            }
            m_Wrapper.m_UniversalActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Pause.started += instance.OnPause;
                @Pause.performed += instance.OnPause;
                @Pause.canceled += instance.OnPause;
            }
        }
    }
    public UniversalActions @Universal => new UniversalActions(this);
    public interface IDreamworldActions
    {
        void OnDash(InputAction.CallbackContext context);
        void OnReload(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface ITextingActions
    {
        void OnContinue(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
    }
    public interface IUniversalActions
    {
        void OnPause(InputAction.CallbackContext context);
    }
}
