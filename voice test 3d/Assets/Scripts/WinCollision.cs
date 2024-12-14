using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinCollision : MonoBehaviour
{
    public GameObject player;
    public GameObject WinUI;

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision player)
    {
        WinUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
