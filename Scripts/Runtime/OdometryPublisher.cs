using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using OdomMsg = RosMessageTypes.Nav.OdometryMsg;

namespace Sample.UnityROSPlugins
{
    public class OdometryPublisher : MonoBehaviour
    {
        public GameObject publishOdomObject;
        private Transform publishOdomTransform;
        private ArticulationBody publishOdomArticulationBody;
        public string odometryTopicName = "odom";
        private OdomMsg odomMsg;
        private Commons commons;
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
        private TfPublisher tfPublisher;
        private int index;
        // [HideInInspector] public OdometryPublisher Setting = new OdometryPublisher();

        // Start is called before the first frame update
        
        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            publishOdomTransform = publishOdomObject.GetComponent<Transform>();
            publishOdomArticulationBody = publishOdomObject.GetComponent<ArticulationBody>();
            commons.ros.RegisterPublisher<OdomMsg>(odometryTopicName, 1);
            odomMsg = new OdomMsg();
            odomMsg.header.frame_id = odometryTopicName;
            odomMsg.child_frame_id = publishOdomObject.name;
            if(publishTf){
                gameObject.AddComponent<TfPublisher>();
                tfPublisher = gameObject.GetComponent<TfPublisher>();
                tfPublisher.ROSConnectionCommon = ROSConnectionCommon;
                tfPublisher.publishBySelf = false;
                // GameObject tfObj = publishOdomObject;
                tfPublisher.publishTfObjects = new List<GameObject>();
                tfPublisher.publishTfObjects.Add(publishOdomObject);
                index = tfPublisher.publishTfObjects.Count-1;
                tfPublisher.AttachedCommponent();
            }
            // Debug.Log(publishOdomObject.name);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            commons.SetTime(odomMsg.header.stamp);
            commons.SetPose(odomMsg.pose.pose, publishOdomTransform);
            commons.SetTwist(odomMsg.twist.twist, publishOdomArticulationBody);
            // if(publishTf){
            //     // Debug.Log
            //     // tfPublisher.tfStampMsgList[index].header.stamp = odomMsg.header.stamp;
            //     tfPublisher.SetTfMsg();
            //     tfPublisher.PublishMsg();
            // }
            commons.ros.Publish(odometryTopicName, odomMsg);
            
        }
    }
}
