# Unity_Simulation_Server


These are the scripts that should be inside the Scripts folder in the Assets in the unity project.

Also you will find rviz configuration for easy usage with rviz.

Note that Image_View package was required to show the jpeg compressed image that was published from unity.

First run the unity simulation, and tick the publish box in the camera properties, then execute the following commands:

```
roslaunch ros_tcp_endpoint endpoint.launch
```
And in a second terminal do:
```
rosrun teleop_twist_keyboard teleop_twist_keyboard.py cmd_vel:=bluerov1/cmd_vel
```
And in a third terminal do:
```
rosrun image_view image_view image:=/camera1/image
```
And in a Fourth terminal do:
```
rosrun rviz rviz
```

Now using the second terminal, you can control the ROV and see all the changes in real time, enjoy!

Find below video showing results using keyboard teleoperation and publishing images and showing them on Rviz


https://user-images.githubusercontent.com/36744004/200203557-d9bf8355-fd01-4692-abb0-6e8d85b93626.mp4

