using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PannelToggle : MonoBehaviour
{
    public GameObject pannel;

    // Start is called before the first frame update
    void Start()
    {
        pannel.SetActive(false);
    }

    public void TogglePannel ()
    {
        bool isActive = pannel.activeSelf;
        pannel.SetActive(!isActive);
    }
}
