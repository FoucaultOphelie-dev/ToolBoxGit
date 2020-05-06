using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;

namespace ToolBoxEngine
{
    public class MoveArround2D : MonoBehaviour
    {
        [SerializeField]
        [FormerlySerializedAs("radius")]
        private float _radius = 1f;

        public float speed = 90f;

        private float _angle = 0f;

        private Vector3 _center = Vector3.zero;

        public Vector3 GetCenter()
        {
            return _center;
        }
        void Start()
        {
            _center = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            _angle += speed * Time.deltaTime;
            _angle = _angle % 360;

            float radAngle = _angle * Mathf.Deg2Rad;
            Vector3 newPosition = transform.position;
            newPosition.x = _radius * Mathf.Cos(radAngle) + _center.x;
            newPosition.y = _radius * Mathf.Sin(radAngle) + _center.y;
            transform.position = newPosition;
        }

#if UNITY_EDITOR

        public bool guiDebug = false;
        public Color guiDebugTextColor = Color.white;
        public int guiDebugTextSize = 24;
        private GUIStyle guiStyle = null;

        public bool gizmoDebug = false;
        public Color gizmosCenterColor = Color.green;
        public Color gizmosPositionColor = Color.blue;
        public Color gizmosLineColor = Color.red;
        private void OnGUI()
        {
            if (!guiDebug) { return; }

            if (null == guiStyle)
            {
                guiStyle = new GUIStyle();

            }
            guiStyle.fontSize = guiDebugTextSize;
            guiStyle.normal.textColor = guiDebugTextColor;
            GUILayout.BeginVertical();
            GUILayout.Label("Radius = " + _radius, guiStyle);
            GUILayout.Label("Speed = " + speed, guiStyle);
            GUILayout.Label("Angle = " + _angle, guiStyle);
            GUILayout.EndVertical();
        }

        private void OnDrawGizmos()
        {
            if (!gizmoDebug) { return; }

            //Draw the position of the center
            Gizmos.color = gizmosCenterColor;
            Gizmos.DrawWireSphere(_center, 0.1f);

            //Draw the position of the object
            Gizmos.color = gizmosPositionColor;
            Gizmos.DrawWireSphere(transform.position, 0.1f);

            //Draw line bestween our two points
            Gizmos.color = gizmosLineColor;
            Gizmos.DrawLine(_center, transform.position);
        }
#endif

    }
}
