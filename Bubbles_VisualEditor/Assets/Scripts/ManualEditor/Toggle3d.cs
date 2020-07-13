using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle3d : MonoBehaviour
{
    private bool is3d = false;
    [SerializeField] GameObject Cam2d, Cam3d;
    public void SwapMode()
    {
        is3d = !is3d;
        Cam2d.SetActive(!is3d);
        Cam3d.SetActive(is3d);
    }
}
