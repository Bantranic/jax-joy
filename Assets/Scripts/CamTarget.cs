using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    public EntityHealth Player1;
    public EntityHealth Player2;

    void Update()
    {
        if (Player1.state != EDamageState.Death && Player2.state != EDamageState.Death)
            transform.position = (PlayerController.Player1.transform.position + PlayerController.Player2.transform.position) / 2;
        else if (Player1.state == EDamageState.Death)
        { transform.position = PlayerController.Player2.transform.position;}
        else if (Player2.state == EDamageState.Death)
        { transform.position = PlayerController.Player1.transform.position; }
    }
}