
namespace BlackCat.UnityUtils
{
	public abstract class ARandom<T>
	{
		protected System.Random randomize;
		public abstract T Rand();
	}
}