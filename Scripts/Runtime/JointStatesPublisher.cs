using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using JointStateMsg = RosMessageTypes.Sensor.JointStateMsg;

namespace Sample.UnityROSPlugins
{
    public class JointStatesPublisher : MonoBehaviour
    {
        public string[] jointNames; 
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
            jointStatePublishArticulationBodys = new List<ArticulationBody>();
            for(int i=0; i<jointStatePublishObjects.Count; ++i){
                Debug.Log(jointStatePublishObjects[i].name);
                jointStatePublishArticulationBodys.Add(jointStatePublishObjects[i].GetComponent<ArticulationBody>());
            }
            commons = ROSConnectionCommon.GetComponent<Commons>();
            commons.ros.RegisterPublisher<JointStateMsg>(topicName, 1);
            jointStateMsg.name = jointNames;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            commons.SetTime(jointStateMsg.header.stamp);
            SetJointStates();
            // GetQuaternion(jointStatePublishArticulationBodys);
            // commons.ros.Publish(topicName, jointStateMsg);
        }

        double[] GetJointPosition(List<ArticulationBody> articulationBodies){
            List<double> jointPositions = new List<double>();
            GameObject g;
            g = articulationBodies[0].GetComponent<GameObject>();
            foreach(ArticulationBody articulationBody in articulationBodies){
                List<float> positions = new List<float>();
                List<int> indies = new List<int>();
                int i = articulationBody.GetJointPositions(positions);
                Debug.Log(i);
                articulationBody.GetDofStartIndices(indies);
                jointPositions.Add(positions[positions.Count-1]);
                ArticulationBody[] articulationBody1;
                articulationBody1 = articulationBody.GetComponentsInChildren<ArticulationBody>();
                Debug.Log("start");
                foreach(int index in indies) Debug.Log(index);
                // foreach(ArticulationBody ab in articulationBody1){
                //     Debug.Log(ab.name);
                // }
                // Debug.Log(indies.Count);
                // foreach(int index in indies) Debug.Log(index);
                // Debug.Log(positions.Count);
                for(int j = 0; j < positions.Count; ++j){
                    Debug.Log($"{j}: {positions[j]}");
                }
                foreach(ArticulationBody ab in articulationBody1){
                    ab.GetJointPositions(positions);
                    foreach(double position in positions){
                        Debug.Log(position);
                    }
                }
                // foreach(float position in positions) Debug.Log(position);
                Debug.Log("end");
                // Debug.Log($"{articulationBody.name}: {positions[positions.Count-2]}");
                // }
            }
            return jointPositions.ToArray();
        }

        // void GetQuaternion(List<ArticulationBody> articulationBodies){
        //     foreach(ArticulationBody articulationBody in articulationBodies){
        //         Transform transform = articulationBody.GetComponent<Transform>();
        //         Debug.Log(transform.localRotation);
        //     }
        // }

        double[] GetJointVelocity(List<ArticulationBody> articulationBodies){
            List<double> jointVelocities = new List<double>();
            foreach(ArticulationBody articulationBody in articulationBodies){
                List<float> velocities = new List<float>();
                articulationBody.GetJointVelocities(velocities);
                jointVelocities.Add(velocities[velocities.Count-2]);
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
                jointForces.Add(forces[forces.Count-2]);
                // Debug.Log($"{articulationBody.name}: {floats.Count}");
                // foreach(float f in floats){
                // Debug.Log($"{articulationBody.name}: {positions[positions.Count-1]}");
                // }
            }
            return jointForces.ToArray();
        }

        void SetJointStates(){
            jointStateMsg.position = GetJointPosition(jointStatePublishArticulationBodys);
            // jointStateMsg.velocity = GetJointVelocity(jointStatePublishArticulationBodys);
            // jointStateMsg.effort = GetJointForce(jointStatePublishArticulationBodys);
        }
    }
}
