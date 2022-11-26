using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using OdomMsg = RosMessageTypes.Nav.OdometryMsg;
using TfMsg = RosMessageTypes.Tf2.TFMessageMsg;

namespace Sample.UnityROSPlugins
{
    public class OdometryPublisher : MonoBehaviour
    {
        public GameObject publishOdomObject;
        private Transform publishOdomTransform;
        private ArticulationBody publishOdomArticulationBody;
        public string odometryTopicName = "odom";
        private ROSConnection ros;
        private OdomMsg odomMsg;
        private Commons common;
        public GameObject ROSConnectionCommon;
        public enum TranslateDirection{
            x, y, z
        }
        public enum RotateAxis{
            x, y, z
        }

        public TranslateDirection translateDirection = TranslateDirection.x;
        public RotateAxis rotateAxis = RotateAxis.y;
        public bool publishTf = true;
        public string tfTopicName = "tf";
        // [HideInInspector] public OdometryPublisher Setting = new OdometryPublisher();

        // Start is called before the first frame update
        void Start()
        {
            common = ROSConnectionCommon.GetComponent<Commons>();
            publishOdomTransform = publishOdomObject.GetComponent<Transform>();
            publishOdomArticulationBody = publishOdomObject.GetComponent<ArticulationBody>();
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<OdomMsg>(odometryTopicName, 1);
            odomMsg = new OdomMsg();
            odomMsg.header.frame_id = odometryTopicName;
            odomMsg.child_frame_id = publishOdomObject.name;
            // Debug.Log(publishOdomObject.name);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Debug.Log(translateDirection);
            common.SetTime(odomMsg.header.stamp);
            common.SetPose(odomMsg.pose.pose, publishOdomTransform);
            common.SetTwist(odomMsg.twist.twist, publishOdomArticulationBody);
            ros.Publish(odometryTopicName, odomMsg);
        }
    }
}
