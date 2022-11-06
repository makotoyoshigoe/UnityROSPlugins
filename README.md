# このリポジトリについて
unity_ros_pluginsは, ROSで動作するロボットのシミュレーションをUnity上で行うために必要な機能が格納されているリポジトリです. 

# 開発環境
## OS
Ubuntu20.04
## Unity
2021.3.11f
## 依存パッケージ
- Burst: 1.6.6
- Mathematics: 1.2.6
- [ROS TCP Connecto](https://github.com/Unity-Technologies/ROS-TCP-Connector)r: 0.7.0
- [URDF Importer](https://github.com/Unity-Technologies/URDF-Importer): 0.5.2
- [UnitySensors](https://github.com/Field-Robotics-Japan/UnitySensors): 0.1.1
- [UnitySensorsROS](https://github.com/Field-Robotics-Japan/UnitySensorsROS): 0.0.1
# インストール方法
1. 画面左上の`Window` -> `Package Manager`をクリックし, Package Managerを開きます. 
2. 左上の`+`をクリックし, "Add package from git URL..."を選択します. 
3. 入力欄に, 以下のURLを入力し, `Install`ボタンをクリックします.
   - https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector
   - https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer#v0.5.2
   - https://github.com/Field-Robotics-Japan/UnitySensorsROS.git
   - https://github.com/Field-Robotics-Japan/UnitySensors.git
   - https://github.com/makotoyoshigoe/UnityROSPlugins.git?path=/Packages/UnityROSPlugins
# ライセンスについて
このリポジトリには[Apache-2.0 license](https://github.com/makotoyoshigoe/UnityROSPlugins/blob/master/LICENSE.md)が適用されています.

