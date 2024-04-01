using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTerrainMenu : MonoBehaviour
{
    [SerializeField]private Transform spawnPoint;
    [SerializeField]private Transform target;
    [SerializeField]private float speed;

    void Update()
    {
        Vector3 aux = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime * speed);

        transform.position = aux;

        if(transform.position.z < target.position.z)
            transform.position = spawnPoint.position;
    }
}
