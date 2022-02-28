using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace NikolayTrofimovHTC
{
    [System.Serializable]
    public struct MagnitePoint
    {
        public List<SpringJoint> JointList;
        public List<Rigidbody> RG;
        public List<ParticleSystem> HighLight;
        public Transform BlueObj;
        public Transform RedObj;
        public Vector3 BluePos;
        public Vector3 RedPos;
    }

    public class CharMagnetic : MonoBehaviour
    {
        [SerializeField] private float _spellDistance = 20;
        [SerializeField] private float _maxMagniteForce = 50;
        [SerializeField] private MagnitePoint _magniteSpell;
        [SerializeField] private Transform _blueHolder;
        [SerializeField] private Transform _redHolder;
        [SerializeField] private Material _redMat;
        [SerializeField] private Material _blueMat;
        [SerializeField] private Material _yellowMat;
        [SerializeField] private ParticleSystem _hlReference;

        public void SetBlue(Transform trans)
        {
            _magniteSpell.BlueObj = trans;
            _magniteSpell.BlueObj.position = trans.position;
            Highlighting(true, trans);
            CheckToJoint();
        }

        public void SetRed(Transform trans)
        {
            _magniteSpell.RedObj = trans;
            _magniteSpell.RedObj.position = trans.position;
            Highlighting(false, trans);
            CheckToJoint();
        }

        public void SetBlue(Vector3 trans)
        {
            _magniteSpell.BlueObj = _blueHolder;
            _magniteSpell.BluePos = trans;
            _blueHolder.position = trans;
            _blueHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }

        public void SetRed(Vector3 trans)
        {
            _magniteSpell.RedObj = _redHolder;
            _magniteSpell.RedPos = trans;
            _redHolder.position = trans;
            _redHolder.GetChild(0).gameObject.SetActive(true);
            CheckToJoint();
        }

        private void CheckToJoint()
        { 
            if(_magniteSpell.BlueObj != null && _magniteSpell.RedObj != null)
            {
                if(Vector3.Distance(_magniteSpell.RedPos, _magniteSpell.BluePos) < _spellDistance)
                {
                    CreateJoint();
                }
                else
                {
                    EreaseSpell();
                }
            }
        }

        private void CreateJoint()
        {
            SpringJoint sp = _magniteSpell.BlueObj.gameObject.AddComponent<SpringJoint>();
            sp.autoConfigureConnectedAnchor = false;
            sp.anchor = Vector3.zero;
            sp.connectedAnchor = Vector3.zero;
            sp.enableCollision = true;
            sp.enablePreprocessing = false;

            sp.connectedBody = _magniteSpell.RedObj.GetComponent<Rigidbody>();

            EreaseSpell();
            _magniteSpell.JointList.Add(sp);

            Rigidbody rg = sp.GetComponent<Rigidbody>();
            _magniteSpell.RG.Add(rg);

            AddRG(sp.connectedBody);
        }

        private void AddRG(Rigidbody rg)
        {
            if (_magniteSpell.RG == null) return;

            for (int i = 0; i < _magniteSpell.RG.Count; i++)
            {
                if (rg == _magniteSpell.RG[i]) break;
                if (i == _magniteSpell.RG.Count - 1)
                {
                    _magniteSpell.RG.Add(rg);
                    break;
                }
            }
        }

        public void Highlighting(bool isBlue, Transform trans)
        {
            ParticleSystem ps = Instantiate(_hlReference, trans, false);

            if (isBlue) ps.GetComponent<Renderer>().material = _blueMat;
            else ps.GetComponent<Renderer>().material = _redMat;

            _magniteSpell.HighLight.Add(ps);
        }

        private void EreaseSpell()
        {
            _magniteSpell.BlueObj = null;
            _magniteSpell.RedObj = null;

            for (int i = 0; i < _magniteSpell.HighLight.Count; i++)
            {
                Destroy(_magniteSpell.JointList[i]);
            }

            for (int i = 0; i < _magniteSpell.RG.Count; i++)
            {
                _magniteSpell.RG[i].angularDrag = 0.05f;
                _magniteSpell.RG[i].drag = 0;
                _magniteSpell.RG[i].WakeUp();
            }

            _magniteSpell.HighLight.Clear();
            _magniteSpell.RG.Clear();
            EreaseSpell();

            for(int i = 0; i < _magniteSpell.HighLight.Count; i++)
            {
                Destroy(_magniteSpell.HighLight[i]);
            }
            _magniteSpell.HighLight.Clear();
            DisableHolders();
        }

        private void DisableHolders()
        {
            _blueHolder.GetChild(0).gameObject.SetActive(false);
            _redHolder.GetChild(0).gameObject.SetActive(false);
        }

        public void ChangeSpringPower(float fNum)
        {
            if(_magniteSpell.JointList.Count > 0)
            {
                for(int i = 0; i < _magniteSpell.JointList.Count; i++)
                {
                    _magniteSpell.JointList[i].spring += fNum;
                    _magniteSpell.JointList[i].damper += fNum;

                    _magniteSpell.JointList[i].damper = Mathf.Clamp(_magniteSpell.JointList[i].damper, 0, _maxMagniteForce);
                    _magniteSpell.JointList[i].spring = Mathf.Clamp(_magniteSpell.JointList[i].spring, 0, _maxMagniteForce);
                }

                for(int i= 0; i < _magniteSpell.RG.Count; i++)
                {
                    _magniteSpell.RG[i].WakeUp();
                    _magniteSpell.RG[i].angularDrag += fNum;
                    _magniteSpell.RG[i].drag += fNum;
                    _magniteSpell.RG[i].angularDrag = Mathf.Clamp(_magniteSpell.RG[i].angularDrag, 0, _maxMagniteForce);
                    _magniteSpell.RG[i].drag = Mathf.Clamp(_magniteSpell.RG[i].drag, 0, _maxMagniteForce);
                }
            }
        }
    }
}