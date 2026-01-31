using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainCamera : MonoBehaviour
{

    [SerializeField,Header("カメラとプレイヤーの距離")]
    float distance = 7.5f;
    [SerializeField,Header("プレイヤーからの高さ")]
    float height = 0.5f;
    [SerializeField, Header("感度")]
    float sensitivity = 0.1f;

    [SerializeField]
    GameObject player;

    float angle = 0.0f;

    public void OnMouse(InputAction.CallbackContext context)
    {
        Vector3 delta = context.ReadValue<Vector2>();
        angle -= 0.01f * delta.x * sensitivity;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 playerPosition = player.transform.position;
        Vector3 newPosition = playerPosition + new Vector3(Mathf.Cos(angle),0.0f,Mathf.Sin(angle)) * distance;
        newPosition.y += height; 

        transform.position = newPosition;

        transform.LookAt(player.transform);
    }
}
