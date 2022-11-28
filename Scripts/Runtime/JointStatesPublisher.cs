using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using JointStateMsg = RosMessageTypes.Sensor.JointStateMsg;

namespace Sample.UnityROSPlugins
{
    public class JointStatesPublisher : MonoBehaviour
    {
        public List<GameObject> jointStatePublishObjects;
        private List<ArticulationBody> jointStatePublishArticulationBodys;
        public string topicName = "joint_states";
        private JointStateMsg jointStateMsg = new JointStateMsg();
        private List<JointStateMsg> jointStateMsgList = new List<JointStateMsg>();
        public GameObject ROSConnectionCommon;
        private Commons commons;
        private ROSConnection ros;

        // Start is called before the first frame update
        void Start()
        {
            List<string> names = new List<string>();
            jointStatePublishArticulationBodys = new List<ArticulationBody>();
            for(int i=0; i<jointStatePublishObjects.Count; ++i){
                Debug.Log(jointStatePublishObjects[i].name);
                names.Add(jointStatePublishObjects[i].name);
                jointStatePublishArticulationBodys.Add(jointStatePublishObjects[i].GetComponent<ArticulationBody>());
            }
            commons = ROSConnectionCommon.GetComponent<Commons>();
            commons.ros.RegisterPublisher<JointStateMsg>(topicName, 1);
            jointStateMsg.name = names.ToArray();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            // commons.SetTime(jointStateMsg.header.stamp);
            // commons.ros.Publish(topicName, jointStateMsg);
            GetJointPosition(jointStatePublishArticulationBodys);
        }

        void GetJointPosition(List<ArticulationBody> articulationBodies){
            foreach(ArticulationBody articulationBody in articulationBodies){
                List<float> floats = new List<float>();
                articulationBody.GetJointPositions(floats);
                // Debug.Log($"{articulationBody.name}: {floats.Count}");
                // foreach(float f in floats){
                Debug.Log($"{articulationBody.name}: {floats[floats.Count-3]}");
                // }
            }
        }
    }
}
