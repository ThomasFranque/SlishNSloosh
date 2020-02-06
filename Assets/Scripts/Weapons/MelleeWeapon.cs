using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class MelleeWeapon : MonoBehaviour
{
    [SerializeField] private GameObject _selfPrefab = null;
    [SerializeField] private float _staminaConsumptionSpeed = 15.0f;
    [SerializeField] private float _baseDamage = 10.0f;
    [SerializeField] private float _knockBackIntensity = 180.0f;
    [SerializeField] private float _maxRotationSpeed = 900.0f;
    [SerializeField] private float _rotationAcceleration = 80.0f;
    [SerializeField] private float _rotationFalloffFactor = 3.0f;
    [SerializeField] private bool _exponentialSpeed = false;

    [Tooltip("Should update rotation (true) or use the parent (false)?")]
    [SerializeField] private bool _isMainWeapon = false;

    private MelleeWeapon _parentWeapon;
    private Collider2D _selfCol;
    private TrailRenderer _tr;
    private float _maxTrailTime;

    private bool _spinning;
    private float _currentRotationSpeed;

    public float StaminaConsumptionSpeed => _staminaConsumptionSpeed * SpeedFactor;

    protected float SpeedFactor =>  _currentRotationSpeed / _maxRotationSpeed;

    protected float GetFinalDamage
    {
        get
        {
            float finalDmg = _baseDamage * SpeedFactor;
            finalDmg = Mathf.Clamp(finalDmg, 1, Mathf.Infinity);

            return finalDmg;
        }
    }
    protected float GetFinalKnockBackIntensity
    {
        get
        {
            float finalKnock = _knockBackIntensity * SpeedFactor;
            return finalKnock;
        }
    }

    private void Awake()
    {
        if (_exponentialSpeed) _AccelerationCalculation = AccelerationExponentialIncrease;
        else _AccelerationCalculation = AccelerationSomatoryIncrease;

        _selfCol = GetComponent<Collider2D>();
        _selfCol.isTrigger = true;
        _tr = GetComponentInChildren<TrailRenderer>();
        _maxTrailTime = _tr.time;

        if (_isMainWeapon)
        {
            _SwordBehaviour += UpdateCurrentAcceleration;
            _SwordBehaviour += RotateArround;
        }
        else
        {
            FindParentWeapon();
            _SwordBehaviour += GetParentWeaponCurrentSpeed;
        }

        _SwordBehaviour += UpdateTrail;
    }

    private void FindParentWeapon()
    {
        MelleeWeapon[] mellees = GetComponentsInParent<MelleeWeapon>();
        for (int i = 0; i < mellees.Length; i++)
        {
            if (mellees[i].transform == transform.parent)
            {
                _parentWeapon = mellees[i];
                return;
            }
        }

        Debug.LogError($"Parent Weapon not found on: {name} !");
    }

    protected virtual void Update()
    {
        _SwordBehaviour?.Invoke();
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

    private void GetParentWeaponCurrentSpeed()
    {
        _currentRotationSpeed = _parentWeapon._currentRotationSpeed;
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

    private void UpdateTrail()
    {
        _tr.time = (_maxTrailTime * _currentRotationSpeed) / _maxRotationSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Attackable")
        {
            Vector2 hitDirection = (other.transform.position - transform.parent.transform.position).normalized;
            Debug.DrawRay(other.transform.position, hitDirection * GetFinalKnockBackIntensity, Color.red, 0.3f);
            OnAttackableCollision(other.gameObject, hitDirection);
        }

    }

    protected virtual void OnAttackableCollision(GameObject go, Vector2 hitDirection)
    {
        go.GetComponent<Entity>().Hit(GetFinalDamage, hitDirection, GetFinalKnockBackIntensity);
    }

    private Action _AccelerationCalculation;
    private Action _SwordBehaviour;
}
