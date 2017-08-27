using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour {

    public float speed;
    private CarPhysicsModel model;

    public GameObject leftWheel;
    public GameObject rightWheel;

    public float width;
    public float weight;

    //rotation
    public float smooth = 2.0F;
    public float tiltAngle = 30.0F;

    // Use this for initialization
    void Start () {
        model = new CarPhysicsModel(width, weight, new Vector3(0, speed, 0));
	}

    // Update is called once per frame
    void Update() {
        float h = Input.GetAxis("Horizontal");

        Vector3 currentPos = transform.position;
        //transform.position = new Vector3(currentPos.x + (speed * h * Time.deltaTime), currentPos.y, currentPos.z);

        float v = Input.GetAxis("Vertical");
        /*currentPos = transform.position;
        transform.position = new Vector3(currentPos.x, currentPos.y + (speed * v * Time.deltaTime), currentPos.z);*/

        /*float tiltAroundZ = tiltAngle * -h;
        Quaternion target = Quaternion.Euler(0, 0, tiltAroundZ);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * (Mathf.Abs(lastH) - Mathf.Abs(h) > 0 ? 20:smooth));*/

        CarPhysicsModel.CarUpdate carUpdate = model.Update(Time.deltaTime, h, transform.rotation.z);

        transform.position = new Vector3(currentPos.x + carUpdate.deltaPosition.x, currentPos.y + carUpdate.deltaPosition.y, currentPos.z);
        
        Quaternion target = Quaternion.Euler(0, 0, carUpdate.angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        print(h + " / " + carUpdate.angle);

        target = Quaternion.Euler(0, 0, carUpdate.wheelsOrientation);
        leftWheel.transform.rotation = Quaternion.Slerp(leftWheel.transform.rotation, target, Time.deltaTime * smooth);
        target = Quaternion.Euler(0, 0, carUpdate.wheelsOrientation);
        rightWheel.transform.rotation = Quaternion.Slerp(rightWheel.transform.rotation, target, Time.deltaTime * smooth);

        if (h == 0 && Random.value<0.003) {
            //GameObject.Find("jeep").GetComponent<Animator>().SetTrigger("bumpTrigger");
        }
    }

    void FixedUpdate() {
        
    }
}
