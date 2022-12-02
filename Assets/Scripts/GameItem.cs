using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameItem : MonoBehaviour
{
    public Item item;

    private void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Renderer>().enabled && collision.gameObject.tag == "Player")
        {
            GameController.instance.Add(item);
            Destroy(this.gameObject);
        }
    }
}
