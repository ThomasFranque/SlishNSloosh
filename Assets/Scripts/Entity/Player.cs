﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [SerializeField] private float _maxStamina = 100.0f;
    [SerializeField] private float _staminaRegenSpeed = 10.0f;
    [SerializeField] private float _maxSpeed = 100.0f;
    [SerializeField] private float _acceleration = 20.0f;

    private float _stamina;

    private Vector2 moveAxis;
    private float _weaponSpinAxis;

	private bool _depletedState;

    private MelleeWeapon _mellee;
    private InputManager _inputMaster;
	private PlayerUIManager _pUIMngr;

    private bool SpinIntention => _weaponSpinAxis != 0;

	private bool Stamina0 => _stamina <= 0;
	private bool Stamina100 => _stamina >= 100;

	//private bool CanSpin =>

    protected override void Awake()
    {
        base.Awake();
        _stamina = _maxStamina;
        _mellee = GetComponentInChildren<MelleeWeapon>();
		_pUIMngr = GetComponent<PlayerUIManager>();
        moveAxis = Vector2.zero;
        _inputMaster = new InputManager();
        _inputMaster.Player.Movement.performed += ctx => moveAxis = ctx.ReadValue<Vector2>();
        _inputMaster.Player.WeaponSpin.performed += ctx => _weaponSpinAxis = ctx.ReadValue<float>();
    }

    protected override void OnHit(float dmg, Vector2 hitDirection, float knockBackIntensity)
    {
    }

    private void Update()
    {
        if (SpinIntention && !_depletedState)
			SpiningWeapon();
        else
			NotSpiningWeapon();

		if (Stamina0 && !_depletedState)
			StaminaDepleted();
		else if (Stamina100 && _depletedState)
			StaminaRestored();

		UpdateGFXAnimator();

		
		transform.rotation = _mellee.transform.localRotation;
    }

    private void FixedUpdate()
    {
        Vector2 newVelocity = GetRigidBodyVelocity();
        Vector2 inputAxis = Vector2.ClampMagnitude(moveAxis, 1.0f);

        newVelocity.x += _acceleration * inputAxis.x;
        newVelocity.y += _acceleration * inputAxis.y;

        if (Mathf.Abs(newVelocity.x) > _maxSpeed) newVelocity.x = _maxSpeed * Mathf.Sign(newVelocity.x);
        if (Mathf.Abs(newVelocity.y) > _maxSpeed) newVelocity.y = _maxSpeed * Mathf.Sign(newVelocity.y);

        SetRigidBodyVelocity(newVelocity);
    }

	private void UpdateGFXAnimator()
	{
		_entityVisuals.GfxAnim.SetBool("Walking", Mathf.Abs(moveAxis.x) > 0 || Mathf.Abs(moveAxis.y) > 0);
	}

    private void SpiningWeapon()
    {
		_stamina -= _mellee.StaminaConsumptionSpeed * Time.deltaTime;
		if (_stamina < 0) _stamina = 0;
        _pUIMngr.RefillingStamina = false;


        _pUIMngr.Depleeting = true;
		_pUIMngr.UpdateStaminaFill(_stamina / _maxStamina);

        _mellee.SpinHeld();
    }

    private void NotSpiningWeapon()
    {
		_stamina += _staminaRegenSpeed * Time.deltaTime;
		if (_stamina > 100) _stamina = 100;
        _pUIMngr.RefillingStamina = _stamina < 100;

        _pUIMngr.Depleeting = false;
		_pUIMngr.UpdateStaminaFill(_stamina / _maxStamina);

        _mellee.SpinNotHeld();
    }

	private void StaminaDepleted()
	{
		_depletedState = true;
		_stamina = 0;
        _pUIMngr.RefillingStamina = true;
	}

	private void StaminaRestored()
	{
		_depletedState = false;
		_stamina = 100;
        _pUIMngr.RefillingStamina = false;
	}

    private void OnEnable()
    {
        _inputMaster.Enable();
    }
    private void OnDisable()
    {
        _inputMaster.Disable();
    }
}
