using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStun : MonoBehaviour
{

    public float bounceBack;
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
        if (collision.gameObject.CompareTag("Player"))
        {
            print("works");
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.up * bounceBack, ForceMode2D.Impulse);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(transform.position.x * new Vector2(bounceBack,0), ForceMode2D.Impulse);
        }
    }
}
