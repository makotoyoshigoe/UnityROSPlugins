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
            commons.SetTime(jointStateMsg.header.stamp);
            SetJointStates();
            commons.ros.Publish(topicName, jointStateMsg);
        }

        double[] GetJointPosition(List<ArticulationBody> articulationBodies){
            List<double> jointPositions = new List<double>();
            foreach(ArticulationBody articulationBody in articulationBodies){
                List<float> positions = new List<float>();
                articulationBody.GetJointPositions(positions);
                jointPositions.Add(positions[positions.Count-1]);
                // Debug.Log($"{articulationBody.name}: {doubles.Count}");
                // foreach(double f in doubles){
                // Debug.Log($"{articulationBody.name}: {positions[positions.Count-1]}");
                // }
            }
            return jointPositions.ToArray();
        }

        double[] GetJointVelocity(List<ArticulationBody> articulationBodies){
            List<double> jointVelocities = new List<double>();
            foreach(ArticulationBody articulationBody in articulationBodies){
                List<float> velocities = new List<float>();
                articulationBody.GetJointVelocities(velocities);
                jointVelocities.Add(velocities[velocities.Count-1]);
                // Debug.Log($"{articulationBody.name}: {doubles.Count}");
                // foreach(double f in doubles){
                // Debug.Log($"{articulationBody.name}: {positions[positions.Count-1]}");
                // }
            }
            return jointVelocities.ToArray();
        }

        double[] GetJointForce(List<ArticulationBody> articulationBodies){
            List<double> jointForces = new List<double>();
            foreach(ArticulationBody articulationBody in articulationBodies){
                List<float> forces = new List<float>();
                articulationBody.GetJointForces(forces);
                jointForces.Add(forces[forces.Count-1]);
                // Debug.Log($"{articulationBody.name}: {floats.Count}");
                // foreach(float f in floats){
                // Debug.Log($"{articulationBody.name}: {positions[positions.Count-1]}");
                // }
            }
            return jointForces.ToArray();
        }

        void SetJointStates(){
            jointStateMsg.position = GetJointPosition(jointStatePublishArticulationBodys);
            jointStateMsg.velocity = GetJointVelocity(jointStatePublishArticulationBodys);
            jointStateMsg.effort = GetJointForce(jointStatePublishArticulationBodys);
        }
    }
}
