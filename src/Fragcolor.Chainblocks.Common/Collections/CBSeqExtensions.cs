/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fragcolor.Chainblocks.Collections
{
  public static class CBSeqExtensions
  {
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref CBVar At(this ref CBSeq seq, uint index)
    {
      return ref seq[index];
    }

    public static void Insert(this ref CBSeq seq, uint index, ref CBVar var)
    {
      if (index > seq._length) throw new IndexOutOfRangeException();

      if (index == seq._length)
      {
        Push(ref seq, ref var);
        return;
      }

      var insertDelegate = Marshal.GetDelegateForFunctionPointer<SeqInsertDelegate>(Native.Core._seqInsert);
      insertDelegate(ref seq, index, ref var);
    }

    public static CBVar Pop(this ref CBSeq seq)
    {
      if (seq._length == 0) throw new InvalidOperationException();

      var popDelegate = Marshal.GetDelegateForFunctionPointer<SeqPopDelegate>(Native.Core._seqPop);
      return popDelegate(ref seq);
    }

    public static void Push(this ref CBSeq seq, ref CBVar var)
    {
      var pushDelegate = Marshal.GetDelegateForFunctionPointer<SeqPushDelegate>(Native.Core._seqPush);
      pushDelegate(ref seq, ref var);
    }

    public static void RemoveAt(this ref CBSeq seq, uint index)
    {
      if (index >= seq._length) throw new IndexOutOfRangeException();

      var deleteDelegate = Marshal.GetDelegateForFunctionPointer<SeqSlowDeleteDelegate>(Native.Core._seqSlowDelete);
      deleteDelegate(ref seq, index);
    }
  }

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate void SeqSlowDeleteDelegate(ref CBSeq seq, uint index);

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate void SeqInsertDelegate(ref CBSeq seq, uint index, ref CBVar var);

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate CBVar SeqPopDelegate(ref CBSeq seq);

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate void SeqPushDelegate(ref CBSeq seq, ref CBVar var);
}
