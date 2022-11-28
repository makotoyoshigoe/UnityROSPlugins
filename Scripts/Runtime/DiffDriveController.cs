using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using TwistMsg = RosMessageTypes.Geometry.TwistMsg;
using Unity.Robotics.UrdfImporter.Control;

namespace Sample.UnityROSPlugins
{
    public class DiffDriveController : MonoBehaviour
    {
        public string cmdVelTopicName = "sim_cmd_vel";
        public enum ControlMode { Keyboard, ROS };
        public GameObject rightWheel;
        public GameObject leftWheel;
        public ControlMode mode = ControlMode.ROS;
        private ArticulationBody wA1;
        private ArticulationBody wA2;
        public float maxLinearSpeed = 0.5f; //  m/s
        public float maxRotationalSpeed = 2.84f;//
        public float wheelRadius = 0.0762f; //meters
        public float trackWidth = 0.27918f; // meters Distance between tyres
        public float forceLimit = 657f;
        public float damping = 10;
        public float casterRadius = 0.009f;
        private float ROSTimeout = 0.5f;
        private float lastCmdReceived = 0f;
        ROSConnection ros;
        private RotationDirection direction;
        private float rosLinear = 0f;
        private float rosAngular = 0f;
        private float k1, k2, k3;

        void Start()
        {
            wA1 = rightWheel.GetComponent<ArticulationBody>();
            wA2 = leftWheel.GetComponent<ArticulationBody>();
            k1 = trackWidth / 2;
            k2 = Mathf.Rad2Deg / wheelRadius;
            k3 = ((2 * maxLinearSpeed) / wheelRadius) * Mathf.Rad2Deg;
            SetParameters(wA1);
            SetParameters(wA2);
            ros = ROSConnection.GetOrCreateInstance();
            ros.Subscribe<TwistMsg>(cmdVelTopicName, ReceiveROSCmd);
            // Debug.Log("Setup end");
        }

        void ReceiveROSCmd(TwistMsg cmdVel)
        {
            rosLinear = (float)cmdVel.linear.x;
            rosAngular = (float)cmdVel.angular.z;
            // Debug.Log(rosLinear);
            // Debug.Log(rosAngular);
            lastCmdReceived = Time.time;
        }

        void FixedUpdate()
        {
            if (mode == ControlMode.Keyboard)
            {
                KeyBoardUpdate();
            }
            else if (mode == ControlMode.ROS)
            {
                ROSUpdate();
            }
        }

        private void SetParameters(ArticulationBody joint)
        {
            ArticulationDrive drive = joint.xDrive;
            drive.forceLimit = forceLimit;
            drive.damping = damping;
            joint.xDrive = drive;
        }

        private void SetParameterForCaster(WheelCollider wheel, ArticulationBody joint)
        {
            wheel.mass = joint.mass;
            wheel.radius = casterRadius;
        }

        private void SetSpeed(ArticulationBody joint, float wheelSpeed = float.NaN)
        {
            ArticulationDrive drive = joint.xDrive;
            if (float.IsNaN(wheelSpeed))
            {
                Debug.Log("NaN");
                drive.targetVelocity = k3 * (int)direction;
                //Debug.Log("speed: "+drive.targetVelocity);
            }
            else
            {
                drive.targetVelocity = wheelSpeed;
                //Debug.Log("speed: " + wheelSpeed);
            }
            joint.xDrive = drive;
        }

        private void KeyBoardUpdate()
        {
            float moveDirection = Input.GetAxis("Vertical");
            float inputSpeed;
            float inputRotationSpeed;
            if (moveDirection > 0)
            {
                inputSpeed = maxLinearSpeed;
            }
            else if (moveDirection < 0)
            {
                inputSpeed = -maxLinearSpeed;
            }
            else
            {
                inputSpeed = 0;
            }

            float turnDirction = Input.GetAxis("Horizontal");
            if (turnDirction > 0)
            {
                inputRotationSpeed = -maxRotationalSpeed;
            }
            else if (turnDirction < 0)
            {
                inputRotationSpeed = maxRotationalSpeed;
            }
            else
            {
                inputRotationSpeed = 0;
            }
            RobotInput(inputSpeed, inputRotationSpeed);
        }


        private void ROSUpdate()
        {
            if (Time.time - lastCmdReceived > ROSTimeout)
            {
                rosLinear = 0f;
                rosAngular = 0f;
            }
            RobotInput(rosLinear, -rosAngular);
        }

        private void RobotInput(float speed, float rotSpeed) // m/s and rad/s
        {
            if (speed > maxLinearSpeed) speed = maxLinearSpeed;
            if (rotSpeed > maxRotationalSpeed) rotSpeed = maxRotationalSpeed;
            float rightRotation = (k1 * rotSpeed + speed) * k2;
            float leftRotation = (speed - k1 * rotSpeed) * k2;
            // Debug.Log($"dps={rightRotation}, {leftRotation}");

            SetSpeed(wA1, rightRotation); // articulation object(wheel), angular velocity(deg/s)
            SetSpeed(wA2, leftRotation);
        }
    }
}
