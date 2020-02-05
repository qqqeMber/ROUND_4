using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    private Enemy enemy;
    private Player2 player;
    public Text HP, SCORE, HISCORE, AMMO;
    public static int hiscore = 0;// 游戏最高得分
    public static int score = 0; // 游戏得分
    public static int ammo = 30; // 弹药数量
    private float timer = 0.8f;//换弹时间
    // Start is called before the first frame update
    void Start()
    {
        // 获取主角
        player = GameObject.Find("Player2").GetComponent<Player2>();
    }

    // Update is called once per frame
    void Update()
    {
        SCORE.text = score.ToString();
        HISCORE.text = hiscore.ToString();
        HP.text = player.life.ToString();
        if (ammo <= 0 || Input.GetKeyDown(KeyCode.R))
        {
            ammo = 0;
            Invoke("SetAmmo", timer);
        }
            
        AMMO.text = ammo.ToString()+"/∞";
    }
    private void SetAmmo()
    {
            ammo = 30;
    }
}
