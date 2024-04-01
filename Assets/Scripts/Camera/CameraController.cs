using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Character _player;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private Vector3 _cameraOffset;
    [SerializeField] private Transform _cameraPos;
    
    private void FixedUpdate()
    {
        Vector3 originalPos = transform.position;

        /*
        Vector3 newPos = _player.transform.position;
        newPos.z += _cameraOffset.x;
        newPos.y += _cameraOffset.y;
        newPos.x += _cameraOffset.z;
        */

        transform.position = Vector3.Lerp(originalPos, _cameraPos.position, _cameraSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _cameraPos.rotation, _cameraSpeed * Time.deltaTime);
    }
}
