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
        public GameObject rightWheel;
        public GameObject leftWheel;
        private ArticulationBody wA1;
        private ArticulationBody wA2;
        public float maxLinearSpeed = 0.5f; //  m/s
        public float maxRotationalSpeed = 2.84f;//
        public float wheelRadius = 0.0762f; //meters
        public float trackWidth = 0.27918f; // meters Distance between tyres
        public float forceLimit = 657f;
        public float damping = 10;
        private float lastCmdReceived = 0f;
        public GameObject ROSConnectionCommon;
        private Commons commons;
        private RotationDirection direction;
        private float rosLinear = 0f;
        private float rosAngular = 0f;
        private float k1, k2, k3;

        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            commons.ros = ROSConnection.GetOrCreateInstance();
            commons.ros.Subscribe<TwistMsg>(cmdVelTopicName, ReceiveROSCmd);
            wA1 = rightWheel.GetComponent<ArticulationBody>();
            wA2 = leftWheel.GetComponent<ArticulationBody>();
            k1 = trackWidth / 2;
            k2 = Mathf.Rad2Deg / wheelRadius;
            k3 = ((2 * maxLinearSpeed) / wheelRadius) * Mathf.Rad2Deg;
            SetParameters(wA1);
            SetParameters(wA2);
            
        }

        void ReceiveROSCmd(TwistMsg cmdVel)
        {
            rosLinear = (float)cmdVel.linear.x;
            rosAngular = (float)cmdVel.angular.z;
            lastCmdReceived = Time.time;
        }

        void FixedUpdate()
        {
            ROSUpdate();
        }

        private void SetParameters(ArticulationBody joint)
        {
            ArticulationDrive drive = joint.xDrive;
            drive.forceLimit = forceLimit;
            drive.damping = damping;
            joint.xDrive = drive;
        }

        private void SetSpeed(ArticulationBody joint, float wheelSpeed = float.NaN)
        {
            ArticulationDrive drive = joint.xDrive;
            if (float.IsNaN(wheelSpeed))
            {
                Debug.Log("NaN");
                drive.targetVelocity = k3 * (int)direction;
            }
            else
            {
                drive.targetVelocity = wheelSpeed;
            }
            joint.xDrive = drive;
        }


        private void ROSUpdate()
        {
            RobotInput(rosLinear, -rosAngular);
        }

        private void RobotInput(float speed, float rotSpeed) // m/s and rad/s
        {
            if (speed > maxLinearSpeed) speed = maxLinearSpeed;
            if (rotSpeed > maxRotationalSpeed) rotSpeed = maxRotationalSpeed;
            float rightRotation = (speed - k1 * rotSpeed) * k2;
            float leftRotation = (k1 * rotSpeed + speed) * k2;

            SetSpeed(wA1, rightRotation*1.25f); // articulation object(wheel), angular velocity(deg/s)
            SetSpeed(wA2, leftRotation*1.25f);
        }
    }
}
