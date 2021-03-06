/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

using System.Runtime.InteropServices;

namespace Fragcolor.Shards
{
  /// <summary>
  /// Represents a boolean value.
  /// </summary>
  [StructLayout(LayoutKind.Sequential)]
  public struct SHBool
  {
    //! Native struct, don't edit
    internal byte _value;

    public static implicit operator bool(SHBool b) => b._value != 0;
    public static implicit operator SHBool(bool b) => new() { _value = (byte)(b ? 1: 0)};
  }
}
