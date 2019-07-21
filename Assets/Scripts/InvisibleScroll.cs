using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleScroll : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<PlayerController>().MakePlayerInvisible();
        //AudioSource.PlayClipAtPoint(sfxScrollPickUp, Camera.main.transform.position);
        Destroy(gameObject);
    }
}
