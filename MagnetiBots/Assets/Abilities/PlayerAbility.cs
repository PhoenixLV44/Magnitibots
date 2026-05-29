using UnityEngine;
using UnityEngine.InputSystem;

namespace Player.Abilities
{
    public class PlayerAbility : MonoBehaviour
    {
        protected InputAction activateInput;
        protected InputAction chargeInput;
        protected InputAction fireInput;
        public virtual void Activate()
        {
            throw new System.NotImplementedException();
        }
        public virtual void Charge()
        {
            throw new System.NotImplementedException();
        }
        public virtual void Fire()
        {
            throw new System.NotImplementedException();
        }
        public virtual void GetInputs()
        {
            if (activateInput.)
            {
                Activate();
            }

            if (chargeInput.IsPressed())
            {
                Charge();
            }
            
            if (chargeInput.)
            {
                Fire();
            }
            
        }
    }
}