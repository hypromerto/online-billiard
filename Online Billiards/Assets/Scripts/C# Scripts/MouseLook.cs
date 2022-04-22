using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    public float mouseSensitivity = 1500f;

    public Transform playerBody;
    
    private float xRotation = 0f;

    private bool _shootMode = false;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void OnEnable()
    {
        ChangePerspective.EnteredShootMode += EnterShootMode;
        ChangePerspective.DisabledShootMode += DisableShootMode;
    }
    
    private void OnDisable()
    {
        ChangePerspective.EnteredShootMode -= EnterShootMode;
        ChangePerspective.DisabledShootMode -= DisableShootMode;
    }

    void EnterShootMode()
    {
        _shootMode = true;
        Cursor.lockState = CursorLockMode.None;
    }
    
    void DisableShootMode()
    {
        _shootMode = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_shootMode)
        {
            var mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            var mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
            
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            
            playerBody.Rotate(Vector3.up * mouseX);

        }
    }
}
