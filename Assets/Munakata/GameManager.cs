using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ResultType
{
    None,Clear,Failed
}
public class GameManager : MonoBehaviour
{
    static GameManager instance;
    ResultType result = ResultType.None;

    public static GameManager Instance { get { return instance; } }
    public ResultType ResultType { get { return result; } set { result = value; } }

    [SerializeField]
    Fade fade;

    public void GameFailed()
    {
        //もうすでにフェードが開始されているときは処理しない
        if (fade.IsActive) return;

        Debug.Log("失敗！！");
        result = ResultType.Failed;
        fade.StartFade();
    }
    public void GameClear()
    {
        //もうすでにフェードが開始されているときは処理しない
        if (fade.IsActive) return;

        Debug.Log("クリア！！");
        result = ResultType.Clear;
        fade.StartFade();
    }
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (fade.IsActive && fade.IsFinished)
        {
            SceneManager.LoadScene("ResultScene");
        }
    }
}
