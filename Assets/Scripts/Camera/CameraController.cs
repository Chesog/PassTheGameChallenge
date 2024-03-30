using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Character _player;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private Vector2 _cameraOffset;
    
    private void FixedUpdate()
    {
        Vector3 originalPos = transform.position;
        Vector3 newPos = _player.transform.position;
        newPos.z += _cameraOffset.x;
        newPos.y += _cameraOffset.y;
        transform.position = Vector3.Lerp(originalPos, newPos,_cameraSpeed * Time.deltaTime);
    }
}
