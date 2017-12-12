using System ;
using System . Collections ;
using System . Collections . Generic ;
using System . Linq ;
using System . Xml . Linq ;

namespace RobotPilots . Vision . Managed . Communicate
{

	public abstract class SendDatagram : Datagram
	{

		public override XElement ToXElement ( )
		{
			XElement result = new XElement ( Type . XmlName ) ;

			return result ;
		}

		public virtual void PrepareForSend ( ) { }

	}

}
