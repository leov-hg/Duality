using MyBox;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Separator("Separator", true)]
    [SerializeField] private PlayerRef[] players;
    [SerializeField] private Transform camTarget;

    [Separator("Settings", true)]
    [SerializeField] private float camTargetSmoothSpeed = 5;

    private Vector3 playersCentroid;


    private void LateUpdate()
    {
        int _playersNumber = 1;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playerBase != null)
            {
                playersCentroid += players[i].playerBase.transform.position;
                _playersNumber++;
            }
        }

        playersCentroid /= _playersNumber;

        camTarget.position = Vector3.Lerp(camTarget.position, playersCentroid, Time.deltaTime * camTargetSmoothSpeed);
    }
}
