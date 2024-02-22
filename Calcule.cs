using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Kinect;

namespace Microsoft.Samples.Kinect.BodyBasics
{
   public  class Calcule

    {
        public Calcule ()
        {
            JointTypes.Add(JointType.Head);
            JointTypes.Add(JointType.Neck );
            JointTypes.Add(JointType.SpineShoulder);
            JointTypes.Add(JointType.ShoulderLeft);
            JointTypes.Add(JointType.ShoulderRight);
            JointTypes.Add(JointType.ElbowLeft);
            JointTypes.Add(JointType.ElbowRight);
            JointTypes.Add(JointType.WristLeft);
            JointTypes.Add(JointType.WristRight);
            JointTypes.Add(JointType.ThumbLeft);
            JointTypes.Add(JointType.ThumbRight);
            JointTypes.Add(JointType.HandLeft);
            JointTypes.Add(JointType.HandRight);
            JointTypes.Add(JointType.HandTipLeft);
            JointTypes.Add(JointType.HandTipRight);
            JointTypes.Add(JointType.SpineMid);
            JointTypes.Add(JointType.SpineBase);
            JointTypes.Add(JointType.HipLeft);
            JointTypes.Add(JointType.HipRight);
            JointTypes.Add(JointType.KneeLeft);
            JointTypes.Add(JointType.KneeRight);
            JointTypes.Add(JointType.AnkleLeft);
            JointTypes.Add(JointType.AnkleRight);
            JointTypes.Add(JointType.FootLeft);
            JointTypes.Add(JointType.FootRight);
        }

        public static  Vector4 vFloorClipPlane;

        public double distanceFromKinect(Joint j)
        {

            double distance = Math.Sqrt(
                (j.Position.X * j.Position.X) +
                (j.Position.Y * j.Position.Y) +
                (j.Position.Z * j.Position.Z));

            return distance;
        }

        public double jointDistanceFromTheFloor(Joint joint)
        {
            // joint is your joint - left foot or any other joint
            // j is the position of this joint
            var j = joint.Position;
            float x = j.X;
            float y = j.Y;
            float z = j.Z;


            float A = vFloorClipPlane.X;
            float B = vFloorClipPlane.Y;
            float C = vFloorClipPlane.Z;
            float D = vFloorClipPlane.W; //D is the height of the camera from the floor, in meters.

            float distance = (Math.Abs((x * A) + (y * B) + (z * C) + D))
                / ((float)Math.Sqrt((Math.Pow(A, 2)) + (Math.Pow(B, 2)) + (Math.Pow(C, 2))));
            return distance;
        }

        public double CenterToFloor(Joint p1, Joint p2)
        {

            Joint center = new Joint();

            var j1 = p1.Position;
            var j2 = p2.Position;

            center.Position.X = ((j1.X + j2.X) / 2);
            center.Position.Y = ((j1.Y + j2.Y) / 2);
            center.Position.Z = ((j1.Z + j2.Z) / 2);

            double centerPointDistanceFromTheFloor = jointDistanceFromTheFloor(center);

            return centerPointDistanceFromTheFloor;
        }

        public double calculateAngle(Joint j1, Joint j2, Joint j3, Joint j4)
        {
            //get the position of all the 4 joints
            var A = j1.Position;
            var B = j2.Position;

            var C = j3.Position;
            var D = j4.Position;

            //get the difference between 2 joints to get the vectors Coordinates
            //AB = B-A
            double ABx = B.X - A.X;
            double ABy = B.Y - A.Y;
            double ABz = B.Z - A.Z;
            //CD = D-C
            double CDx = D.X - C.X;
            double CDy = D.Y - C.Y;
            double CDz = D.Z - C.Z;

            //get the dot product = AB . CD
            double dot_product = (ABx * CDx) + (ABy * CDy) + (ABz * CDz);

            //get the vectors lengths using pythagoras
            //||AB||
            double AB_length = Math.Sqrt((ABx * ABx) + (ABy * ABy) + (ABz * ABz));
            //||CD||
            double CD_length = Math.Sqrt((CDx * CDx) + (CDy * CDy) + (CDz * CDz));

            //angle of j2 in radians
            double angle = Math.Acos((dot_product) / (AB_length * CD_length));

            // Convert the angle from radians to degrees
            double degrees = angle * (180 / Math.PI);

            return degrees;

        }

        public double calculateAngleBetweenVectorAndYaxis(Joint j1, Joint j2)
        {
            //get the position of all the 4 joints
            var A = j1.Position;
            var B = j2.Position;

            //get the difference between 2 joints to get the vectors Coordinates
            //AB = B-A
            double ABx = B.X - A.X;
            double ABy = B.Y - A.Y;
            double ABz = B.Z - A.Z;

            //get the vectors lengths using pythagoras
            //||AB||
            double AB_length = Math.Sqrt((ABx * ABx) + (ABy * ABy) + (ABz * ABz));

            //calculate the angles between vector and Y axis
            double angle = Math.Acos(ABy / AB_length);

            // Convert the angle from radians to degrees
            double degrees = angle * (180 / Math.PI);

            return degrees;

        }

        public double jointDistance(Joint p1, Joint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.Position.X - p2.Position.X, 2) +
                Math.Pow(p1.Position.Y - p2.Position.Y, 2) +
                Math.Pow(p1.Position.Z - p2.Position.Z, 2));
        }

        public double jointDistance2D(Joint p1, Joint p2)
        {
            return Math.Sqrt(
                Math.Pow(p1.Position.X - p2.Position.X, 2) +
                Math.Pow(p1.Position.Y - p2.Position.Y, 2));
        }

        public Joint Center(Joint p1, Joint p2)
        {

            Joint center = new Joint();

            var j1 = p1.Position;
            var j2 = p2.Position;

            center.Position.X = ((j1.X + j2.X) / 2);
            center.Position.Y = ((j1.Y + j2.Y) / 2);
            center.Position.Z = ((j1.Z + j2.Z) / 2);
            
            return center;
        }

        public List<JointType> JointTypes = new List<JointType>();

    }
}
