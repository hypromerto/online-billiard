using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController characterController;

    public float speed = 12f;

    private bool _shootMode = false;
    private bool _inputDisabled = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    private void OnEnable()
    {
        ChangePerspective.EnteredShootMode += EnterShootMode;
        ChangePerspective.DisabledShootMode += DisableShootMode;
        ChangePerspective.ActionReceived += DisableInput;
    }
    
    private void OnDisable()
    {
        ChangePerspective.EnteredShootMode -= EnterShootMode;
        ChangePerspective.DisabledShootMode -= DisableShootMode;
        ChangePerspective.ActionReceived -= DisableInput;
    }

    void EnterShootMode()
    {
        _shootMode = true;
    }
    
    void DisableShootMode()
    {
        _shootMode = false;
    }

    void DisableInput()
    {
        _inputDisabled = !_inputDisabled;
    }
    
    void ChangeShootMode()
    {
        _shootMode = !_shootMode;
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        if (!_inputDisabled)
        {
            if (!_shootMode)
            {
                Vector3 move = transform.right * x + transform.forward * z;

                characterController.Move(move * speed * Time.deltaTime);
            }
            else
            {
                Vector3 move = transform.right * x;            
            }
        }
    }
}
