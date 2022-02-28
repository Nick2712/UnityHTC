using HTC.UnityPlugin.Pointer3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NikolayTrofimovHTC
{
    public class MagniteRaycaster : MonoBehaviour
    {
        public enum TypeOfMagnite
        {
            Blue,
            Red
        }

        [SerializeField] private TypeOfMagnite _colorOfMagnite;

        [Tooltip("ссылка на spell персонажа")]
        [SerializeField] private CharMagnetic _refToChar;

        private RaycastResult _curObj;
        private Pointer3DRaycaster _raycaster;

        private void OnValidate()
        {
            _raycaster = GetComponent<Pointer3DRaycaster>();
        }

        private void LateUpdate()
        {
            Raycasting();
        }

        private void Raycasting()
        {
            _curObj = _raycaster.FirstRaycastResult();
        }

        public void StartMagnite()
        {
            if (_curObj.isValid)
            {
                Rigidbody RG = _curObj.gameObject.GetComponent<Rigidbody>();

                switch (_colorOfMagnite)
                {
                    case TypeOfMagnite.Blue:
                        if (RG != null) _refToChar.SetBlue(_curObj.gameObject.transform);
                        else _refToChar.SetBlue(_curObj.worldPosition);
                        break;
                    case TypeOfMagnite.Red:
                        if (RG != null) _refToChar.SetRed(_curObj.gameObject.transform);
                        else _refToChar.SetRed(_curObj.worldPosition);
                        break;
                }
            }
        }
    }
}