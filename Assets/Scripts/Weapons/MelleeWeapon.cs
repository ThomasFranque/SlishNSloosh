﻿using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class MelleeWeapon : MonoBehaviour
{
    [SerializeField] private float _staminaConsumptionSpeed = 5.0f;
    [SerializeField] private float _baseDamage = 1.0f;
    [SerializeField] private float _maxRotationSpeed = 900.0f;
    [SerializeField] private float _rotationAcceleration = 80.0f;
    [SerializeField] private float _rotationFalloffFactor = 2.0f;
    [SerializeField] private bool _exponentialSpeed = false;
    private Collider2D _selfCol;

    private bool _spinning;
    private float _currentRotationSpeed;

    public float StaminaConsumptionSpeed => _staminaConsumptionSpeed * (_currentRotationSpeed / _maxRotationSpeed);
    protected float GetFinalDamage => _baseDamage;

    private void Awake()
    {
        if (_exponentialSpeed) _AccelerationCalculation = AccelerationExponentialIncrease;
        else _AccelerationCalculation = AccelerationSomatoryIncrease;

        _selfCol = GetComponent<Collider2D>();
        _selfCol.isTrigger = true;
    }
    protected virtual void Update()
    {
        UpdateCurrentAcceleration();
        RotateArround();
    }

    public void SpinHeld()
    {
        if (_spinning == false) OnSpinStart();
    }

    public void SpinNotHeld()
    {
        if (_spinning == true) OnSpinStop();
    }

    private void OnSpinStart()
    {
        _spinning = true;
    }

    private void OnSpinStop()
    {
        _spinning = false;
    }


    private void RotateArround()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, _currentRotationSpeed * Time.deltaTime);
    }

    private void UpdateCurrentAcceleration()
    {
        _AccelerationCalculation?.Invoke();
        
        if (_currentRotationSpeed > _maxRotationSpeed) _currentRotationSpeed = _maxRotationSpeed;
        else if (_currentRotationSpeed < 0) _currentRotationSpeed = 0;
    }

    private void AccelerationExponentialIncrease()
    {
        _currentRotationSpeed = _spinning ?
        _currentRotationSpeed * 2 + _rotationAcceleration * Time.deltaTime :
        _currentRotationSpeed - _rotationAcceleration * (_rotationFalloffFactor + _rotationAcceleration) * Time.deltaTime;
    }

    private void AccelerationSomatoryIncrease()
    {
        _currentRotationSpeed = _spinning ?
        _currentRotationSpeed + _rotationAcceleration * Time.deltaTime :
        _currentRotationSpeed - _rotationAcceleration * _rotationFalloffFactor * Time.deltaTime;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Attackable")
            OnAttackableCollision(other.gameObject);

    }

    protected virtual void OnAttackableCollision(GameObject go)
    {
        Debug.Log("Attackable Collision");
    }

    private Action _AccelerationCalculation;
}