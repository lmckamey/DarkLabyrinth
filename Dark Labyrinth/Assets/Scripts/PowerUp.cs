using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public enum ePower_Up
{
    EXTRA_CHARGE,
    RELOAD,
    INVISIBILITY,
    SPEED_BOOST,
    GOAL
}
public class PowerUp : MonoBehaviour
{
    public ePower_Up m_PowerUpType;
}
