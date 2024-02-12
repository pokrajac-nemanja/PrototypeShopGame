using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public InventoryObject inventory;
    public AudioSource audioSrc;
    public Animator playerBodyAnimator;

    private float baseSpeed = 5;
    private float runModifier = 2;
    private float speed;
    private Vest equipedOutfit;
    private Hat equipedHat;
    private Animator hatAnimator;

    // Start is called before the first frame update
    void Start()
    {
        speed = baseSpeed;

        var playerBody = transform.GetChild(0);
        if (inventory.equipedOutfit)
        {
            var outfitSlot = playerBody.GetChild(0);
            if (outfitSlot.childCount > 0)
            {
                for (int i = 0; i < outfitSlot.childCount; i++)
                {
                    Destroy(outfitSlot.GetChild(i).gameObject);
                }
            }
            var newOutfit = Instantiate(inventory.equipedOutfit.prefab);
            newOutfit.transform.parent = outfitSlot;
            equipedOutfit = inventory.equipedOutfit;
        }

        if (inventory.equipedHat)
        {
            var hatSlot = playerBody.GetChild(2);
            if (hatSlot.childCount > 0)
            {
                for (int i = 0; i < hatSlot.childCount; i++)
                {
                    Destroy(hatSlot.GetChild(i).gameObject);
                }
            }
            var newHat = Instantiate(inventory.equipedHat.prefab);
            newHat.transform.parent = hatSlot;
            hatAnimator = newHat.GetComponent<Animator>();
            equipedHat = inventory.equipedHat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSpeed();
        UpdateMovement();

        var playerBody = transform.GetChild(0);
        if (equipedOutfit != inventory.equipedOutfit)
        {
            Destroy(playerBody.GetChild(0).GetChild(0).gameObject);
            var newOutfit = Instantiate(inventory.equipedOutfit.prefab);
            newOutfit.transform.parent = playerBody.GetChild(0);
            newOutfit.transform.localPosition = Vector2.zero;
            equipedOutfit = inventory.equipedOutfit;
        }

        if (equipedHat != inventory.equipedHat)
        {
            Destroy(playerBody.GetChild(2).GetChild(0).gameObject);
            var newHat = Instantiate(inventory.equipedHat.prefab);
            newHat.transform.parent = playerBody.GetChild(2);
            newHat.transform.localPosition = Vector2.zero;
            hatAnimator = newHat.GetComponent<Animator>();
            equipedHat = inventory.equipedHat;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var collectedItem = collision.GetComponent<WorldItem>();
        if (collectedItem)
        {
            audioSrc.Play();
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
        int direction = 0;

        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector2.up * Time.deltaTime * speed);
            direction = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector2.down * Time.deltaTime * speed);
            direction = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector2.left * Time.deltaTime * speed);
            direction = 3;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector2.right * Time.deltaTime * speed);
            direction = 4;
        }

        playerBodyAnimator.SetInteger("Direction", direction);
        if (hatAnimator != null)
        {
            hatAnimator.SetInteger("Direction", direction);
        }
        Camera.main.transform.position = new Vector3(transform.position.x, transform.position.y, -10); 
    }

    private void OnApplicationQuit()
    {
        inventory.Clear();
    }
}
