namespace BlackCat.UnityUtils
{
	public class RandomSetString : RandomSet<string>
	{
		public RandomSetString(RandomSetData<string> setData, OutRangeOption option = OutRangeOption.TakeDefault) : base(setData, option) { }
	}
}