using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rigidBody2D;

    [SerializeField] float moveSpeed;

    Animator enemyAnimator;

    bool isKilled = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody2D = GetComponent<Rigidbody2D>();
        moveSpeed = 3f;
        enemyAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isKilled == true)
        {
            return;
        }

        if (IsFacingRight()) rigidBody2D.velocity = new Vector2(moveSpeed, 0f);
        else rigidBody2D.velocity = new Vector2(-moveSpeed, 0f);

        CheckTouchThrowables();
        CheckCollideWithGate();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.localScale = new Vector2(-Mathf.Sign(rigidBody2D.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
    }

    bool IsFacingRight()
    {
        return transform.localScale.x > 0;
    }

    public void CheckTouchThrowables()
    {
        if ((rigidBody2D.IsTouchingLayers(LayerMask.GetMask("Throwables"))))
        {
            rigidBody2D.bodyType = UnityEngine.RigidbodyType2D.Static;
            enemyAnimator.SetTrigger("enemyDead");
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSecondsRealtime(1);

        Destroy(gameObject);
    }

    void CheckCollideWithGate()
    {
        if (rigidBody2D.IsTouchingLayers(LayerMask.GetMask("Gate")))
        {
            transform.localScale = new Vector2(-Mathf.Sign(rigidBody2D.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
    }
}
