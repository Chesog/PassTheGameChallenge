using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTerrainMenu : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Vector3 aux = new Vector3(transform.position.x, transform.position.y, transform.position.z + 1500);

        transform.position = aux;
    }
}
