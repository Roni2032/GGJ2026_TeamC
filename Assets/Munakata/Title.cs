using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    [SerializeField,Header("ÉQÅ[ÉÄÉVÅ[Éìñº")]
    string gameSceneName;
    InputAction anyAction;
    bool isLoading = false;

    void OnEnable()
    {
        anyAction = new InputAction(
            name: "AnyInput",
            type: InputActionType.PassThrough,
            binding: "*/<Button>"
        );

        anyAction.performed += OnAnyInput;
        anyAction.Enable();
    }

    void OnDisable()
    {
        anyAction.Disable();
    }

    void OnAnyInput(InputAction.CallbackContext ctx)
    {
        if (isLoading) return;

        isLoading = true;
        SceneManager.LoadScene(gameSceneName);
    }
}
