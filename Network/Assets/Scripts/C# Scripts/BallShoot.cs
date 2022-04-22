using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallShoot : MonoBehaviour
{

    public Rigidbody whiteBallRigidbody;
    public Transform whiteBallTransform;

    public LayerMask Ignore;

    public int shootForce;

    private bool _shootMode = false;

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
    
    void DisableShootMode()
    {
        _shootMode = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_shootMode)
        {
            bool leftClick = Input.GetMouseButtonDown(0);

            if (leftClick)
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("whiteball"))
                    {
                        whiteBallRigidbody
                            .AddForce((whiteBallTransform.position - hit.point)
                                .normalized * shootForce, ForceMode.VelocityChange);
                        Debug.Log("Hit Whiteball");
                    }
                    else
                    {
                        Debug.Log("Hit something else");
                    }
 
                }
            }
        }
    }
}
