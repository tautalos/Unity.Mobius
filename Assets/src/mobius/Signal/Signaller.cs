using Tautalos.Unity.Mobius.Channels;

namespace Tautalos.Unity.Mobius.Signals
{
	public class Signaller: ISignaller
	{
		IChannel _channel;
		object _owner;
		
		public Signaller (object owner, IChannel channel)
		{
			_channel = channel ?? EmptyChannel.Instance;
			_owner = owner ?? this;
		}
		
		public Signaller (): this(null, null)
		{
		}

		public ISignal CreateSignal (IEventTag eventTag, IMessage message)
		{
			ISignal signal = EmptySignal.Instance;
			if (eventTag != null && message != null) {
				signal = new Signal (
					signaller: this, 
				    eventTag: eventTag ?? EmptyEventTag.Instance, 
				    message: message ?? EmptyMessage.Instance 
				);
			}
			return signal;
		}

		public void emit (ISignal signal)
		{
			throw new System.NotImplementedException ();
		}

		public void emit (ISignal[] signals)
		{
			throw new System.NotImplementedException ();
		}

		public object Owner {
			get {
				return _owner;
			}
		}

		public IChannel Channel {
			get {
				return _channel;
			}
		}

		public bool IsEmpty {
			get {
				var isEmpty = false;
				if (Channel == EmptyChannel.Instance) {
					isEmpty = true;
				}
				return isEmpty;
			}
		}

	}
}

