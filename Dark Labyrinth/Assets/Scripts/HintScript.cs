using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintScript : MonoBehaviour
{

    public GameObject Target { get; set; }

    [SerializeField] float m_speed = 10.0f;


    private Vector3 m_startPos = Vector3.zero;


    private void Start()
    {
        m_startPos = transform.localPosition;
    }

    void Update()
    {

        if (Target != null)
            transform.LookAt(Target.transform.position);

        //transform.localPosition = Vector3.Lerp(m_startPos, m_startPos+transform.forward, Mathf.Cos(Time.time));
    }
}
