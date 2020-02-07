using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy : MonoBehaviour
{
    public GameObject enemy;
    private new Transform transform;
    private Animator animator; // 动画组件
    private Player2 player; // 主角
    private NavMeshAgent agent; // 寻路组件
    private float moveSpeed = 3f; // 角色移动速度
    private float rotateSpeed = 360; // 角色旋转速度
    private float timer1 = 2, timer2 = 2; // 计时器 1:待机时间 2:每2秒造成伤害
    private int life = 10; // 生命值

    void Start()
    {
        //获取Transform组件
        transform = GetComponent<Transform>();
        // 获取动画组件
        animator = GetComponentInChildren<Animator>();
        // 获取主角
        player = GameObject.Find("Player2").GetComponent<Player2>();
        // 获取寻路组件
        agent = GetComponent<NavMeshAgent>();
        agent.speed = moveSpeed;

    }
    // Update is called once per frame
    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);// 获取当前动画状态
        //待机
        while (timer1 >= 0)
        {
            agent.isStopped = true;
            animator.SetBool("move", false);
            RotateTo();
            timer1 -= Time.deltaTime;
            return;
        }
        // 如果主角生命为0，则什么也不做
        if (player.life <= 0)
        {
            return;
        }

        // 如果距离主角6.5米以内，则每两秒进攻主角
        if (Vector3.Distance(transform.position, player.transform.position) < 6.5f)
        {
            timer2 -= Time.deltaTime;
            // 停止寻路
            agent.isStopped = true;
            animator.SetBool("move", false);
            RotateTo();
            if (timer2 <= 0)
            {
                player.OnDamage(1);
                timer2 = 2;
            }

        }
        else
        {
            agent.isStopped = false;
            agent.destination = player.transform.position;
            animator.SetBool("move", true);
        }
        //死亡

        if (info.fullPathHash == Animator.StringToHash("Base Layer.dead")
            && !animator.IsInTransition(0))
        {
            agent.isStopped = true;
            // 判断死亡动画是否播放完成
            if (info.normalizedTime >= 2.0f)
            {
                // 销毁自身
                EnemyBuild();
                Destroy(gameObject);
                // 销毁自身
                
            }
        }
    }

    //转向
    private void RotateTo()
    {
        // 获取目标方向
        Vector3 targetDirection = player.transform.position - transform.position;
        // 计算旋转角度
        float x, y = 0, z;
        x = targetDirection.x;
        z = targetDirection.z;
        targetDirection = new Vector3(x, y, z);
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, rotateSpeed * Time.deltaTime, 0);
        // 旋转至新方向
        transform.rotation = Quaternion.LookRotation(newDirection);

    }
    //刷新
    private void EnemyBuild()
    {
        var enemy_c = Instantiate(enemy);
        enemy_c.transform.position = new Vector3(0, 0, 30);
        // 加分
        UIControl.score++;
        if (UIControl.hiscore < UIControl.score)
        {
            UIControl.hiscore = UIControl.score;
        }
    }
    public void OnDamage(int damage)
    {
        life -= damage;
        animator.SetTrigger("hited");
        // 如果生命为0，进入死亡状态
        if (life == 0)
        {
            agent.isStopped = true;
            animator.SetBool("Dead", true);
        }
        
    }
}
