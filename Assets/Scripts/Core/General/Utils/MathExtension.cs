


//Checking
//Debug.Log($"16.6666 = {1.666666f}");

//	Debug.Log($"Floor(16.6666, 2) = {MathExtension.Floor(16.6666f, 2)}");
//	Debug.Log($"Floor(16.4444, 2) = {MathExtension.Floor(16.4444f, 2)}");
//	Debug.Log($"Floor(16.44, 2) = {MathExtension.Floor(16.44f, 2)}");

//	Debug.Log($"Ceiling(16.6666, 2) = {MathExtension.Ceiling(16.6666f, 2)}");
//	Debug.Log($"Ceiling(16.4444f, 2) = {MathExtension.Ceiling(16.4444f, 2)}");
//	Debug.Log($"Ceiling(16.44, 2) = {MathExtension.Ceiling(16.44f, 2)}");

//	Debug.Log($"Round(16.6666, 2) = {MathExtension.Round(16.6666f, 2)}");
//	Debug.Log($"Round(16.44444, 2) = {MathExtension.Round(16.4444f, 2)}");
//	Debug.Log($"Round(16.44, 2) = {MathExtension.Round(16.44f, 2)}");


public static class MathExtension
{
	/// <summary>
	/// --------- FLOOR -----------
	/// => Custom Mathf.Floor => to floor a number after floating point. Example: Floor(16.66666, 2) => 16.66,  Floor(16.44444, 2) => 16.44
	/// </summary>
	/// <param name="value"></param>
	/// <param name="floatingPointNum">Limit: [0, 4]</param>
	/// <returns></returns>
	public static float Floor(this float value, int floatingPointNum)
	{
		//UnityEngine.Debug.Log($"Floor ({value}, {floatingPointNum})");
		int pow = 1;
		for (int i = 0; i < floatingPointNum; i++) 
		{
			pow *= 10;
		}

		float powvalue = value * pow;
		int roundValue = (int)powvalue;
		value = (float)roundValue / pow;
		//UnityEngine.Debug.Log($"Floor Result = {value}");
		return value;
	}

	/// <summary>
	/// --------- ROUNDing -----------
	/// => Custom Mathf.Round => to Round a number after floating point. Example: Round(16.66666, 2) => 16.67, Round(16.44444, 2) => 16.44
	/// </summary>
	/// <param name="value"></param>
	/// <param name="floatingPointNum">Limit: [0, 4]</param>
	/// <returns></returns>
	public static float Round(this float value, int floatingPointNum)
	{
		//UnityEngine.Debug.Log($"Round ({value}, {floatingPointNum})");
		int pow = 1;
		for (int i = 0; i < floatingPointNum; i++)
		{
			pow *= 10;
		}

		float powvalue = value * pow + 0.5f;
		int roundValue = (int)powvalue;
		value = (float)roundValue / pow;
		//UnityEngine.Debug.Log($"Round Result = {value}");
		return value;
	}

	/// <summary>
	/// --------- CEILING -----------
	/// => Custom Mathf.Ceiling => to Ceiling a number after floating point. Example: Ceiling(16.66666, 2) => 16.67,    Ceiling(16.44444, 2) => 16.45,    Ceiling(16.44, 2) => 16.44
	/// </summary>
	/// <param name="value"></param>
	/// <param name="floatingPointNum">Limit: [0, 4]</param>
	/// <returns></returns>
	public static float Ceiling(this float value, int floatingPointNum)
	{
		//UnityEngine.Debug.Log($"Ceiling ({value}, {floatingPointNum})");
		int pow = 1;
		for (int i = 0; i < floatingPointNum; i++)
		{
			pow *= 10;
		}

		float powvalue = value * pow;
		int roundValue = (int)powvalue;
		if (roundValue < powvalue)
		{
			roundValue += 1;
		}
		value = (float)roundValue / pow;
		//UnityEngine.Debug.Log($"Ceiling Result = {value}");
		return value;
	}
}