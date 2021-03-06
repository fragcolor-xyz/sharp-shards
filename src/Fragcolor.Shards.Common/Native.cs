/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace Fragcolor.Shards
{
  /// <summary>
  /// Main entry point to native shards.
  /// </summary>
  public static class Native
  {
    private static SHCore _core;
    private static IntPtr _corePtr;

    private static readonly object _syncLock = new();

    /// <summary>
    /// Gets the core struct.
    /// </summary>
    /// <remarks>
    /// Before accessing this property, the env must be initialized (see <see cref="ScriptingEnv"/>).
    /// </remarks>
    public static ref SHCore Core
    {
      get
      {
        if (Volatile.Read(ref _corePtr) != IntPtr.Zero)
        {
          return ref _core;
        }

        lock (_syncLock)
        {
          if (Volatile.Read(ref _corePtr) == IntPtr.Zero)
          {
            var ptr = NativeMethods.shardsInterface(0x20200101);
            _core = Marshal.PtrToStructure<SHCore>(ptr);
            Volatile.Write(ref _corePtr, ptr);
          }
        }

        return ref _core;
      }
    }
  }
}
