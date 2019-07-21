using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    new Rigidbody2D rigidbody2D;
    [SerializeField] float runSpeed = 3f;
    Animator myAnimator;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 3f;
    [SerializeField] AudioClip fallSfx;

    CapsuleCollider2D capsuleCollider2D;

    [SerializeField] GameObject backgroundObject;

    [SerializeField] float delayLoadNextLevel = 2f;

    [SerializeField] KunaiController kunaiController;

    BoxCollider2D feetCollider;

    bool isAlive = true;

    float gravityScaleAtStart;

    bool isInvisible = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravityScaleAtStart = rigidbody2D.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            return;
        }
        Run();
        ThrowWeapon();
        FlipSprite();
        Jump();
        ClimbLadder();
        Die();
        CheckOutOfBound();
        CheckCollideWithGround();
    }

    void Run()
    {
        float horizontalValue = CrossPlatformInputManager.GetAxis("Horizontal"); //-1 => 1
        Vector2 playerVerticalVelocity = new Vector2(horizontalValue * runSpeed, rigidbody2D.velocity.y);
        rigidbody2D.velocity = playerVerticalVelocity;

        myAnimator.SetBool("playerRunning", true);
    }

    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(rigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)   
        {
            //check x scaling based on x velocity
            transform.localScale = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y);
        }
        myAnimator.SetBool("playerRunning", playerHasHorizontalSpeed);
    }
    
    void ClimbLadder()
    {
        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder")))
        {
            myAnimator.SetBool("playerClimbing", false);
            rigidbody2D.gravityScale = gravityScaleAtStart;
            return;
        }

        float climbValue = CrossPlatformInputManager.GetAxis("Vertical");
        Vector2 climbVelocity = new Vector2(rigidbody2D.velocity.x, climbValue * climbSpeed);
        rigidbody2D.velocity = climbVelocity;
        //myAnimator.SetBool("playerClimbing", true);

        bool playerHasClimbSpeed = Mathf.Abs(rigidbody2D.velocity.y) > Mathf.Epsilon;

        if(!playerHasClimbSpeed)
        {
            rigidbody2D.gravityScale = 0;
        }
        myAnimator.SetBool("playerClimbing", playerHasClimbSpeed);
    }

    void Jump()
    {
        if (!feetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) return;

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            Vector2 addedJumpVelocity = new Vector2(0f, jumpSpeed);
            rigidbody2D.velocity += addedJumpVelocity;
        }
    }

    void Die()
    {
        if ((rigidbody2D.IsTouchingLayers(LayerMask.GetMask("Enemy")) && isInvisible == false) || capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Traps")) || feetCollider.IsTouchingLayers(LayerMask.GetMask("Traps")))
        {
            MakePlayerDead();
        }
    }

    public void MakePlayerDead()
    {
        isAlive = false;
        myAnimator.SetTrigger("playerDead");
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        Physics2D.IgnoreLayerCollision(10, 12, false);
        StartCoroutine(FindObjectOfType<GameSession>().onPlayerDead());
    }

    void CheckOutOfBound()
    {
        if (capsuleCollider2D.bounds.max.y <= backgroundObject.GetComponent<PolygonCollider2D>().bounds.min.y)
        {
            AudioSource.PlayClipAtPoint(fallSfx, Camera.main.transform.position);
            MakePlayerDead();
        }

        if (capsuleCollider2D.bounds.min.x >= backgroundObject.GetComponent<PolygonCollider2D>().bounds.max.x)
        {
            StartCoroutine(goToNextLevel());
        }
    }

    IEnumerator goToNextLevel()
    {
        yield return new WaitForSecondsRealtime(delayLoadNextLevel);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(currentSceneIndex + 1);

        FindObjectOfType<GameSession>().ChangeBookToGetAmount();
    }

    public void MakePlayerInvisible()
    {
        isInvisible = true;
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 0.3f);
        Physics2D.IgnoreLayerCollision(10, 12);

        StartCoroutine(ExecuteAfterTime(3));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        yield return new WaitForSeconds(time);

        isInvisible = false;
        GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, 1f);
        Physics2D.IgnoreLayerCollision(10, 12, false);
    }

    void ThrowWeapon()
    {
        if (CrossPlatformInputManager.GetButtonDown("Fire2"))
        {
            if (FindObjectOfType<GameSession>().GetKunaiAmount() > 0)
            {
                kunaiController.IsThrown(transform.position.x, transform.position.y, (transform.localScale.x > 0));
                FindObjectOfType<GameSession>().RemoveKunai();
            }
        }
    }

    void CheckCollideWithGround()
    {

    }
}
