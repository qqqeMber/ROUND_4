using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player2 : MonoBehaviour
{
    // 设置两次枪击的间隔时间
    public float fireRate = 0.1f;

    // 设置玩家可以射击的Unity单位
    public float weaponRange = 50f;

    // 设置射击轨迹显示的时间
    private WaitForSeconds shotDuration = new WaitForSeconds(0.07f);

    // 枪击音效
    private AudioSource gunAudio;
    public AudioClip audioClip;

    // 射击轨迹
    private LineRenderer laserLine;

    // 玩家上次射击后的间隔时间
    private float nextFire;

    public int life = 20;
    //组件
    private new Transform transform;
    private CharacterController controller;
    //视角控制
    private Transform cameraTransform; // 摄像机的Transform组件
    private Vector3 cameraRotation; // 摄像机旋转角度
    private float cameraHeight = 2.0f-1.08f; // 摄像机高度（即主角的眼睛高度）
    //参数
    private float speed = 3.0f;
    private float gravity = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        // 获取AudioSource组件
        gunAudio = GetComponent<AudioSource>();
        // 获取LineRenderer组件
        laserLine = GetComponent<LineRenderer>();
        //获取人物组件
        transform = GetComponent<Transform>();
        controller = GetComponent<CharacterController>();
        // 获取摄像机
        cameraTransform = Camera.main.GetComponent<Transform>();
        // 设置摄像机初始位置
        Vector3 position = transform.position;
        position.y += cameraHeight;
        cameraTransform.position = position;
        // 设置摄像机的初始旋转方向
        cameraTransform.rotation = transform.rotation;
        cameraRotation = cameraTransform.eulerAngles;
        // 锁定鼠标
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = (false);

    }

    // Update is called once per frame
    void Update()
    {
        control();
        Shoot();
        
    }
    //控制角色移动
    private void control()
    {
        //视角控制
        // 获取鼠标移动距离
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");
        // 旋转摄像机
        cameraRotation.x -= rv;
        cameraRotation.y += rh;
        cameraTransform.eulerAngles = cameraRotation;
        // 使主角的面向方向与摄像机一致
        Vector3 rotation = cameraTransform.eulerAngles;
        rotation.x = 0;
        rotation.z = 0;
        transform.eulerAngles = rotation;


        //移动控制
        float x=0, y=0, z=0;
        //重力作用
        y = -gravity * Time.deltaTime;
        // 前后移动
        if (Input.GetKey(KeyCode.W))
        {
            z += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            z -= speed * Time.deltaTime;
        }
        // 左右移动
        if (Input.GetKey(KeyCode.A))
        {
            x -= speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            x += speed * Time.deltaTime;
        }
        // 使用Character Controller而不是Transform提供的Move方法
        // 因为Character Controller提供的Move方法会自动进行碰撞检测
        controller.Move(transform.TransformDirection(new Vector3(x, y, z)));

        // 使摄像机的位置与主角一致
        Vector3 position = transform.position + transform.forward/3;
        position.y += cameraHeight;
        cameraTransform.position = position;
    }
    public void OnDamage(int damage)
    {
        life -= damage;
        // 如果生命为0，游戏结束，取消鼠标锁定
        if (life <= 0)
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    private void Shoot()
    {
        // 检测是否按下射击键以及射击间隔时间是否足够
        if (Input.GetButtonDown("Fire1") && Time.time > nextFire)
        {
            if (UIControl.ammo <= 0)
            {
                return;
            }
            //子弹减少
            UIControl.ammo--;
            // 射击之后更新间隔时间
            nextFire = Time.time + fireRate;

            // 启用ShotEffect携程控制射线显示及隐藏
            StartCoroutine(ShotEffect());

            // 在相机视口中心创建向量
            Vector3 rayOrigin = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

            // 声明RaycastHit存储射线射中的对象信息
            RaycastHit hit;

            // 将射击轨迹起点设置为对象的位置
            Vector3 start = transform.position;
            start.y = 1.8f;
            laserLine.SetPosition(0, start);

            // 检测射线是否碰撞到对象
            if (Physics.Raycast(rayOrigin, cameraTransform.forward, out hit, weaponRange))
            {
                // 将射击轨迹终点设置为碰撞发生的位置
                laserLine.SetPosition(1, hit.point);
                if (hit.transform.tag.Equals("enemy"))
                {
                    // 敌人减少生命
                    hit.transform.GetComponent<Enemy>().OnDamage(1);
                }



            }
            else
            {
                // 如果未射中任何对象，则将射击轨迹终点设为相机前方的武器射程最大距离处
                laserLine.SetPosition(1, rayOrigin + (cameraTransform.forward * weaponRange));
            }
        }
    }
    
    private IEnumerator ShotEffect()
    {
        // 播放音效
        gunAudio.PlayOneShot(audioClip);

        // 显示射击轨迹
        laserLine.enabled = true;

        // 等待
        yield return shotDuration;

        // 等待结束后隐藏轨迹
        laserLine.enabled = false;
    }
}

