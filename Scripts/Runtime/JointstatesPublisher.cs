using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using JointstateMsg = RosMessageTypes.Sensor.JointStateMsg;

namespace Sample.UnityROSPlugins
{
    public class JointstatesPublisher : MonoBehaviour
    {
        public List<GameObject> jointstatePublishObject;
        public string topicName = "joint_states";
        private float ROSTimeout = 0.5f;
        private float lastCmdReceived = 0f;
        private ROSConnection ros;
        private List<JointstateMsg> jointstateMsg;
        // Start is called before the first frame update
        void Start()
        {
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<JointstateMsg>(topicName, 1);
            jointstateMsg = new List<JointstateMsg>(jointstatePublishObject.Count);
            Debug.Log(jointstatePublishObject.Count);
            Debug.Log(jointstatePublishObject[0].name);
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            
        }

        // void Set
    }
}
