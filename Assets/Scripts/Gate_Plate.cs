using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate_Plate : MonoBehaviour
{
    public GameObject gate;
    public GameObject plate;
    public float angle;
    public float speed;
    private Quaternion startAngle;
    private bool isOpen = false;

    private void Start()
    {
        startAngle = gate.transform.rotation;
    }

    private void Update()
    {
        if (plate.GetComponent<PressurePlate>().isPressed && !isOpen)
        {
            if(Mathf.Abs(Quaternion.Angle(startAngle, gate.transform.rotation) - Mathf.Abs(angle)) <= 1 || Quaternion.Angle(startAngle, gate.transform.rotation) > Mathf.Abs(angle))
            {
                isOpen = true;
            }
            gate.GetComponent<Gate_Controles>().Rotate((angle>0 ? 1:-1)*speed*Time.deltaTime);
        }
    }

    private void OnDrawGizmosSelected()
    {
        float angle = this.angle / Mathf.Rad2Deg;
        Vector2 org = gate.transform.position, arb = gate.GetComponent<Gate_Controles>().handle.transform.position;
        Vector2 pos = new(arb.x+(org.x-arb.x)*Mathf.Cos(angle)-(org.y-arb.y)*Mathf.Sin(angle),
            arb.y + (org.x - arb.x) * Mathf.Sin(angle) + (org.y - arb.y) * Mathf.Cos(angle));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(arb, pos);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(arb, plate.transform.position);
    }

}
