using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{
    #region Singleton
    private static InputController _instance;

    public static InputController I => _instance;

    private void Awake()
    {
        if (_instance == null)
            _instance = this;
        else
            Destroy(gameObject);

        _input = new();
    }
    #endregion

    private GameInput _input;

    public static event Action<Vector2> OnTouch; 

    private void OnEnable()
    {
        _input.Enable();

        _input.Gameplay.Touch.performed += ReadTouch;
    }

    private void OnDisable()
    {
        _input.Disable();

        _input.Gameplay.Touch.performed -= ReadTouch;
    }

    //private void Update()
    //{
    //    if(Input.touchCount > 0)
    //    {
    //        var touch = Input.GetTouch(0);
    //        OnTouch?.Invoke(touch.position);
    //    }
    //}

    private void ReadTouch(InputAction.CallbackContext context)
    {
        var touchPos = context.ReadValue<Vector2>();
        OnTouch?.Invoke(touchPos);
    }
}
