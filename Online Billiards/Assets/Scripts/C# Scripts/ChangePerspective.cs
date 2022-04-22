using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ChangePerspective : MonoBehaviour
{
    // Start is called before the first frame update

    private Collider _billiardTableCollider;
    private Transform _whiteBall;

    private Vector3 _verticalOffset = Vector3.up * 0.25f;

    private Vector3 _originalPosition;
    private Quaternion _originalRotation;
    private bool _shootMode = false;

    public delegate void Action();
    public static event Action EnteredShootMode;
    public static event Action DisabledShootMode;
    public static event Action ActionReceived;

    private float transitionDuration = 0.3f;
    private float elapsedTime;

    private bool _disableShootAnimationFinished = false;
    private bool _enableShootAnimationFinished = false;

    void Start()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.rotation;

        _billiardTableCollider = GameObject.Find("CameraBoundary").GetComponent<Collider>();
        _whiteBall = GameObject.Find("White Ball").GetComponent<Transform>();

    }

    // Update is called once per frame
    void Update()
    {
        bool ePressed = Input.GetKeyDown("e");

        if (ePressed)
        {
            _shootMode = !_shootMode;

            if (ActionReceived != null)
                ActionReceived();
            
            if (_shootMode)
            {
                _originalPosition = transform.position;
                _originalRotation = transform.rotation;
                
                Vector3 targetPosition = _billiardTableCollider.ClosestPoint(transform.position) + _verticalOffset;
                Vector3 targetDirection = (_whiteBall.position - targetPosition).normalized;
                
                StartCoroutine(Transition(_originalPosition, targetPosition, transform.rotation, Quaternion.LookRotation(targetDirection), false));
                transform.LookAt(_whiteBall);
            }
            else
            {
                StartCoroutine(Transition(transform.position, _originalPosition, transform.rotation, _originalRotation, true));
            }
        }
        
        if (_shootMode) //We always want to look at the white ball, even if we are moving as a character
        {
            transform.LookAt(_whiteBall
            );
        }
    }

    IEnumerator Transition(Vector3 startPosition, Vector3 targetPosition, Quaternion startRotation, Quaternion desiredRotation, bool isDisableShoot)
    {
        if (isDisableShoot)
            _disableShootAnimationFinished = false;
        else
            _enableShootAnimationFinished = false;
        
        float t = 0.0f;
                
        while (t < 1.0f)
        {
            t += Time.deltaTime * (Time.timeScale/transitionDuration);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Slerp(startRotation, desiredRotation, t);
            yield return 0;
        }
        
        if (isDisableShoot && DisabledShootMode != null)
            DisabledShootMode();
        else if(!isDisableShoot && EnteredShootMode != null)
            EnteredShootMode();
        
        if (ActionReceived != null)
            ActionReceived();

    }
}
