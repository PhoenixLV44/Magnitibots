using UnityEngine;
using Player.States;

namespace Player
{
    public class Controller : MonoBehaviour
    {
        Player.Movement _movement;

        #region Scripts
        private Ability.Lasso _lassoAbility;
        public Ability.Lasso LassoAbility { get { return _lassoAbility; } }
        
        private Ability.Smash _smashAbility;
        public Ability.Smash SmashAbility { get { return _smashAbility; } }
        
        private Ability.Propeller _propellerAbility;
        public Ability.Propeller PropellerAbility { get { return _propellerAbility; } }
        
        private TargetingCursor _targetCursor;
        public TargetingCursor TargetCursor { get { return _targetCursor; } }
            #endregion

        #region States
        Player.States.PlayerStateManager _playerStateManager;
        Ability.StateManager  _abilityStateManager;
            #endregion
            
        private bool _lassoHooked = false;
        public bool  LassoHooked { get => _lassoHooked; set => _lassoHooked = value; }
        
        private RangeIndicator _rangeIndicator;
        public RangeIndicator RangeIndicator { get { return _rangeIndicator; } }
            
        void Start()
        {
            _movement = gameObject.AddComponent<Player.Movement>();
            Quaternion _cameraAdjust = Quaternion.Euler(0,FindFirstObjectByType<Player.Camera>().gameObject.transform.rotation.eulerAngles.y,0);
            _movement.adjustedMovement = _cameraAdjust;

            _targetCursor = gameObject.AddComponent<TargetingCursor>();
            
            _playerStateManager = gameObject.AddComponent<PlayerStateManager>();
            _abilityStateManager = gameObject.AddComponent<Ability.StateManager>();
            
            _playerStateManager.PlayerController = this;
            
            _playerStateManager.PlayerMovement = _movement;
            
            _abilityStateManager.PlayerController = this;
            
            _rangeIndicator = gameObject.AddComponent<RangeIndicator>();

        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OnGUI()
        {
            GUI.skin.label.fontSize = 20;
            GUILayout.BeginArea(new Rect(20, 20, 1000, 500));
            GUILayout.Label("Player State: "  + _playerStateManager.PlayerStateMachine.CurrentState.ToString());
            GUILayout.Label("Current Ability: " + _abilityStateManager.StateMachine.CurrentState.ToString());
            GUILayout.Label("Charge Level: " + _abilityStateManager.StateMachine.CurrentState.Ability.CurrentPowerLevel);
            GUILayout.EndArea();
        }
    }
}
