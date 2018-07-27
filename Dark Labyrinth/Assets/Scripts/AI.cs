using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    [SerializeField] public WayPoint wayPoint = null;
    [SerializeField] [Range(1.0f, 10.0f)] public float m_speed = 1.0f;
    [SerializeField] [Range(1.0f, 2.0f)] public float m_perceptionTime = 1.0f;
    [SerializeField] public AudioClip m_alertSFX = null;
    [SerializeField] public Player m_target = null;
    [SerializeField] GameObject m_deathParticleEffect = null;
    [SerializeField] Light m_spotLight;

    StackStateMachine<AI> m_stateMachine = new StackStateMachine<AI>();
    GameObject m_targetGO = null;
    float perceptionTimer = 0.0f;
    PerceptionSphere m_perception = null;

    private bool isDisabled = false;
    private float resetTimer = 0.0f;
    private float resetTime = 25.0f;
    private float m_chaseSpeed = 0.0f;
    private float m_baseSpeed = 0.0f;
    private NavMeshAgent agent = null;

    void Start()
    {
        m_perception = GetComponent<PerceptionSphere>();

        if (!m_target)
        {
            GameObject.FindGameObjectWithTag("Player");
        }

        m_stateMachine.AddState("Alert", new AlertState<AI>(this));
        m_stateMachine.AddState("Wander", new WanderState<AI>(this));
        m_stateMachine.PushState("Wander");

        m_chaseSpeed = m_speed * 1.8f;

        agent = gameObject.GetComponent<NavMeshAgent>();

    }

    void Update()
    {
        if (isDisabled)
        {
            resetTimer += Time.deltaTime;
            if (resetTimer >= resetTime)
            {
                isDisabled = false;
                m_speed = m_speed * 1.5f;
                m_baseSpeed = m_speed;
                m_chaseSpeed = m_speed * 1.8f;
                agent.speed = m_speed;
                m_deathParticleEffect.SetActive(false);
                m_spotLight.enabled = true;
                gameObject.tag = "Guard";
                GetComponent<AudioSource>().enabled = true;
                resetTimer = 0.0f;
            }
        }
        else
        {
            m_stateMachine.Update();
            agent.isStopped = false;
        }
    }

    public void SetWayPoint(WayPoint next)
    {
        wayPoint = next;
    }

    public void Incapacitate(float delay)
    {
        if (m_deathParticleEffect != null)
        {
            m_deathParticleEffect.SetActive(true);
        }
        gameObject.tag = "Untagged";
        m_spotLight.enabled = false;
        GetComponent<AudioSource>().enabled = false;
        agent.isStopped = true;
        isDisabled = true;
    }

    class WanderState<T> : State<T> where T : AI
    {
        public WanderState(T owner) : base(owner)
        {
            //
        }
        public override void Enter()
        {
        }
        public override void Update()
        {


            if (m_owner.wayPoint != null)
            {
                m_owner.agent.SetDestination(m_owner.wayPoint.transform.position);
            }

            m_owner.perceptionTimer += Time.deltaTime;

            if (m_owner.perceptionTimer >= m_owner.m_perceptionTime)
            {
                m_owner.perceptionTimer = 0.0f;
                if (m_owner.m_perception)
                {
                    m_owner.m_targetGO = m_owner.m_perception.GetGameObjectWithTag("Player");

                    if (m_owner.m_targetGO)
                    {
                        m_owner.m_stateMachine.PushState("Alert");
                    }
                }

            }

        }

    }
    class AlertState<T> : State<T> where T : AI
    {
        public AlertState(T owner) : base(owner)
        {
            //
        }
        public override void Enter()
        {
            m_owner.m_baseSpeed = m_owner.m_speed;
            m_owner.m_speed = m_owner.m_chaseSpeed;
            m_owner.agent.speed = m_owner.m_speed;

            if (m_owner.m_alertSFX != null)
            {
                m_owner.GetComponent<AudioSource>().PlayOneShot(m_owner.m_alertSFX, 1.0f);
            }
            else
            {
                print("Sound is null.......");
            }
        }
        public override void Update()
        {

            m_owner.m_targetGO = m_owner.m_perception.GetGameObjectWithTag("Player");

            if (m_owner.m_targetGO == null)
            {
                m_owner.m_stateMachine.PopState();
            }
            else
            {
                float distance = (m_owner.m_targetGO.transform.position - m_owner.transform.position).magnitude;
                m_owner.agent.SetDestination(m_owner.m_targetGO.transform.position);

                if (!m_owner.m_targetGO)
                {
                    m_owner.m_stateMachine.PopState();
                }
            }

        }

        public override void Exit()
        {
            m_owner.m_speed = m_owner.m_baseSpeed;
            m_owner.agent.speed = m_owner.m_speed;
        }

    }

}


