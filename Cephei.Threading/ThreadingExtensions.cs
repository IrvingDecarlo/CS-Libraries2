using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Cephei.Threading
{
  /// <summary>
  /// The ThreadingExtensions class contains extension methods for tasks and other thread-related functionalities.
  /// </summary>
  public static class ThreadingExtensions
  {
    /// <summary>
    /// Gets all incomplete tasks from a collection of ValueTasks.
    /// </summary>
    /// <param name="tasks">Collection of ValueTasks.</param>
    /// <returns>The list of incomplete tasks.</returns>
    public static List<Task> GetIncompleteTasks(this IEnumerable<ValueTask> tasks)
    {
      List<Task> taskx = new List<Task>();
      foreach (ValueTask task in tasks)
      {
        if (!task.IsCompleted) taskx.Add(task.AsTask());
      }
      return taskx;
    }

    /// <summary>
    /// Gets a list of disposable tasks from a collection of disposable objects. Objects that were instantly disposed of are not included.
    /// </summary>
    /// <typeparam name="T">Disposable object type.</typeparam>
    /// <param name="disps">Collection of disposable objects.</param>
    /// <returns>The list of disposable tasks.</returns>
    public static List<Task> GetDisposableTasks<T>(this IEnumerable<T> disps) where T : IAsyncDisposable
    {
      List<Task> tasks = new List<Task>();
      ValueTask task;
      foreach (IAsyncDisposable disp in disps)
      {
        task = disp.DisposeAsync();
        if (!task.IsCompleted) tasks.Add(task.AsTask());
      }
      return tasks;
    }

    /// <summary>
    /// Disposes all objects in a collection asynchronously and simultaneously.
    /// </summary>
    /// <typeparam name="T">The IAsyncDisposable object type.</typeparam>
    /// <param name="disps">Collection of disposables.</param>
    public static Task DisposeAllConcurrentAsync<T>(this IEnumerable<T> disps) where T : IAsyncDisposable
      => Task.WhenAll(disps.GetDisposableTasks());

    /// <summary>
    /// Disposes of all objects in a collection asynchronously.
    /// </summary>
    /// <typeparam name="T">The IAsyncDisposable object type.</typeparam>
    /// <param name="disps">Collection of disposables.</param>
    public static async ValueTask DisposeAllAsync<T>(this IEnumerable<T> disps) where T : IAsyncDisposable
    {
      foreach (IAsyncDisposable disp in disps) await disp.DisposeAsync();
    }
  }
}
