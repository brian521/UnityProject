using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCtrl : MonoBehaviour
{
    public GameObject Player;  //A��� GameObject���� ����
    Transform AT;
    void Start()
    {
        AT = Player.transform;
    }
    void LateUpdate()
    {
        transform.position = new Vector3(AT.position.x + 4, transform.position.y, transform.position.z);
    }
}