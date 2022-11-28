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
        public List<GameObject> publishTfObjects;
        private List<Transform> publishTfTransforms = new List<Transform>();
        private TfMsg tfMsg = new TfMsg();
        private List<TfStampMsg> tfStampMsgList;
        public GameObject ROSConnectionCommon;
        private Commons commons;
        public string topicName = "tf";
        public string frame_id = "/odom";
        [HideInInspector] public bool publishBySelf = true;
        // Start is called before the first frame update
        void Start()
        {
            commons = ROSConnectionCommon.GetComponent<Commons>();
            commons.ros.RegisterPublisher<TfMsg>(topicName, 1);
            AttachedCommponent();
        }

        public void AttachedCommponent(){
            tfStampMsgList = new List<TfStampMsg>();
            foreach(GameObject gameObject in publishTfObjects){
                TfStampMsg tfStampMsg = new TfStampMsg();
                publishTfTransforms.Add(gameObject.GetComponent<Transform>());
                tfStampMsg.header.frame_id = frame_id;
                tfStampMsg.child_frame_id = gameObject.name;
                tfStampMsgList.Add(tfStampMsg);
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            SetTfMsg();
            PublishMsg();
        }

        public void SetTfMsg(){
            int i=0;
            foreach(TfStampMsg stampMsg in tfStampMsgList){
                stampMsg.header.seq++;
                commons.SetTime(stampMsg.header.stamp);
                commons.SetTranslationAndRotation(stampMsg.transform, publishTfObjects[i].GetComponent<Transform>());
                ++i;
            }
            tfMsg.transforms = tfStampMsgList.ToArray();
        }

        public void PublishMsg(){
            commons.ros.Publish(topicName, tfMsg);
        }
    }
}
