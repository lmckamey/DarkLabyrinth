using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] GameObject target = null;
    [SerializeField] float m_smoothSpeed = 0.25f;
    [SerializeField] float m_rotationSpeed = 2.0f;


    public Vector3 offset;
    private void Start()
    {
        Vector3 center = new Vector3(Screen.width / 2, Screen.height / 2);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void Update()
    {
        if (target != null)
        {


            Quaternion rotationQuat = Quaternion.Euler(0.0f, (Input.GetAxisRaw("Mouse X")) * m_rotationSpeed, 0.0f);
            offset = rotationQuat * offset;

            Vector3 desiredPos = target.transform.position + offset;
            Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, m_smoothSpeed);
            transform.position = smoothedPos;

            transform.LookAt(target.transform);


            //Prevent wall Clipping
            RaycastHit ray = new RaycastHit();
            Ray ray2 = new Ray(target.transform.position, smoothedPos - target.transform.position);

            if (Physics.Raycast(ray2, out ray, 5.0f))
            {
                if (ray.distance < 5.0f)
                {
                    Vector3 dir = (ray.point - target.transform.position) * 0.95f;
                    transform.position = target.transform.position + dir;
                }
            }
        }


    }
}
