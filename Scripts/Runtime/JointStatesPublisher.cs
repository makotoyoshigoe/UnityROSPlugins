using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics;
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
        private List<float> positions;
        private List<float> velocities;
        private List<float> efforts;
        private List<int> indexes;
        private List<string> names;
        private int startJointIndex;

        // Start is called before the first frame update
        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            commons.ros.RegisterPublisher<JointStateMsg>(topicName, 1);

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
            commons.SetTime(jointStateMsg.header.stamp);

            robotModelArticulationBody.GetJointPositions(positions);
            robotModelArticulationBody.GetJointPositions(velocities);
            robotModelArticulationBody.GetJointPositions(efforts);

            for(int i=startJointIndex; i<positions.Count; ++i){
                int index = i-startJointIndex;
                jointStateMsg.position[index] = positions[i];
                jointStateMsg.velocity[index] = velocities[i];
                jointStateMsg.effort[index] = efforts[i];
            }

            commons.ros.Publish(topicName, jointStateMsg);
            // for(int i=0; i<publishPositions.Count; ++i){
            //     Debug.Log($"{jointStateMsg.name[i]}: {publishPositions[i]}");
            // }
        }

        // double[] GetJointPosition(ArticulationBody articulationBody){
        //     articulationBody.GetJointPositions(positions);
        //     for(int i=startJointIndex; i<positions.Count; ++i) publishPositions[i-startJointIndex] = positions[i];
        //     // for(int i=0; i<publishPositions.Count; ++i){
        //     //     Debug.Log($"{jointStateMsg.name[i]}: {publishPositions[i]}");
        //     // }
        //     Debug.Log("end");
        //     return jointPositions.ToArray();
        // }

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
            // jointStateMsg.position = GetJointPosition(jointStatePublishArticulationBodys);
            // jointStateMsg.velocity = GetJointVelocity(jointStatePublishArticulationBodys);
            // jointStateMsg.effort = GetJointForce(jointStatePublishArticulationBodys);
        }
    }
}
