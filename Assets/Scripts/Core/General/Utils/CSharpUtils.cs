


//CSharpUtils
public static partial class Utils
{
	/// <summary>
	/// --------------- ENUM ---------------
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="val"></param>
	/// <param name="defaultValue"></param>
	/// <returns></returns>
	/// <exception cref="System.ArgumentException"></exception>
	#region ToEnum

	public static T ToEnum<T>(this string val, T defaultValue) where T : struct, System.IConvertible
	{
		if (!typeof(T).IsEnum) throw new System.ArgumentException("T must be an enumerated type");

		try
		{
			T result = (T)System.Enum.Parse(typeof(T), val, true);
			return result;
		}
		catch
		{
			return defaultValue;
		}
	}

	public static T ToEnum<T>(this int val, T defaultValue) where T : struct, System.IConvertible
	{
		if (!typeof(T).IsEnum) throw new System.ArgumentException("T must be an enumerated type");

		object obj = val;
		if (System.Enum.IsDefined(typeof(T), obj))
		{
			return (T)obj;
		}
		else
		{
			return defaultValue;
		}
	}

	public static T ToEnum<T>(this object val, T defaultValue) where T : struct, System.IConvertible
	{
		return ToEnum<T>(System.Convert.ToString(val), defaultValue);
	}

	public static T ToEnum<T>(this string val) where T : struct, System.IConvertible
	{
		return ToEnum<T>(val, default(T));
	}

	public static T ToEnum<T>(this int val) where T : struct, System.IConvertible
	{
		return ToEnum<T>(val, default(T));
	}

	public static T ToEnum<T>(this object val) where T : struct, System.IConvertible
	{
		return ToEnum<T>(System.Convert.ToString(val), default(T));
	}

	public static System.Enum ToEnumOfType(this System.Type enumType, object value)
	{
		return System.Enum.Parse(enumType, System.Convert.ToString(value), true) as System.Enum;
	}

	public static bool TryToEnum<T>(this object val, out T result) where T : struct, System.IConvertible
	{
		if (!typeof(T).IsEnum) throw new System.ArgumentException("T must be an enumerated type");

		try
		{
			result = (T)System.Enum.Parse(typeof(T), System.Convert.ToString(val), true);
			return true;
		}
		catch
		{
			result = default(T);
			return false;
		}
	}

	#endregion
}