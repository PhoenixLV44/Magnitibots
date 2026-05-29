using UnityEngine;
using Player.Abilities;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement movement;
        LassoAbility lassoAbility;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            movement = gameObject.AddComponent<Player.Movement>();
            lassoAbility = gameObject.AddComponent<LassoAbility>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
