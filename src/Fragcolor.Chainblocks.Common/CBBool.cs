/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

using System.Runtime.InteropServices;

namespace Fragcolor.Chainblocks
{
  [StructLayout(LayoutKind.Sequential)]
  public struct CBBool
  {
    internal byte _value;

    public static implicit operator bool(CBBool b) => b._value != 0;
    public static implicit operator CBBool(bool b) => new() { _value = (byte)(b ? 1: 0)};
  }
}
