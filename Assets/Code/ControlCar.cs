using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//car game was mistake
public class ControlCar : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 normal_mass_point = new Vector3(0f, 0f,0f);
    private bool is_drifting = false;
    public float default_ff, default_sf;
    public float forward_friction = 0.5f;
    public float sideway_friction = 0.3f;
    //colliders
    public WheelCollider front_r;
    public WheelCollider front_l;
    public WheelCollider back_r;
    public WheelCollider back_l;
    public List<WheelCollider> wheels = new List<WheelCollider>();
    //wheel transforms
    public Transform mesh_front_r;
    public Transform mesh_front_l;
    public Transform mesh_back_r;
    public Transform mesh_back_l;

    public float accelration = 1000f;
    public float breakingforce = 800f;
    public float drift_break = 400f;

    private float currentAcceleration = 0f;
    private float currentBreackForce = 0f;
    private float c_turn_angle = 0f;
    public float max_turn_angle = 20f;

    private void Start(){
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = normal_mass_point;
        wheels.Add(front_r);
        wheels.Add(front_l);
        wheels.Add(back_r);
        wheels.Add(back_l);
    }
    //Fixed update mainly used
    private void FixedUpdate(){
        currentAcceleration = (accelration * Input.GetAxis("Vertical"))/4;  //will change to back trigger

        if (Input.GetKey(KeyCode.R)){
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
        handbreak(Input.GetKey(KeyCode.Space));

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

    //In future it would be nice to give WheelFrictionCurve but I have no idea how that shit works
    private void change_friction(List<WheelCollider> wheels, float value){
        foreach(WheelCollider wheel in wheels){
            WheelFrictionCurve frictionCurve = wheel.sidewaysFriction;            
            frictionCurve.stiffness = value;
            wheel.sidewaysFriction = frictionCurve;
        }
    }

    private void handbreak(bool x){
        if(x && !is_drifting){
            is_drifting = true;
            //do we need to close back wheels or what
            //maybe give more force to move
            back_l.brakeTorque = drift_break;
            back_r.brakeTorque = drift_break;

            change_friction(wheels, sideway_friction);
        }
        if(!x && is_drifting){
            is_drifting = false;
            change_friction(wheels, default_sf);
            }
        else {return;}
    }
    //Need collider check that resets our speed if colliding something.
    //might need to check little bit how curves works in unity to make better motor feel
}
