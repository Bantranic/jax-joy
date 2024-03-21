using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class JoyCamTarget : MonoBehaviour
{
    void Update()
    {
        transform.position = (PlayerController.Player2.transform.position);

    }
}