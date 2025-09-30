using System;
using UnityEngine;

public static class Constant
{
    // Joystick Axes Constants
    public const string HORIZONTAL_JOY_RIGHT = "HorizontalRightJoystick";
    public const string HORIZONTAL_JOY_LEFT = "Horizontal";
    public const string VERTICAL_JOY_RIGHT = "VerticalRightJoystick";
    public const string VERTICAL_JOY_LEFT = "Vertical";

    public static readonly Joystick JOY_RIGHT = new Joystick(HORIZONTAL_JOY_RIGHT, VERTICAL_JOY_RIGHT);
    public static readonly Joystick JOY_LEFT = new Joystick(HORIZONTAL_JOY_LEFT, VERTICAL_JOY_LEFT);
}

public struct Joystick
{
    public string Horizontal { get; }
    public string Vertical { get; }

    public Joystick(string horizontal, string vertical)
    {
        Horizontal = horizontal;
        Vertical = vertical;
    }
}

public enum CardinalDirection
{
    North,
    South,
    East,
    West
}


public static class GridDirection
{
    public static readonly Vector3Int North = new Vector3Int(0, 1, 0);
    public static readonly Vector3Int South = new Vector3Int(0, -1, 0);
    public static readonly Vector3Int East  = new Vector3Int(1, 0, 0);
    public static readonly Vector3Int West  = new Vector3Int(-1, 0, 0);
}
