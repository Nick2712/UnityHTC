using UnityEngine;


namespace NikolayTrofimovHTC
{
    public class Centering : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private CapsuleCollider _myCol;

        private Vector3 _vec;

        private void OnValidate()
        {
            _myCol = GetComponent<CapsuleCollider>();
        }

        private void Start()
        {
            FindTeleportPivotTarget();
            _vec.y = _myCol.center.y;
        }

        private void Update()
        {
            _vec.x = _pivot.localPosition.x;
            _vec.z = _pivot.localPosition.z;

            _myCol.center = _vec;
        }

        private void FindTeleportPivotTarget()
        {
            foreach(var cam in Camera.allCameras)
            {
                if(!cam.enabled) 
                { 
                    continue; 
                }
                if(cam.stereoTargetEye != StereoTargetEyeMask.Both)
                {
                    continue;
                }
                _pivot = cam.transform;
            }
        }
    }
}