# このリポジトリについて
unity_ros_pluginsは, ROSで動作するロボットのシミュレーションをUnity上で行うために必要な機能をリポジトリです. 

# 開発環境
## OS
Ubuntu20.04
## Unity
2021.3.11f
## 依存パッケージ
- [ROS TCP Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector): 0.7.0
- [URDF Importer](https://github.com/Unity-Technologies/URDF-Importer): 0.5.2
# インストール方法
1. Unityのプロジェクトを開きます. （はじめてプロジェクトを作成される方は[こちら](https://learn.unity.com/tutorial/create-your-first-unity-project-jp?language=ja#)を参考に作成してください. ）
1. 画面左上の`Window` -> `Package Manager`をクリックし, Package Managerを開きます. 
1. 左上の`+`をクリックし, `Add package from git URL...`を選択します. 
1. 入力欄に, 以下のURLを入力し, `Install`ボタンをクリックします. 
   - https://github.com/Unity-Technologies/ROS-TCP-Connector.git?path=/com.unity.robotics.ros-tcp-connector
   - https://github.com/Unity-Technologies/URDF-Importer.git?path=/com.unity.robotics.urdf-importer#v0.5.2
   - https://github.com/makotoyoshigoe/unity_ros_plugins.git

# ロボットのシミュレーション
Unity上でロボットのシミュレーションを行う場合は以下の方法に沿って進めてください. 
## ロボットモデルの設置
1. 設置したいロボットのurdfファイルを用意します. 
2. urdfファイルを`右クリック`して, `Import URDF`をクリックします. 

## ROS Noeticのインストール
[ryuuichiueda](https://github.com/ryuichiueda)様の作成されたリポジトリを使用します. 
- Ubuntu20.04の場合
   1. ```
      git clone https://github.com/ryuichiueda/ros_setup_scripts_Ubuntu20.04_desktop.git
      ```
      でリポジトリをクローンしてください. 
   1. [こちら](https://github.com/ryuichiueda/ros_setup_scripts_Ubuntu20.04_desktop)のREADMEに沿ってインストールしてください. 
- Windows10/11の場合
   1. [こちら](https://learn.microsoft.com/ja-jp/windows/wsl/install)に沿ってWSL2をインストールしてください
   1. ターミナルを開いて, `Ubuntu20.04の場合`と同様の操作を行ってください. 
# ライセンスについて
このリポジトリには[Apache-2.0 license](https://github.com/makotoyoshigoe/UnityROSPlugins/blob/master/LICENSE.md)が適用されています.

