using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace BubblesManualEditor
{
    public class DragableController : TabController
    {
        [SerializeField] private FeatureData[] FeatureOptions;
        public int selectedType = 0;
        [SerializeField] private string name;
       // [SerializeField] private bool cutout;
        //  [SerializeField] private RectTransform UiControls, UiDrag1, Options;
        [SerializeField] private Dragable SelectedDragable;
        [SerializeField] private bool Next=false,Prev=false;

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
                    if (SelectedDragable == null)
                    {
                        //UiDrag1.gameObject.SetActive(true);
                        //UiDrag1.position = new float3(mousePosition.xy, 0);
                        var go = new GameObject(name, typeof(Dragable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                        SelectedDragable = go.GetComponent<Dragable>();
                        SelectedDragable.SetFeature(FeatureOptions[selectedType]);
                        SelectedDragable.UpdatePosition(point);
                        Blueprint.Instance.AddDragable(SelectedDragable);
                        

                    }
                    float d1 = MathHelper.SqrDistance(point, SelectedDragable.startPosition+ SelectedDragable.Position);
                    float d2 = MathHelper.SqrDistance(point, SelectedDragable.endPosition+ SelectedDragable.Position);
                    float d3 = MathHelper.SqrDistance(point, SelectedDragable.Position+(SelectedDragable.startPosition + SelectedDragable.endPosition)/2);
                    if (Next)
                    {
                        float t = SelectedDragable.MoveStart(point);
                        if (t < 0)
                        {
                            var nextWall = SelectedDragable.Wall.StartCorner.GetNextWall(SelectedDragable.Wall);
                            var go = new GameObject(name, typeof(Dragable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                            var oldwall = SelectedDragable.Wall;
                            SelectedDragable.Deselect();
                            SelectedDragable = go.GetComponent<Dragable>();
                            SelectedDragable.SetFeature(FeatureOptions[selectedType]);
                            SelectedDragable.SetWall(nextWall,oldwall, 0, .2f);
                           // SelectedDragable.UpdatePosition(point);
                           // SelectedDragable.FlipOffset();
                            Blueprint.Instance.AddDragable(SelectedDragable);

                        }

                    }
                    else if (Prev)
                    {
                        float t = SelectedDragable.MoveEnd(point);
                        if (t > 1)
                        {
                            var nextWall = SelectedDragable.Wall.EndCorner.GetPreviousWall(SelectedDragable.Wall);
                            var go = new GameObject(name, typeof(Dragable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                            var oldwall = SelectedDragable.Wall;
                            SelectedDragable.Deselect();
                            SelectedDragable = go.GetComponent<Dragable>();
                            SelectedDragable.SetFeature(FeatureOptions[selectedType]);
                            SelectedDragable.SetWall(nextWall,oldwall, .8f, 1f);
                           // SelectedDragable.UpdatePosition(point);
                            //SelectedDragable.FlipOffset();
                            Blueprint.Instance.AddDragable(SelectedDragable);
                        }
                    }
                    else if (d1 < d2 && d1 < d3)
                    {
                        float t = SelectedDragable.MoveStart(point);
                        if (t < 0)
                        {
                            var nextWall = SelectedDragable.Wall.StartCorner.GetNextWall(SelectedDragable.Wall);
                            var go = new GameObject(name, typeof(Dragable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                            var oldwall = SelectedDragable.Wall;
                            SelectedDragable.Deselect();
                            SelectedDragable = go.GetComponent<Dragable>();
                            SelectedDragable.SetFeature(FeatureOptions[selectedType]);
                            SelectedDragable.SetWall(nextWall,oldwall, 0, .2f);
                          //  SelectedDragable.UpdatePosition(point);
                            //SelectedDragable.FlipOffset();
                            Blueprint.Instance.AddDragable(SelectedDragable);
                            Prev = true;
                        }

                    }
                    else if (d2 < d1 && d2 < d3)
                    {
                        float t = SelectedDragable.MoveEnd(point);
                        if (t > 1)
                        {
                            var nextWall = SelectedDragable.Wall.EndCorner.GetPreviousWall(SelectedDragable.Wall);
                            var go = new GameObject(name, typeof(Dragable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                            var oldwall = SelectedDragable.Wall;
                            SelectedDragable.Deselect();
                            SelectedDragable = go.GetComponent<Dragable>();
                            SelectedDragable.SetFeature(FeatureOptions[selectedType]);
                            SelectedDragable.SetWall(nextWall, oldwall, .8f, 1f);
                            //SelectedDragable.UpdatePosition(point);
                            //SelectedDragable.FlipOffset();
                            Blueprint.Instance.AddDragable(SelectedDragable);
                            Next = true;
                        }
                    }
                    else
                    {
                        SelectedDragable.UpdatePosition(point);
                    }
                    Vector3 sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.Position+(SelectedDragable.startPosition+ SelectedDragable.endPosition)/2);
                    InputManger.Instance.MoveUiOptions(sPoint);

                    sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.startPosition+ SelectedDragable.Position);
                    InputManger.Instance.MoveUIDrag1(sPoint);

                    sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.endPosition+ SelectedDragable.Position);
                    InputManger.Instance.MoveUIDrag2(sPoint);
                }
            }
        }


        public override void HandleRelease(float2 mousePosition)
        {
          //  Deselect();
            //throw new System.NotImplementedException();
        }
        private void Tap(RaycastHit hit, float2 mousePosition, float3 point)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name == name)
            {
                SelectedDragable = hit.transform.GetComponent<Dragable>();
                //Options.gameObject.SetActive(true);
                Vector3 sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.Position + (SelectedDragable.startPosition + SelectedDragable.endPosition) / 2);
                InputManger.Instance.MoveUiOptions(sPoint);

                sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.startPosition + SelectedDragable.Position);
                InputManger.Instance.MoveUIDrag1(sPoint);

                sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.endPosition + SelectedDragable.Position);
                InputManger.Instance.MoveUIDrag2(sPoint);

            }
            else if (SelectedDragable != null)
            {
                Deselect();


            }
            else
            {
                if (SelectedDragable == null)
                {
                    //UiDrag1.gameObject.SetActive(true);
                    //UiDrag1.position = new float3(mousePosition.xy, 0);
                    var go = new GameObject(name, typeof(Dragable), typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider));
                    go.layer = LayerMask.NameToLayer("Dragables");
                    SelectedDragable = go.GetComponent<Dragable>();
                    SelectedDragable.SetFeature(FeatureOptions[selectedType]);
                    SelectedDragable.UpdatePosition(point);
                    Blueprint.Instance.AddDragable(SelectedDragable);
                    Vector3 sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.Position);
                    InputManger.Instance.MoveUiOptions(sPoint);


                    sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.startPosition + SelectedDragable.Position);
                    InputManger.Instance.MoveUIDrag1(sPoint);

                    sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.endPosition + SelectedDragable.Position);
                    InputManger.Instance.MoveUIDrag2(sPoint);
                }
                else
                {

                    float d1 = MathHelper.SqrDistance(point, SelectedDragable.startPosition + SelectedDragable.Position);
                    float d2 = MathHelper.SqrDistance(point, SelectedDragable.endPosition + SelectedDragable.Position);
                    float d3 = MathHelper.SqrDistance(point, SelectedDragable.Position + (SelectedDragable.startPosition + SelectedDragable.endPosition) / 2);
                    if (d1<d2&&d1<d3)
                    {
                        SelectedDragable.MoveStart(point);
                        
                    }
                    else if(d2 < d1 && d2 < d3)
                    {
                        SelectedDragable.MoveEnd(point);
                    }
                    else
                    {
                        SelectedDragable.UpdatePosition(point);
                    }
                    Vector3 sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.Position);
                    InputManger.Instance.MoveUiOptions(sPoint);

                    sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.startPosition + SelectedDragable.Position);
                    InputManger.Instance.MoveUIDrag1(sPoint);

                    sPoint = Camera.main.WorldToScreenPoint(SelectedDragable.endPosition + SelectedDragable.Position);
                    InputManger.Instance.MoveUIDrag2(sPoint);

                }




            }
        }

        private void Deselect()
        {
            if (SelectedDragable == null) return;
            if (!SelectedDragable.vaild)
            {
                Blueprint.Instance.RemoveDragable(SelectedDragable);
                GameObject.Destroy(SelectedDragable.gameObject);

            }
            else if (!SelectedDragable.data.FreeFloating)
            {
                Blueprint.Instance.AddDragable(SelectedDragable);
            }
            SelectedDragable.Deselect();
            SelectedDragable = null;
            InputManger.Instance.DisableUI();
            Next = false;
            Prev = false;
        }

        public override void Delete()
        {
           Blueprint.Instance.RemoveDragable(SelectedDragable);
            GameObject.Destroy(SelectedDragable.gameObject);
            SelectedDragable = null;
            InputManger.Instance.DisableUI();
            Next = false;
            Prev = false;
        }

        public override void Rotate(float rot)
        {
            
        }
    }

}
