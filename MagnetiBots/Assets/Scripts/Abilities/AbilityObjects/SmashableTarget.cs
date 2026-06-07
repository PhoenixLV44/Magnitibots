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

        private int _health;

        private void Start()
        {
            switch (healthLevel)
            {
                case HealthLevelEnum.Low:
                    _health = 1;
                    break;
                case HealthLevelEnum.Medium:
                    _health = 2;
                    break;
                case HealthLevelEnum.High:
                    _health = 3;
                    break;
            }
        }
    }
}
