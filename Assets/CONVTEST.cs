using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONVTEST : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        (double x, double y, double z) = CoordConv.BLH2XYZ(35.67509, 139.76392, 0.0);
        Debug.Log(""+x+" "+y+" "+z);
        (double b, double l, double h) = CoordConv.XYZ2BLH(x,y,z);
        Debug.Log(""+b+" "+l+" "+h);
    }
}
