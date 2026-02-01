using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawResult : MonoBehaviour
{
    [SerializeField]
    Image resultImage;
    // Start is called before the first frame update
    void Start()
    {
        if(GameManager.Instance.ResultType == ResultType.Clear)
        {
            Debug.Log("成功！！");
        }
        else
        {
            Debug.Log("失敗！！また挑戦してね！！");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
