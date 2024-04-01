using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMoveMenu : MonoBehaviour
{
    [SerializeField] private float speed;

    void Update()
    {
        Vector3 aux = new Vector3(transform.position.x, transform.position.y, transform.position.z + Time.deltaTime * speed);

        transform.position = aux;
    }
}
