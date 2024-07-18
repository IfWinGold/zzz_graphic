using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ActorCamera : MonoBehaviour
{
    private CinemachineVirtualCamera vcamera;
    private Cinemachine3rdPersonFollow vcameraFollow;
    private float foffsetSpeed = 10.0f;

    private void UpdateRotate()
    {
        float ybody = Input.GetAxis("Mouse Y");

        Vector3 shoulderOffset = vcameraFollow.ShoulderOffset;
        shoulderOffset.y += ybody * Time.deltaTime * foffsetSpeed;
        shoulderOffset.y = Mathf.Clamp(shoulderOffset.y, 0.1f, 3.5f);
        vcameraFollow.ShoulderOffset = shoulderOffset;
    }
    void Start()
    {
        vcamera = GetComponent<CinemachineVirtualCamera>();
        vcameraFollow = vcamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotate();
    }
}
