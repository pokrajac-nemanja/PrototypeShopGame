using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopingPointController : MonoBehaviour
{
    public GameObject prompt;
    public GameObject shop;

    private GameObject player;
    private Collider2D pointCollider;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        pointCollider = GetComponent<Collider2D>();
        prompt.SetActive(false);
        shop.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (pointCollider.IsTouching(player.GetComponent<Collider2D>()) && Input.GetKeyDown(KeyCode.F))
        {
            shop.SetActive(!shop.activeSelf);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            prompt.SetActive(true);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            prompt.SetActive(false);
            shop.SetActive(false);
        }
    }
}
