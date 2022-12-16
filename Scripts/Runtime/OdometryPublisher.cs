using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OdomMsg = RosMessageTypes.Nav.OdometryMsg;
using TfMsg = RosMessageTypes.Tf2.TFMessageMsg;
using TfStampMsg = RosMessageTypes.Geometry.TransformStampedMsg;

namespace Sample.UnityROSPlugins
{
    public class OdometryPublisher : MonoBehaviour
    {
        public GameObject publishOdomObject;
        private Transform publishOdomTransform;
        private ArticulationBody publishOdomArticulationBody;
        public string odometryTopicName = "odom", tfTopicName = "tf";
        private OdomMsg odomMsg;
        private TfStampMsg tfStampMsg = new TfStampMsg();
        private TfMsg tfMsg = new TfMsg();
        private List<TfStampMsg> tfStampMsgList;
        private Commons commons;
        public GameObject ROSConnectionCommon;
        public bool publishTf = true;
        private float time;

        // Start is called before the first frame update
        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            publishOdomTransform = publishOdomObject.GetComponent<Transform>();
            publishOdomArticulationBody = publishOdomObject.GetComponent<ArticulationBody>();

            commons.ros.RegisterPublisher<OdomMsg>(odometryTopicName, commons.queueSize);
            odomMsg = new OdomMsg();
            odomMsg.header.frame_id = odometryTopicName;
            odomMsg.child_frame_id = publishOdomObject.name;
            odomMsg.pose.covariance = new double[]{1e-05, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1e-05, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1000000000000.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1000000000000.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 1000000000000.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.001};
            if(publishTf){
                commons.ros.RegisterPublisher<TfMsg>(tfTopicName);
                tfStampMsgList = new List<TfStampMsg>();
                tfStampMsg.header.frame_id = odometryTopicName;
                tfStampMsg.child_frame_id = publishOdomObject.name;
                tfStampMsgList.Add(tfStampMsg);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            time += Time.deltaTime;
            if(time < commons.hz2t) return;
            time = 0;
            
            commons.SetTime(odomMsg.header.stamp);
            commons.SetPose(odomMsg.pose.pose, publishOdomTransform);
            commons.SetTwist(odomMsg.twist.twist, publishOdomArticulationBody);

            if(publishTf){
                commons.SetTime(tfStampMsgList[0].header.stamp);
                tfStampMsgList[0].transform.translation.x = odomMsg.pose.pose.position.x;
                tfStampMsgList[0].transform.translation.y = odomMsg.pose.pose.position.y;
                tfStampMsgList[0].transform.translation.z = odomMsg.pose.pose.position.z;
                tfStampMsgList[0].transform.rotation.x = -odomMsg.pose.pose.orientation.x;
                tfStampMsgList[0].transform.rotation.y = -odomMsg.pose.pose.orientation.y;
                tfStampMsgList[0].transform.rotation.z = -odomMsg.pose.pose.orientation.z;
                tfStampMsgList[0].transform.rotation.w = -odomMsg.pose.pose.orientation.w;
                tfMsg.transforms = tfStampMsgList.ToArray();
                commons.ros.Publish(tfTopicName, tfMsg);
            }
            commons.ros.Publish(odometryTopicName, odomMsg);
            
        }
    }
}
