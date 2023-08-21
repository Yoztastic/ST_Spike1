namespace StorageSpike.Application.Core.DiscriminatedUnions;

public static class MaybeExtensions
{
    public static Maybe<T, NotFound> SingleOrNotFound<T>(this IEnumerable<T> ts, Func<T, bool> predicate)
    {
        var firstOrDefault = ts.SingleOrDefault(predicate);
        return firstOrDefault != null ? firstOrDefault : NotFound.Create<T>(); // would be nice to be more explicit but I cant find a good way of describing the predicate
    }

    public static async Task<Maybe<TResult, NotFound, TNot>> ProjectAsync<T, TResult, TNot>(this Maybe<T, NotFound> maybe,
        Func<T, Task<Maybe<TResult, TNot>>> mapFunc)
    {
        if (mapFunc == null)
            throw new ArgumentNullException(nameof(mapFunc));
        return maybe.Index switch
        {
            0 => (await mapFunc(maybe.AsT0)).Match<Maybe<TResult, NotFound, TNot>>(f0 => f0, f1 => f1),
            1 => maybe.AsT1,
            _ => throw new InvalidOperationException()
        };
    }

    public static async Task<Maybe<T, NotFound>> ProjectAsync<T>(this Maybe<T, NotFound> maybe,
        Func<T, Task<T>> mapFunc)
    {
        if (mapFunc == null)
            throw new ArgumentNullException(nameof(mapFunc));
        return maybe.Index switch
        {
            0 => await mapFunc(maybe.AsT0),
            1 => maybe.AsT1,
            _ => throw new InvalidOperationException()
        };
    }

    public static async Task<Maybe<TResult, NotFound>> ProjectAsync<T, TResult>(this Maybe<T, NotFound> maybe,
        Func<T, Task<Maybe<TResult, NotFound>>> mapFunc)
    {
        if (mapFunc == null)
            throw new ArgumentNullException(nameof(mapFunc));
        return maybe.Index switch
        {
            0 => await mapFunc(maybe.AsT0),
            1 => maybe.AsT1,
            _ => throw new InvalidOperationException()
        };
    }

    public static Maybe<TResult, NotFound> Project<T, TResult>(this Maybe<T, NotFound> maybe,
        Func<T, TResult> mapFunc)
    {
        if (mapFunc == null)
            throw new ArgumentNullException(nameof(mapFunc));
        return maybe.Index switch
        {
            0 => mapFunc(maybe.AsT0),
            1 => maybe.AsT1,
            _ => throw new InvalidOperationException()
        };
    }

    public static Maybe<T, NotFound, TNot> Project<T, TNot>(this Maybe<T, NotFound> maybe,
        Func<T, Maybe<T, NotFound, TNot>> mapFunc)
    {
        if (mapFunc == null)
            throw new ArgumentNullException(nameof(mapFunc));
        return maybe.Index switch
        {
            0 => mapFunc(maybe.AsT0),
            1 => maybe.AsT1,
            _ => throw new InvalidOperationException()
        };
    }

    public static Maybe<TResult, NotFound, TNot> Project<T, TResult, TNot>(this Maybe<T, NotFound, TNot> maybe,
        Func<T, TResult> mapFunc)
    {
        if (mapFunc == null)
            throw new ArgumentNullException(nameof(mapFunc));
        return maybe.Index switch
        {
            0 => mapFunc(maybe.AsT0),
            1 => maybe.AsT1,
            2 => maybe.AsT2,
            _ => throw new InvalidOperationException()
        };
    }
}
