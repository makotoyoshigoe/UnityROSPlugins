using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using TfMsg = RosMessageTypes.Tf2.TFMessageMsg;

namespace Sample.UnityROSPlugins
{
    public class TfPublisher : MonoBehaviour
    {
        public GameObject publishTfObject;
        private Transform publishTfTransform;
        private ArticulationBody publishTfAculationBody;
        public string topicName = "tf";
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
