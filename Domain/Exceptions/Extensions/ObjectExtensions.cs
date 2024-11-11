using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Throw Argument Null Exception if object is null with the name of the object as name
		/// </summary>
		/// <param name="obj"></param>
		public static T ThrowArgumentNullExceptionOnNull<T>(this T obj) => obj ?? throw new ArgumentNullException(typeof(T).Name);

		/// <summary>
		/// Throw Argument Null Exception if object is null with the name of the object as name
		/// </summary>
		/// <param name="obj"></param>
		/// <param name="message"></param>
		public static T ThrowArgumentNullExceptionOnNull<T>(this T obj, string message) => obj ?? throw new ArgumentNullException(typeof(T).Name, message);

		/// <summary>
		/// Check if Nullable Datetime is null or default value
		/// </summary>
		/// <param name="date"><see cref="DateTime"/></param>
		/// <returns><see cref="bool"/></returns>
		public static bool IsEmpty(this DateTime? date) => date == null || date == default;

		/// <summary>
		/// Check if Nullable DateTimeOffset is null or default value
		/// </summary>
		/// <param name="dateOffset"><see cref="DateTimeOffset"/></param>
		/// <returns><see cref="bool"/></returns>
		public static bool IsEmpty(this DateTimeOffset? dateOffset) => dateOffset == null || dateOffset == default;

		/// <summary>
		/// Get string representation of a Enum field
		/// </summary>
		/// <typeparam name="T">Type of the Enum</typeparam>
		/// <param name="enumValue">Enum value</param>
		/// <returns>Name of the enum field</returns>
		public static string ToEnum<T>(this T? enumValue) where T : struct, IConvertible => Enum.GetName(typeof(T), enumValue);

		/// <summary>
		/// Get enum from string representation
		/// </summary>
		/// <typeparam name="T">Type of the Enum</typeparam>
		/// <param name="name">name value</param>
		/// <returns>The enum field</returns>
		public static T? FromEnum<T>(this string name) where T : struct, IConvertible => Enum.TryParse(name, out T enumValue) ? enumValue : (T?)null;

		/// <summary>
		/// Get enum from string representation
		/// </summary>
		/// <typeparam name="T">Type of the Enum</typeparam>
		/// <param name="name">name value</param>
		/// <param name="defaultValue">The default value if not parse</param>
		/// <returns>The enum field</returns>
		public static T FromEnumOrValue<T>(this string name, T defaultValue) where T : struct, IConvertible => Enum.TryParse(name, out T enumValue) ? enumValue : defaultValue;

		/// <summary>
		/// Get a seperated string with a delimiter to array of strings
		/// </summary>
		/// <param name="values">the string values</param>
		/// <param name="delimiter">the delimiter</param>
		/// <returns>Array of strings</returns>
		public static IEnumerable<string> Seperate(this string values, char delimiter = ',') => values.Split(delimiter).Select(s => s.Trim()) ?? null;

		/// <summary>
		/// convert object to array
		/// </summary>
		public static T[] AsArray<T>(this T obj) => new T[] { obj };

		/// <summary>
		/// Get value of a property by property name
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <typeparam name="TPropertyTypeValue"></typeparam>
		/// <param name="object"></param>
		/// <param name="propertyName"></param>
		/// <returns></returns>
		public static object GetValueByPropertyName<T, TPropertyTypeValue>(this T @object, string propertyName) where T : notnull
		{
			var value = typeof(T).GetProperty(propertyName)?.GetValue(@object, null);

			return (value is TPropertyTypeValue) ? value : null;
		}

		/// <summary>
		/// Get object equality by hash code;
		/// </summary>
		/// <param name="obj1"></param>
		/// <param name="obj2"></param>
		/// <returns></returns>
		public static bool IsEqual(this object obj1, object obj2) => obj1.GetHashCode() == obj2.GetHashCode();

		/// <summary>
		/// Check if implement ICollection
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsCollection(this object type) => type is ICollection;

		/// <summary>
		/// Check if implement ICollection and is not empty
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsNotEmptyCollection(this object type)
		{
			if (!type.IsCollection()) return true;

			return (type as ICollection)?.Count > 0;
		}
	}

}
