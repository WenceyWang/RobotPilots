using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

using JetBrains.Annotations;

using RobotPilots.Vision.Managed.Communicate;
using RobotPilots.Vision.Managed.Communicate.Gun;
using RobotPilots.Vision.Managed.Utility;

namespace RobotPilots.Vision.Managed.Control
{

	public abstract class Gun : NeedRegisBase<GunType, GunAttribute, Gun>
	{

		private float _frictionSpeed;

		public Gun(byte id, GunSize size)
		{
			Id = id;
			Size = size;
		}

		public static Gun Parse(XElement element)
		{
			return Create(TypeList.Single(type => type.Name == element.Name), element);
		}


		public Gun(XElement element)
		{
			Id = ReadNecessaryValue<byte>(element, nameof(Id));
			Size = ReadNecessaryValue<GunSize>(element, nameof(Size));
		}

		public byte Id { get; set; }

		public GunSize Size { get; set; }




		public abstract float BulletSpeed { get; set; }

		public float FrictionSpeed
		{
			get => _frictionSpeed;
			set
			{
				CommunicateModule.Current.SerialManager.SendDatagram(new FrictionSpeedDatagram(Id, value));
				_frictionSpeed = value;
			}
		}

		[Startup]
		public static void LoadGun()
		{
			LoadAll();
		}


		public void Fire(byte amount)
		{
			CommunicateModule.Current.SerialManager.SendDatagram(new FireDatagram(Id, amount));
		}

	}

	public class GunType : RegisType<GunType, GunAttribute, Gun>
	{

		public GunType([NotNull] Type entryType) : base(entryType)
		{
		}

	}

	public class GunAttribute : NeedRegisBase.NeedRegisAttributeBase
	{

	}

}
