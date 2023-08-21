using OneOf;

namespace StorageSpike.Application.Core.DiscriminatedUnions;

public class Maybe<T,TNot> : OneOfBase<T,TNot>
{
    protected Maybe(OneOf<T,TNot> input) : base(input) { }
    public static implicit operator Maybe<T,TNot>(T _) => new(_);
    public static implicit operator Maybe<T,TNot>(TNot _) => new(_);
}

public class Maybe<T,T0,T1> : OneOfBase<T,T0,T1>
{
    private Maybe(OneOf<T,T0,T1> input) : base(input) { }
    public static implicit operator Maybe<T,T0,T1>(T _) => new(_);
    public static implicit operator Maybe<T,T0,T1>(T0 _) => new(_);
    public static implicit operator Maybe<T,T0,T1>(T1 _) => new(_);
}

public class Maybe<T,T0,T1,T2> : OneOfBase<T,T0,T1,T2>
{
    private Maybe(OneOf<T,T0,T1,T2> input) : base(input) { }
    public static implicit operator Maybe<T,T0,T1,T2>(T _) => new(_);
    public static implicit operator Maybe<T,T0,T1,T2>(T0 _) => new(_);
    public static implicit operator Maybe<T,T0,T1,T2>(T1 _) => new(_);
    public static implicit operator Maybe<T,T0,T1,T2>(T2 _) => new(_);
}
