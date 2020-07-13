// GENERATED AUTOMATICALLY FROM 'Assets/Settings/ControlMaster.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ControlMaster : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ControlMaster()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ControlMaster"",
    ""maps"": [
        {
            ""name"": ""Placement"",
            ""id"": ""92c83101-7bcd-41df-8d0c-663999b90176"",
            ""actions"": [
                {
                    ""name"": ""Tap"",
                    ""type"": ""Button"",
                    ""id"": ""9c5776a6-b5ff-419b-8245-0f6149688bfe"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Tap""
                },
                {
                    ""name"": ""Hold"",
                    ""type"": ""PassThrough"",
                    ""id"": ""6131dedb-93d0-4f64-bd1d-092fe01ba43d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Hold""
                },
                {
                    ""name"": ""Position"",
                    ""type"": ""PassThrough"",
                    ""id"": ""67516d3c-69e6-4552-a6a8-cdaa132534c4"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""fb9f28e8-4542-4f31-aca5-328e9dfef416"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Hold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e48df13b-b860-4299-8c87-37b8389b5ecc"",
                    ""path"": ""<Touchscreen>/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Hold"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c8ef23f9-71e4-43b7-8fbd-e5b92c29dac5"",
                    ""path"": ""<Mouse>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b1753962-5823-49ac-9988-d37544eb9e30"",
                    ""path"": ""<Touchscreen>/position"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Position"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c9faa8aa-dba7-4596-b620-e0cf3e2e30f1"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""cfd38e15-8d51-4128-84bf-1f09a6106a2c"",
                    ""path"": ""<Touchscreen>/primaryTouch/tap"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Tap"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""FreeLook"",
            ""id"": ""1e00d143-0d03-464f-89be-7e08b974db47"",
            ""actions"": [
                {
                    ""name"": ""Delta"",
                    ""type"": ""PassThrough"",
                    ""id"": ""e47dcbdf-5339-4136-862e-f3cd14a1e50d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Rotate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""8adfd9e2-4b7f-4a73-bfb4-a6847716d84e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Pan"",
                    ""type"": ""PassThrough"",
                    ""id"": ""431f6b40-a2e4-47e9-8f94-b673a8a9b9fd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""d7bbc357-3643-4fa7-a8fd-d079d243b817"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Delta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""16790151-ae62-4b7a-9df6-929b44bd4f32"",
                    ""path"": ""<Touchscreen>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Delta"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""74a3acb3-c3e0-430a-85c1-7d8729d5dc82"",
                    ""path"": ""<Touchscreen>/touch2/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2335d12b-cd60-4792-be64-20bac2d52e52"",
                    ""path"": ""<Mouse>/middleButton"",
                    ""interactions"": ""Press"",
                    ""processors"": """",
                    ""groups"": ""Desktop"",
                    ""action"": ""Rotate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c97024d3-2cfa-43f1-a8b6-9c0a8098daad"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f782f4e-0ebb-4e99-ae94-16b5e9f3a8b3"",
                    ""path"": ""<Touchscreen>/touch1/press"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pan"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Desktop"",
            ""bindingGroup"": ""Desktop"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Placement
        m_Placement = asset.FindActionMap("Placement", throwIfNotFound: true);
        m_Placement_Tap = m_Placement.FindAction("Tap", throwIfNotFound: true);
        m_Placement_Hold = m_Placement.FindAction("Hold", throwIfNotFound: true);
        m_Placement_Position = m_Placement.FindAction("Position", throwIfNotFound: true);
        // FreeLook
        m_FreeLook = asset.FindActionMap("FreeLook", throwIfNotFound: true);
        m_FreeLook_Delta = m_FreeLook.FindAction("Delta", throwIfNotFound: true);
        m_FreeLook_Rotate = m_FreeLook.FindAction("Rotate", throwIfNotFound: true);
        m_FreeLook_Pan = m_FreeLook.FindAction("Pan", throwIfNotFound: true);
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

    // Placement
    private readonly InputActionMap m_Placement;
    private IPlacementActions m_PlacementActionsCallbackInterface;
    private readonly InputAction m_Placement_Tap;
    private readonly InputAction m_Placement_Hold;
    private readonly InputAction m_Placement_Position;
    public struct PlacementActions
    {
        private @ControlMaster m_Wrapper;
        public PlacementActions(@ControlMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Tap => m_Wrapper.m_Placement_Tap;
        public InputAction @Hold => m_Wrapper.m_Placement_Hold;
        public InputAction @Position => m_Wrapper.m_Placement_Position;
        public InputActionMap Get() { return m_Wrapper.m_Placement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlacementActions set) { return set.Get(); }
        public void SetCallbacks(IPlacementActions instance)
        {
            if (m_Wrapper.m_PlacementActionsCallbackInterface != null)
            {
                @Tap.started -= m_Wrapper.m_PlacementActionsCallbackInterface.OnTap;
                @Tap.performed -= m_Wrapper.m_PlacementActionsCallbackInterface.OnTap;
                @Tap.canceled -= m_Wrapper.m_PlacementActionsCallbackInterface.OnTap;
                @Hold.started -= m_Wrapper.m_PlacementActionsCallbackInterface.OnHold;
                @Hold.performed -= m_Wrapper.m_PlacementActionsCallbackInterface.OnHold;
                @Hold.canceled -= m_Wrapper.m_PlacementActionsCallbackInterface.OnHold;
                @Position.started -= m_Wrapper.m_PlacementActionsCallbackInterface.OnPosition;
                @Position.performed -= m_Wrapper.m_PlacementActionsCallbackInterface.OnPosition;
                @Position.canceled -= m_Wrapper.m_PlacementActionsCallbackInterface.OnPosition;
            }
            m_Wrapper.m_PlacementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Tap.started += instance.OnTap;
                @Tap.performed += instance.OnTap;
                @Tap.canceled += instance.OnTap;
                @Hold.started += instance.OnHold;
                @Hold.performed += instance.OnHold;
                @Hold.canceled += instance.OnHold;
                @Position.started += instance.OnPosition;
                @Position.performed += instance.OnPosition;
                @Position.canceled += instance.OnPosition;
            }
        }
    }
    public PlacementActions @Placement => new PlacementActions(this);

    // FreeLook
    private readonly InputActionMap m_FreeLook;
    private IFreeLookActions m_FreeLookActionsCallbackInterface;
    private readonly InputAction m_FreeLook_Delta;
    private readonly InputAction m_FreeLook_Rotate;
    private readonly InputAction m_FreeLook_Pan;
    public struct FreeLookActions
    {
        private @ControlMaster m_Wrapper;
        public FreeLookActions(@ControlMaster wrapper) { m_Wrapper = wrapper; }
        public InputAction @Delta => m_Wrapper.m_FreeLook_Delta;
        public InputAction @Rotate => m_Wrapper.m_FreeLook_Rotate;
        public InputAction @Pan => m_Wrapper.m_FreeLook_Pan;
        public InputActionMap Get() { return m_Wrapper.m_FreeLook; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(FreeLookActions set) { return set.Get(); }
        public void SetCallbacks(IFreeLookActions instance)
        {
            if (m_Wrapper.m_FreeLookActionsCallbackInterface != null)
            {
                @Delta.started -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnDelta;
                @Delta.performed -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnDelta;
                @Delta.canceled -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnDelta;
                @Rotate.started -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnRotate;
                @Rotate.performed -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnRotate;
                @Rotate.canceled -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnRotate;
                @Pan.started -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnPan;
                @Pan.performed -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnPan;
                @Pan.canceled -= m_Wrapper.m_FreeLookActionsCallbackInterface.OnPan;
            }
            m_Wrapper.m_FreeLookActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Delta.started += instance.OnDelta;
                @Delta.performed += instance.OnDelta;
                @Delta.canceled += instance.OnDelta;
                @Rotate.started += instance.OnRotate;
                @Rotate.performed += instance.OnRotate;
                @Rotate.canceled += instance.OnRotate;
                @Pan.started += instance.OnPan;
                @Pan.performed += instance.OnPan;
                @Pan.canceled += instance.OnPan;
            }
        }
    }
    public FreeLookActions @FreeLook => new FreeLookActions(this);
    private int m_DesktopSchemeIndex = -1;
    public InputControlScheme DesktopScheme
    {
        get
        {
            if (m_DesktopSchemeIndex == -1) m_DesktopSchemeIndex = asset.FindControlSchemeIndex("Desktop");
            return asset.controlSchemes[m_DesktopSchemeIndex];
        }
    }
    public interface IPlacementActions
    {
        void OnTap(InputAction.CallbackContext context);
        void OnHold(InputAction.CallbackContext context);
        void OnPosition(InputAction.CallbackContext context);
    }
    public interface IFreeLookActions
    {
        void OnDelta(InputAction.CallbackContext context);
        void OnRotate(InputAction.CallbackContext context);
        void OnPan(InputAction.CallbackContext context);
    }
}
