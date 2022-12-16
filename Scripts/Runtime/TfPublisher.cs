using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using TfMsg = RosMessageTypes.Tf2.TFMessageMsg;
using TfStampMsg = RosMessageTypes.Geometry.TransformStampedMsg;

namespace Sample.UnityROSPlugins
{
    public class TfPublisher : MonoBehaviour
    {
        public GameObject baseLinkObject;
        private ArticulationBody baseLinkArticulationBody;
        private ArticulationBody[] baseLinkArticulationBodies;
        private TfMsg tfMsg, tfStaticMsg;
        private List<TfStampMsg> tfStampMsgList;
        public GameObject ROSConnectionCommon;
        private Commons commons;
        private string tfTopicName = "tf";
        public string BaseLinkFrameId = "odom";
        [HideInInspector] public bool publishBySelf = true;
        private float time;
        private ROSConnection ros;
        // Start is called before the first frame update
        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<TfMsg>(tfTopicName, commons.queueSize);

            baseLinkArticulationBody = baseLinkObject.GetComponent<ArticulationBody>();
            baseLinkArticulationBodies = baseLinkArticulationBody.GetComponentsInChildren<ArticulationBody>();

            tfStampMsgList = new List<TfStampMsg>();

            foreach(ArticulationBody articulationBody in baseLinkArticulationBodies){
                TfStampMsg tfStampMsg = new TfStampMsg();
                commons.SetTime(tfStampMsg.header.stamp);
                tfStampMsg.child_frame_id = articulationBody.name;

                ArticulationBody[] parents = articulationBody.GetComponentsInParent<ArticulationBody>();
                tfStampMsg.header.frame_id = parents.Length == 1 ? BaseLinkFrameId : parents[1].name;

                commons.SetTranslationAndRotation(tfStampMsg.transform, articulationBody.transform, true);

                tfStampMsgList.Add(tfStampMsg);
            }

            tfMsg = new TfMsg();
            tfMsg.transforms = tfStampMsgList.ToArray();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            commons.SetTime(tfMsg.transforms[0].header.stamp);
            commons.SetTranslationAndRotation(tfMsg.transforms[0].transform, baseLinkArticulationBodies[0].transform);

            for(int i=1; i<baseLinkArticulationBodies.Length; ++i){
                commons.SetTime(tfMsg.transforms[i].header.stamp);
                commons.SetTranslationAndRotation(tfMsg.transforms[i].transform, baseLinkArticulationBodies[i].transform, true);
            }
            ros.Publish(tfTopicName, tfMsg);
        }
    }
}
