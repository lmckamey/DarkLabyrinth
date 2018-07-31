using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{

    public GameObject m_goal = null;

    [SerializeField] GameObject m_hintArrow = null;
    [SerializeField] GameObject m_tryAgainMenu = null;
    [SerializeField] GameObject m_winMenu = null;
    [SerializeField] GameObject m_attackParticle = null;
    [SerializeField] GameObject m_hitParticle = null;
    [SerializeField] [Range(0.0f, 50.0f)] float m_speed = 5.0f;
    [SerializeField] [Range(0.0f, 50.0f)] float m_jumpForce = 5.0f;
    [SerializeField] Text m_AmmoUI = null;
    [SerializeField] Text m_KeysUI = null;
    [SerializeField] Text m_HUDText = null;
    [SerializeField] ParticleSystem m_LeftHeadLightning;
    [SerializeField] ParticleSystem m_RightHeadLightning;



    private bool isGrounded = true;
    private bool isAttacking = false;
    private bool isActive = true;
    private Vector3 previousOffSet = Vector3.zero;
    private Rigidbody m_rigidbody = null;
    private int m_numOfCharges = 1;
    private int m_maxNumOfCharges = 1;
    private int m_numOfKeys = 0;
    private float m_attackTimer = 0.0f;
    private float m_attackTime = 0.5f;
    private AudioSource m_audioSource = null;
    private Color m_baseLightningColor;

    public bool HasKey()
    {
        return m_numOfKeys > 0;
    }

    public void RemoveKey()
    {
        m_numOfKeys--;
    }
    public void AddKey()
    {
        m_numOfKeys++;
    }
    void Start()
    {
        m_rigidbody = gameObject.GetComponent<Rigidbody>();
        m_AmmoUI.text = m_numOfCharges + "/" + m_maxNumOfCharges;
        m_audioSource = GetComponent<AudioSource>();
        m_baseLightningColor = m_RightHeadLightning.startColor;
    }

    void Update()
    {
        if (isActive)
        {

            if (m_goal != null)
                m_hintArrow.GetComponent<HintScript>().Target = m_goal;
            m_AmmoUI.text = m_numOfCharges + "/" + m_maxNumOfCharges;
            m_KeysUI.text = "Keys: " + m_numOfKeys;
            Vector3 velocity = Vector3.zero;
            velocity.z = Input.GetAxis("Vertical");
            velocity.x = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                m_rigidbody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            //Set Player Forward based on camera
            Vector3 tempforward = Vector3.zero;

            tempforward.x = Camera.main.transform.forward.x;
            tempforward.z = Camera.main.transform.forward.z;

            transform.forward = tempforward;


            velocity = Camera.main.transform.rotation * velocity;
            velocity.y = 0.0f;

            transform.position += velocity * m_speed * Time.deltaTime;

            if (Input.GetKey(KeyCode.H))
            {
                m_hintArrow.SetActive(true);
            }
            else
            {
                m_hintArrow.SetActive(false);
            }

            if (isAttacking)
            {
                m_attackTimer += Time.deltaTime;
                if (m_attackTimer >= m_attackTime)
                {
                    isAttacking = false;
                    m_attackTimer = 0.0f;
                    m_attackParticle.SetActive(false);
                }
            }
            if (Input.GetButton("Fire2"))
            {
                m_LeftHeadLightning.startColor = Color.yellow;
                m_RightHeadLightning.startColor = Color.yellow;
                if (Input.GetButtonDown("Fire1") && m_numOfCharges > 0)
                {

                    isAttacking = true;
                    m_attackParticle.SetActive(true);
                    m_audioSource.Play();
                    RaycastHit hit = new RaycastHit();

                    Physics.Raycast(transform.position, transform.forward, out hit);
                    m_numOfCharges--;
                    Destroy(Instantiate(m_hitParticle, hit.point, Quaternion.identity),1.0f);

                    if (hit.collider.gameObject.tag == "Guard")
                    {
                        hit.collider.gameObject.GetComponent<AI>().Incapacitate(1.0f);
                    }
                }
            }
            else
            {
                m_LeftHeadLightning.startColor = m_baseLightningColor;
                m_RightHeadLightning.startColor = m_baseLightningColor;
            }
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGrounded = true;
        }

        if (collision.gameObject.tag == "Guard")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            m_tryAgainMenu.SetActive(true);
            Time.timeScale = 0.0f;
            isActive = false;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "KillBox")
        {
            Destroy(this.gameObject, 1.0f); ;
            m_tryAgainMenu.SetActive(true);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Goal")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            m_winMenu.SetActive(true);
            Time.timeScale = 0.0f;
            isActive = false;
        }

        if (other.tag == "PowerUp")
        {
            switch (other.gameObject.GetComponent<PowerUp>().m_PowerUpType)
            {
                case ePower_Up.EXTRA_CHARGE:
                    m_HUDText.enabled = true;
                    m_HUDText.text = "Gained and Extra Charge";
                    m_numOfCharges++;
                    m_maxNumOfCharges++;
                    break;
                case ePower_Up.SPEED_BOOST:
                    m_HUDText.enabled = true;
                    m_HUDText.text = "Speed increased";
                    m_speed += 5.0f;
                    break;
                case ePower_Up.RELOAD:
                    m_HUDText.enabled = true;
                    m_HUDText.text = "Charges Reset";
                    m_numOfCharges = m_maxNumOfCharges;
                    break;
                case ePower_Up.INVISIBILITY:
                    break;
            }
            Destroy(other.gameObject);
        }
    }
}
