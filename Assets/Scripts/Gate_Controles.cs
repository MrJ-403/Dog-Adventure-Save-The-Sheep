using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate_Controles : MonoBehaviour
{
    public GameObject handle;

    public void Rotate(float angle)
    {
        gameObject.transform.RotateAround(handle.transform.position, Vector3.forward, angle);
    }
}
