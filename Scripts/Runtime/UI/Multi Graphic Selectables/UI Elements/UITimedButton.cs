using UnityEngine;

namespace JvLib.UI
{
    [AddComponentMenu("JvLib/UI/Buttons/Timed Button")]
    public class UITimedButton : UIButton
    {
        [Tooltip("Number of seconds before the button is re-enabled after a press")]
        [SerializeField] private float _Timeout;
        private float _disabledTime;

        protected override void Awake()
        {
            base.Awake();
            _disabledTime = Time.time;
            _OnClick.AddListener(() => _disabledTime = Time.time + _Timeout);
        }
        public override bool IsInteractable() => base.IsInteractable() && (Time.time >= _disabledTime);

        public float TimeoutProgress() => 1f - Mathf.Clamp01((_disabledTime - Time.time) / _Timeout);
    }
}
