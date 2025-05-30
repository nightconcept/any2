// <copyright file="Error.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System;
using System.IO;

namespace Night
{
  /// <summary>
  /// Defines the delegate for handling unhandled exceptions from game code.
  /// The error handler function will be called when an unhandled exception occurs
  /// within the game's Load, Update, Draw, or input callback methods.
  /// </summary>
  /// <param name="e">The exception that occurred.</param>
  public delegate void ErrorHandlerDelegate(Exception e);

  /// <summary>
  /// Manages the user-defined error handler.
  /// </summary>
  public static class Error
  {
    /// <summary>
    /// Gets the currently set custom error handler. Returns null if no custom handler is set.
    /// This is used internally by the framework to invoke the handler.
    /// </summary>
    internal static ErrorHandlerDelegate? CustomErrorHandler { get; private set; }

    /// <summary>
    /// Sets a custom function to be called when an unhandled error occurs in game code.
    /// </summary>
    /// <param name="handler">The delegate to handle errors.</param>
    public static void SetHandler(ErrorHandlerDelegate handler)
    {
      CustomErrorHandler = handler;
    }

    /// <summary>
    /// Gets the currently set custom error handler.
    /// This is used internally by the framework.
    /// </summary>
    /// <returns>The custom error handler delegate, or null if none is set.</returns>
    internal static ErrorHandlerDelegate? GetHandler()
    {
      return CustomErrorHandler;
    }
  }
}
