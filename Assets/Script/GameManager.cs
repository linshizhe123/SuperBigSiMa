using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    // 人物死亡
    public static bool isLive = true;

    // 人物形态
    public static bool isHat = false;
    public static bool isSword = false;

    // 踩敌人
    public static bool isTread = false;

    // 砖块碎的判断
    public static bool isBrick = true;

    private void Awake()
    {
        if (instance != null)
            return;
        instance = this;
        DontDestroyOnLoad(this);
    }

    // 砖块同时撞击哪个碎的判断函数
    // 我他妈太机智了！！
    public static bool BricksBrokenCheck()
    {
        bool flag = false;
        if (isBrick)
        {
            isBrick = false;
            instance.Invoke("ChangeIsBrick", 0.1f);
            flag = true;
        }
        else
        {
            flag = false;
        }
        return flag;
    }

    public void ChangeIsBrick()
    {
        isBrick = true;
    }

    // 踩敌人状态函数
    public static void isTreadEnemies(bool tread)
    {
        if (!isTread)
            isTread = tread;
    }

    // 死亡状态函数
    public static void isLiveOrDead(bool isLiveOrDead)
    {
        isLive = isLiveOrDead;
    }

    // 死亡函数
    public static void PlayerDied()
    {
        instance.Invoke("RestartScene", 1.5f);
    }

    // 场景重置
    void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isHat = false;
        isSword = false;
        Time.timeScale = 1f;
    }

}
