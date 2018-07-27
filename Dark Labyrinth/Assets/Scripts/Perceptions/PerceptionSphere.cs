using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerceptionSphere : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 90.0f)] float m_FOV = 70.0f;
    [SerializeField] [Range(1.0f, 100.0f)] float m_radius = 5.0f;
    [SerializeField] LayerMask m_layers;

    public GameObject GetGameObjectWithTag(string tag)
    {
        GameObject targetGO = null;

       Collider[] colliders = Physics.OverlapSphere(transform.position, m_radius, m_layers);

        foreach (var collider in colliders)
        {
                Vector3 direction = collider.gameObject.transform.position - transform.position;

                Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + (direction.normalized * m_radius), Color.red);
        }

        foreach (Collider collider in colliders)
        {
            if (collider.tag == tag)
            {
                Vector3 direction = collider.gameObject.transform.position - (transform.position + Vector3.up);
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < m_FOV)
                {
                    RaycastHit raycastHit;
                    Debug.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + (direction.normalized * m_radius), Color.blue);

                    if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out raycastHit, m_radius, m_layers))
                    {
                        if (raycastHit.collider.gameObject == collider.gameObject)
                        {
                            targetGO = collider.gameObject;
                        }
                    }
                    break;
                }
            }
        }

        return targetGO;
    }
}
