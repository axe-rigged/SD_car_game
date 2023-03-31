using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_control : MonoBehaviour
{
    public Camera player_camera;
    public GameObject player_car;
    private Transform camera_position;
    public Vector3 cameras_rest_position = new Vector3(0f, 3f, -5f);
    
    // Start is called before the first frame update
    void Start()
    {
        //should find Tag for player car object
        //Not sure if want to attach this script camera controller or to player and find camera with tag but now its here
        camera_position = player_camera.transform;
    }

    void FixedUpdate()
    {
        camera_position.rotation = Quaternion.Euler(15f, player_car.transform.eulerAngles.y, 0f);
        //Forgot how global and local space works in position
        var move_position = player_car.transform.TransformPoint(cameras_rest_position);
        //slerping
        //camera_position.position = move_position; 
        camera_position.position = Vector3.Lerp(camera_position.position, move_position, .25f);
    }
}
