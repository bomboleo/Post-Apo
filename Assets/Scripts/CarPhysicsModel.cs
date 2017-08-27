using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarPhysicsModel {

    private Vector3 position;
    private Vector3 velocity;
    private float baseSpeed;
    private float wheelsOrientation;
    private float weight;
    private float width;

    private float lastHAbs = 0;

    private float currentOrientation = 0;

    public float maxWheelsOrientation = Mathf.PI / 6;

	public CarPhysicsModel(float width, float weight, Vector3 velocity) {
        this.width = width;
        this.weight = weight;
        this.velocity = velocity;
        this.baseSpeed = velocity.magnitude;
    }

    public CarUpdate Update(float t, float h, float carAngle) {
        currentOrientation = carAngle;
        /*if (Mathf.Abs(h) < lastHAbs) {
            this.wheelsOrientation = currentOrientation;
        } else {*/
        wheelsOrientation = -h * maxWheelsOrientation;
        //}

        lastHAbs = Mathf.Abs(h);

        float vertical_gap = (t * velocity.magnitude) * -Mathf.Sin(wheelsOrientation);
        //velocity = new Vector3(Mathf.Min(velocity.x - Mathf.Abs(vertical_gap), baseSpeed) , Mathf.Max(0, velocity.y - Mathf.Abs(vertical_gap)), velocity.z);

        //Solving SAS Triangle
        float b = velocity.magnitude;
        float c = width;
        float angleA = (Mathf.PI/2 + (Mathf.PI / 2 - Mathf.Abs(wheelsOrientation)) - Mathf.Abs(currentOrientation));
        float a = Mathf.Sqrt(b * b + c * c - 2 * b * c * Mathf.Cos(angleA));
        float angleB = Mathf.Sign(-h) * Mathf.Asin(b * Mathf.Sin(angleA) / a);

        Vector3 deltaPosition = new Vector3(vertical_gap, 0, 0);

        CarUpdate carUpdate = new CarUpdate(angleB * Mathf.Rad2Deg, wheelsOrientation * Mathf.Rad2Deg, deltaPosition);

        return carUpdate;
    }

    public class CarUpdate {
        public float angle;
        public float wheelsOrientation;
        public Vector3 deltaPosition;

        public CarUpdate(float angle, float wheelsOrientation, Vector3 deltaPosition) {
            this.angle = angle;
            this.wheelsOrientation = wheelsOrientation;
            this.deltaPosition = deltaPosition;
        }
    }
}
