using System.Threading;
using Debug = UnityEngine.Debug;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
  [SerializeField] float moveSpeed = 5f;
  GameObject currentFloor;

  [SerializeField] int Hp;
  [SerializeField] GameObject HpBar;
  [SerializeField] Text scoreText;
  int score;
  float scoreTime;
  Animator anim;
  SpriteRenderer render;
  AudioSource deathSound;
  [SerializeField] GameObject replayBtn;
  void Start()
  {
    Hp = 10;
    score = 0;
    scoreTime = 0f;
    anim = GetComponent<Animator>();
    render = GetComponent<SpriteRenderer>();
    deathSound = GetComponent<AudioSource>();
  }

  // Update is called once per frame
  void Update()
  {
    // transform.Translate(1,0,0); //x軸y軸z軸的數值分別增加(x, y, z)   
    // transform.Translate(0.1,0,0);//這樣不行 要加上f
    // transform.Translate(0.01f,0,0);
    // transform.Translate(0.01f*Time.deltaTime,0,0); //才不會因為裝置有時間差

    if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
    {
      transform.Translate(0, moveSpeed * Time.deltaTime, 0);
    }
    else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
    {
      transform.Translate(-moveSpeed * Time.deltaTime, 0, 0);
      render.flipX = true;
      anim.SetBool("run", true);
    }
    else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
    {
      transform.Translate(0, -moveSpeed * Time.deltaTime, 0);
    }
    else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
    {
      transform.Translate(moveSpeed * Time.deltaTime, 0, 0);
      render.flipX = false; //控制角色面左面右邊
      anim.SetBool("run", true);
    }
    else
    {
      anim.SetBool("run", false);
    }
    UpdateHpBar();
    UpdateScore();
  }
  void OnCollisionEnter2D(Collision2D other)
  {
    if (other.gameObject.tag == "Normal")
    {
      // Debug.Log(other.contacts[0].normal);//取得法線跟法向量
      // Debug.Log(other.contacts[1].normal);
      if (other.contacts[0].normal == new Vector2(0f, 1f))
      {
        Debug.Log("撞到了階梯1");
        currentFloor = other.gameObject;
        ModifyHp(1);
        other.gameObject.GetComponent<AudioSource>().Play();

      }
    }
    else if (other.gameObject.tag == "Nails")
    {

      if (other.contacts[0].normal == new Vector2(0f, 1f))
      {
        Debug.Log("撞到了尖刺");
        currentFloor = other.gameObject;
        ModifyHp(-3);
        anim.SetTrigger("hurt");
        other.gameObject.GetComponent<AudioSource>().Play();
      }
      // Debug.Log(other.contacts[0].normal);
      // Debug.Log(other.contacts[1].normal);
    }
    else if (other.gameObject.tag == "Ceiling")
    {
      Debug.Log("撞到了天花板");
      currentFloor.GetComponent<BoxCollider2D>().enabled = false;
      //碰撞天花板時將判斷功能關掉 不然會卡住
      ModifyHp(-3);
      anim.SetTrigger("hurt");
      other.gameObject.GetComponent<AudioSource>().Play();
    }
    UpdateHpBar();
  }
  void OnTriggerEnter2D(Collider2D other)
  {
    if (other.gameObject.tag == "deathline")
    {
      Debug.Log("你輸了!");
      Die();
    }
  }
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
      Die();
    }
    UpdateHpBar();

  }
  void UpdateHpBar()
  {
    for (int i = 0; i < HpBar.transform.childCount; i++)
    {
      if (Hp > i)
      {
        HpBar.transform.GetChild(i).gameObject.SetActive(true);
        //大於則設定hpbar子物件顯示
      }
      else
      {
        HpBar.transform.GetChild(i).gameObject.SetActive(false);
      }
    }
  }
  void UpdateScore()
  {
    scoreTime += Time.deltaTime;
    if (scoreTime > 2f)
    {
      score++;
      scoreTime = 0f;
      scoreText.text = "地下" + score.ToString() + "層";
    }
  }
  void Die()
  {
    deathSound.Play();
    Time.timeScale = 0f;//暫停遊戲
    replayBtn.SetActive(true);
  }

  public void Replay()
  {
    Time.timeScale = 1f;
    SceneManager.LoadScene("SampleScene");
  }
}