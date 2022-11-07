using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;

public class CameraCapturer : MonoBehaviour
{
  public bool IsCaptureEnable = false;
  public bool IsPublishEnable = false;
  Camera _camera;
  public int resWidth;
  public int resHeight;
  public byte[] jpg;

  ROSConnection ros;
  public string topic = "camera1/image/compressed";
  // Publish every N seconds
  public float publishMessageFrequency = 0.0f; //20 frames/sec
  // Used to determine how much time has elapsed since the last message was published
  private float timeElapsed;

  // Start is called before the first frame update
  void Start(){
    this._camera = GetComponent<Camera>();
    // start the ROS connection
    ros = ROSConnection.GetOrCreateInstance();
    ros.RegisterPublisher<CompressedImageMsg>(topic);
  }

  private byte[] getJPGFromCurrentCamera(){
    try {
      Texture2D screenShot = getTexture2DFromCurrentCamera();
      byte[] bytes = screenShot.EncodeToJPG();
      return bytes;
    }
    catch (Exception e) {
      return null;
    }
  }

    private Texture2D getTexture2DFromCurrentCamera(){
    try {
      RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
      _camera.targetTexture = rt;
      Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
      _camera.Render();
      RenderTexture.active = rt;
      screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
      _camera.targetTexture = null;
      RenderTexture.active = null;
      Destroy(rt);
      return screenShot;
    }
    catch (Exception e) {
      return null;
    }
  }

  private void Update(){
    if (this.IsCaptureEnable)
    {
      this.jpg = getJPGFromCurrentCamera();
      if (this.jpg != null){
        Debug.Log("OK");
      }
      else{
        Debug.Log("NULL");
      }
      this.IsCaptureEnable = false;
    }
    if (this.IsPublishEnable)
    {
      timeElapsed += Time.deltaTime;

      if (timeElapsed > publishMessageFrequency)
      {
        // Texture2D screenShot = getTexture2DFromCurrentCamera();
        this.jpg = getJPGFromCurrentCamera();
        var message = new CompressedImageMsg(new HeaderMsg(), "jpeg", this.jpg);
        // Finally send the message to server_endpoint.py running in ROS
        ros.Publish(topic, message);

        timeElapsed = 0;         
      }

    }
  }

  public byte[] getCapturedJpegImage(){
    this.IsCaptureEnable = true;
    while (this.IsCaptureEnable) { }
    if (this.jpg != null){
      return this.jpg;
    }
    return null;
  }
}
