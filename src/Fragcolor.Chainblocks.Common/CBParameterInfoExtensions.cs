/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

using System.Runtime.CompilerServices;

using Fragcolor.Chainblocks.Collections;

namespace Fragcolor.Chainblocks
{
  public static class CBParameterInfoExtensions
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string? Name(this ref CBParameterInfo info)
    {
      return (string?)info._name;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref CBTypesInfo Types(this ref CBParameterInfo info)
    {
      return ref info._types;
    }
  }
}
