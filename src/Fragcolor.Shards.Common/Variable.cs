/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Fragcolor.Shards
{
  /// <summary>
  /// Wraps a <see cref="SHVar"/> as a managed object.
  /// </summary>
  /// <remarks>
  /// Once this instance is not used anymore, <see cref="Dispose()"/> must be called to clean up the unmanaged resource.
  /// </remarks>
  public sealed class Variable : IDisposable
  {
    private IntPtr _mem;
    private readonly bool _destroy;
    private int _disposeState;

    /// <summary>
    /// Gets a reference to the unmanaged <see cref="SHVar"/>.
    /// </summary>
    public ref SHVar Value
    {
      get
      {
        unsafe
        {
          return ref Unsafe.AsRef<SHVar>(_mem.ToPointer());
        }
      }
    }

    /// <summary>
    /// Gets a pointer to the memory location of the unmanaged <see cref="SHVar"/>.
    /// </summary>
    public IntPtr Ptr => _mem;

    public Variable(bool destroy = true)
    {
      _destroy = destroy;
      _mem = Native.Core.Alloc(32);
    }

    ~Variable()
    {
      Dispose(false);
    }

    /// <summary>
    /// Returns a clone of the variable with the same value and type.
    /// </summary>
    /// <returns>A clone of the variable with the same value and type.</returns>
    /// <seealso cref="SHCoreExtensions.CloneVar(ref SHCore, ref SHVar, ref SHVar)"/>
    public Variable Clone()
    {
      var variable = new Variable();
      Native.Core.CloneVar(ref variable.Value, ref Value);
      return variable;
    }

    /// <summary>
    /// Cleans up the wrapped <see cref="SHVar"/> and releases its memory.
    /// </summary>
    public void Dispose()
    {
      Dispose(true);
      GC.SuppressFinalize(this);
    }

    private void Dispose(bool _)
    {
      if (Interlocked.CompareExchange(ref _disposeState, 1, 0) != 0) return;

      if (_destroy)
      {
        Native.Core.DestroyVar(_mem);
      }
      Native.Core.Free(_mem);
      _mem = IntPtr.Zero;
    }
  }
}
