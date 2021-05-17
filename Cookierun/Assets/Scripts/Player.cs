using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rigid; //�̵��� ������ٵ�2D
    private bool isJumping1;
    private bool isJumping2;
    private bool onGround;
    private bool isSliding;
    private float startTime = 0;

    public AudioClip audioJump;
    public AudioClip audioSlide;
    public AudioClip audioDamaged;

    AudioSource audioSource;

    Animator animator; // �ִϸ�����

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
        float hor = Input.GetAxis("Horizontal"); //�����̵�

        rigid.velocity = new Vector2(hor * 4.0f, rigid.velocity.y);

        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal")); // x��ǥ���� ����� ���� walk �ִϸ��̼�

        if (Input.GetKeyDown(KeyCode.Space)) //�����̽��� ������ ��
        {
            onGround = false;

            if (!isJumping1)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, 6.0f);
                isJumping1 = true;
                animator.SetBool("Jump1", true); // 1�� ���� �ִϸ��̼�
                PlaySound("JUMP");
            }
            else
            {
                if(!isJumping2)
                {
                    rigid.velocity = new Vector2(rigid.velocity.x, 6.0f);
                    isJumping2 = true;
                    animator.SetBool("Jump2", true); // 2�� ���� �ִϸ��̼�
                    PlaySound("JUMP");
                }
                else
                {
                    return;
                }
            }     
        }

        if(Input.GetKey(KeyCode.LeftShift) && onGround == true) // �����̵�
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
        //�ٴڿ� ������
        if (col.gameObject.CompareTag("Ground"))
        {
            //�������� �ʱ�ȭ
            Debug.Log("Jump Reset");
            isJumping1 = false;
            isJumping2 = false;
            onGround = true;
            animator.SetBool("Jump1", false);  // 1�� ���� �ִϸ��̼� ����
            animator.SetBool("Jump2", false);  // 2�� ���� �ִϸ��̼� ����

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
