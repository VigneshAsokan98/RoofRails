using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 12f;

    Vector2 PrevTouchPostion;
    float SlideMagnitude;
    [Range(1f,10f)]
    public float TouchSens = 1f;

    [SerializeField]
    Transform bar;

    Animator animator;

    [SerializeField]
    Transform[] BarTips;

    public static PlayerController instance;

    bool isGrounded = true;
    public bool startGame = false;
    public bool isgameOver = false;

    public float force= 10000f;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        transform.position = new Vector3(0, -2f, 10);
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("IsGrounded", isGrounded);
        animator.SetBool("OnRail", false);
    }
    public void FixedUpdate()
    {
        if (!startGame || isgameOver)
            return;
        transform.position += speed * transform.forward * Time.deltaTime;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y + 12.5f, transform.position.z - 13f);
        if (isGrounded)
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        }
        else
        {
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationX;
        }
    }

    internal void ResetPlayer()
    {
        transform.position = new Vector3(0, -2f, 10);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
        bar.localScale = new Vector3(0.4f, 3f, 0.4f);
    }

    private void Update()
    {
        if (!startGame || isgameOver)
            return;
#if UNITY_EDITOR
        float x = Input.GetAxis("Horizontal");
        Vector3 _pos = transform.position;
        _pos += x * transform.right * Time.deltaTime * TouchSens * 10;
        _pos.x = Mathf.Clamp(_pos.x, -5.8f, 5.8f);
        transform.position = _pos;
#endif
#if UNITY_ANDROID
        SlideMagnitude = 0;
        if (Input.touchCount == 0)
            return;
        
        Touch touch = Input.GetTouch(0); 
        
        if(touch.phase == TouchPhase.Began)
            PrevTouchPostion = touch.position;

        SlideMagnitude = PrevTouchPostion.x - touch.position.x;
        Debug.Log(SlideMagnitude);
        Vector3 pos = transform.position;
        pos -= SlideMagnitude * transform.right * Time.deltaTime * TouchSens;
        pos.x = Mathf.Clamp(pos.x, -5.8f, 5.8f);
        transform.position = pos;
        PrevTouchPostion = touch.position;
        Debug.Log("Android");        
#endif
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Ground"))
        {
            isGrounded = true;
            animator.SetBool("IsGrounded", isGrounded);
            Debug.Log("Player hit Ground");
        }

        if (collision.collider.CompareTag("Rail"))
        {
            animator.SetBool("OnRail", true);
        }
        if (collision.collider.CompareTag("FinishLine"))
        {
            Debug.Log("Collider ::" + collision.gameObject.name);
            float distance = Vector3.Distance(transform.position, collision.transform.position);
            int multiplier = (int)distance / 6;
            if(multiplier == 0)
                multiplier = 1;
            GameManager.instance.LevelComplete(multiplier);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            isGrounded = false;
            animator.SetBool("IsGrounded", isGrounded);
        }
        if(collision.collider.CompareTag("Rail"))
        {
            animator.SetBool("OnRail", false);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Adder"))
        {
            bar.localScale += new Vector3(0, 1, 0);
            Destroy(other.gameObject);
        }
        if(other.CompareTag("FallTrigger"))
        {
            Debug.Log("Player Fall");
            GameManager.instance.GameOver();
        }
        if (other.CompareTag("Trampoline"))
        {
            Debug.Log("Player Trampoline");
            GetComponent<Rigidbody>().AddForce((transform.forward + transform.up) * force);
            Destroy(other.gameObject);
        }
        if (other.CompareTag("Gem"))
        {
            GameManager.instance.GemCollected();
            Destroy(other.gameObject);
        }
        if (other.CompareTag("FireTile"))
        {
            bar.localScale -= new Vector3(0, 0.5f, 0);
            if (bar.localScale.y <= 0)
                GameManager.instance.GameOver();
        }
        if (other.CompareTag("Saw"))
        {
            Debug.Log("SawHIt");
            float BarSize = Vector3.Distance(BarTips[0].position, BarTips[1].position);
            float fraction = Vector3.Distance(transform.position, other.GetComponentInChildren<Transform>().position);
            float cutRatio = 1 - (fraction / BarSize);
            bar.localScale = new Vector3(bar.localScale.x, bar.localScale.y * cutRatio, bar.localScale.z);
        }

        if (other.CompareTag("Bullet"))
        {
            GameManager.instance.GameOver();
        }
    }
}
