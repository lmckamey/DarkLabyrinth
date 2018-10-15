using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class GameBall : GameBallBehavior
{
    private Rigidbody rigidbodyRef;
    private GameLogic gameLogic;

    private void Awake()
    {
        rigidbodyRef = GetComponent<Rigidbody>();
        gameLogic = FindObjectOfType<GameLogic>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!networkObject.IsOwner)
        {
            transform.position = networkObject.position;
            return;
        }
        networkObject.position = transform.position;    
    }

    public void Reset()
    {
        transform.position = Vector3.up * 10.0f;
        rigidbodyRef.velocity = Vector3.zero;
        
        Vector3 force =  Vector3.zero;
        force.x = Random.Range(300, 500);
        force.z = Random.Range(300, 500);

        if (Random.value < 0.5f)
            force.x *= -1.0f;

        if (Random.value < 0.5f)
            force.z *= -1.0f;

        rigidbodyRef.AddForce(force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!networkObject.IsServer)
        {
            return;
        }

        if( collision.gameObject.GetComponent<Player>() == null)
        {
            return;
        }

        gameLogic.networkObject.SendRpc("PlayerScored", Receivers.All, collision.transform.GetComponent<Player>().Name);

        Reset();
            
    }
}
