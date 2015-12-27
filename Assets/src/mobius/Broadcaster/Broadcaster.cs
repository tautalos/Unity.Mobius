using System;
using System.Collections.Generic;
using Tautalos.Unity.Mobius.Channels;
using Tautalos.Unity.Mobius.Signals;
using UniRx;

namespace Tautalos.Unity.Mobius.Broadcasters
{
	public class Broadcaster: IBroadcaster
	{
		string _name;
		IChannel _channel;
		List<IEventTag> _eventTags;
		Subject<ISignal> _subject;
		
		public Broadcaster (IChannel channel, ICollection<IEventTag> eventTags, string name = "anonymous-broadcaster")
		{
			_name = name;
			_channel = channel ?? EmptyChannel.Instance;
			_setEventTags (eventTags);
			_SubscribeToChannel ();
		}
				
		public void OnCompleted ()
		{
		}
		
		public void OnError (Exception error)
		{
			Console.WriteLine (string.Format ("<Broadcaster> ERROR => Broadcaster[{0}] on Channel[{1}]", Name, Channel.Name));
		}
		
		public void OnNext (ISignal signal)
		{
			if (signal != null && 
				signal.Signaller.Channel == _channel && 
				_channel.GetBroadcasterFor (signal.EventTag) == this) {
				_subject.OnNext (signal); 
			}
		}
				
		void _SubscribeToChannel ()
		{
			_subject = new Subject<ISignal> ();
			var broadcaster = this;
			Channel.Subscribe (broadcaster);
		}
		
		void _registerEventTags ()
		{
			if (_eventTags.Count > 0 && !Channel.IsEmpty) {
				foreach (IEventTag tag in GetEventTags()) {
					var entry = new EventEntry (eventTag: tag, broadcaster: this);
					Channel.AddEventEntry (entry);
				}
			}
		}
		
		void _setEventTags (ICollection<IEventTag> eventTags)
		{
			if (eventTags == null || eventTags.Count == 0) {
				_eventTags = new List<IEventTag> ();
			} else {
				_eventTags = new List<IEventTag> ();
				foreach (IEventTag tag in eventTags) {
					if (tag != null) {
						_eventTags.Add (tag);
					}
				}
			}
		}

		public IChannel Channel {
			get {
				return _channel;
			}
		}
		
		public string Name {
			get { return _name; }
		}
		
		public bool IsEmpty {
			get { return false; }
		}
		
		public IEventTag[] GetEventTags ()
		{
			return _eventTags.ToArray ();
		}
		
		public bool HasEventTag (IEventTag eventTag)
		{
			return false;
		}
		
		public void Silence ()
		{
			
		}

	}
}

