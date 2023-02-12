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
        public GameObject rightWheel, leftWheel;
        private ArticulationBody rightWheelArticulationBody, leftWheelArticulationBody;
        private float rightRotation, leftRotation;
        private float preRightRotation = 0.0f, preLeftRotation = 0.0f;
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
        private float k1, k2;

        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            commons.ros = ROSConnection.GetOrCreateInstance();
            commons.ros.Subscribe<TwistMsg>(cmdVelTopicName, ReceiveROSCmd);
            rightWheelArticulationBody = rightWheel.GetComponent<ArticulationBody>();
            leftWheelArticulationBody = leftWheel.GetComponent<ArticulationBody>();
            k1 = trackWidth / 2;
            k2 = Mathf.Rad2Deg / wheelRadius;
            SetParameters(rightWheelArticulationBody);
            SetParameters(leftWheelArticulationBody);
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

        private void SetSpeed(ArticulationBody joint, float wheelSpeed = float.NaN, float preWheelSpeed = float.NaN)
        {
            ArticulationDrive drive = joint.xDrive;
            drive.targetVelocity = float.IsNaN(wheelSpeed) ? preWheelSpeed : wheelSpeed;
            joint.xDrive = drive;
        }


        private void ROSUpdate()
        {
            RobotInput(rosLinear, -rosAngular);
        }

        private void RobotInput(float speed, float rotSpeed) // m/s and rad/s
        {
            preRightRotation = rightRotation;
            preLeftRotation = leftRotation;

            rightRotation = (speed - k1 * rotSpeed) * k2;
            leftRotation = (k1 * rotSpeed + speed) * k2;

            SetSpeed(rightWheelArticulationBody, rightRotation, preRightRotation); // articulation object(wheel), angular velocity(deg/s)
            SetSpeed(leftWheelArticulationBody, leftRotation, preLeftRotation);
        }
    }
}
