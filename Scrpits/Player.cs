using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private new Rigidbody rigidbody;
    //特效
    private GameObject LiZi;
    private Animator animator;
    //判定
    private float distance;
    private float stateTime;
    //盒子
    private GameObject CurrentBox;
    public GameObject Box;
    private Collider LastBox;
    private Vector3 Direction = new Vector3(1, 0, 0), Direction2;
    //UI
    public Text ScoreText,Highest;
    private int score = 0;
    static int hightest = 0;    
    //参数
    public float MaxDistance = 2;
    public float Rate = 3;//跳跃倍率
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody>();
        LiZi = GameObject.Find("LiZi");
        LiZi.SetActive(false);
        rigidbody.centerOfMass=Vector3.zero;//防摔
        CurrentBox = Box;
        LastBox = CurrentBox.GetComponent<Collider>();
        BoxBuild();
        Highest.text = hightest.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateTime = Time.time;
            LiZi.SetActive(true);
            animator.SetBool("InPressure", true);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            var elapse = Time.time - stateTime;
            OnJump(elapse);
            LiZi.SetActive(false);
            animator.SetBool("InPressure", false);
        }
    }
    void OnJump(float elapse)//跳跃
    {
        rigidbody.AddForce((new Vector3(0, 1, 0)+Direction2) * elapse * Rate, ForceMode.Impulse);
    }
    void BoxBuild()//随机生成随机大小和颜色的跳台
    {
        
        var box = Instantiate(Box);
        box.transform.position = CurrentBox.transform.position + Direction * Random.Range(1.1f,MaxDistance);
        var randomScale = Random.Range(0.5f, 1);
        box.transform.localScale = new Vector3(randomScale, 0.5f, randomScale);
        box.GetComponent<Renderer>().material.color = new Color(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1));
        Direction2 = box.transform.position - transform.position ;
        Direction2 = Direction2.normalized;
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("Box") && collision.collider != LastBox)
        {
            GameObject toDestory = CurrentBox;
            LastBox = collision.collider;
            CurrentBox = collision.gameObject;
            RandomDirection();
            Box = CurrentBox;
            BoxBuild();
            Destroy(toDestory);
            distance = Vector3.Distance(transform.position, CurrentBox.transform.position);
            if (distance<=0.21)
            {
                score+=4;
            }
            else
            {
                score++;
            }
            if (hightest < score)
                hightest = score;
            ScoreText.text = score.ToString();
            Highest.text = hightest.ToString();
        }
        if (collision.gameObject.name == "地板")
        {
            SceneManager.LoadScene("Jump A Jump");
        }
    }
    //随机方向
    void RandomDirection()
    {
        var seed = Random.Range(0, 2);
        if (seed == 0)
        {
            Direction = new Vector3(1, 0, 0);
        }
        else
        {
            Direction = new Vector3(0, 0, -1);
        }
    }

}
