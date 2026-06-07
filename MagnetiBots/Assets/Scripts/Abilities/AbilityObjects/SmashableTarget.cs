using System;
using UnityEngine;

namespace Ability.Object
{
    public class SmashableTarget : MonoBehaviour
    {
        enum HealthLevelEnum
        {
            Low,
            Medium,
            High
        }

        [SerializeField] private HealthLevelEnum healthLevel;

    }
}
