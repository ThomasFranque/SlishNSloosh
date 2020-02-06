using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityVisuals : MonoBehaviour
{
    private SpriteRenderer _sr;

    #region Flash
    private Color _defaultSpriteColor;
    private Color _flashColor;
    private Shader _shaderGUItext;
    private Shader _shaderSpritesDefault;
    private float _timeOfLastFlashStart;
    private float _flashDuration;
    private int _amountOfFlashes;
    private int _currentFlashAmount;
    # endregion

    #region Slowtime
    private float _slowDownDurationSpeed;
    private float _slowDownAmount;
    private float _timeOfSlowDown;

    #endregion

    public Animator GfxAnim { get; private set; }

    private void Awake()
    {
        GfxAnim = GetComponentInChildren<Animator>();
        _sr = GetComponentInChildren<SpriteRenderer>();

        _defaultSpriteColor = _sr.color;
        _shaderGUItext = Shader.Find("GUI/Text Shader");
        _shaderSpritesDefault = Shader.Find("Sprites/Default"); // or whatever sprite shader is being used
    }

    private void Update()
    {
        UpdateBehaviour?.Invoke();
    }

    private void UpdateSlowDown()
    {
        Time.timeScale = Mathf.MoveTowards(Time.timeScale, 1.0f, _slowDownDurationSpeed * Time.unscaledDeltaTime);

        if (Time.timeScale >= 1.0f)
        {
            UpdateBehaviour -= UpdateSlowDown;
            Time.timeScale = 1.0f;
        }

    }

    private void UpdateFlashTime()
    {
        if (_currentFlashAmount >= _amountOfFlashes)
        {
            UpdateBehaviour -= UpdateFlashTime;
            return;
        }
        if (Time.time - _timeOfLastFlashStart > _flashDuration)
        {
            // Change to Other color
            if (_sr.color == _defaultSpriteColor)
            {
                FlashSpriteWithColor();
                _timeOfLastFlashStart = Time.time;
            }
            // Change to default color
            else
            {
                NormalSpriteColor();
                _timeOfLastFlashStart = Time.time;
                _currentFlashAmount++;
            }
        }
    }
    public void Slowtime(float amount, float durationSpeed)
    {
        _timeOfSlowDown = Time.time;
        Time.timeScale = amount;
        _slowDownDurationSpeed = durationSpeed;
        _slowDownAmount = amount;
        UpdateBehaviour += UpdateSlowDown;
    }

    public void FlashSprite(Color flashColor, float flashDuration, int amountOfFlashes = 1, bool durationIsTotalTime = false)
    {
        _flashColor = flashColor;
        _flashDuration = durationIsTotalTime ? flashDuration / amountOfFlashes : flashDuration;
        _amountOfFlashes = amountOfFlashes;
        _currentFlashAmount = 0;
        _timeOfLastFlashStart = Time.time - _flashDuration;

        UpdateBehaviour += UpdateFlashTime;
    }

    private void FlashSpriteWithColor()
    {
        _sr.material.shader = _shaderGUItext;
        _sr.color = _flashColor;
    }

    private void NormalSpriteColor()
    {
        _sr.material.shader = _shaderSpritesDefault;
        _sr.color = _defaultSpriteColor;
    }


    private Action UpdateBehaviour;

}
