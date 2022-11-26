// using UnityEditor;
// using Sample.UnityROSPlugins;
// [CustomEditor(typeof(OdometryPublisher))]
// public class OdometryPublisherEditor : Editor
// {
//     private OdometryPublisher _target;


//     private void Awake()
//     {
//         _target = target as OdometryPublisher;
//     }

//     public override void OnInspectorGUI()
//     {
//         EditorGUI.BeginDisabledGroup(_target.publishTf);
//         _target.Setting.tfTopicName = EditorGUILayout.TextField("Tf Topic Name", _target.Setting.tfTopicName);
//         EditorGUI.EndDisabledGroup();
//         // EditorGUI.BeginChangeCheck();

//         // if (_target.publishTf)
//         // {
//         //     _target.Setting.tfTopicName = EditorGUILayout.TextField("Tf Topic Name", _target.Setting.tfTopicName);
//         // }

//         // // GUIの更新があったら実行
//         // if (EditorGUI.EndChangeCheck())
//         // {
//         //     EditorUtility.SetDirty(_target);
//         // }
//     }
// }
