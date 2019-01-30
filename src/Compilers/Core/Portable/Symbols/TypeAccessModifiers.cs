using System;
using System.Diagnostics;

namespace StarkPlatform.CodeAnalysis
{
    [Flags]
    public enum TypeAccessModifiers
    {
        None = 0,

        ReadOnly = 1 << 0,

        Transient = 1 << 1,
    }

    public static class TypeAccessModifiersHelper
    {
        public static TypeAccessModifiers GetRequiredAccessModifiers(this ITypeSymbol source, ITypeSymbol destination, out TypeAccessModifiers destModifiers)
        {
            Debug.Assert(source != null && destination != null);

            var sourceExtended = source as IExtendedTypeSymbol;
            var destinationExtended = destination as IExtendedTypeSymbol;

            var sourceModifiers = sourceExtended?.AccessModifiers ?? TypeAccessModifiers.None;
            destModifiers = destinationExtended?.AccessModifiers ?? TypeAccessModifiers.None;
            if (sourceModifiers == destModifiers)
            {
                return TypeAccessModifiers.None;
            }

            if ((destModifiers & TypeAccessModifiers.ReadOnly) != 0 && sourceModifiers == 0) sourceModifiers = TypeAccessModifiers.ReadOnly;


            var requiredModifiers = sourceModifiers ^ destModifiers;
            return requiredModifiers;
        }
    }
}
