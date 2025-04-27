using System;

namespace BlackCat.UnityUtils
{
	//public class RandomSetEnum : RandomSetString
	//{
	//	protected Type enumType;
	//	public Type EnumType { get => enumType; set => enumType = value; }

	//	public RandomSetEnum(RandomSetData<string> setData, Type enumType, OutRangeOption option = OutRangeOption.TakeDefault) : base(setData, option) 
	//	{
	//		this.enumType = enumType;
	//	}

	//}

	public class RandomSetEnum : RandomSet<Enum>
	{
		public RandomSetEnum(RandomSetData<Enum> setData, OutRangeOption option = OutRangeOption.TakeDefault) : base(setData, option) { }
	}

}