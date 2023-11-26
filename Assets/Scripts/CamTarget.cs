using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    void Update()
    {
        if (PlayerController.Player2 != null)
            transform.position = (PlayerController.Player1.transform.position + PlayerController.Player2.transform.position) / 2;
    }
}