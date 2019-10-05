using System;
using System.Collections.Generic;
using System.Linq;
using Game.Code;
using UnityEngine;

namespace Game.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public Grid grid;
        public CharacterTile characterTilePrefab;
        public CursorTile cursorTile;
        public Camera mainCamera;
        public WordList wordList;
        public WinnerPanel winnerPanel;
        public float panSpeed = 0.05f;

        private List<CharacterTile> _tiles = new List<CharacterTile>();

        private bool _dragging;
        private Vector2Int _dragStart;

        private bool _cameraDragging;
        private Vector3 _cameraDragMouseStart;

        private int _steps;
        private bool _won;

        private void Start()
        {
            SetWord("nothing", new Vector2Int(-3, 0), WordDirection.Horizontal);
        }

        private void Update()
        {
            HandleCameraDragging();

            if (_cameraDragging)
                return;

            HandleTileSelection();
        }

        private void HandleTileSelection()
        {
            var offset = new Vector3(0.5f, 0.5f);
            var cursorPosition =
                (Vector2Int) grid.WorldToCell(mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset);
            cursorTile.transform.position = grid.CellToWorld((Vector3Int) cursorPosition);

            if (!_dragging && Input.GetMouseButtonDown(0))
            {
                _dragging = true;
                _dragStart = cursorPosition;
            }

            if (_dragging)
            {
                cursorTile.CursorState = cursorPosition.x == _dragStart.x || cursorPosition.y == _dragStart.y
                    ? CursorState.Inserting
                    : CursorState.Invalid;
            }

            if (_dragging && Input.GetMouseButtonUp(0))
            {
                _dragging = false;
                cursorTile.CursorState = CursorState.Regular;

                var direction = WordDirection.None;
                var startPosition = Vector2Int.zero;
                var endPosition = Vector2Int.zero;
                var wordLength = 0;

                // Check if selection was vertical
                if (cursorPosition.x == _dragStart.x)
                {
                    direction = WordDirection.Vertical;
                    startPosition = cursorPosition.y > _dragStart.y ? cursorPosition : _dragStart;
                    endPosition = cursorPosition.y < _dragStart.y ? cursorPosition : _dragStart;
                    wordLength = startPosition.y - endPosition.y + 1;
                }

                // Check if selection was horizontal
                if (cursorPosition.y == _dragStart.y)
                {
                    direction = WordDirection.Horizontal;
                    startPosition = cursorPosition.x < _dragStart.x ? cursorPosition : _dragStart;
                    endPosition = cursorPosition.x > _dragStart.x ? cursorPosition : _dragStart;
                    wordLength = endPosition.x - startPosition.x + 1;
                }

                // Only continue on straight selection line
                if (direction == WordDirection.None)
                    return;

                // Require minimum length
                if (wordLength < 3)
                    return;

                var pattern = string.Empty;
                var currentPosition = startPosition;
                var originalWordLength = wordLength;
                var originalStartPosition = startPosition;

                // Include tiles before selection in pattern
                while (true)
                {
                    if (direction == WordDirection.Horizontal)
                        currentPosition += new Vector2Int(-1, 0);
                    else
                        currentPosition += new Vector2Int(0, 1);

                    var tile = GetTile(currentPosition);

                    if (tile is null)
                        break;

                    pattern = tile.Character + pattern;
                    startPosition = currentPosition;
                    wordLength++;
                }

                currentPosition = originalStartPosition;
                // Capture selected tiles
                for (var i = 0; i < originalWordLength; i++)
                {
                    var tile = GetTile(currentPosition);

                    if (tile is null)
                        pattern += '.';
                    else
                        pattern += tile.Character;

                    if (direction == WordDirection.Horizontal)
                        currentPosition += new Vector2Int(1, 0);
                    else
                        currentPosition += new Vector2Int(0, -1);
                }

                // Include tiles after selection in pattern
                while (true)
                {
                    var tile = GetTile(currentPosition);

                    if (tile is null)
                        break;

                    pattern += tile.Character;
                    endPosition = currentPosition;
                    wordLength++;

                    if (direction == WordDirection.Horizontal)
                        currentPosition += new Vector2Int(1, 0);
                    else
                        currentPosition += new Vector2Int(0, -1);
                }

                // Require at least one intersection
                if (pattern.All(p => p == '.'))
                    return;

                Debug.Log(pattern);

                // Search for a fitting word
                string word = wordList.GetRandomMatchingWord(pattern);

                if (word is null)
                    return;

                SetWord(word, startPosition, direction);
                _steps++;

                if (word == "everything" && !_won)
                {
                    winnerPanel.Show(_steps);
                    _won = true;
                }
            }
        }

        private CharacterTile GetTile(Vector2Int position)
        {
            return _tiles.FirstOrDefault(t => t.Position == position);
        }

        private void HandleCameraDragging()
        {
            if (Input.mouseScrollDelta.y < 0)
                mainCamera.orthographicSize = Math.Min(mainCamera.orthographicSize * 1.1f, 13f);
            if (Input.mouseScrollDelta.y > 0)
                mainCamera.orthographicSize = Math.Max(mainCamera.orthographicSize * 0.9f, 5f);

            if (Input.GetMouseButtonDown(1))
            {
                _dragging = false;
                _cameraDragging = true;
                _cameraDragMouseStart = Input.mousePosition;
            }

            if (Input.GetMouseButtonUp(1))
            {
                _cameraDragging = false;
            }

            if (_cameraDragging)
            {
                var delta = Input.mousePosition - _cameraDragMouseStart;
                var orthographicSize = mainCamera.orthographicSize;
                mainCamera.transform.Translate(
                    delta.x * -panSpeed * orthographicSize / 5f,
                    delta.y * -panSpeed * orthographicSize / 5f, 0);
                _cameraDragMouseStart = Input.mousePosition;
            }
        }

        public void ResetCamera()
        {
            var mainCameraTransform = mainCamera.transform;
            mainCameraTransform.position = new Vector3(0, 0, mainCameraTransform.position.z);
        }

        private void SetWord(string word, Vector2Int position, WordDirection direction)
        {
            var currentPosition = position;
            foreach (var character in word)
            {
                var characterTile = GetTile(currentPosition);
                if (characterTile is null)
                {
                    characterTile = Instantiate(characterTilePrefab, grid.transform);
                    characterTile.name = $"{character.ToString().ToUpper()} [{currentPosition.x}|{currentPosition.y}]";
                    characterTile.Position = currentPosition;
                    characterTile.transform.position = grid.CellToWorld((Vector3Int) currentPosition);
                    _tiles.Add(characterTile);
                }

                characterTile.Character = character;
                currentPosition = direction == WordDirection.Vertical
                    ? new Vector2Int(currentPosition.x, currentPosition.y - 1)
                    : new Vector2Int(currentPosition.x + 1, currentPosition.y);
            }
        }
    }
}