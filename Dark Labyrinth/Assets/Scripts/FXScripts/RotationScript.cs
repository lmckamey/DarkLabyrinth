using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationScript : MonoBehaviour
{

    [SerializeField] float m_rotationSpeed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        Vector3 rotate = Vector3.zero;
        rotate.y = 1.0f;
        transform.Rotate(rotate, m_rotationSpeed);
    }
}
