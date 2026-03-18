using UnityEngine;

namespace Controller
{
    [RequireComponent(typeof(CreatureMover))]
    public class MovePlayerInput : MonoBehaviour
    {
        [Header("Character")]
        [SerializeField]
        private string m_HorizontalAxis = "Horizontal";
        [SerializeField]
        private string m_VerticalAxis = "Vertical";
        [SerializeField]
        private string m_JumpButton = "Jump";
        [SerializeField]
        private KeyCode m_RunKey = KeyCode.LeftShift;
        [Header("Input Tuning")]
        [SerializeField, Range(0.1f, 1f)]
        private float m_HorizontalSensitivity = 0.6f;
        [SerializeField, Range(0f, 0.3f)]
        private float m_InputSmoothTime = 0.08f;
        [SerializeField, Range(0f, 0.2f)]
        private float m_DeadZone = 0.05f;

        [Header("Camera")]
        [SerializeField]
        private PlayerCamera m_Camera;
        [SerializeField]
        private string m_MouseX = "Mouse X";
        [SerializeField]
        private string m_MouseY = "Mouse Y";
        [SerializeField]
        private string m_MouseScroll = "Mouse ScrollWheel";

        private CreatureMover m_Mover;

        private Vector2 m_Axis;
    private Vector2 m_AxisVelocity;
        private bool m_IsRun;
        private bool m_IsJump;

        private Vector3 m_Target;
        private Vector2 m_MouseDelta;
        private float m_Scroll;

        private void Awake()
        {
            m_Mover = GetComponent<CreatureMover>();
        }

        private void Update()
        {
            GatherInput();
            SetInput();
        }

        public void GatherInput()
        {
            var rawAxis = new Vector2(Input.GetAxis(m_HorizontalAxis), Input.GetAxis(m_VerticalAxis));
            rawAxis.x *= m_HorizontalSensitivity;

            if (rawAxis.sqrMagnitude < m_DeadZone * m_DeadZone)
            {
                rawAxis = Vector2.zero;
            }

            m_Axis = Vector2.SmoothDamp(m_Axis, rawAxis, ref m_AxisVelocity, m_InputSmoothTime);
            m_Axis = Vector2.ClampMagnitude(m_Axis, 1f);
            m_IsRun = Input.GetKey(m_RunKey);
            m_IsJump = Input.GetButton(m_JumpButton);

            m_Target = (m_Camera == null) ? transform.position + transform.forward : m_Camera.Target;
            m_MouseDelta = new Vector2(Input.GetAxis(m_MouseX), Input.GetAxis(m_MouseY));
            m_Scroll = Input.GetAxis(m_MouseScroll);
        }

        public void BindMover(CreatureMover mover)
        {
            m_Mover = mover;
        }

        public void SetInput()
        {
            if (m_Mover != null)
            {
                m_Mover.SetInput(in m_Axis, in m_Target, in m_IsRun, m_IsJump);
            }

            if (m_Camera != null)
            {
                m_Camera.SetInput(in m_MouseDelta, m_Scroll);
            }
        }
    }
}