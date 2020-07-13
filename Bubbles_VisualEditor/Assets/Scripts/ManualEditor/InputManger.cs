using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UIAddons;
using Unity.Mathematics;

namespace BubblesManualEditor
{
    public class InputManger : Singleton<InputManger>
    {
        //just some debug stuff
        //public Camera cam;
        //private GUIStyle guiStyle = new GUIStyle();
        //void Start()
        //{
        //    guiStyle.fontSize = 36;
        //    cam = Camera.main;
        //}
        //void OnGUI()
        //{
           
        //    Vector3 point = new Vector3();
        //    Event currentEvent = Event.current;
        //    Vector2 mousePos = new Vector2();

        //    // Get the mouse position from Event.
        //    // Note that the y position from Event is inverted.
        //    mousePos.x = currentEvent.mousePosition.x;
        //    mousePos.y = cam.pixelHeight - mousePosition.y;

        //    point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam));

        //    GUILayout.BeginArea(new Rect(20, 400, 800, 800));
            
        //    GUILayout.Label("Screen pixels: " + cam.pixelWidth + ":" + cam.pixelHeight, guiStyle);
        //    GUILayout.Label("Mouse position: " + mousePos, guiStyle);
        //    GUILayout.Label("World position: " + point.ToString("F3"), guiStyle);
        //    GUILayout.EndArea();
        //}

        [SerializeField] private TabGroup tabGroup;
        [SerializeField] private Transform CameraHolder;
        [SerializeField] private List<TabController> tabControllers = new List<TabController>();
        [SerializeField] private RectTransform UiDrag1, UiDrag2, UiOptions, UiRotation;

        //controls
        private ControlMaster inputActions;
        private bool tap, holding, RotateCam,Pan;
        private float2 mousePosition;
        private float2 CameraMovement;
        public float CameraRotationSpeed = .1f;
        private void Awake()
        {

            inputActions = new ControlMaster();

            inputActions.Placement.Tap.performed += ctx => tap = ctx.performed && !EventSystem.current.IsPointerOverGameObject();
            //inputActions.Placement.Hold.performed += ctx => tap = ctx.performed && !EventSystem.current.IsPointerOverGameObject();
            inputActions.Placement.Hold.performed += ctx => holding = ctx.performed && !EventSystem.current.IsPointerOverGameObject();
            inputActions.Placement.Hold.canceled += ctx => { holding = ctx.performed; OnRelease(); };
            inputActions.Placement.Position.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
            inputActions.FreeLook.Delta.performed += ctx => CameraMovement = ctx.ReadValue<Vector2>() * CameraRotationSpeed;
            inputActions.FreeLook.Rotate.performed += ctx => RotateCam = ctx.performed && !EventSystem.current.IsPointerOverGameObject();
            inputActions.FreeLook.Rotate.canceled += ctx => RotateCam = ctx.performed;
            inputActions.FreeLook.Pan.performed += ctx => Pan = ctx.performed && !EventSystem.current.IsPointerOverGameObject();
            inputActions.FreeLook.Pan.canceled += ctx => Pan = ctx.performed;
            Cinemachine.CinemachineCore.GetInputAxis = GetAxisCustom;
        }
        private void OnEnable()
        {
            inputActions.Enable();
        }
        private void OnDisable()
        {
            inputActions.Disable();
        }
        // Update is called once per frame
        void Update()
        {
            int selected = tabGroup.SelectedTab - 1;
            //Debug.Log("click "+ selected);
            if(RotateCam)
            {

            }
            else if (Pan)
            {
                CameraHolder.position = new Vector3(CameraHolder.position.x + CameraMovement.x, 0, CameraHolder.position.z + CameraMovement.y);
            }
            else if (selected >= 0 && (tap || holding))
            {
              //  Debug.Log("update");
                tabControllers[selected].HandleInput(tap, holding, mousePosition);
                
            }
            tap = false;

        }
        public void Delete()
        {
            int selected = tabGroup.SelectedTab - 1;
            tabControllers[selected].Delete();
        }
        public void OnTabChange()
        {
            int selected = tabGroup.SelectedTab - 1;
            tabControllers[selected].HandleRelease(float2.zero);
        }
        private void OnRelease()
        {
            int selected = tabGroup.SelectedTab - 1;
            if (selected >= 0)
            tabControllers[selected].HandleRelease(mousePosition);
        }
        public float GetAxisCustom(string axisName)
        {
            if (!RotateCam) return 0;
            if (axisName == "Mouse X")
            {
                return CameraMovement.x;
            }
            else if (axisName == "Mouse Y")
            {
                return CameraMovement.y;
            }
            return 0;
        }
        public void Rotate(float rot)
        {
            Debug.Log(rot);
            int selected = tabGroup.SelectedTab - 1;
            if (selected >= 0)
                tabControllers[selected].Rotate(-UiRotation.eulerAngles.z);
        }
       
        public void DisableUI()
        {
            UiDrag1.gameObject.SetActive(false);
            UiDrag2.gameObject.SetActive(false);
            UiOptions.gameObject.SetActive(false);
            UiRotation.gameObject.SetActive(false);
        }
        public void MoveUIDrag1(float3 position)
        {
            UiDrag1.gameObject.SetActive(true);
            UiDrag1.position = position;
        }
        public void MoveUIDrag2(float3 position)
        {
            UiDrag2.gameObject.SetActive(true);
            UiDrag2.position = position;
        }
        public void MoveUiOptions(float3 position)
        {
            UiOptions.gameObject.SetActive(true);
            UiOptions.position = position;
        }
        public void MoveUiRotation(float3 position)
        {
            UiRotation.gameObject.SetActive(true);
            UiRotation.position = position;
        }
        public void CenterUiOptions()
        {
            UiOptions.gameObject.SetActive(true);
            UiOptions.position = (UiDrag1.position + UiDrag2.position) / 2;
        }
    }
}