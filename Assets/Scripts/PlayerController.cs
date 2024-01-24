using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryObject inventory;

    private float baseSpeed = 5;
    private float runModifier = 2;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        speed = baseSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        UpdateMovement();
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var collectedItem = collision.GetComponent<WorldItem>();
        if (collectedItem)
        {
            inventory.AddItem(collectedItem.item);
            Destroy(collision.gameObject);
        }
    }

    private void UpdateSpeed()
    {
        speed = baseSpeed;
        if(Input.GetKey(KeyCode.LeftShift))
        {
            speed *= runModifier;
        }
    }

    private void UpdateMovement()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
        }
    }

    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
    }
}
