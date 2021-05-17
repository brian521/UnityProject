using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //이동용 리지드바디2D
    private bool isJumping1;
    private bool isJumping2;
    private bool onGround;
    private bool isSliding;
    private float startTime = 0;

    public AudioClip audioJump;
    public AudioClip audioSlide;
    public AudioClip audioDamaged;

    AudioSource audioSource;

    Animator animator; // 애니메이터

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        Debug.Log("Start");
        rigid = GetComponent<Rigidbody2D>();
        isJumping1 = false;
        isJumping2 = false;
        onGround = true;
        isSliding = false;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float hor = Input.GetAxis("Horizontal"); //수평이동

        rigid.velocity = new Vector2(hor * 4.0f, rigid.velocity.y);

        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal")); // x좌표값이 양수로 가면 walk 애니메이션

        if (Input.GetKeyDown(KeyCode.Space)) //스페이스가 눌렸을 때
        {
            onGround = false;

            if (!isJumping1)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 6.0f);
                isJumping1 = true;
                animator.SetBool("Jump1", true); // 1단 점프 애니메이션
                PlaySound("JUMP");
            }
            else
            {
                if(!isJumping2)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, 6.0f);
                    isJumping2 = true;
                    animator.SetBool("Jump2", true); // 2단 점프 애니메이션
                    PlaySound("JUMP");
                }
                else
                {
                    return;
                }
            }     
        }

        if(Input.GetKey(KeyCode.LeftShift) && onGround == true) // 슬라이딩
        {
            animator.SetBool("Slide", true);
            if (!isSliding)
            {
                PlaySound("SLIDE");
            }
            isSliding = true;
            GetComponent<BoxCollider2D>().offset = new Vector2(0.1f, -0.55f);
            GetComponent<BoxCollider2D>().size = new Vector2(1.1f, 0.6f);
        }
        else
        {
            animator.SetBool("Slide", false);
            isSliding = false;
            GetComponent<BoxCollider2D>().offset = new Vector2(-0.1f, -0.23f);
            GetComponent<BoxCollider2D>().size = new Vector2(1f, 1.3f);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //바닥에 닿으면
        if (col.gameObject.CompareTag("Ground"))
        {
            //점프가능 초기화
            Debug.Log("Jump Reset");
            isJumping1 = false;
            isJumping2 = false;
            onGround = true;
            animator.SetBool("Jump1", false);  // 1단 점프 애니메이션 종료
            animator.SetBool("Jump2", false);  // 2단 점프 애니메이션 종료

        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Obstacle"))
        {
            if (Time.time - startTime < 1)
            {

            }
            else
            {
                startTime = Time.time;
                animator.SetTrigger("Collision");
                PlaySound("DAMAGED");
            }
        }
    }


    void PlaySound(string action)
    {
        switch(action)
        {
            case "JUMP":
                audioSource.clip=audioJump;
                break;
            case "SLIDE":
                audioSource.clip=audioSlide;
                break;
            case "DAMAGED":
                audioSource.clip = audioDamaged;
                break;
        }
        audioSource.Play();
    }
}
