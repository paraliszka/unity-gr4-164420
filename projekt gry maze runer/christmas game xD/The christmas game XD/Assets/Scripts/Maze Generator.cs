using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Platformer
{
    public class MazeGenerator : MonoBehaviour
    {
        [SerializeField]
        private MazeCell _mazeCellPrefab;

        [SerializeField]
        private int _mazeWidth = 10;

        [SerializeField]
        private int _mazeDepth = 10;

        private MazeCell[,] _mazeGrid;

        [SerializeField]
        private GameObject _playerPrefab;

        [SerializeField]
        private Camera _mainCamera;

        [SerializeField]
        private GameObject _awardPrefab;

        [SerializeField]
        private List<GameObject> _obstaclePrefabs;

        private GameManager _gameManager;

        [SerializeField]
        private float _cellSpacing = 1.0f;

        [SerializeField]
        private int _loopCount = 5;

        private GameObject _player;
        private GameObject _award;

        public void SetMazeSize(int size)
        {
            _mazeWidth = size;
            _mazeDepth = size;
        }

        IEnumerator Start()
        {
            _mazeWidth = GameSettings.MazeSize;
            _mazeDepth = GameSettings.MazeSize;

            _mazeGrid = new MazeCell[_mazeWidth, _mazeDepth];

            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeDepth; z++)
                {
                    _mazeGrid[x, z] = Instantiate(_mazeCellPrefab,
                        new Vector3(x * _cellSpacing, 0, z * _cellSpacing), Quaternion.identity);
                }
            }
            AdjustCameraPosition();

            yield return GenerateMaze(null, _mazeGrid[0, 0]);

            AddLoops();
            SpawnPlayer();
            SpawnAward();
            SpawnObstacles();
            StartTimer();
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
            var unvisitedCells = GetUnvisitedCells(currentCell);

            return unvisitedCells.OrderBy(_ => Random.Range(1, 10)).FirstOrDefault();
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
            for (int i = 0; i < _loopCount; i++)
            {
                int x = Random.Range(0, _mazeWidth);
                int z = Random.Range(0, _mazeDepth);

                MazeCell currentCell = _mazeGrid[x, z];
                var neighbors = GetVisitedNeighbors(currentCell).ToList();

                if (neighbors.Count > 0)
                {
                    MazeCell neighbor = neighbors[Random.Range(0, neighbors.Count)];
                    ClearWalls(currentCell, neighbor);
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

        private void SpawnPlayer()
        {
            Vector3 playerSpawnPosition = new Vector3(0, 0.5f, 0);
            _player = Instantiate(_playerPrefab, playerSpawnPosition, Quaternion.identity);

            Camera playerCamera = _player.GetComponentInChildren<Camera>();
            if (playerCamera != null)
            {
                _mainCamera.gameObject.SetActive(false);
                playerCamera.gameObject.SetActive(true);
            }
        }

        private void SpawnAward()
        {
            Vector3 awardPosition = new Vector3((_mazeWidth - 1) * _cellSpacing, 0, (_mazeDepth - 1) * _cellSpacing);

            _award = Instantiate(_awardPrefab, awardPosition, Quaternion.identity);
            
            MazeCell awardCell = _award.GetComponent<MazeCell>();
            if (awardCell != null)
            {
                awardCell.ClearLeftWall();
                awardCell.ClearRightWall();
                awardCell.ClearFrontWall();
                awardCell.ClearBackWall();
            }
        }

        private void SpawnObstacles()
        {
            List<Vector2> occupiedPositions = new List<Vector2>
            {
                new Vector2(0, 0),
                new Vector2(_mazeWidth - 1, _mazeDepth - 1)
            };

            for (int x = 0; x < _mazeWidth; x++)
            {
                for (int z = 0; z < _mazeDepth; z++)
                {
                    if (occupiedPositions.Contains(new Vector2(x, z)))
                        continue;

                    if (_mazeGrid[x, z].IsVisited && Random.Range(0f, 1f) < 0.1f)
                    {
                        Vector3 obstaclePosition = new Vector3(
                            x * _cellSpacing,
                            0,
                            z * _cellSpacing
                        );

                        GameObject obstaclePrefab = _obstaclePrefabs[Random.Range(0, _obstaclePrefabs.Count)];
                        GameObject obstacle = Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);

                        MazeCell obstacleCell = obstacle.GetComponent<MazeCell>();
                        if (obstacleCell != null)
                        {
                            obstacleCell.ClearLeftWall();
                            obstacleCell.ClearRightWall();
                            obstacleCell.ClearFrontWall();
                            obstacleCell.ClearBackWall();
                        }
                    }
                }
            }
        }

        private void StartTimer()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _gameManager.StartTimer();
        }

        private void AdjustCameraPosition()
        {
            float mazeWidth = _mazeWidth * _cellSpacing;
            float mazeDepth = _mazeDepth * _cellSpacing;

            Vector3 mazeCenter = new Vector3(mazeWidth / 2 - _cellSpacing / 2, 0, mazeDepth / 2 - _cellSpacing / 2);

            _mainCamera.transform.position = new Vector3(mazeCenter.x, Mathf.Max(mazeWidth, mazeDepth), mazeCenter.z);
            _mainCamera.transform.LookAt(mazeCenter);
        }
    }
}
