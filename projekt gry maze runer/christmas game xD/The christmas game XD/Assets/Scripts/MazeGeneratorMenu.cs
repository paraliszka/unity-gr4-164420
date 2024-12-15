using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer
{
    public class MazeGeneratorMenu : MonoBehaviour
    {
        [SerializeField]
        private MazeCell _mazeCellPrefab;

        [SerializeField]
        private int _mazeWidth;

        [SerializeField]
        private int _mazeDepth;

        private MazeCell[,] _mazeGrid;

        [SerializeField]
        private Camera _mainCamera;

        private GameManager _gameManager;

        [SerializeField]
        private float _cellSpacing = 1.0f;

        [SerializeField]
        private int _loopCount = 5;

        [SerializeField]
        private float _mazeDelay = 2.0f;

        IEnumerator Start()
        {
            while (true)
            {
                ClearMaze();

                _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

                for (int x = 0; x < _mazeWidth; x++)
                {
                    for (int z = 0; z < _mazeDepth; z++)
                    {
                        _mazeGrid[x, z] = Instantiate(_mazeCellPrefab,
                            new Vector3(x * _cellSpacing, 0, z * _cellSpacing), Quaternion.identity, transform);
                    }
                }

                AdjustCameraPosition();

                yield return GenerateMaze(null, _mazeGrid[0, 0]);

                AddLoops();

                yield return new WaitForSeconds(_mazeDelay);
            }
        }

        private IEnumerator GenerateMaze(MazeCell previousCell, MazeCell currentCell)
        {
            currentCell.Visit();
            ClearWalls(previousCell, currentCell);

            yield return new WaitForSeconds(0.05f);

            MazeCell nextCell;

            do
            {
                nextCell = GetNextUnvisitedCell(currentCell);

                if (nextCell != null)
                {
                    yield return GenerateMaze(currentCell, nextCell);
                }
            } while (nextCell != null);
        }

        private MazeCell GetNextUnvisitedCell(MazeCell currentCell)
        {
            var unvisitedCells = GetUnvisitedCells(currentCell).ToList();

            if (unvisitedCells.Count == 0) return null;

            return unvisitedCells[Random.Range(0, unvisitedCells.Count)];
        }

        private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currentCell)
        {
            int x = Mathf.RoundToInt(currentCell.transform.position.x / _cellSpacing);
            int z = Mathf.RoundToInt(currentCell.transform.position.z / _cellSpacing);

            if (x + 1 < _mazeWidth)
            {
                var cellToRight = _mazeGrid[x + 1, z];
                if (cellToRight.IsVisited == false)
                {
                    yield return cellToRight;
                }
            }

            if (x - 1 >= 0)
            {
                var cellToLeft = _mazeGrid[x - 1, z];
                if (cellToLeft.IsVisited == false)
                {
                    yield return cellToLeft;
                }
            }

            if (z + 1 < _mazeDepth)
            {
                var cellToFront = _mazeGrid[x, z + 1];
                if (cellToFront.IsVisited == false)
                {
                    yield return cellToFront;
                }
            }

            if (z - 1 >= 0)
            {
                var cellToBack = _mazeGrid[x, z - 1];
                if (cellToBack.IsVisited == false)
                {
                    yield return cellToBack;
                }
            }
        }

        private void ClearWalls(MazeCell previousCell, MazeCell currentCell)
        {
            if (previousCell == null) return;

            if (previousCell.transform.position.x < currentCell.transform.position.x)
            {
                previousCell.ClearRightWall();
                currentCell.ClearLeftWall();
                return;
            }

            if (previousCell.transform.position.x > currentCell.transform.position.x)
            {
                previousCell.ClearLeftWall();
                currentCell.ClearRightWall();
                return;
            }

            if (previousCell.transform.position.z < currentCell.transform.position.z)
            {
                previousCell.ClearFrontWall();
                currentCell.ClearBackWall();
                return;
            }

            if (previousCell.transform.position.z > currentCell.transform.position.z)
            {
                previousCell.ClearBackWall();
                currentCell.ClearFrontWall();
                return;
            }
        }

        private void AddLoops()
        {
            int loopsAdded = 0;

            while (loopsAdded < _loopCount)
            {
                int x = Random.Range(0, _mazeWidth);
                int z = Random.Range(0, _mazeDepth);

                MazeCell currentCell = _mazeGrid[x, z];
                var neighbors = GetVisitedNeighbors(currentCell).Where(n => !AreCellsConnected(currentCell, n)).ToList();

                if (neighbors.Count > 0)
                {
                    MazeCell neighbor = neighbors[Random.Range(0, neighbors.Count)];
                    ClearWalls(currentCell, neighbor);
                    loopsAdded++;
                }
            }
        }

        private IEnumerable<MazeCell> GetVisitedNeighbors(MazeCell currentCell) 
        {
            int x = Mathf.RoundToInt(currentCell.transform.position.x / _cellSpacing);
            int z = Mathf.RoundToInt(currentCell.transform.position.z / _cellSpacing);

            if (x + 1 < _mazeWidth)
            {
                var cellToRight = _mazeGrid[x + 1, z];
                if (cellToRight.IsVisited)
                {
                    yield return cellToRight;
                }
            }

            if (x - 1 >= 0)
            {
                var cellToLeft = _mazeGrid[x - 1, z];
                if (cellToLeft.IsVisited)
                {
                    yield return cellToLeft;
                }
            }

            if (z + 1 < _mazeDepth)
            {
                var cellToFront = _mazeGrid[x, z + 1];
                if (cellToFront.IsVisited)
                {
                    yield return cellToFront;
                }
            }

            if (z - 1 >= 0)
            {
                var cellToBack = _mazeGrid[x, z - 1];
                if (cellToBack.IsVisited)
                {
                    yield return cellToBack;
                }
            }
        }

        private void AdjustCameraPosition()
        {
            float mazeWidth = _mazeWidth * _cellSpacing;
            float mazeDepth = _mazeDepth * _cellSpacing;

            Vector3 mazeCenter = new Vector3(mazeWidth / 2 - _cellSpacing / 2, 0, mazeDepth / 2 - _cellSpacing / 2);

            _mainCamera.transform.position = new Vector3(mazeCenter.x, Mathf.Max(mazeWidth, mazeDepth), mazeCenter.z);
            _mainCamera.transform.LookAt(mazeCenter);
        }

        private void ClearMaze()
        {
            if (_mazeGrid != null)
            {
                foreach (var cell in _mazeGrid)
                {
                    if (cell != null)
                    {
                        Destroy(cell.gameObject);
                    }
                }
            }

            _mazeGrid = null;
        }

        private bool AreCellsConnected(MazeCell cellA, MazeCell cellB)
        {
            if (cellA == null || cellB == null) return false;

            Vector3 direction = cellB.transform.position - cellA.transform.position;

            if (direction.x > 0) return !cellA.HasRightWall() && !cellB.HasLeftWall();
            if (direction.x < 0) return !cellA.HasLeftWall() && !cellB.HasRightWall();
            if (direction.z > 0) return !cellA.HasFrontWall() && !cellB.HasBackWall();
            if (direction.z < 0) return !cellA.HasBackWall() && !cellB.HasFrontWall();

            return false;
        }
    }
}
