using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraTriggers : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera[] _cameraSettings;
    [SerializeField] private int _whichCamera;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (var c in _cameraSettings)
            {
                c.Priority = 10; //resets camera priority to 10
            }

            _cameraSettings[_whichCamera].Priority = 15;
        }
    }
}
