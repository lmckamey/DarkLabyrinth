using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour {

    [SerializeField] WayPoint m_NextWayPoint = null;

    private void OnTriggerEnter(Collider other)
    {
       AI ai = other.gameObject.GetComponent<AI>();

       if(ai)
       {
            ai.SetWayPoint(m_NextWayPoint);
       }
    }
}
