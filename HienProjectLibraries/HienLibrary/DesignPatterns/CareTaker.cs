using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShoesStore.DesignPatterns
{
    public static class CareTaker<T> where T : ICloneable
    {
        private static List<Memento<T>> mementoList = new List<Memento<T>>();
        public static void SaveState(Originator<T> orig)
        {
            mementoList.Add(orig.CreateMemento());
        }
        public static void RestoreState(Originator<T> orig, int checkPoint)
        {
            orig.RestoreMemento(mementoList[checkPoint]);
        }
    }
}