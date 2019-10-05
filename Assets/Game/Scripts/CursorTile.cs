using System;
using System.Collections;
using System.Collections.Generic;
using Game.Code;
using UnityEngine;
using UnityEngine.Rendering;

public class CursorTile : MonoBehaviour
{
    public SpriteRenderer cursorSprite;
    public Color regularColor;
    public Color invalidColor;
    public Color insertingColor;
    public Color undecidedColor;

    private CursorState _cursorState = CursorState.Regular;

    public CursorState CursorState
    {
        get => _cursorState;
        set
        {
            _cursorState = value;
            Color targetColor;
            switch (value)
            {
                case CursorState.Regular:
                    targetColor = regularColor;
                    break;
                case CursorState.Invalid:
                    targetColor = invalidColor;
                    break;
                case CursorState.Inserting:
                    targetColor = insertingColor;
                    break;
                case CursorState.Undecided:
                    targetColor = undecidedColor;
                    break;
                default:
                    throw new ArgumentException();
            }

            cursorSprite.color = targetColor;
        }
    }
}