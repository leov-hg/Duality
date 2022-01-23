using System;
using MyBox;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum InputType { GetKey, GetKeyDown }

public class PlayerBase : MonoBehaviour
{
    [Separator("References", true)]
    [SerializeField] private PlayerRef playerRef;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Transform upperBody;
    [SerializeField] protected Transform lowerBody;
    [SerializeField] private Transform vacuumingEffect;
    [SerializeField] private ParticleSystem bumpEffect;

    [Separator("Settings", true)]
    [SerializeField] private float rotationSmoothSpeed = 1;
    [SerializeField] private float speed = 1;
    [SerializeField] private InputType inputType;
    [Space(10)]
    [SerializeField] private LayerMask groundLayer;

    protected float _currentSpeed = 0;
    protected List<PhysicsHandler> _detectedObjects = new List<PhysicsHandler>();
    protected List<Collider> _detectedCollider = new List<Collider>();
    protected Animator _animator;

    private Vector3 _direction;
    private Vector3 _targetView;
    private Camera _mainCam;
    private Ray _camRay;
    private bool _active;
    private float _vacuumEffectGlobalAlpha;
    private Tween _vacuumEffectAlphaTween;


    private void Awake()
    {
        _mainCam = Camera.main;
        playerRef.playerBase = this;
        _animator = GetComponentInChildren<Animator>();

        _currentSpeed = speed;

        if (vacuumingEffect)
        {
            vacuumingEffect.gameObject.SetActive(false);
            vacuumingEffect.localScale = new Vector3(1, 1, 0);
            _vacuumEffectGlobalAlpha = 1;
            Shader.SetGlobalFloat("GLOBAL_Alpha", _vacuumEffectGlobalAlpha);
        }
    }

    private void Start()
    {
        GameManager.Instance.onLevelStart += OnLevelStart;
    }

    protected virtual void Update()
    {
        if (!_active) return;


        _direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        _direction = Vector3.ClampMagnitude(_direction, 1);

        lowerBody.LookAt(lowerBody.transform.position + _direction);

        _camRay = _mainCam.ScreenPointToRay(Input.mousePosition + Vector3.forward);
        if (Physics.Raycast(_camRay, out RaycastHit hitInfo, 100, groundLayer))
        {
            _targetView = hitInfo.point.SetY(upperBody.transform.position.y);
        }
        
        upperBody.rotation = Quaternion.Lerp(upperBody.rotation, Quaternion.LookRotation((_targetView - upperBody.position).normalized), Time.deltaTime * rotationSmoothSpeed);

        if (inputType == InputType.GetKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Interact();
                _animator.SetTrigger("Bump");
                //bumpEffect.Play();
            }
        }
        else if (inputType == InputType.GetKey)
        {
            if (Input.GetKey(KeyCode.E))
            {
                Interact();
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                vacuumingEffect.gameObject.SetActive(true);
                vacuumingEffect.DOKill();
                vacuumingEffect.DOScaleZ(.7f, 0.25f).SetEase(Ease.OutQuad);

                _vacuumEffectAlphaTween.Kill();
                _vacuumEffectAlphaTween = DOTween.To(() => _vacuumEffectGlobalAlpha, x => _vacuumEffectGlobalAlpha = x, 1, .05f);
                _vacuumEffectAlphaTween.onUpdate += () => Shader.SetGlobalFloat("GLOBAL_Alpha", _vacuumEffectGlobalAlpha);

                _animator.SetBool("Vacuuming", true);
            }
            if (Input.GetKeyUp(KeyCode.E))
            {
                vacuumingEffect.DOKill();
                vacuumingEffect.DOScaleZ(0, 0.15f).onComplete += () => vacuumingEffect.gameObject.SetActive(false);

                _vacuumEffectAlphaTween.Kill();
                _vacuumEffectAlphaTween = DOTween.To(() => _vacuumEffectGlobalAlpha, x => _vacuumEffectGlobalAlpha = x, 0, .05f).SetDelay(.1f);
                _vacuumEffectAlphaTween.onUpdate += () => Shader.SetGlobalFloat("GLOBAL_Alpha", _vacuumEffectGlobalAlpha);

                _animator.SetBool("Vacuuming", false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!_active) return;

        rb.AddForce(_direction * speed, ForceMode.Acceleration);
    }

    virtual protected void Interact()
    {
        ScanForObjects();
    }

    protected void ScanForObjects()
    {
        _detectedObjects.Clear();
        foreach (Collider col in _detectedCollider)
        {
            PhysicsHandler rb = col.GetComponentInParent<PhysicsHandler>();
            if (rb != null)
                _detectedObjects.Add(rb);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _detectedCollider.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        _detectedCollider.Remove(other);
    }

    public void OnLevelStart()
    {
        _active = true;
    }
}
