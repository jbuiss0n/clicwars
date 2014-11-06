using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebServer.Spells
{
	[Flags]
	public enum SpellType
	{
		Projectile = 0x01,
		Targetable = 0x02,
		Zone = 0x04,
	}

	public abstract class Spell
	{
		public abstract SpellType Type { get; }

		public abstract int ManaCost { get; }

		public abstract TimeSpan Delay { get; }

		public Mobile Caster { get; protected set; }

		public Spell(Mobile caster)
		{
			Caster = caster;
		}

		public virtual bool Cast()
		{
			if (Caster.Mana < ManaCost)
				return false;

			Caster.Mana -= ManaCost;

			return true;
		}

		public virtual void OnCast()
		{
		}

		public virtual void Delete()
		{

		}

		public virtual void OnDelete()
		{

		}
	}
}
