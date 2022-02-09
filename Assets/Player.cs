using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    // https://blog.csdn.net/cbaili/article/details/110157060 VSCode无法显示Unity代码提示的真正解决办法

    // 預設是prrivate
    //SerializeField 輸入這個可以在 private的情況下 出現在unity的UI上
    [SerializeField] float movespeed = 1.1f;
    [SerializeField] int Hp;
    [SerializeField] int addHpNum = 1;
    [SerializeField] int loseHpNum = -3;

    [SerializeField] GameObject HpBar;
    [SerializeField] Text scoreText;

    [SerializeField] GameObject replayBtn;
    // 以下只是簡寫用(簡化code用)
    SpriteRenderer render;
    Animator anim;
    AudioSource deathSound;
    // 
    double scoreTime = 1;

    GameObject currentFloor;

    // public float movespeed = 1.1f;
    // Start is called before the first frame update
    void Start()
    {
        Hp = 10;
        render = this.transform.GetComponent<SpriteRenderer>(); //this.transform. 可省略
        anim = GetComponent<Animator>();
        deathSound = GetComponent<AudioSource>();
        // https://www.youtube.com/watch?v=nPW6tKeapsM&t=1089s&ab_channel=GrandmaCan-%E6%88%91%E9%98%BF%E5%AC%A4%E9%83%BD%E6%9C%83
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            render.flipX = false;
            transform.Translate(movespeed * Time.deltaTime, 0, 0);
            anim.SetBool("run", true);
            //  transform.rotation=Quaternion.Euler(0f,0f,0.0f);
        }
        else if (
        Input.GetKey(KeyCode.LeftArrow)
        )
        {
            render.flipX = true;
            transform.Translate(-movespeed * Time.deltaTime, 0, 0);
            anim.SetBool("run", true);
            //   transform.rotation=Quaternion.Euler(0f,180f,0.0f);
        }
        else
        {
            anim.SetBool("run", false);
        }
        //Time.deltaTime 間隔間距(時間)(跑一輪花多久時間)  1秒兩輪，間距=0.5   實際一秒跑，1.1f*0.5*2(一秒執行兩次)
        setScore();
    }
    private void OnCollisionEnter2D(Collision2D other)
    { //碰撞器撞到
      // Debug.Log(other.gameObject.tag);

        // contacts與GetContact,contacts盡量避免用他，他會產生暫存,GetContact不能用foreach

        // foreach (var item in other.contacts)
        // {
        //     Debug.Log(item.point);
        // }

        // Debug.Log(other.GetContact(0).normal);
        // Debug.Log(other.GetContact(1).point);

        // Collision.contacts. :
        // normal	接触点的法线。 垂直於該平面的線
        // otherCollider	在该点接触的其他碰撞体。
        // point	接触点。
        // separation	在该接触点的碰撞体之间的距离。
        // thisCollider	在该点接触的第一个碰撞体。

        if (other.gameObject.tag == "floor1")
        {
            ModifyHp(addHpNum);
            // 接觸到player腳底的才被記錄下來
            if (other.GetContact(0).normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
            }
            other.gameObject.GetComponent<AudioSource>().Play();

        }
        else if (other.gameObject.tag == "floor2")
        {
            ModifyHp(loseHpNum);
            if (other.GetContact(0).normal == new Vector2(0f, 1f))
            {
                currentFloor = other.gameObject;
                anim.SetTrigger("hurt");
                other.gameObject.GetComponent<AudioSource>().Play();
            }
        }
        else if (other.gameObject.tag == "Ceiling")
        {
            ModifyHp(loseHpNum);
            // 撞到天花板尖刺時，腳下的地板碰撞器關閉
            currentFloor.GetComponent<BoxCollider2D>().enabled = false;
            anim.SetTrigger("hurt");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        // Debug.Log(other.gameObject.name + 1);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log(other.gameObject.name + 2);
        die();
    }


    //二：接触的两种方式

    // 1：Collision碰撞，造成物理碰撞，可以在碰撞时执行OnCollision事件。

    // 2：Trigger触发，取消所有的物理碰撞，可以在触发时执行OnTrigger事件。

    // 注：两个物体接触不可能同时产生碰撞+接触，最多产生一种。但是可以AB产生碰撞，AC产生触发。



    // 三：产生不同方式接触的条件

    // 1：Collision碰撞

    //      （1）：双方都有碰撞体

    //      （2）：运动的一方必须有刚体

    //      （3）：双方不可同时勾选Kinematic运动学。

    //      （4）：双方都不可勾选Trigger触发器。

    // 2：Trigger触发

    //      （1）：双方都有碰撞体

    //      （2）：运动的一方必须是刚体

    //      （3）：至少一方勾选Trigger触发器



    // 四：接触后事件细分为Enter,Stay,Exit三种（以Trigger为例，分别为OnTriggerEnter、OnTriggerStay、OnTriggerExit）

    // 1：Enter事件表示两物体接触瞬间，会执行一次。

    // 2：Stay事件表示两物体持续接触，会不断执行。

    // 3：Exit事件当两物体分开瞬间，会执行一次。



    // 五：OnTriggerEnter这类的属于Trigger触发，OnCollisionEnter这类的属于Collision碰撞
    void ModifyHp(int num)
    {
        Hp += num;
        if (Hp > 10)
        {
            Hp = 10;
        }
        else if (Hp <= 0)
        {
            Hp = 0;
            die();
        }

        hpControl();
    }
    void hpControl()
    {

        for (int a = 0; a < HpBar.transform.childCount; a++)
        {
            if (
        a < Hp
            )
            {
                HpBar.transform.GetChild(a).gameObject.SetActive(true);
            }
            else
            {
                HpBar.transform.GetChild(a).gameObject.SetActive(false);
            }

        }
    }
    void setScore()
    {
        scoreTime += Time.deltaTime;
        // Debug.Log(scoreTime);
        scoreText.text = "地下" + Mathf.Floor(Mathf.Abs((float)scoreTime / 2)).ToString() + "層";
    }
    void die()
    {
        deathSound.Play();
        Time.timeScale = 0; //遊戲以0倍執行 暫停
        replayBtn.SetActive(true);
    }

    public void replay(){
        Time.timeScale=1;//速度 恢復正常
        // 重新載入場景
        SceneManager.LoadScene("SampleScene");

    }
}
