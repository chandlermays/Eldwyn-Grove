using UnityEngine;
//---------------------------------

namespace EldwynGrove.Saving
{
    [System.Serializable]
    public class MySerializableVector3
    {
        private readonly float x;
        private readonly float y;
        private readonly float z;

        /*---------------------
        | --- Constructor --- |
        ---------------------*/
        public MySerializableVector3(Vector3 vector)
        {
            x = vector.x;
            y = vector.y;
            z = vector.z;
        }

        /*--------------------------------------------------
        | --- ToVector3: Convert the Data to a Vector3 --- |
        --------------------------------------------------*/
        public Vector3 ToVector3()
        {
            return new Vector3(x, y, z);
        }
    }
}