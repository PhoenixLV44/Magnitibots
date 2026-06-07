using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Ability.Object;

namespace Ability
{
    public class Smash : Parent
    {
        private GameObject _smashObjectPrefab;
        private GameObject _smashBall;
        private SmashObject _smashObjectScript;
        private void Start()
        {
            InitializeAbility();
        }

        public override void Activate()
        {
            //base.Activate();
        }

        public override IEnumerator Charge()
        {
            while (isCharging)
            {
                yield return null;
            }
        }

        public override void Fire()
        {
            //base.Fire();
        }

        public override void InitializeAbility()
        {
            base.InitializeAbility();
            _smashObjectPrefab = Resources.Load<GameObject>("Prefabs/SmashPrefab");
            _smashBall = Instantiate(_smashObjectPrefab);
            _smashObjectScript = _smashBall.GetComponent<SmashObject>();
        }
    }   
}