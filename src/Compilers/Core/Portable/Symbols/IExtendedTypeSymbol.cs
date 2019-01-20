namespace Microsoft.CodeAnalysis
{
    public interface IExtendedTypeSymbol : ITypeWithElementTypeSymbol
    {
        TypeAccessModifiers AccessModifiers { get; }
    }
}
