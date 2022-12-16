using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JointStateMsg = RosMessageTypes.Sensor.JointStateMsg;

namespace Sample.UnityROSPlugins
{
    public class JointStatesPublisher : MonoBehaviour
    {
        public GameObject robotBaseLink;
        private ArticulationBody robotModelArticulationBody;
        private ArticulationBody[] robotPartArticulationBody;
        public string topicName = "joint_states";
        private JointStateMsg jointStateMsg = new JointStateMsg();
        public GameObject ROSConnectionCommon;
        private Commons commons;
        private List<float> positions, velocities, efforts;
        private List<int> indexes;
        private List<string> names;
        private int startJointIndex;
        private float time;

        // Start is called before the first frame update
        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            commons.ros.RegisterPublisher<JointStateMsg>(topicName, commons.queueSize);

            robotModelArticulationBody = robotBaseLink.GetComponent<ArticulationBody>();
            robotPartArticulationBody = robotModelArticulationBody.GetComponentsInChildren<ArticulationBody>();

            indexes = new List<int>();
            names = new List<string>();
            for(int i=0; i<robotPartArticulationBody.Length; ++i){
                if(robotPartArticulationBody[i].jointType != ArticulationJointType.FixedJoint){
                    indexes.Add(robotPartArticulationBody[i].index);
                    names.Add(robotPartArticulationBody[i].name);
                }
            }

            jointStateMsg.position = new double[indexes.Count];
            jointStateMsg.velocity = new double[indexes.Count];
            jointStateMsg.effort = new double[indexes.Count];
            

            positions = new List<float>();
            velocities = new List<float>();
            efforts = new List<float>();

            robotModelArticulationBody.GetJointPositions(positions);
            robotModelArticulationBody.GetJointPositions(velocities);
            robotModelArticulationBody.GetJointPositions(efforts);
            startJointIndex = positions.Count - indexes.Count;

            for(int i=0; i<indexes.Count; ++i){
                for(int j=i+1; j<indexes.Count; ++j){
                    if(indexes[i] > indexes[j]){
                        int tmp = indexes[i];
                        indexes[i] = indexes[j];
                        indexes[j] = tmp;
                        
                        string s = names[i];
                        names[i] = names[j];
                        names[j] = s;
                    }
                }
            }

            jointStateMsg.name = names.ToArray();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            time += Time.deltaTime;
            if(time < commons.hz2t) return;
            time = 0;
            
            commons.SetTime(jointStateMsg.header.stamp);

            robotModelArticulationBody.GetJointPositions(positions);
            robotModelArticulationBody.GetJointVelocities(velocities);
            robotModelArticulationBody.GetJointForces(efforts);

            for(int i=startJointIndex; i<positions.Count; ++i){
                int index = i-startJointIndex;
                jointStateMsg.position[index] = positions[i];
                jointStateMsg.velocity[index] = velocities[i];
                jointStateMsg.effort[index] = efforts[i];
            }

            commons.ros.Publish(topicName, jointStateMsg);
        }
    }
}
