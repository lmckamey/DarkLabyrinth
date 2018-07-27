using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perception : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 90.0f)] float m_FOV = 70.0f;

    List<GameObject> m_gameObjects = new List<GameObject>();

    void Update()
    {
        foreach (GameObject go in m_gameObjects)
        {
            Vector3 direction = go.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

            Color color = (angle < m_FOV) ? Color.blue : Color.red;

            Debug.DrawLine(transform.position, go.transform.position, color);
        }
    }

    public GameObject GetGameObjectWithTag(string tag)
    {
        GameObject targetGO = null;


        foreach (GameObject go in m_gameObjects)
        {
            if (go.tag == tag)
            {
                Vector3 direction = go.transform.position - transform.position;
                float angle = Vector3.Angle(direction, transform.forward);
                if (angle < m_FOV)
                {
                    RaycastHit raycastHit;

                    if (Physics.Raycast(transform.position + Vector3.up, direction.normalized, out raycastHit))
                    {
                        if (raycastHit.collider.gameObject == go)
                        {
                            targetGO = go;
                        }
                    }
                    break;
                }
            }
        }

        return targetGO;
    }
    private void OnTriggerEnter(Collider other)
    {
        m_gameObjects.Add(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        m_gameObjects.Remove(other.gameObject);
    }
}
