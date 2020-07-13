
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace BubblesManualEditor
{
    public class PlaceableController : TabController
    {
        [SerializeField] private FeatureData[] FeatureOptions;
        public int selectedType = 0;
        [SerializeField] private string name;
       
      //  [SerializeField] private RectTransform UiControls, UiDrag1, Options;
        [SerializeField] private Placeable SelectedPlaceable;
        public override void HandleInput(bool tap, bool hold, float2 mousePosition)
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay((Vector2)mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 point = hit.point;
                point.y = 0;
                if (tap && !hold)
                {
                    Tap(hit, mousePosition, point);
                }
                else if (hold)
                {
                    if (SelectedPlaceable == null)
                    {
                        //UiDrag1.gameObject.SetActive(true);
                        //UiDrag1.position = new float3(mousePosition.xy, 0);
                        var go = new GameObject(name, typeof(Placeable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                        SelectedPlaceable = go.GetComponent<Placeable>();
                        SelectedPlaceable.SetFeature(FeatureOptions[selectedType]);
                        SelectedPlaceable.UpdatePosition(point);
                        if (SelectedPlaceable.data.Cutout) Blueprint.Instance.AddCutout(SelectedPlaceable);
                        else Blueprint.Instance.AddPlaceable(SelectedPlaceable);
                        
                    }
                    SelectedPlaceable.UpdatePosition(point);
                    Vector3 sPoint = Camera.main.WorldToScreenPoint(SelectedPlaceable.Position);
                    InputManger.Instance.MoveUIDrag1(sPoint);
                    InputManger.Instance.MoveUiOptions(sPoint);
                    InputManger.Instance.MoveUiRotation(sPoint);
                    
                    // Hold(hit, mousePosition, point);
                }
            }
        }
        public override void Rotate(float rot)
        {
            
            SelectedPlaceable.Rotate(rot);
        }

        public override void HandleRelease(float2 mousePosition)
        {
            //Deselect();
            //throw new System.NotImplementedException();
        }
        private void Tap(RaycastHit hit, float2 mousePosition, float3 point)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name == name)
            {
                SelectedPlaceable = hit.transform.GetComponent<Placeable>();
                //Options.gameObject.SetActive(true);
                Vector3 sPoint = Camera.main.WorldToScreenPoint(SelectedPlaceable.Position);
                InputManger.Instance.MoveUIDrag1(sPoint);
                InputManger.Instance.MoveUiOptions(sPoint);

                InputManger.Instance.MoveUiRotation(sPoint);
            }
            else if (SelectedPlaceable != null)
            {
                Deselect();


            }
            else
            {
                if (SelectedPlaceable == null)
                {
                    //UiDrag1.gameObject.SetActive(true);
                    //UiDrag1.position = new float3(mousePosition.xy, 0);
                    var go = new GameObject(name, typeof(Placeable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                    go.layer = LayerMask.NameToLayer("Placeables");
                    SelectedPlaceable = go.GetComponent<Placeable>();
                    SelectedPlaceable.SetFeature(FeatureOptions[selectedType]);
                    SelectedPlaceable.UpdatePosition(point);
                    if (SelectedPlaceable.data.Cutout) Blueprint.Instance.AddCutout(SelectedPlaceable);
                    else Blueprint.Instance.AddPlaceable(SelectedPlaceable);
                    Vector3 sPoint = Camera.main.WorldToScreenPoint(SelectedPlaceable.Position);
                    InputManger.Instance.MoveUIDrag1(sPoint);
                    InputManger.Instance.MoveUiOptions(sPoint);

                    InputManger.Instance.MoveUiRotation(sPoint);
                }
                else
                {
                    SelectedPlaceable.UpdatePosition(point);
                }
                



            }
        }
        private void Deselect()
        {
            if (SelectedPlaceable == null) return;
            if(!SelectedPlaceable.vaild)
            {
                if (SelectedPlaceable.data.Cutout) Blueprint.Instance.RemoveCutout(SelectedPlaceable);
                else Blueprint.Instance.RemovePlaceable(SelectedPlaceable);
                GameObject.Destroy(SelectedPlaceable.gameObject);
                
            }
            else if(!SelectedPlaceable.data.FreeFloating)
            {
                if (SelectedPlaceable.data.Cutout) SelectedPlaceable.Wall.AddCutout(SelectedPlaceable);
                else SelectedPlaceable.Wall.AddPlaceable(SelectedPlaceable);
            }
            SelectedPlaceable.Deselect();
            SelectedPlaceable = null;
            InputManger.Instance.DisableUI();

        }

        public override void Delete()
        {
            if (SelectedPlaceable.data.Cutout) Blueprint.Instance.RemoveCutout(SelectedPlaceable);
            else Blueprint.Instance.RemovePlaceable(SelectedPlaceable);
            GameObject.Destroy(SelectedPlaceable.gameObject);
            SelectedPlaceable = null; 
            InputManger.Instance.DisableUI();

        }
    }
}

