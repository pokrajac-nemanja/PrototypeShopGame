using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject shopPannel;
    public GameObject playerPannel;
    public GameObject buyButton;
    public GameObject sellButton;

    private InventoryDisplay shopInventoryDisplay;
    private InventoryDisplay playerInventoryDisplay;

    // Start is called before the first frame update
    void Start()
    {
        buyButton.transform.GetComponent<Button>().interactable = false;
        buyButton.transform.GetComponent<Button>().onClick.AddListener(Buy);

        sellButton.transform.GetComponent<Button>().interactable = false;
        sellButton.transform.GetComponent<Button>().onClick.AddListener(Sell);

        shopInventoryDisplay = shopPannel.transform.GetComponent<InventoryDisplay>();
        playerInventoryDisplay = playerPannel.transform.GetComponent<InventoryDisplay>();
    }

    // Update is called once per frame
    void Update()
    {
        if (shopInventoryDisplay.HasSelected())
        {
            buyButton.transform.GetComponent<Button>().interactable = true;
        } else
        {
            buyButton.transform.GetComponent<Button>().interactable = false;
        }

        if (playerInventoryDisplay.HasSelected())
        {
            sellButton.transform.GetComponent<Button>().interactable = true;
        } else
        {
            sellButton.transform.GetComponent<Button>().interactable = false;
        }
    }

    public void Buy()
    {
        int price = shopInventoryDisplay.GetPrice();
        if (playerInventoryDisplay.GiveMoney(price))
        {
            TradableItem item = shopInventoryDisplay.GiveItem();
            playerInventoryDisplay.TakeItem(item);
            shopInventoryDisplay.TakeMoney(price);
        }
    }

    public void Sell()
    {
        int price = playerInventoryDisplay.GetPrice();
        if (shopInventoryDisplay.GiveMoney(price))
        {
            TradableItem item = playerInventoryDisplay.GiveItem();
            shopInventoryDisplay.TakeItem(item);
            playerInventoryDisplay.TakeMoney(price);
        }
    }
}
