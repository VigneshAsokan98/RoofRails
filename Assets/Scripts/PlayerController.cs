using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float speed = 15f;

    Vector2 PrevTouchPostion;
    float SlideMagnitude;
    [Range(1f,10f)]
    public float TouchSens = 1f;

    [SerializeField]
    Transform bar;

    Animator animator;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        animator.SetBool("IsGrounded", true);
        animator.SetBool("OnRail", false);
    }
    private void FixedUpdate()
    {
        transform.position += speed * transform.forward * Time.deltaTime;
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y + 12.5f, transform.position.z - 13f);
    }

    private void Update()
    {
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
            animator.SetBool("IsGrounded", true);
        }
        if (collision.collider.CompareTag("Rail"))
        {
            animator.SetBool("OnRail", true);
        }

    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            animator.SetBool("IsGrounded", false);
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
            other.gameObject.SetActive(false);
        }
        if(other.CompareTag("FallTrigger"))
        {
            Debug.Log("Player Fall");
            GameManager.instance.resetLevel();
        }
        if(other.CompareTag("Gem"))
        {
            Debug.Log("GemCollected");
            other.gameObject.SetActive(false);
            GameManager.instance.GemCollected();
        }
        if (other.CompareTag("FireTile"))
        {
            bar.localScale -= new Vector3(0, 0.5f, 0);
        }
    }
}
