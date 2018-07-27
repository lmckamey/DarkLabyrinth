using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Game : MonoBehaviour
{
    [SerializeField] GameObject[] m_powerUpPrefabs;
    [SerializeField] GameObject[] m_doors;
    [SerializeField] DoorSwitch[] m_doorSwitches;
    [SerializeField] Transform[] m_spawnPoints;
    [SerializeField] Transform[] m_powerUpLocations;
    [SerializeField] AudioClip m_proximityMusic;
    [SerializeField] GameObject m_player;
    [SerializeField] GameObject m_pauseMenu;
    [SerializeField] GameObject m_HUDText;
    [SerializeField] int m_numOfSpeedBoosts = 2;
    [SerializeField] int m_numOfReloads = 7;
    [SerializeField] int m_numOfExtraCharges = 4;


    private float textTimer = 1.0f;

    void Start()
    {
        SetPlayerSpawn();
        SetGoalAndPowerupLocations();
        AttachSwtichesToDoors();
        Time.timeScale = 1.0f;

    }

    private void SetPlayerSpawn()
    {
        int num = Random.Range(0, m_spawnPoints.Length);
        m_player.transform.position = m_spawnPoints[num].position;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            m_pauseMenu.SetActive(true);
            Time.timeScale = 0.0f;
        }
    }

    private void SetGoalAndPowerupLocations()
    {
        //Create List of Indexes and populate it
        List<int> remainingIndices = new List<int>();
        for (int i = 0; i < m_powerUpLocations.Length; i++)
        {
            remainingIndices.Add(i);
        }

        //Set Goal Location
        int randomGoalIndex = Random.Range(0, remainingIndices.Count - 1);

        m_player.GetComponent<Player>().m_goal = Instantiate(m_powerUpPrefabs[3], m_powerUpLocations[remainingIndices[randomGoalIndex]]);
        remainingIndices.Remove(remainingIndices[randomGoalIndex]);

        //Set Reload Locations
        for (int i = 0; i < m_numOfReloads; i++)
        {
            int randomNum = Random.Range(0, remainingIndices.Count - 1);

            Instantiate(m_powerUpPrefabs[1], m_powerUpLocations[remainingIndices[randomNum]]);
            remainingIndices.Remove(remainingIndices[randomNum]);
        }
        //Set Extra Charge Locations
        for (int i = 0; i < m_numOfExtraCharges; i++)
        {

            int randomNum = Random.Range(0, remainingIndices.Count - 1);

            Instantiate(m_powerUpPrefabs[0], m_powerUpLocations[remainingIndices[randomNum]]);
            remainingIndices.Remove(remainingIndices[randomNum]);
        }
        //Set Speed Boost Locations
        for (int i = 0; i < m_numOfSpeedBoosts; i++)
        {

            int randomNum = Random.Range(0, remainingIndices.Count - 1);

            Instantiate(m_powerUpPrefabs[2], m_powerUpLocations[remainingIndices[randomNum]]);
            remainingIndices.Remove(remainingIndices[randomNum]);
        }


    }

    private void AttachSwtichesToDoors()
    {

        for (int i = 0; i < m_doors.Length; i++)
        {
            m_doorSwitches[i].DoorObject = m_doors[i];
        }

    }
}
