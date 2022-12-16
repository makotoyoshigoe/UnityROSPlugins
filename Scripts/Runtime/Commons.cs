using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using TimeMsg = RosMessageTypes.BuiltinInterfaces.TimeMsg;
using PoseMsg = RosMessageTypes.Geometry.PoseMsg;
using TwistMsg = RosMessageTypes.Geometry.TwistMsg;
using ClockMsg = RosMessageTypes.Rosgraph.ClockMsg;
using TransformMsg = RosMessageTypes.Geometry.TransformMsg;

namespace Sample.UnityROSPlugins
{
    public class Commons : MonoBehaviour
    {
        [HideInInspector] public ROSConnection ros;
        private string topicName = "clock";
        private ClockMsg clockMsg = new ClockMsg();
        public int queueSize = 5;
        public float publishFrequency = 60;
        [HideInInspector] public float hz2t;
        private float time;
        // Start is called before the first frame update

        private void Awake(){
            ros = ROSConnection.GetOrCreateInstance();
            ros.RegisterPublisher<ClockMsg>(topicName, queueSize);
            hz2t = 1 / publishFrequency;
        }

        void Start()
        {
        }

        void FixedUpdate(){
            time += Time.deltaTime;
            if(time < hz2t) return;
            time = 0;

            SetTime(clockMsg.clock);
            ros.Publish(topicName, clockMsg);
        }

        public void SetTime(TimeMsg timeMsg){
            float t = Time.time, sec = Mathf.Floor(t), nsec = (t-sec)*1000000000;
            timeMsg.sec = (uint)sec;
            timeMsg.nanosec = (uint)nsec;
        }

        public void SetPose(PoseMsg poseMsg, Transform transform){
            poseMsg.position.x = transform.position.z;
            poseMsg.position.y = -transform.position.x;
            poseMsg.position.z = transform.position.y;
            poseMsg.orientation.x = transform.rotation.z;
            poseMsg.orientation.y = -transform.rotation.x;
            poseMsg.orientation.z = transform.rotation.y;
            poseMsg.orientation.w = -transform.rotation.w;
        }

        public void SetTwist(TwistMsg twistMsg, ArticulationBody articulationBody){
            twistMsg.linear.x = articulationBody.velocity.magnitude;
            twistMsg.linear.y = 0;
            twistMsg.linear.z = 0;
            twistMsg.angular.x = 0;
            twistMsg.angular.y = 0;
            twistMsg.angular.z = articulationBody.angularVelocity.magnitude;
        }

        public void SetTranslationAndRotation(TransformMsg transformMsg, Transform transform, bool isLocal = false){
            if(!isLocal){
                transformMsg.translation.x = transform.position.z;
                transformMsg.translation.y = -transform.position.x;
                transformMsg.translation.z = transform.position.y;
                transformMsg.rotation.x = transform.rotation.z;
                transformMsg.rotation.y = -transform.rotation.x;
                transformMsg.rotation.z = transform.rotation.y;
                transformMsg.rotation.w = -transform.rotation.w;
            }else{
                transformMsg.translation.x = transform.localPosition.z;
                transformMsg.translation.y = -transform.localPosition.x;
                transformMsg.translation.z = transform.localPosition.y;
                transformMsg.rotation.x = transform.localRotation.z;
                transformMsg.rotation.y = -transform.localRotation.x;
                transformMsg.rotation.z = transform.localRotation.y;
                transformMsg.rotation.w = -transform.localRotation.w;
            }
                
        }
    }
}
