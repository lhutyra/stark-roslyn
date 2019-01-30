namespace StarkPlatform.CodeAnalysis
{
    public interface IExtendedTypeSymbol : ITypeWithElementTypeSymbol
    {
        TypeAccessModifiers AccessModifiers { get; }
    }
}
