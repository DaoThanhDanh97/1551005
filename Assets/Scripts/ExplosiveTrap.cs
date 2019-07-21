using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveTrap : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip sfxScrollPickUp;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<PlayerController>().MakePlayerDead();
        AudioSource.PlayClipAtPoint(sfxScrollPickUp, Camera.main.transform.position);
        GetComponent<Animator>().SetBool("isExplode", true);
        Destroy(gameObject, 1.0f);
    }
}
