using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
namespace BubblesManualEditor
{
    public class WallController : TabController
    {
        [SerializeField] private MeshData WallMesh;
        [SerializeField] private float Snap = .1f * .1f;
      //  [SerializeField] private RectTransform UiControls, UiDrag1, UiDrag2, Options;
        [SerializeField] private CornerLogic selectedCorner1, selectedCorner2;
        [SerializeField] private WallLogic SelectedWall;
        public bool AxisAlign = true;
        public bool Continuous = true;
        private int selectedCorner1Index;
        private int selectedCorner2Index;
        [SerializeField] private CornerLogic lastCorner;

        public void SetContinuous(bool active)
        {
            Continuous = active;
        }
        public void SetNotContinuous(bool active)
        {
            Continuous = !active;
        }
        private CornerLogic CreateCorner(float3 point, out int cornerIndex)
        {
            var go = new GameObject("Corner", typeof(CornerLogic));
            //go.transform.parent = Blueprint.Instance.transform;
            var corner = go.GetComponent<CornerLogic>();
            corner.ChangePosition(point);
            Blueprint.Instance.AddCorner(corner);
            cornerIndex = Blueprint.Corners.IndexOf(corner);//can prob just do count-1 but w/e

            List<CornerLogic> corners = Blueprint.Corners;

            int close = -1;
            float min = float.MaxValue;

            for (int i = 0; i < corners.Count; i++)
            {
                if (i != cornerIndex)
                {

                    float d = MathHelper.SqrDistance(corners[i].Position, corner.Position);
                    if (d < Snap && d < min)
                    {
                        min = d;
                        close = i;
                    }
                }


            }

            if (close != -1 && close != cornerIndex)
            {
                var c = Blueprint.Corners[close];
                corner.SnapToOther(c);
            }
            return corner;
        }
        private WallLogic CreateWall()
        {
            var go = new GameObject("Wall", typeof(WallLogic));
            go.layer = LayerMask.NameToLayer("Walls");
            var wall = go.GetComponent<WallLogic>();
            wall.WallMesh = WallMesh;
            wall.SetStartCorner(selectedCorner1);
            wall.SetEndCorner(selectedCorner2);
            Blueprint.Instance.AddWall(wall);
            return wall;
        }
        
