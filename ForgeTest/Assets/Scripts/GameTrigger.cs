using System.Collections;
using System.Collections.Generic;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

public class GameTrigger : MonoBehaviour
{
    private bool started;
    
    void Update()
    {
        if (FindObjectOfType<GameBall>() != null)
            Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (started)
            return;

        if (!NetworkManager.Instance.IsServer)
            return; 

        Player player = other.GetComponent<Player>();

        if (player == null)
            return;

        started = true;

        GameBall ball = NetworkManager.Instance.InstantiateGameBallNetworkObject() as GameBall;

        ball.Reset();

        Destroy(gameObject);
    }
}
