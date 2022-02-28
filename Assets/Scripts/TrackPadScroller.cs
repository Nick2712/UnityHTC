using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


namespace NikolayTrofimovHTC
{
    public class TrackPadScroller : MonoBehaviour
    {
        [SerializeField] private float _speed = 10;
        [SerializeField] private float _deadZone = 0.1f;

        //private SteamVR_RenderModel _vive; // - нет у меня такого
        private CharMagnetic _magnite;

        private void Update()
        {
            //if (_vive == null)
            //    _vive = GetComponentInChildren<SteamVR_RenderModel>();

            float dp = ViveInput.GetPadTouchDelta(HandRole.RightHand).y;
            float dl = ViveInput.GetPadTouchDelta(HandRole.LeftHand).y; // <- для левой руки

            ControllerEffect(dp);
            ControllerEffect(dl);

            //if (ViveInput.GetPressUp(HandRole.RightHand, ControllerButton.PadTouch))
            //    vive.controllerModeState.bScrollWheelVisible = false;

            void ControllerEffect(float d)
            {
                if (Mathf.Abs(d) > _deadZone)
                {
                    _magnite.ChangeSpringPower(dp * _speed);
                    //vive.controllerModeState.bScrollWheelVisible = true;
                }
            }
        }
    }
}