using System;
using GameEngine; 
namespace GameEngine.Engine
{
    public class CardProfileEvent
    {

        //TODO: Namespaces? Do we need them? Should we need them? Would we benefit from using them? 

        public enum UIEventType
        {
            NotSet,

            Press,
            Click,
            Open,
            Close
        }

        public UIEventType Type = UIEventType.NotSet;

        public OrbCardProfile Profile;

        public Card CardType;

        public bool IsCardOpen { get; private set; }

        private CardProfileEvent()
        {
        }        

        public static readonly CardProfileEvent _instance = new CardProfileEvent(); 
        
        public static CardProfileEvent GetInstance(OrbCardProfile profile, UIEventType type = UIEventType.NotSet)
        {
            _instance.Profile = profile;
            _instance.IsCardOpen = profile.gameObject.activeSelf; 
            _instance.CardType = profile.internalCard;
            _instance.Type = type; 

            return _instance; 
        }
         
    }
}

