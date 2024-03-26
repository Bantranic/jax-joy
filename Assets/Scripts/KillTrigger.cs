using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider TheOneWhoMightDie)
    {
        var Health = TheOneWhoMightDie.gameObject.GetComponent<EntityHealth>();
        if (Health == null)
            return;
        Health.ApplyDamage(99999999, gameObject, EDamageType.StrongFist); //whoever this is, they are being PUNCHED 99999999 times in a single frame i hope you know this.
    }
}
