// <copyright file="ErrorHandler.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace Night
{
  /// <summary>
  /// Defines the delegate for handling unhandled exceptions from game code.
  /// </summary>
  /// <param name="e">The exception that occurred.</param>
  public delegate void ErrorHandlerDelegate(Exception e);

  /// <summary>
  /// Manages the user-defined error handler.
  /// </summary>
  public static class Error
  {
    internal static ErrorHandlerDelegate? CustomErrorHandler { get; private set; }

    /// <summary>
    /// Sets a custom function to be called when an unhandled error occurs in game code.
    /// </summary>
    /// <param name="handler">The delegate to handle errors.</param>
    public static void SetHandler(ErrorHandlerDelegate handler)
    {
      CustomErrorHandler = handler;
    }

    internal static ErrorHandlerDelegate? GetHandler()
    {
      return CustomErrorHandler;
    }
  }
}
