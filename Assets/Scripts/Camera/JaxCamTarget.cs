using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class JaxCamTarget : MonoBehaviour
{
    void Update()
    {
        transform.position = (PlayerController.Player1.transform.position);

    }
}