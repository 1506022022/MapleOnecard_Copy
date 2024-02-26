using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ObserverPattern
{
    public static class Broadcaster
    {
        // ############################ 채널 선언부
        private static Channel gameStartChannel;
        private static Channel turnChangedChannel;
        private static Channel turnEndChannel;
        private static Channel reverseTurnChannel;
        private static Channel submitTimeOutChannel;
        private static Channel lastCardChannel;
        private static Channel victoryChannel;
        private static Channel gameoverChannel;
        private static Channel dropoutChannel;
        private static Channel cardSubmitChannel;
        private static Channel restartChannel;
        private static Channel gameendChannel;
        // ############################ 프로퍼티 선언부
        public static Channel GameStartChannel
        {
            get
            {
                if (gameStartChannel == null) gameStartChannel = new Channel(null);
                return gameStartChannel;
            }
        }
        public static Channel TurnChangedChannel
        {
            get
            {
                if (turnChangedChannel == null) turnChangedChannel = new Channel(null);
                return turnChangedChannel;
            }
        }
        public static Channel TurnEndChannel
        {
            get
            {
                if (turnEndChannel == null) turnEndChannel = new Channel(null);
                return turnEndChannel;
            }
        }
        public static Channel ReverseTurnChannel
        {
            get
            {
                if (reverseTurnChannel == null) reverseTurnChannel = new Channel(null);
                return reverseTurnChannel;
            }
        }
        public static Channel LastcardChannel
        {
            get
            {
                if (lastCardChannel == null) lastCardChannel = new Channel(null);
                return lastCardChannel;
            }
        }
        public static Channel VictoryChannel
        {
            get
            {
                if (victoryChannel == null) victoryChannel = new Channel(null);
                return victoryChannel;
            }
        }
        public static Channel GameOverChannel
        {
            get {
                if (gameoverChannel == null) gameoverChannel = new Channel(null);
                return gameoverChannel;
                    }

}
        public static Channel DropoutChannel
        {
            get
            {
                if (dropoutChannel == null) dropoutChannel = new Channel(null);
                return dropoutChannel;
            }
        }
        public static Channel SubmitTimeOutChannel
        {
            get
            {
                if (submitTimeOutChannel == null) submitTimeOutChannel = new Channel(null);
                return submitTimeOutChannel;
            }
        }
        public static Channel CardSubmitChannel
        {
            get
            {
                if (cardSubmitChannel == null) cardSubmitChannel = new Channel(null);
                return cardSubmitChannel;
            }
        }
        public static Channel RestartChannel
        {
            get
            {
                if (restartChannel == null) restartChannel = new Channel(null);
                return restartChannel;
            }
        }
        public static Channel GameendChannel
        {
            get
            {
                if (gameendChannel == null) gameendChannel = new Channel(null);
                return gameendChannel;
            }
        }

        //#########################################
        public static void Reset()
        {
            gameStartChannel = null;
            turnChangedChannel = null;
            turnEndChannel = null;
            reverseTurnChannel = null;
            submitTimeOutChannel = null;
            lastCardChannel = null;
            victoryChannel = null;
            gameoverChannel = null;
            dropoutChannel = null;
            cardSubmitChannel = null;
        }
    }

    public class Channel : ISubject
    {
        private ObserverBot observer;
        private List<Observer> observers;
        public Channel(ISubject subject)
        {
            AddSuject(subject);
        }

        public void Notify()
        {
            if (observers == null) return;
            foreach(var o in observers)
            {
                o.OnNotify();
            }
        }

        public void AddObserver(Observer o)
        {
            if (observers == null) observers = new List<Observer>();
            if (o != null)
            observers.Add(o);
            
        }

        public void RemoveObserver(Observer o)
        {
            if (observers == null) return;
            observers.Remove(o);
        }
        public void AddSuject(ISubject subject)
        {
            if (observer ==null) observer = new ObserverBot(Notify);
            if (subject != null)
            {
                subject.AddObserver(observer);
            }
        }
    }
}