﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using Random = System.Random;

public class Tile : NetworkBehaviour
{
    public TileCoord Coord;
    public Connection[] Connections = new Connection[6];
    public Token Token;

    [SyncVar]
    public bool IsSolid;
    public bool IsSelectable;

    private GameObject _model;
    private GameObject _selector;

    void Start()
    {
        _model = transform.Find("Model").gameObject;
        _selector = transform.Find("Selector").gameObject;
    }
    void Update()
    {
        _selector.SetActive(IsSelectable);
        _model.SetActive(IsSolid);
    }

    public void SelectTile()
    {
        Player.LocalPlayer.CmdSelectTile(Coord.R, Coord.Q);
    }

    [TargetRpc]
    public void TargetIsSelectable(NetworkConnection connectionToClient, bool isSelectable)
    {
        IsSelectable = isSelectable;
    }

    [ClientRpc]
    public void RpcMove(int r, int q)
    {
        Coord = new TileCoord(r,q);
        transform.position = Coord.Position;
    }
}

[Serializable]
public class TileCoord
{
    //Cubic Coordinates
    public int X => Q;
    public int Y => - Q - R;
    public int Z => R;

    //Axial Coordinates
    public int R { get; }
    public int Q { get; }

    //World Coordinates
    private float VerticalOffset { get; }
    private Vector3 Axis => new Vector3(0.5f, 0, 0.866f).normalized;
    public Vector3 Position => Vector3.Scale(Axis * R + Vector3.right * Q + Vector3.up * VerticalOffset, new Vector3(1, 0, 1));

    public TileCoord(int x, int y, int z)
    {
        Q = x;
        R = z;
        var r = new Random();
        VerticalOffset = r.Next(-300, 300) * 0.0001f;
    }
    public TileCoord(int r, int q)
    {
        R = r;
        Q = q;
        var random = new Random();
        VerticalOffset = random.Next(-300, 300) * 0.0001f;
    }

}