using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Platformer
{
    public class MazeCell : MonoBehaviour
    {
        [SerializeField]
        private GameObject _leftWall;

        [SerializeField]
        private GameObject _rightWall;

        [SerializeField]
        private GameObject _frontWall;

        [SerializeField]
        private GameObject _backWall;

        [SerializeField]
        private GameObject _unvisitedBlock;

        public bool IsVisited { get; private set; }

        public void Visit()
        {
            IsVisited = true;
            if (_unvisitedBlock != null)
            {
                _unvisitedBlock.SetActive(false);
            }
        }

        public void ClearLeftWall()
        {
            if (_leftWall != null)
            {
                _leftWall.SetActive(false);
            }
        }

        public void ClearRightWall()
        {
            if (_rightWall != null)
            {
                _rightWall.SetActive(false);
            }
        }

        public void ClearFrontWall()
        {
            if (_frontWall != null)
            {
                _frontWall.SetActive(false);
            }
        }

        public void ClearBackWall()
        {
            if (_backWall != null)
            {
                _backWall.SetActive(false);
            }
        }

        public bool HasLeftWall() => _leftWall != null && _leftWall.activeSelf;
        public bool HasRightWall() => _rightWall != null && _rightWall.activeSelf;
        public bool HasFrontWall() => _frontWall != null && _frontWall.activeSelf;
        public bool HasBackWall() => _backWall != null && _backWall.activeSelf;

        public void ResetCell()
        {
            IsVisited = false;

            if (_unvisitedBlock != null)
            {
                _unvisitedBlock.SetActive(true);
            }

            if (_leftWall != null)
            {
                _leftWall.SetActive(true);
            }
            if (_rightWall != null)
            {
                _rightWall.SetActive(true);
            }
            if (_frontWall != null)
            {
                _frontWall.SetActive(true);
            }
            if (_backWall != null)
            {
                _backWall.SetActive(true);
            }
        }
    }
}
