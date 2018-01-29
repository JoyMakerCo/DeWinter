using System;
using System.Collections;

namespace Ambition
{
	public class EnemyVO : GuestVO
	{
	    public int attackNumber;
	    public string Faction;
	    public int imageInt;
	    public string FlavorText;

	    public EnemyVO() : base() {}
	    public EnemyVO(GuestVO guest) : base(guest) {}
	    public EnemyVO(EnemyVO enemy) : base(enemy as GuestVO)
	    {
	    	Faction = enemy.Faction;
	    	imageInt = enemy.imageInt;
	    	attackNumber = enemy.attackNumber;
			FlavorText = enemy.FlavorText;
	    }
	}
}