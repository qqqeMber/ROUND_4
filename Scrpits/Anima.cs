using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Anima : MonoBehaviour
{
    private Animator animator;
    private Player2 player;
    // 脚步音效
    private AudioSource stepAudio;
    
    // Start is called before the first frame update
    void Start()
    {
        // 获取AudioSource组件
        stepAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("Player2").GetComponent<Player2>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);// 获取当前动画状态
        //移动动画
        float v = Input.GetAxisRaw("Vertical");
        animator.SetInteger("Vertical", (int)v);

        float h = Input.GetAxisRaw("Horizontal");
        animator.SetInteger("Horizontal", (int)h);

        if (info.fullPathHash == Animator.StringToHash("Base Layer.forward")
            || info.fullPathHash == Animator.StringToHash("Base Layer.backward")
            || info.fullPathHash == Animator.StringToHash("Base Layer.right")
            || info.fullPathHash == Animator.StringToHash("Base Layer.left"))
        {
            stepAudio.Play();
        }

        //死亡动画
        if (player.life <= 0)
        {
            animator.SetBool("Dead", true);
        }
        //装弹
        if (Input.GetKey(KeyCode.R))
        {
            animator.SetTrigger("Loading");
        }
        //换武器（无用）
        if (Input.GetKey(KeyCode.F))
        {
            animator.SetTrigger("Change");
        }

    }
}
