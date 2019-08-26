namespace Beryllium.EntityFrameworkCore
{
   using System;

   public static class TypeExtensions
   {
      public static bool IsBoolean(this Type type)
      {
         Type t = Nullable.GetUnderlyingType(type) ?? type;

         return t == typeof(bool);
      }

      public static bool IsTrueEnum(this Type type)
      {
         Type t = Nullable.GetUnderlyingType(type) ?? type;

         return t.IsEnum;
      }
   }
}
