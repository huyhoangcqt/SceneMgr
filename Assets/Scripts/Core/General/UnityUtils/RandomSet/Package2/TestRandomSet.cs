using System.Collections.Generic;
using UnityEngine;

namespace BlackCat.UnityUtils
{
	public enum NormalBox
	{
		Item1,
		Item2,
		Item3,
		Item4,
	}

	public enum SilverBox
	{
		Item100,
		Item101,
		Item102,
	}

	public class RandomSetName
	{
		public const string NORMAL_BOX = "NormalBox";
		public const string SILVER_BOX = "SilverBox";

		public const string POISON_CHANCE = "PoisonChance";
		public const string FLINCH_CHANCE = "FlinchChance";
		public const string CRIT_RATE = "CritRate";
	}

	public class RatioConfig
	{
		public static Dictionary<string, float> NormalBoxChance = new Dictionary<string, float>()
		{
			{NormalBox.Item1.ToString(), 0.25f},
			{NormalBox.Item2.ToString(), 0.25f},
			{NormalBox.Item3.ToString(), 0.25f},
			{NormalBox.Item4.ToString(), 0.25f},
		};

		public static Dictionary<SilverBox, float> SilverBoxChance = new Dictionary<SilverBox, float>()
		{
			{SilverBox.Item100, 0.33f},
			{SilverBox.Item101, 0.66f},
		};

		public const float poisonChance = 0.15f;
		public const float flinchChance = 0.33f;
		public const float critRate = 0.57f;
	}

	public class TestRandomSet : MonoBehaviour
	{
		RandomSet<SilverBox> silverBoxRandomize;

		private void Awake()
		{
			//StartTest

			//String //ItemName
			RandomMgr.CreateSet(RandomSetName.NORMAL_BOX, RatioConfig.NormalBoxChance);

			//Enum
			silverBoxRandomize = RandomMgr.CreateSet<SilverBox>(RandomSetName.SILVER_BOX, RatioConfig.SilverBoxChance);

			//Random YesNo // Chance like CritRate
			RandomMgr.Create(RandomSetName.POISON_CHANCE, RatioConfig.poisonChance);

			Random();
		}

		private void Random()
		{
			//string gatchaItem = RandomMgr.GetSet<string>(RandomSetName.NORMAL_BOX).Rand();
			//NormalBox gatchaItemEnum = gatchaItem.ToEnum<NormalBox>();
			//Debugger.Log(LogTags.Test_RandomSet ,"Gatcha NormalBox Item: " + gatchaItemEnum.ToString());

			SilverBox gatchaItem2 = silverBoxRandomize.Rand();
			Debugger.Warning(LogTags.Test_RandomSet, $"Gatcha SilverBox Item: " + gatchaItem2.ToString());


			//bool isTakePoison = RandomMgr.Get(RandomSetName.POISON_CHANCE).Rand();
			//Debugger.Error(LogTags.Test_RandomSet, $"Poison {RatioConfig.poisonChance * 100}%: is {isTakePoison}");
		}

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				Random();
			}
		}
	}
}