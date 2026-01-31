using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ResultType
{
    None,Clear,Failed
}
public class GameManager : MonoBehaviour
{
    static GameManager instance;
    ResultType result;

    public static GameManager Instance { get { return instance; } }
    public ResultType ResultType { get { return result; } set { result = value; } }


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
