/* SPDX-License-Identifier: BSD-3-Clause */
/* Copyright © 2022 Fragcolor Pte. Ltd. */

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Fragcolor.Shards.Collections
{
  /// <summary>
  /// Extension methods for <see cref="SHExposedTypesInfo"/>.
  /// </summary>
  public static class SHExposedTypesInfoExtensions
  {
    /// <summary>
    /// Gets a reference to the <see cref="SHExposedTypeInfo"/> at the specified index.
    /// </summary>
    /// <param name="infos">A reference to the collection.</param>
    /// <param name="index">The element index.</param>
    /// <returns>A reference to the element at the specified index.</returns>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is equal to or greater than <see cref="SHExposedTypesInfo.Count"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static ref SHExposedTypeInfo At(this ref SHExposedTypesInfo infos, uint index)
    {
      if (index >= infos._length) throw new ArgumentOutOfRangeException(nameof(index));
      return ref infos[index];
    }

    /// <summary>
    /// Inserts a <see cref="SHExposedTypeInfo"/> at the specified index in the collection.
    /// </summary>
    /// <param name="infos">A reference to the collection.</param>
    /// <param name="index">The index at which the element is inserted.</param>
    /// <param name="info">A reference to the element to insert in the collection.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is greater than <see cref="SHExposedTypesInfo.Count"/>.</exception>
    public static void Insert(this ref SHExposedTypesInfo infos, uint index, ref SHExposedTypeInfo info)
    {
      if (index > infos._length) throw new ArgumentOutOfRangeException(nameof(index));

      if (index == infos._length)
      {
        Push(ref infos, ref info);
        return;
      }

      var insertDelegate = Marshal.GetDelegateForFunctionPointer<ExposedTypesInfoInsertDelegate>(Native.Core._expTypesInsert);
      insertDelegate(ref infos, index, ref info);
    }

    /// <summary>
    /// Returns and removes the <see cref="SHExposedTypeInfo"/> at the end of the collection.
    /// </summary>
    /// <param name="infos">A reference to the collection.</param>
    /// <returns>The element that is removed from the end of the collection.</returns>
    /// <exception cref="InvalidOperationException">The collection is empty.</exception>
    public static SHExposedTypeInfo Pop(this ref SHExposedTypesInfo infos)
    {
      if (infos._length == 0) throw new InvalidOperationException();

      var popDelegate = Marshal.GetDelegateForFunctionPointer<ExposedTypesInfoPopDelegate>(Native.Core._expTypesPop);
      return popDelegate(ref infos);
    }

    /// <summary>
    /// Adds a <see cref="SHExposedTypeInfo"/> to the end of the collection.
    /// </summary>
    /// <param name="infos">A reference to the collection.</param>
    /// <param name="info">A reference to the element to add to the collection.</param>
    public static void Push(this ref SHExposedTypesInfo infos, ref SHExposedTypeInfo info)
    {
      var pushDelegate = Marshal.GetDelegateForFunctionPointer<ExposedTypesInfoPushDelegate>(Native.Core._expTypesPush);
      pushDelegate(ref infos, ref info);
    }

    /// <summary>
    /// Removes the element at the specified <paramref name="index"/> of the collection.
    /// </summary>
    /// <param name="infos">A reference to the collection.</param>
    /// <param name="index">The index of the element to remove.</param>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="index"/> is equal to or greater than <see cref="SHExposedTypesInfo.Count"/>.</exception>
    public static void RemoveAt(this ref SHExposedTypesInfo infos, uint index)
    {
      if (index >= infos._length) throw new ArgumentOutOfRangeException(nameof(index));

      var deleteDelegate = Marshal.GetDelegateForFunctionPointer<ExposedTypesInfoSlowDeleteDelegate>(Native.Core._expTypesSlowDelete);
      deleteDelegate(ref infos, index);
    }
  }

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate void ExposedTypesInfoSlowDeleteDelegate(ref SHExposedTypesInfo infos, uint index);

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate void ExposedTypesInfoInsertDelegate(ref SHExposedTypesInfo infos, uint index, ref SHExposedTypeInfo info);

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate SHExposedTypeInfo ExposedTypesInfoPopDelegate(ref SHExposedTypesInfo infos);

  [UnmanagedFunctionPointer(NativeMethods.CallingConv)]
  internal delegate void ExposedTypesInfoPushDelegate(ref SHExposedTypesInfo infos, ref SHExposedTypeInfo info);
}