        private void MergeAndDeselect()
        {
            List<CornerLogic> corners = Blueprint.Corners;

            int close1 = -1, close2 = -1;
            float min1 = float.MaxValue;
            float min2 = float.MaxValue;
            for (int i = 0; i < corners.Count; i++)
            {
                if (i != selectedCorner1Index && i != selectedCorner2Index)
                {
                    float d1 = MathHelper.SqrDistance(corners[i].Position, selectedCorner1.Position);
                    if (d1 < Snap && d1 < min1)
                    {
                        min1 = d1;
                        close1 = i;
                    }
                    float d2 = MathHelper.SqrDistance(corners[i].Position, selectedCorner2.Position);
                    if (d2 < Snap && d2 < min2)
                    {
                        min2 = d2;
                        close2 = i;
                    }
                }


            }
            if (close1 == close2 && close1 != -1)
            {

                Blueprint.Instance.RemoveWall(SelectedWall);
                Blueprint.Instance.RemoveCorner(selectedCorner1);
                Blueprint.Instance.RemoveCorner(selectedCorner2);
            }
            else
            {
                if (close1 != -1 && close1 != selectedCorner1Index)
                {
                    var c = Blueprint.Corners[close1];
                    c.MergeOther(selectedCorner1);
                }
                if (close2 != -1 && close2 != selectedCorner2Index)
                {
                    var c = Blueprint.Corners[close2];
                    c.MergeOther(selectedCorner2);
                }
            }
            SelectedWall = null;
            selectedCorner1 = null;
            selectedCorner2 = null;
            selectedCorner1Index = -1;
            selectedCorner2Index = -1;
            InputManger.Instance.DisableUI();
        }
        private float3 MoveCorner(float3 point, CornerLogic corner, int cornerindex , CornerLogic other, int otherindex)
        {
            corner.ChangePosition(point);
            if (AxisAlign) corner.AxisAlignWithOther(other);
            List<CornerLogic> corners = Blueprint.Corners;

            int close = -1;
            float min = float.MaxValue;

            for (int i = 0; i < corners.Count; i++)
            {
                if (i != cornerindex && i != otherindex)
                {

                    float d = MathHelper.SqrDistance(corners[i].Position, corner.Position);
                    if (d < Snap && d < min)
                    {
                        min = d;
                        close = i;
                    }
                }


            }

            if (close != -1 && close != cornerindex)
            {
                var c = Blueprint.Corners[close];
                corner.SnapToOther(c);
            }



            Vector3 sPoint = Camera.main.WorldToScreenPoint(corner.Position);
            return sPoint;
        }
        public override void HandleInput(bool tap, bool hold, float2 mousePosition)
        {
            if (!tap && !hold) return;
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
                    Hold(hit, mousePosition, point);
                }
            }

        }
        public override void Delete()
        {
            Blueprint.Instance.RemoveWall(SelectedWall);
            if (selectedCorner1.GetWallsCount() == 0) Blueprint.Instance.RemoveCorner(selectedCorner1);
            if (selectedCorner2.GetWallsCount() == 0) Blueprint.Instance.RemoveCorner(selectedCorner2);

            SelectedWall = null;
            selectedCorner1 = null;
            selectedCorner2 = null;
            selectedCorner1Index = -1;
            selectedCorner2Index = -1;
            InputManger.Instance.DisableUI();
        }
        private void Hold(RaycastHit hit, float2 mousePosition, float3 point)
        {
            if (selectedCorner1 == null)
            {
                //UiDrag1.gameObject.SetActive(true);
                //UiDrag1.position = new float3(mousePosition.xy, 0);
                selectedCorner1 = CreateCorner(point, out selectedCorner1Index);
                Vector3 sPoint = Camera.main.WorldToScreenPoint(selectedCorner1.Position);
                //UiDrag1.position = sPoint;
                InputManger.Instance.MoveUIDrag1(sPoint);


            }
            else if (selectedCorner2 == null)
            {
                //UiDrag2.gameObject.SetActive(true);
                selectedCorner2 = CreateCorner(point, out selectedCorner2Index);
                Vector3 sPoint = Camera.main.WorldToScreenPoint(selectedCorner2.Position);
                InputManger.Instance.MoveUIDrag2(sPoint);
                //UiDrag2.position = sPoint;
                SelectedWall = CreateWall();
                //Options.gameObject.SetActive(true);
                if (Continuous) lastCorner = selectedCorner2;
            }
            else
            {
                float d1 = MathHelper.SqrDistance(point, selectedCorner1.Position);
                float d2 = MathHelper.SqrDistance(point, selectedCorner2.Position);
                if (d1 < d2)
                    InputManger.Instance.MoveUIDrag1(MoveCorner(point, selectedCorner1, selectedCorner1Index, selectedCorner2, selectedCorner2Index));
                else
                    InputManger.Instance.MoveUIDrag2(MoveCorner(point, selectedCorner2, selectedCorner2Index, selectedCorner1, selectedCorner1Index));
                InputManger.Instance.CenterUiOptions();
                //Options.position = (UiDrag1.position + UiDrag2.position) / 2;
            }
        }
        private void Tap(RaycastHit hit, float2 mousePosition, float3 point)
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name == "Wall")
            {
                SelectedWall = hit.transform.GetComponent<WallLogic>();
                selectedCorner1 = SelectedWall.StartCorner;
                selectedCorner2 = SelectedWall.EndCorner;
                selectedCorner1Index = Blueprint.Corners.IndexOf(selectedCorner1);
                selectedCorner2Index = Blueprint.Corners.IndexOf(selectedCorner2);

                Vector3 sPoint = Camera.main.WorldToScreenPoint(selectedCorner1.Position);
                InputManger.Instance.MoveUIDrag1(sPoint);

                sPoint = Camera.main.WorldToScreenPoint(selectedCorner2.Position);
                InputManger.Instance.MoveUIDrag2(sPoint);
                InputManger.Instance.CenterUiOptions();
                if (Continuous) lastCorner = null;
            }
            else if (SelectedWall != null)
            {
                MergeAndDeselect();


            }
            else if(Continuous&&lastCorner!=null)
            {
                selectedCorner1 = CreateCorner(lastCorner.Position, out selectedCorner1Index); 
                Vector3 sPoint = Camera.main.WorldToScreenPoint(selectedCorner1.Position);
                InputManger.Instance.MoveUIDrag1(sPoint);
                selectedCorner2 = CreateCorner(point, out selectedCorner2Index);
                if (AxisAlign) selectedCorner2.AxisAlignWithOther(selectedCorner1);
                sPoint = Camera.main.WorldToScreenPoint(selectedCorner2.Position);

                InputManger.Instance.MoveUIDrag2(sPoint);
                SelectedWall = CreateWall();
                InputManger.Instance.CenterUiOptions();

                if (Continuous) lastCorner = selectedCorner2;
            }
            else
            {
                if (selectedCorner1 == null)
                {
                   
                    selectedCorner1 = CreateCorner(point, out selectedCorner1Index);
                    Vector3 sPoint = Camera.main.WorldToScreenPoint(selectedCorner1.Position);
                    InputManger.Instance.MoveUIDrag1(sPoint);


                }
                else if (selectedCorner2 == null)
                {
                   
                    selectedCorner2 = CreateCorner(point, out selectedCorner2Index);
                    if(AxisAlign)selectedCorner2.AxisAlignWithOther(selectedCorner1);
                    Vector3 sPoint = Camera.main.WorldToScreenPoint(selectedCorner2.Position);

                    InputManger.Instance.MoveUIDrag2(sPoint);
                    SelectedWall = CreateWall();
                    InputManger.Instance.CenterUiOptions();

                    if (Continuous) lastCorner = selectedCorner2;
                }

            }
        }

        public override void HandleRelease(float2 mousePosition)
        {
            SelectedWall.Vaildate();
            //MergeAndDeselect();
        }
        public override void Rotate(float rot)
        {

        }
    }
}
