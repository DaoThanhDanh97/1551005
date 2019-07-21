using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            print(FindObjectOfType<GameSession>().CheckBookAmount());
            if (FindObjectOfType<GameSession>().CheckBookAmount())
            {
                gameObject.GetComponent<Rigidbody2D>().bodyType = UnityEngine.RigidbodyType2D.Dynamic;
                gameObject.GetComponent<Rigidbody2D>().angularDrag = 0;
                gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            }
        }
    }
}
