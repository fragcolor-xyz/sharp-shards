/* SPDX-License-Identifier: BUSL-1.1 */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

#nullable enable

using System;

using UnityEngine;

namespace Fragcolor.Chainblocks.UnityEngine
{
  public class ChainblocksController : MonoBehaviour
  {
    private static bool _initialized;

    private static LispEnv? _env;
    private static CBNodeRef _node;

    public static LispEnv Env
    {
      get
      {
        if (!_initialized)
          throw new InvalidOperationException();
        return _env!;
      }
      private set { _env = value; }
    }

    public static CBNodeRef Node
    {
      get
      {
        if (!_initialized)
          throw new InvalidOperationException();
        return _node;
      }
      private set { _node = value; }
    }

    void Awake()
    {
      if (!_initialized)
      {
        Env = new LispEnv();
        Node = Native.Core.CreateNode();
        _initialized = true;
      }
      else
      {
        Destroy(this.gameObject);
      }
    }

    void Update()
    {
      Native.Core.Tick(Node);
    }
  }
}