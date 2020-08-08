using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDiesWhenTouched : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().PlayerDies();
            collision.GetComponent<Rewind>().currentRewindAmount = 500f;
        }
    }
}
