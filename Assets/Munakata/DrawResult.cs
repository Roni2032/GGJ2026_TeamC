using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DrawResult : MonoBehaviour
{
    [SerializeField]
    Image resultImage;
    [SerializeField]
    Sprite clear;
    [SerializeField]
    Sprite failed;

    InputAction anyAction;

    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.ResultType == ResultType.Clear)
        {
            Debug.Log("成功！！");
            resultImage.sprite = clear;
        }
        else
        {
            Debug.Log("失敗！！また挑戦してね！！");
            resultImage.sprite = failed;
        }

        anyAction = new InputAction(
           name: "AnyInput",
           type: InputActionType.PassThrough,
           binding: "*/<Button>"
       );

        anyAction.performed += OnAnyInput;
        anyAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDisable()
    {
        anyAction.Disable();
    }

    void OnAnyInput(InputAction.CallbackContext ctx)
    {

        SceneManager.LoadScene("TitleScene");
    }
}
