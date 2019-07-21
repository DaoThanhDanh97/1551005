using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiController : MonoBehaviour
{
    [SerializeField] float throwSpeed = 30f;
    Rigidbody2D rigidbody2D;

    [SerializeField] EnemyController enemyController;

    GameObject cloneGameObj;

    bool isTouchGround = false;

    Collider2D[] touchedColliders;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        touchedColliders = new Collider2D[0];
    }

    // Update is called once per frame
    void Update()
    {
        CheckTouch();
    }

    public void IsThrown(float positionX, float positionY, bool isFacingRight)
    {
        int rotationValue = (isFacingRight) ? -90 : 90;

        cloneGameObj = Instantiate(gameObject, new Vector2(positionX, positionY), Quaternion.Euler(0, 0, rotationValue));

        Vector2 throwVector = (isFacingRight) ? (Vector2.right * throwSpeed) : (Vector2.left * throwSpeed);

        cloneGameObj.GetComponent<Rigidbody2D>().AddForce(throwVector);
    }

    void CheckTouch()
    {
        if (rigidbody2D.IsTouchingLayers(LayerMask.GetMask("Enemy", "Ground")))
        {
            rigidbody2D.velocity = Vector2.zero;

            if (rigidbody2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
            {
                isTouchGround = true;
            }

            if (rigidbody2D.IsTouchingLayers(LayerMask.GetMask("Enemy")) && isTouchGround) {
                print(rigidbody2D.GetAttachedColliders(touchedColliders));
                print(touchedColliders);
            }

            StartCoroutine(SelfDestruct());
        }
    }

    IEnumerator SelfDestruct()
    {
        yield return new WaitForSecondsRealtime(2);

        Destroy(gameObject);
    }

}
