using System;
using System.Collections.Generic;

namespace Scripts.Utility.Extensions
{
  public static class FunctionalExtensions
  {
    public static T With<T>(this T self, Action<T> action, bool when)
    {
      if (when)
        action?.Invoke(self);

      return self;
    }

    public static T With<T>(this T self, Action<T> action) =>
      self.With(action, true);

    public static T With<T>(this T self, Action<T> action, Func<bool> when) =>
      self.With(action, when());

    public static IEnumerable<T> Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
      foreach (T obj in source)
        action(obj);
      return source;
    }

    public static void Branch(bool boolean, Action ifTrue, Action ifFalse)
    {
      if (boolean) ifTrue?.Invoke();
      else ifFalse?.Invoke();
    }
  }
}