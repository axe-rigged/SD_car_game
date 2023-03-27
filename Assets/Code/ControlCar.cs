using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCar : MonoBehaviour
{
    public WheelCollider front_r;
    public WheelCollider front_l;
    public WheelCollider back_r;
    public WheelCollider back_l;
    //wheel transforms
    public Transform mesh_front_r;
    public Transform mesh_front_l;
    public Transform mesh_back_r;
    public Transform mesh_back_l;

    public float accelration = 500f;
    public float breakingforce = 300f;

    private float currentAcceleration = 0f;
    private float currentBreackForce = 0f;
    private float c_turn_angle = 0f;
    public float max_turn_angle = 20f;

    // Start is called before the first frame update
    private void FixedUpdate(){
        currentAcceleration = accelration * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space)){
            currentBreackForce = breakingforce;
        }
        else {
            currentBreackForce = 0f;
        }

        front_l.motorTorque = currentAcceleration;
        front_r.motorTorque = currentAcceleration;

        front_l.brakeTorque = currentBreackForce;
        front_r.brakeTorque = currentBreackForce;
        back_l.brakeTorque = currentBreackForce;
        back_r.brakeTorque = currentBreackForce;

        //get change on turning wheel
        c_turn_angle = max_turn_angle * Input.GetAxis("Horizontal");
        front_l.steerAngle = c_turn_angle;
        front_r.steerAngle = c_turn_angle;
    
        turn_wheel_mesh(mesh_back_l, back_l, true);
        turn_wheel_mesh(mesh_back_r, back_r, false);
        turn_wheel_mesh(mesh_front_l, front_l, true);
        turn_wheel_mesh(mesh_front_r, front_r, false);
    }

    //fucntion for turning wheels and rotating them
    private void turn_wheel_mesh(Transform mesh, WheelCollider col, bool left_site){
        Vector3 position;
        Quaternion rotation;
        
        col.GetWorldPose(out position, out rotation);
        if(left_site){
            mesh.rotation = rotation;
            mesh.position = position;
        }
        else {
            mesh.rotation = rotation * Quaternion.Euler(0,180,0);
            mesh.position = position;
        }
    }
}
