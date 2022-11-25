using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using OdomMsg = RosMessageTypes.Nav.OdometryMsg;

public class OdometryPublisher : MonoBehaviour
{
    // public GameObject robotBodyGameObject;
    public GameObject publishOdomObject;
    private Transform publishOdomTransform;
    private ArticulationBody publishOdomArticulationBody;
    public string topicName = "odom";
    private ROSConnection ros;
    private OdomMsg odomMsg;
    private Common common;
    public GameObject ROSConnectionCommon;
    public enum TranslateDirection{
        x, y, z
    }
    public enum RotateAxis{
        x, y, z
    }
    public TranslateDirection translateDirection = TranslateDirection.x;
    public RotateAxis rotateAxis = RotateAxis.y;

    // Start is called before the first frame update
    void Start()
    {
        common = ROSConnectionCommon.GetComponent<Common>();
        publishOdomTransform = publishOdomObject.GetComponent<Transform>();
        publishOdomArticulationBody = publishOdomObject.GetComponent<ArticulationBody>();
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<OdomMsg>(topicName, 1);
        odomMsg = new OdomMsg();
        odomMsg.header.frame_id = topicName;
        odomMsg.child_frame_id = publishOdomObject.name;
        // Debug.Log(publishOdomObject.name);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Debug.Log(translateDirection);
        common.SetTime(odomMsg.header.stamp);
        common.SetPose(odomMsg.pose.pose, publishOdomTransform);
        common.SetTwist(odomMsg.twist.twist, publishOdomArticulationBody);
        ros.Publish(topicName, odomMsg);
    }
}
